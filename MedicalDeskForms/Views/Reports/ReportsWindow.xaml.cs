using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Reports;

public partial class ReportsWindow : Window
{
    private readonly ReportRepository repository =
        new();

    public ReportsWindow()
    {
        InitializeComponent();
    }

    private void btnRequestsReport_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridReport.ItemsSource =
            new List<object>
            {
                repository
                .GetRequestStatistics()
            };
    }

    private void btnSpecialistsReport_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridReport.ItemsSource =
            repository.GetSpecialists();
    }

    private void btnEquipmentReport_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridReport.ItemsSource =
            new List<object>
            {
                repository
                .GetEquipmentStatistics()
            };
    }

    private void btnExcel_Click(
        object sender,
        RoutedEventArgs e)
    {
        MessageBox.Show(
            "Экспорт Excel будет подключен следующим этапом");
    }

    private void btnPdf_Click(
        object sender,
        RoutedEventArgs e)
    {
        MessageBox.Show(
            "Экспорт PDF будет подключен следующим этапом");
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}