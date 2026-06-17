using MySql.Data.MySqlClient;
using MedicalDeskLib.Database;

namespace MedicalDeskLib.Services;

public static class NotificationService
{
    public static void Create(
        int? userId,
        string eventType,
        string text)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        INSERT INTO Notifications
        (
            UserId,
            EventType,
            MessageText
        )
        VALUES
        (
            @UserId,
            @EventType,
            @MessageText
        )
        """;

        using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@UserId",
            userId);

        command.Parameters.AddWithValue(
            "@EventType",
            eventType);

        command.Parameters.AddWithValue(
            "@MessageText",
            text);

        command.ExecuteNonQuery();
    }
}