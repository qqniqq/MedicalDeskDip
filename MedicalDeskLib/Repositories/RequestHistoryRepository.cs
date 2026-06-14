using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class RequestHistoryRepository
{
    public List<RequestHistory> GetByRequest(int requestId)
    {
        List<RequestHistory> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            h.*,
            u.FullName
        FROM RequestHistory h
        LEFT JOIN Users u
            ON u.Id=h.UserId
        WHERE RequestId=@RequestId
        ORDER BY EventDate DESC
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        cmd.Parameters.AddWithValue(
            "@RequestId",
            requestId);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new RequestHistory
            {
                Id = Convert.ToInt32(reader["Id"]),
                RequestId = Convert.ToInt32(reader["RequestId"]),
                EventDate = Convert.ToDateTime(reader["EventDate"]),
                EventType = reader["EventType"].ToString()!,
                Description = reader["Description"].ToString()!,
                UserName = reader["FullName"]?.ToString() ?? ""
            });
        }

        return list;
    }

    public void Add(
        int requestId,
        string eventType,
        string description,
        int? userId)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        INSERT INTO RequestHistory
        (
            RequestId,
            EventType,
            Description,
            UserId
        )
        VALUES
        (
            @RequestId,
            @EventType,
            @Description,
            @UserId
        )
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        cmd.Parameters.AddWithValue("@RequestId", requestId);
        cmd.Parameters.AddWithValue("@EventType", eventType);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@UserId", userId);

        cmd.ExecuteNonQuery();
    }
}