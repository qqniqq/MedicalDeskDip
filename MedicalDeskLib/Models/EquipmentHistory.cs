namespace MedicalDeskLib.Models;

public class EquipmentHistory
{
    public int Id { get; set; }

    public int EquipmentId { get; set; }

    public DateTime ServiceDate { get; set; }

    public string FaultDescription { get; set; } = "";

    public string WorkPerformed { get; set; } = "";

    public string CommentText { get; set; } = "";

    public string ExecutorName { get; set; } = "";
}