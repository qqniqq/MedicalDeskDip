namespace MedicalDeskLib.Models;

public class Equipment
{

    public int Id { get; set; }

    public string InventoryNumber { get; set; } = "";

    public string EquipmentName { get; set; } = "";

    public string Model { get; set; } = "";

    public string SerialNumber { get; set; } = "";

    public string Manufacturer { get; set; } = "";

    public string RoomNumber { get; set; } = "";

    public int? UserId { get; set; }

    public string UserName { get; set; } = "";

    public DateTime CommissioningDate { get; set; }

    public int StateId { get; set; }

    public string StateName { get; set; } = "";

    public string Notes { get; set; } = "";
}