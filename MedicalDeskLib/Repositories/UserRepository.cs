using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MedicalDeskLib.Security;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class UserRepository
{
    public List<User> GetAll()
    {
        List<User> users = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            u.*,
            r.Name AS RoleName
        FROM Users u
        INNER JOIN Roles r
            ON r.Id = u.RoleId
        ORDER BY u.FullName
        """;

        using var command =
            new MySqlCommand(sql, connection);

        using var reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new User
            {
                Id = Convert.ToInt32(reader["Id"]),
                FullName = reader["FullName"].ToString()!,
                Phone = reader["Phone"].ToString()!,
                Login = reader["Login"].ToString()!,
                PasswordHash = reader["PasswordHash"].ToString()!,
                RoleId = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"].ToString()!,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                IsActive = Convert.ToBoolean(reader["IsActive"])
            });
        }

        return users;
    }
    public void ResetPassword(
    int userId,
    string newPassword)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    UPDATE Users
    SET PasswordHash=@Password
    WHERE Id=@Id
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Id",
            userId);

        cmd.Parameters.AddWithValue(
            "@Password",
            PasswordHasher.Hash(
                newPassword));

        cmd.ExecuteNonQuery();
    }
    public User? GetById(
        int id)
    {
        return GetAll()
            .FirstOrDefault(
                x => x.Id == id);
    }


    public User? GetByLogin(
        string login)
    {
        return GetAll()
            .FirstOrDefault(
                x => x.Login == login);
    }

    public bool LoginExists(
        string login)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT COUNT(*)
        FROM Users
        WHERE Login=@Login
        """;

        using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@Login",
            login);

        return Convert.ToInt32(
            command.ExecuteScalar()) > 0;
    }

    public void Add(
        User user)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        INSERT INTO Users
        (
            FullName,
            Phone,
            Login,
            PasswordHash,
            RoleId,
            IsActive
        )
        VALUES
        (
            @FullName,
            @Phone,
            @Login,
            @PasswordHash,
            @RoleId,
            @IsActive
        )
        """;

        using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@FullName",
            user.FullName);

        command.Parameters.AddWithValue(
            "@Phone",
            user.Phone);

        command.Parameters.AddWithValue(
            "@Login",
            user.Login);

        command.Parameters.AddWithValue(
            "@PasswordHash",
            user.PasswordHash);

        command.Parameters.AddWithValue(
            "@RoleId",
            user.RoleId);

        command.Parameters.AddWithValue(
            "@IsActive",
            user.IsActive);

        command.ExecuteNonQuery();
    }

    public void Update(
        User user)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        UPDATE Users
        SET
            FullName=@FullName,
            Phone=@Phone,
            Login=@Login,
            PasswordHash=@PasswordHash,
            RoleId=@RoleId,
            IsActive=@IsActive
        WHERE Id=@Id
        """;

        using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@Id",
            user.Id);

        command.Parameters.AddWithValue(
            "@FullName",
            user.FullName);

        command.Parameters.AddWithValue(
            "@Phone",
            user.Phone);

        command.Parameters.AddWithValue(
            "@Login",
            user.Login);

        command.Parameters.AddWithValue(
            "@PasswordHash",
            user.PasswordHash);

        command.Parameters.AddWithValue(
            "@RoleId",
            user.RoleId);

        command.Parameters.AddWithValue(
            "@IsActive",
            user.IsActive);

        command.ExecuteNonQuery();
    }

    public void Delete(
        int id)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        DELETE FROM Users
        WHERE Id=@Id
        """;

        using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@Id",
            id);

        command.ExecuteNonQuery();
    }

    public void Block(
        int id)
    {
        ToggleBlock(
            id,
            false);
    }

    public void Unblock(
        int id)
    {
        ToggleBlock(
            id,
            true);
    }

    public void ToggleBlock(
        int id,
        bool active)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        UPDATE Users
        SET IsActive=@IsActive
        WHERE Id=@Id
        """;

        using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@Id",
            id);

        command.Parameters.AddWithValue(
            "@IsActive",
            active);

        command.ExecuteNonQuery();
    }

    
}