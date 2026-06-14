using System.Windows;
using MedicalDeskLib.Repositories;
using EquipmentModel = MedicalDeskLib.Models.Equipment;

namespace MedicalDeskForms.Views.Equipment;

public partial class EquipmentEditWindow : Window
{
    private readonly EquipmentRepository repository =
        new();

    private EquipmentModel? equipment;

    public EquipmentEditWindow()
    {
        InitializeComponent();

        LoadStates();
    }

    public EquipmentEditWindow(
        EquipmentModel equipment)
            : this()
    {
        this.equipment = equipment;

        txtInventory.Text =
            equipment.InventoryNumber;

        txtName.Text =
            equipment.EquipmentName;

        txtModel.Text =
            equipment.Model;

        txtSerial.Text =
            equipment.SerialNumber;

        txtManufacturer.Text =
            equipment.Manufacturer;

        txtRoom.Text =
            equipment.RoomNumber;

        txtNotes.Text =
            equipment.Notes;
    }

    private void LoadStates()
    {
        cmbState.Items.Add("Исправно");
        cmbState.Items.Add("Требует ремонта");
        cmbState.Items.Add("На ремонте");
        cmbState.Items.Add("Списано");

        cmbState.SelectedIndex = 0;
    }

    private void btnSave_Click(
        object sender,
        RoutedEventArgs e)
    {
        EquipmentModel item =
            equipment ?? new EquipmentModel();

        item.InventoryNumber =
            txtInventory.Text;

        item.EquipmentName =
            txtName.Text;

        item.Model =
            txtModel.Text;

        item.SerialNumber =
            txtSerial.Text;

        item.Manufacturer =
            txtManufacturer.Text;

        item.RoomNumber =
            txtRoom.Text;

        item.StateId =
            cmbState.SelectedIndex + 1;

        item.Notes =
            txtNotes.Text;

        if (equipment == null)
            repository.Add(item);
        else
            repository.Update(item);

        Close();
    }
}