namespace MedicalDeskLib.Models;

public class Notification
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime NotificationDate { get; set; }

    public string EventType { get; set; } = "";

    public string MessageText { get; set; } = "";

    public bool IsRead { get; set; }
}