namespace MedicalDeskLib.Models;

public class AuditLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string ActionType { get; set; } = "";

    public string Description { get; set; } = "";
    public string FullName { get; set; } = "";
    public DateTime ActionDate { get; set; }
}