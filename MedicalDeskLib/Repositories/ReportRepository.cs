using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class ReportRepository
{
    public ReportRequestStatistics
        GetRequestStatistics()
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        ReportRequestStatistics model =
            new();

        string sql =
        """
        SELECT
        COUNT(*) Total,
        SUM(CASE WHEN StatusId=1 THEN 1 ELSE 0 END) NewCount,
        SUM(CASE WHEN StatusId=3 THEN 1 ELSE 0 END) ActiveCount,
        SUM(CASE WHEN StatusId=6 THEN 1 ELSE 0 END) CompletedCount,
        SUM(CASE WHEN StatusId=7 THEN 1 ELSE 0 END) CancelledCount
        FROM Requests
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        using var reader =
            cmd.ExecuteReader();

        if (reader.Read())
        {
            model.TotalRequests =
                Convert.ToInt32(reader["Total"]);

            model.NewRequests =
                Convert.ToInt32(reader["NewCount"]);

            model.ActiveRequests =
                Convert.ToInt32(reader["ActiveCount"]);

            model.CompletedRequests =
                Convert.ToInt32(reader["CompletedCount"]);

            model.CancelledRequests =
                Convert.ToInt32(reader["CancelledCount"]);
        }

        return model;
    }

    public List<ReportSpecialistStatistics>
        GetSpecialists()
    {
        List<ReportSpecialistStatistics>
            list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            u.FullName,
            COUNT(
                CASE WHEN r.StatusId=6
                THEN 1 END
            ) CompletedRequests,

            COUNT(
                CASE WHEN r.StatusId=3
                THEN 1 END
            ) ActiveRequests
        FROM Users u
        LEFT JOIN Requests r
            ON r.ExecutorId=u.Id
        WHERE u.RoleId=2
        GROUP BY u.Id
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(
                new ReportSpecialistStatistics
                {
                    SpecialistName =
                        reader["FullName"].ToString()!,

                    CompletedRequests =
                        Convert.ToInt32(
                            reader["CompletedRequests"]),

                    ActiveRequests =
                        Convert.ToInt32(
                            reader["ActiveRequests"])
                });
        }

        return list;
    }

    public ReportEquipmentStatistics
        GetEquipmentStatistics()
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        ReportEquipmentStatistics model =
            new();

        string sql =
        """
        SELECT COUNT(*) Total
        FROM Equipment
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        model.TotalEquipment =
            Convert.ToInt32(
                cmd.ExecuteScalar());

        return model;
    }
}