using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class EquipmentHistoryRepository
{
    public List<EquipmentHistory> GetByEquipment(
        int equipmentId)
    {
        List<EquipmentHistory> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            eh.*,
            u.FullName
        FROM EquipmentHistory eh
        LEFT JOIN Users u
            ON u.Id=eh.ExecutorId
        WHERE EquipmentId=@EquipmentId
        ORDER BY ServiceDate DESC
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        cmd.Parameters.AddWithValue(
            "@EquipmentId",
            equipmentId);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new EquipmentHistory
            {
                Id = Convert.ToInt32(reader["Id"]),
                EquipmentId = Convert.ToInt32(reader["EquipmentId"]),
                ServiceDate = Convert.ToDateTime(reader["ServiceDate"]),
                FaultDescription = reader["FaultDescription"].ToString()!,
                WorkPerformed = reader["WorkPerformed"].ToString()!,
                CommentText = reader["CommentText"].ToString()!,
                ExecutorName = reader["FullName"]?.ToString() ?? ""
            });
        }


        return list;
    }
    public void Add(
    int equipmentId,
    string action,
    string comment)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    INSERT INTO EquipmentHistory
    (
        EquipmentId,
        ServiceDate,
        FaultDescription,
        WorkPerformed,
        CommentText,
        ExecutorId
    )
    VALUES
    (
        @EquipmentId,
        NOW(),
        @FaultDescription,
        @WorkPerformed,
        @CommentText,
        NULL
    )
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@EquipmentId",
            equipmentId);

        cmd.Parameters.AddWithValue(
            "@FaultDescription",
            action);

        cmd.Parameters.AddWithValue(
            "@WorkPerformed",
            action);

        cmd.Parameters.AddWithValue(
            "@CommentText",
            comment);

        cmd.ExecuteNonQuery();
    }
    public void AddService(
    int equipmentId,
    string fault,
    string work,
    string comment,
    int executorId)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    INSERT INTO EquipmentHistory
    (
        EquipmentId,
        ServiceDate,
        FaultDescription,
        WorkPerformed,
        CommentText,
        ExecutorId
    )
    VALUES
    (
        @EquipmentId,
        NOW(),
        @FaultDescription,
        @WorkPerformed,
        @CommentText,
        @ExecutorId
    )
    """;

        using var cmd =
            new MySqlCommand(sql, connection);

        cmd.Parameters.AddWithValue(
            "@EquipmentId",
            equipmentId);

        cmd.Parameters.AddWithValue(
            "@FaultDescription",
            fault);

        cmd.Parameters.AddWithValue(
            "@WorkPerformed",
            work);

        cmd.Parameters.AddWithValue(
            "@CommentText",
            comment);

        cmd.Parameters.AddWithValue(
            "@ExecutorId",
            executorId);

        cmd.ExecuteNonQuery();
    }
}