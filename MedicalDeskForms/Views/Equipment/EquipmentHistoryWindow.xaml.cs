using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Equipment;

public partial class EquipmentHistoryWindow : Window
{
    private readonly EquipmentHistoryRepository
        repository = new();

    public EquipmentHistoryWindow(
        int equipmentId)
    {
        InitializeComponent();

        LoadData(
            equipmentId);
    }

    private void LoadData(
        int equipmentId)
    {
        gridHistory.ItemsSource =
            repository.GetByEquipment(
                equipmentId);
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}