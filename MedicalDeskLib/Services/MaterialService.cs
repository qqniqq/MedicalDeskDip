using MedicalDeskLib.Database;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;
using MySql.Data.MySqlClient;
using MedicalDeskLib.Models;

namespace MedicalDeskLib.Services;

public class MaterialService
{
    public void WriteOff(
        int materialId,
        int requestId,
        int quantity,
        string reason)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        using var transaction =
            connection.BeginTransaction();

        try
        {
            string update =
            """
            UPDATE Materials
            SET Quantity =
                Quantity - @Quantity
            WHERE Id=@Id
            """;

            using var updateCmd =
                new MySqlCommand(
                    update,
                    connection,
                    transaction);

            updateCmd.Parameters.AddWithValue(
                "@Quantity",
                quantity);

            updateCmd.Parameters.AddWithValue(
                "@Id",
                materialId);

            updateCmd.ExecuteNonQuery();

            string history =
            """
            INSERT INTO MaterialHistory
            (
                MaterialId,
                RequestId,
                SpecialistId,
                Quantity,
                WriteOffReason
            )
            VALUES
            (
                @MaterialId,
                @RequestId,
                @SpecialistId,
                @Quantity,
                @Reason
            )
            """;

            using var historyCmd =
                new MySqlCommand(
                    history,
                    connection,
                    transaction);

            historyCmd.Parameters.AddWithValue(
                "@MaterialId",
                materialId);

            historyCmd.Parameters.AddWithValue(
                "@RequestId",
                requestId);

            historyCmd.Parameters.AddWithValue(
                "@SpecialistId",
                SessionManager.CurrentUser!.Id);

            historyCmd.Parameters.AddWithValue(
                "@Quantity",
                quantity);

            historyCmd.Parameters.AddWithValue(
                "@Reason",
                reason);

            historyCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    public void CheckMinimumStock()
    {
        MaterialRepository repository =
            new();

        var lowStock =
            repository.GetLowStock();

        UserRepository users =
      new();

        foreach (var user in users.GetAll())
        {
            if (user.RoleName == "Администратор"
                || user.RoleName == "Специалист")
            {
                foreach (var material in lowStock)
                {
                    NotificationService.Create(
                        user.Id,
                        "Склад",
                        $"Минимальный остаток: {material.Name}");
                }
            }
        }
    }
}