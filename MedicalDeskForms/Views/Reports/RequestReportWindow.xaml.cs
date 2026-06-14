using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Reports;

public partial class RequestReportWindow : Window
{
    private readonly RequestRepository repository =
        new();

    public RequestReportWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        gridRequests.ItemsSource =
            repository.GetAll();
    }

    private void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
    {
        LoadData();
    }
}