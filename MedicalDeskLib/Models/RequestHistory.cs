namespace MedicalDeskLib.Models;

public class RequestHistory
{
    public int Id { get; set; }

    public int RequestId { get; set; }

    public DateTime EventDate { get; set; }

    public string EventType { get; set; } = "";

    public string Description { get; set; } = "";

    public int? UserId { get; set; }

    public string UserName { get; set; } = "";
}