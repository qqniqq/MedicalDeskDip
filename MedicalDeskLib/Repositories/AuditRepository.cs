using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class AuditRepository
{
    public List<AuditLog> GetAll()
    {
        List<AuditLog> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            a.*,
            u.FullName
        FROM AuditLog a
        LEFT JOIN Users u
            ON u.Id = a.UserId
        ORDER BY a.ActionDate DESC
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new AuditLog
            {
                Id =
                    Convert.ToInt32(
                        reader["Id"]),

                UserId =
                    reader["UserId"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        reader["UserId"]),

                FullName =
                    reader["FullName"]?.ToString()
                    ?? "",

                ActionType =
                    reader["ActionType"]
                    .ToString()!,

                Description =
                    reader["Description"]
                    .ToString()!,

                ActionDate =
                    Convert.ToDateTime(
                        reader["ActionDate"])
            });
        }

        return list;
    }

    public void Add(
        int? userId,
        string actionType,
        string description)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        INSERT INTO AuditLog
        (
            UserId,
            ActionType,
            Description,
            ActionDate
        )
        VALUES
        (
            @UserId,
            @ActionType,
            @Description,
            NOW()
        )
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@UserId",
            userId);

        cmd.Parameters.AddWithValue(
            "@ActionType",
            actionType);

        cmd.Parameters.AddWithValue(
            "@Description",
            description);

        cmd.ExecuteNonQuery();
    }
}