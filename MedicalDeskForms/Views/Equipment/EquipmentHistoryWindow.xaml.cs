using System.Windows;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Equipment;

public partial class EquipmentHistoryWindow : Window
{
    public EquipmentHistoryWindow(
        int equipmentId)
    {
        InitializeComponent();

        EquipmentHistoryRepository repository =
            new();

        gridHistory.ItemsSource =
            repository.GetByEquipment(
                equipmentId);
    }
}