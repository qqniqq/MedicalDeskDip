namespace MedicalDeskLib.Models;

public class Material
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = "";

    public int Quantity { get; set; }

    public int MinimumQuantity { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string Notes { get; set; } = "";
}