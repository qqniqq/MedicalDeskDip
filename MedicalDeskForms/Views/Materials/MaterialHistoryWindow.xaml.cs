using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Materials;

public partial class MaterialHistoryWindow : Window
{
    private readonly MaterialHistoryRepository
        repository =
            new();

    public MaterialHistoryWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        gridHistory.ItemsSource =
            repository.GetAll();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}