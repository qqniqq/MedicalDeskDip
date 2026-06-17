using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class NotificationRepository
{
    public List<Notification> GetAll()
    {
        List<Notification> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT *
        FROM Notifications
        ORDER BY NotificationDate DESC
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(Map(reader));
        }

        return list;
    }

    public List<Notification> GetForUser(
    int userId)
    {
        List<Notification> list =
            new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    SELECT *
    FROM Notifications
    WHERE UserId=@UserId
    ORDER BY NotificationDate DESC
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@UserId",
            userId);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(Map(reader));
        }

        return list;
    }
    public int GetAllUnreadCount()
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    SELECT COUNT(*)
    FROM Notifications
    WHERE IsRead=0
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        return Convert.ToInt32(
            cmd.ExecuteScalar());
    }
    public void MarkAsRead(
        int id)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        UPDATE Notifications
        SET IsRead=1
        WHERE Id=@Id
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Id",
            id);

        cmd.ExecuteNonQuery();
    }

    public int GetUnreadCount(
    int userId)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    SELECT COUNT(*)
    FROM Notifications
    WHERE IsRead=0
      AND UserId=@UserId
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@UserId",
            userId);

        return Convert.ToInt32(
            cmd.ExecuteScalar());
    }
    private Notification Map(
        MySqlDataReader reader)
    {
        return new Notification
        {
            Id =
                Convert.ToInt32(
                    reader["Id"]),

            UserId =
                reader["UserId"] ==
                DBNull.Value
                ? null
                : Convert.ToInt32(
                    reader["UserId"]),

            EventType =
                reader["EventType"]
                    .ToString()!,

            MessageText =
                reader["MessageText"]
                    .ToString()!,

            NotificationDate =
                Convert.ToDateTime(
                    reader["NotificationDate"]),

            IsRead =
                Convert.ToBoolean(
                    reader["IsRead"])
        };
    }
}