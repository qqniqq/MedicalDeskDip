using System.Linq;
using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views;

public partial class DashboardWindow : Window
{
    private readonly RequestRepository
        requestRepository = new();

    private readonly MaterialRepository
        materialRepository = new();

    public DashboardWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        var requests =
            requestRepository.GetAll();

        var materials =
            materialRepository.GetLowStock();

        txtRequests.Text =
            requests.Count.ToString();

        txtInWork.Text =
            requests
            .Count(x =>
                x.StatusName ==
                "В работе")
            .ToString();

        txtCompleted.Text =
            requests
            .Count(x =>
                x.StatusName ==
                "Завершена")
            .ToString();

        txtLowStock.Text =
            materials.Count.ToString();

        gridLowStock.ItemsSource =
            materials;
    }
}