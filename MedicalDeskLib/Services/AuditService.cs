using MySql.Data.MySqlClient;
using MedicalDeskLib.Database;

namespace MedicalDeskLib.Services;

public static class AuditService
{
    public static void Log(
        int? userId,
        string action,
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
            Description
        )
        VALUES
        (
            @UserId,
            @ActionType,
            @Description
        )
        """;

        using var command =
            new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@ActionType", action);
        command.Parameters.AddWithValue("@Description", description);

        command.ExecuteNonQuery();
    }
}