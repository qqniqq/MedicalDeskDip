using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MedicalDeskLib.Security;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class MaterialHistoryRepository
{
    public List<MaterialHistory> GetAll()
    {
        List<MaterialHistory> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    SELECT
        mh.*,
        m.Name MaterialName,
        u.FullName SpecialistName
    FROM MaterialHistory mh
    LEFT JOIN Materials m
        ON m.Id = mh.MaterialId
    LEFT JOIN Users u
        ON u.Id = mh.SpecialistId
    ORDER BY mh.OperationDate DESC
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new MaterialHistory
            {
                Id = Convert.ToInt32(reader["Id"]),
                MaterialId = Convert.ToInt32(reader["MaterialId"]),
                RequestId =
                    reader["RequestId"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(reader["RequestId"]),
                SpecialistId =
                    Convert.ToInt32(reader["SpecialistId"]),
                Quantity =
                    Convert.ToInt32(reader["Quantity"]),
                WriteOffReason =
                    reader["WriteOffReason"]
                    .ToString()!,
                OperationDate =
                    Convert.ToDateTime(
                        reader["OperationDate"]),
                MaterialName =
                    reader["MaterialName"]
                    .ToString()!,
                SpecialistName =
                    reader["SpecialistName"]
                    .ToString()!
            });
        }

        return list;
    }

    public void AddIncome(
        int materialId,
        int quantity,
        string comment)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
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
            NULL,
            @SpecialistId,
            @Quantity,
            @Comment
        )
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@MaterialId",
            materialId);

        cmd.Parameters.AddWithValue(
            "@SpecialistId",
            SessionManager.CurrentUser!.Id);

        cmd.Parameters.AddWithValue(
            "@Quantity",
            quantity);

        cmd.Parameters.AddWithValue(
            "@Comment",
            comment);

        cmd.ExecuteNonQuery();
    }
}