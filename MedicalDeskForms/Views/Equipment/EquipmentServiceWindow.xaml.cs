using System.Windows;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;

namespace MedicalDeskForms.Views.Equipment;

public partial class EquipmentServiceWindow : Window
{
    private readonly int equipmentId;

    private readonly EquipmentHistoryRepository
        repository = new();

    public EquipmentServiceWindow(
        int equipmentId)
    {
        InitializeComponent();

        this.equipmentId =
            equipmentId;
    }

    private void btnSave_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
            txtFault.Text))
        {
            MessageBox.Show(
                "Укажите неисправность");

            return;
        }

        if (string.IsNullOrWhiteSpace(
            txtWork.Text))
        {
            MessageBox.Show(
                "Укажите выполненные работы");

            return;
        }

        repository.AddService(
            equipmentId,
            txtFault.Text.Trim(),
            txtWork.Text.Trim(),
            txtComment.Text.Trim(),
            SessionManager.CurrentUser!.Id);

        MessageBox.Show(
            "Запись об обслуживании добавлена");

        Close();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}