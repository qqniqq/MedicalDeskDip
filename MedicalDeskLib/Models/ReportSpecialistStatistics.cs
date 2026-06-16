namespace MedicalDeskLib.Models;

public class ReportSpecialistStatistics
{
    public string SpecialistName { get; set; } = "";

    public int CompletedRequests { get; set; }

    public int ActiveRequests { get; set; }

    public double AverageHours { get; set; }
}