using System.Windows;

namespace MedicalDeskForms.Views.Reports;

public partial class ReportsWindow : Window
{
    public ReportsWindow()
    {
        InitializeComponent();
    }

    private void btnRequestsReport_Click(
        object sender,
        RoutedEventArgs e)
    {
        RequestReportWindow window =
            new();

        window.ShowDialog();
    }

    private void btnMaterialsReport_Click(
        object sender,
        RoutedEventArgs e)
    {
        MaterialReportWindow window =
            new();

        window.ShowDialog();
    }

    private void btnAuditReport_Click(
        object sender,
        RoutedEventArgs e)
    {
        AuditReportWindow window =
            new();

        window.ShowDialog();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}