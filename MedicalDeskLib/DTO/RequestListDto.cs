namespace MedicalDeskLib.DTO;

public class RequestListDto
{
    public int Id { get; set; }

    public string RequestNumber { get; set; } = "";

    public string RoomNumber { get; set; } = "";

    public string ApplicantName { get; set; } = "";

    public string RequestType { get; set; } = "";

    public string Status { get; set; } = "";

    public string Executor { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}