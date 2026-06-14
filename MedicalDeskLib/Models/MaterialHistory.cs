namespace MedicalDeskLib.Models;

public class MaterialHistory
{
    public int Id { get; set; }

    public int MaterialId { get; set; }

    public int RequestId { get; set; }

    public int SpecialistId { get; set; }

    public int Quantity { get; set; }

    public string WriteOffReason { get; set; } = "";

    public DateTime OperationDate { get; set; }

    public string SpecialistName { get; set; } = "";

    public string MaterialName { get; set; } = "";
}