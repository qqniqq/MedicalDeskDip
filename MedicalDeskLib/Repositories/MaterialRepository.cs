using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class MaterialRepository
{
    public List<Material> GetAll()
    {
        List<Material> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            m.*,
            c.Name CategoryName
        FROM Materials m
        JOIN MaterialCategories c
            ON c.Id=m.CategoryId
        ORDER BY m.Name
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        using var reader =
            cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Material
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString()!,
                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                CategoryName = reader["CategoryName"].ToString()!,
                Quantity = Convert.ToInt32(reader["Quantity"]),
                MinimumQuantity = Convert.ToInt32(reader["MinimumQuantity"]),
                ReceiptDate = Convert.ToDateTime(reader["ReceiptDate"]),
                Notes = reader["Notes"].ToString()!
            });
        }

        return list;
    }

    public Material? GetById(
        int id)
    {
        return GetAll()
            .FirstOrDefault(
                x => x.Id == id);
    }

    public void Add(
        Material material)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    INSERT INTO Materials
    (
        Name,
        CategoryId,
        Quantity,
        MinimumQuantity,
        ReceiptDate,
        Notes
    )
    VALUES
    (
        @Name,
        @CategoryId,
        @Quantity,
        @MinimumQuantity,
        @ReceiptDate,
        @Notes
    )
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Name",
            material.Name);

        cmd.Parameters.AddWithValue(
            "@CategoryId",
            material.CategoryId);

        cmd.Parameters.AddWithValue(
            "@Quantity",
            material.Quantity);

        cmd.Parameters.AddWithValue(
            "@MinimumQuantity",
            material.MinimumQuantity);

        cmd.Parameters.AddWithValue(
            "@ReceiptDate",
            material.ReceiptDate);

        cmd.Parameters.AddWithValue(
            "@Notes",
            material.Notes);

        cmd.ExecuteNonQuery();
    }
    public void Update(
    Material material)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    UPDATE Materials
    SET
        Name=@Name,
        CategoryId=@CategoryId,
        Quantity=@Quantity,
        MinimumQuantity=@MinimumQuantity,
        ReceiptDate=@ReceiptDate,
        Notes=@Notes
    WHERE Id=@Id
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Id",
            material.Id);

        cmd.Parameters.AddWithValue(
            "@Name",
            material.Name);

        cmd.Parameters.AddWithValue(
            "@CategoryId",
            material.CategoryId);

        cmd.Parameters.AddWithValue(
            "@Quantity",
            material.Quantity);

        cmd.Parameters.AddWithValue(
            "@MinimumQuantity",
            material.MinimumQuantity);

        cmd.Parameters.AddWithValue(
            "@ReceiptDate",
            material.ReceiptDate);

        cmd.Parameters.AddWithValue(
            "@Notes",
            material.Notes);

        cmd.ExecuteNonQuery();
    }

    public void Delete(
        int id)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        DELETE FROM Materials
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

    public void IncreaseQuantity(
        int id,
        int quantity)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        UPDATE Materials
        SET Quantity =
            Quantity + @Quantity
        WHERE Id=@Id
        """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Id",
            id);

        cmd.Parameters.AddWithValue(
            "@Quantity",
            quantity);

        cmd.ExecuteNonQuery();
    }

    public List<Material> GetLowStock()
    {
        return GetAll()
            .Where(
                x => x.Quantity <=
                     x.MinimumQuantity)
            .ToList();
    }
   
    public void AddQuantity(
    int id,
    int quantity)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    UPDATE Materials
    SET Quantity =
        Quantity + @Quantity
    WHERE Id=@Id
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        cmd.Parameters.AddWithValue(
            "@Id",
            id);

        cmd.Parameters.AddWithValue(
            "@Quantity",
            quantity);

        cmd.ExecuteNonQuery();
    }
}