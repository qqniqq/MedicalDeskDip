using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Reports;

public partial class MaterialReportWindow : Window
{
    private readonly MaterialRepository repository =
        new();

    public MaterialReportWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        gridMaterials.ItemsSource =
            repository.GetAll();
    }

    private void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
    {
        LoadData();
    }
}