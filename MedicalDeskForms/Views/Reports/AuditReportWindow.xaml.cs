using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Reports;

public partial class AuditReportWindow : Window
{
    private readonly AuditRepository repository =
        new();

    public AuditReportWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        gridAudit.ItemsSource =
            repository.GetAll();
    }

    private void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
    {
        LoadData();
    }
}