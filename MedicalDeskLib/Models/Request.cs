namespace MedicalDeskLib.Models;

public class Request
{
    public int Id { get; set; }

    public string RequestNumber { get; set; } = "";

    public string RoomNumber { get; set; } = "";

    public string ApplicantName { get; set; } = "";

    public string ApplicantPhone { get; set; } = "";

    public int RequestTypeId { get; set; }

    public string RequestTypeName { get; set; } = "";

    public string ProblemDescription { get; set; } = "";

    public int StatusId { get; set; }

    public string StatusName { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public DateTime? AcceptedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? ExecutorId { get; set; }

    public string ExecutorName { get; set; } = "";

    public string SpecialistComment { get; set; } = "";

    public int AuthorId { get; set; }
}