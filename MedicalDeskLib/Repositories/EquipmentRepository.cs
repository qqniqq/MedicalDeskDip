using MedicalDeskLib.Database;
using MedicalDeskLib.Models;
using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Repositories;

public class EquipmentRepository
{
    public List<Equipment> GetAll()
    {
        List<Equipment> list = new();

        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        SELECT
            e.*,
            es.Name StateName,
            u.FullName UserName
        FROM Equipment e
        JOIN EquipmentStates es
            ON es.Id=e.StateId
        LEFT JOIN Users u
            ON u.Id=e.UserId
        ORDER BY EquipmentName
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

    public Equipment? GetById(int id)
    {
        return GetAll()
            .FirstOrDefault(x => x.Id == id);
    }

    public void Add(Equipment equipment)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        INSERT INTO Equipment
        (
            InventoryNumber,
            EquipmentName,
            Model,
            SerialNumber,
            Manufacturer,
            RoomNumber,
            UserId,
            CommissioningDate,
            StateId,
            Notes
        )
        VALUES
        (
            @InventoryNumber,
            @EquipmentName,
            @Model,
            @SerialNumber,
            @Manufacturer,
            @RoomNumber,
            @UserId,
            @CommissioningDate,
            @StateId,
            @Notes
        )
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        FillParameters(cmd, equipment);

        cmd.ExecuteNonQuery();
    }

    public void Update(Equipment equipment)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        UPDATE Equipment
        SET
            EquipmentName=@EquipmentName,
            Model=@Model,
            SerialNumber=@SerialNumber,
            Manufacturer=@Manufacturer,
            RoomNumber=@RoomNumber,
            UserId=@UserId,
            CommissioningDate=@CommissioningDate,
            StateId=@StateId,
            Notes=@Notes
        WHERE Id=@Id
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        FillParameters(cmd, equipment);

        cmd.Parameters.AddWithValue("@Id", equipment.Id);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
        DELETE FROM Equipment
        WHERE Id=@Id
        """;

        using var cmd =
            new MySqlCommand(sql, connection);

        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }

    private void FillParameters(
        MySqlCommand cmd,
        Equipment e)
    {
        cmd.Parameters.AddWithValue("@InventoryNumber", e.InventoryNumber);
        cmd.Parameters.AddWithValue("@EquipmentName", e.EquipmentName);
        cmd.Parameters.AddWithValue("@Model", e.Model);
        cmd.Parameters.AddWithValue("@SerialNumber", e.SerialNumber);
        cmd.Parameters.AddWithValue("@Manufacturer", e.Manufacturer);
        cmd.Parameters.AddWithValue("@RoomNumber", e.RoomNumber);
        cmd.Parameters.AddWithValue("@UserId", e.UserId);
        cmd.Parameters.AddWithValue("@CommissioningDate", e.CommissioningDate);
        cmd.Parameters.AddWithValue("@StateId", e.StateId);
        cmd.Parameters.AddWithValue("@Notes", e.Notes);
    }

    private Equipment Map(MySqlDataReader r)
    {
        return new Equipment
        {
            Id = Convert.ToInt32(r["Id"]),
            InventoryNumber = r["InventoryNumber"].ToString()!,
            EquipmentName = r["EquipmentName"].ToString()!,
            Model = r["Model"].ToString()!,
            SerialNumber = r["SerialNumber"].ToString()!,
            Manufacturer = r["Manufacturer"].ToString()!,
            RoomNumber = r["RoomNumber"].ToString()!,
            UserName = r["UserName"]?.ToString() ?? "",
            StateName = r["StateName"].ToString()!,
            Notes = r["Notes"]?.ToString() ?? ""
        };
    }
    public string GenerateInventoryNumber()
    {
        using var connection =
            DbConnectionFactory.Create();

        connection.Open();

        string sql =
        """
    SELECT IFNULL(MAX(Id),0)+1
    FROM Equipment
    """;

        using var cmd =
            new MySqlCommand(
                sql,
                connection);

        int nextId =
            Convert.ToInt32(
                cmd.ExecuteScalar());

        return $"INV-{DateTime.Now:yyyy}-{nextId:0000}";
    }
}