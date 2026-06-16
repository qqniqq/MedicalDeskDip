namespace MedicalDeskLib.Models;

public class ReportRequestStatistics
{
    public int TotalRequests { get; set; }

    public int NewRequests { get; set; }

    public int ActiveRequests { get; set; }

    public int CompletedRequests { get; set; }

    public int CancelledRequests { get; set; }
}