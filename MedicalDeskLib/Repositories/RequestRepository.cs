using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class RequestRepository : IRequestRepository
{
    public List<Request> GetAll()
    {
        List<Request> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            r.*,
            r.AcceptedAt,
            r.CompletedAt,
            r.ResolutionComment,
            rt.Name RequestTypeName,
            rs.Name StatusName,
            u.FullName ExecutorName
        FROM Requests r
        JOIN RequestTypes rt
            ON rt.Id = r.RequestTypeId
        JOIN RequestStatuses rs
            ON rs.Id = r.StatusId
        LEFT JOIN Users u
            ON u.Id = r.ExecutorId
        ORDER BY r.CreatedAt DESC
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(
                Map(reader));
        }

        return list;
    }

    public Request? GetById(
        int id)
    {
        return GetAll()
            .FirstOrDefault(
                x => x.Id == id);
    }

    public void Create(
        Request request)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        INSERT INTO Requests
        (
            RoomNumber,
            ApplicantName,
            ApplicantPhone,
            RequestTypeId,
            ProblemDescription,
            StatusId,
            AuthorId
        )
        VALUES
        (
            @RoomNumber,
            @ApplicantName,
            @ApplicantPhone,
            @RequestTypeId,
            @ProblemDescription,
            1,
            @AuthorId
        )
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@RoomNumber",
            request.RoomNumber);

        cmd.Parameters.AddWithValue(
            "@ApplicantName",
            request.ApplicantName);

        cmd.Parameters.AddWithValue(
            "@ApplicantPhone",
            request.ApplicantPhone);

        cmd.Parameters.AddWithValue(
            "@RequestTypeId",
            request.RequestTypeId);

        cmd.Parameters.AddWithValue(
            "@ProblemDescription",
            request.ProblemDescription);

        cmd.Parameters.AddWithValue(
            "@AuthorId",
            request.AuthorId);

        cmd.ExecuteNonQuery();
    }

    public void TakeToWork(
        int requestId,
        int specialistId)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        UPDATE Requests
        SET
            StatusId = 2,
            ExecutorId = @ExecutorId,
            AcceptedAt = NOW()
        WHERE Id = @Id
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@ExecutorId",
            specialistId);

        cmd.Parameters.AddWithValue(
            "@Id",
            requestId);

        cmd.ExecuteNonQuery();
    }

    public void Complete(
        int requestId,
        string comment)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        UPDATE Requests
        SET
            StatusId = 3,
            CompletedAt = NOW(),
            ResolutionComment = @Comment
        WHERE Id = @Id
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Comment",
            comment);

        cmd.Parameters.AddWithValue(
            "@Id",
            requestId);

        cmd.ExecuteNonQuery();
    }

    public void Cancel(
        int requestId)
    {
        using var connection =
        DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    UPDATE Requests
    SET StatusId = 4
    WHERE Id = @Id
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Id",
            requestId);

        cmd.ExecuteNonQuery();
    }

    public List<Request> Search(
        string text)
    {
        return GetAll()
            .Where(x =>
                x.RequestNumber.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.RoomNumber.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.ApplicantName.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Request> GetByStatus(
        string status)
    {
        return GetAll()
            .Where(x =>
                x.StatusName == status)
            .ToList();
    }

    public List<Request> GetByAuthor(
        int authorId)
    {
        return GetAll()
            .Where(x =>
                x.AuthorId == authorId)
            .ToList();
    }

    private Request Map(
        MySqlDataReader reader)
    {
        Request request =
            new()
            {
                Id =
                    Convert.ToInt32(
                        reader["Id"]),

                RequestNumber =
                    reader["RequestNumber"]
                    .ToString()!,
                ResolutionComment =
    reader["ResolutionComment"]
    ?.ToString() ?? "",

                RoomNumber =
                    reader["RoomNumber"]
                    .ToString()!,

                ApplicantName =
                    reader["ApplicantName"]
                    .ToString()!,

                ApplicantPhone =
                    reader["ApplicantPhone"]
                    .ToString()!,

                ProblemDescription =
                    reader["ProblemDescription"]
                    .ToString()!,

                RequestTypeName =
                    reader["RequestTypeName"]
                    .ToString()!,

                StatusName =
                    reader["StatusName"]
                    .ToString()!,

                ExecutorName =
                    reader["ExecutorName"]
                    ?.ToString() ?? "",


                CreatedAt =
                    Convert.ToDateTime(
                        reader["CreatedAt"]),

                AcceptedAt =
                    reader["AcceptedAt"]
                    == DBNull.Value
                        ? null
                        : Convert.ToDateTime(
                            reader["AcceptedAt"]),

                CompletedAt =
                    reader["CompletedAt"]
                    == DBNull.Value
                        ? null
                        : Convert.ToDateTime(
                            reader["CompletedAt"]),

                AuthorId =
                    Convert.ToInt32(
                        reader["AuthorId"])
            };

        if (request.AcceptedAt.HasValue &&
            request.CompletedAt.HasValue)
        {
            request.ExecutionHours =
                (int)
                (
                    request.CompletedAt.Value -
                    request.AcceptedAt.Value
                ).TotalHours;
        }

        return request;
    }
    public List<Request> GetMyRequests(
    int authorId)
    {
        List<Request> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            r.*,
            r.AcceptedAt,
            r.CompletedAt,
            r.ResolutionComment,
            rt.Name RequestTypeName,
            rs.Name StatusName,
            u.FullName ExecutorName
        FROM Requests r
        JOIN RequestTypes rt
            ON rt.Id = r.RequestTypeId
        JOIN RequestStatuses rs
            ON rs.Id = r.StatusId
        LEFT JOIN Users u
            ON u.Id = r.ExecutorId
        WHERE r.AuthorId = @AuthorId
        ORDER BY r.CreatedAt DESC
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@AuthorId",
            authorId);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(
                Map(reader));
        }

        return list;
    }

    public List<Request> GetActiveRequestsByAuthor(
        int authorId)
    {
        return GetAll()
            .Where(x =>
                x.AuthorId == authorId
                &&
                x.StatusName != "Завершена"
                &&
                x.StatusName != "Отменена")
            .ToList();
    }

    public List<User> GetUsers()
    {
        List<User> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT *
        FROM Users
        WHERE RoleId = 3
          AND IsActive = 1
        ORDER BY FullName
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(
                new User
                {
                    Id =
                        Convert.ToInt32(
                            reader["Id"]),

                    FullName =
                        reader["FullName"]
                        .ToString()!,

                    Phone =
                        reader["Phone"]
                        .ToString()!
                });
        }

        return list;
    }

    public List<Request> GetMyActiveRequests(
        int userId)
    {
        List<Request> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            r.*,
            r.AcceptedAt,
            r.CompletedAt,
            r.ResolutionComment,
            rt.Name RequestTypeName,
            rs.Name StatusName,
            u.FullName ExecutorName
        FROM Requests r
        JOIN RequestTypes rt
            ON rt.Id = r.RequestTypeId
        JOIN RequestStatuses rs
            ON rs.Id = r.StatusId
        LEFT JOIN Users u
            ON u.Id = r.ExecutorId
        WHERE r.AuthorId = @UserId
          AND rs.Name IN
          (
              'Новая',
              'В работе'
          )
        ORDER BY r.CreatedAt DESC
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
            list.Add(
                Map(reader));
        }

        return list;
    }
    public void Delete(int id)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        using var transaction =
            connection.BeginTransaction();

        try
        {
            string sqlMaterialHistory =
            """
        DELETE FROM MaterialHistory
        WHERE RequestId=@Id
        """;

            using (var cmd =
                new MySqlCommand(
                    sqlMaterialHistory,
                    connection,
                    transaction))
            {
                cmd.Parameters.AddWithValue(
                    "@Id",
                    id);

                cmd.ExecuteNonQuery();
            }

            string sqlRequestHistory =
            """
        DELETE FROM RequestHistory
        WHERE RequestId=@Id
        """;

            using (var cmd =
                new MySqlCommand(
                    sqlRequestHistory,
                    connection,
                    transaction))
            {
                cmd.Parameters.AddWithValue(
                    "@Id",
                    id);

                cmd.ExecuteNonQuery();
            }

            string sqlRequest =
            """
        DELETE FROM Requests
        WHERE Id=@Id
        """;

            using (var cmd =
                new MySqlCommand(
                    sqlRequest,
                    connection,
                    transaction))
            {
                cmd.Parameters.AddWithValue(
                    "@Id",
                    id);

                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }
}