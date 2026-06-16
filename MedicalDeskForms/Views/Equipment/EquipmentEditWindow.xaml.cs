using System.Windows;
using MedicalDeskLib.Repositories;
using EquipmentModel = MedicalDeskLib.Models.Equipment;
using MedicalDeskLib.Models;

namespace MedicalDeskForms.Views.Equipment;

public partial class EquipmentEditWindow : Window
{
    private readonly EquipmentRepository
        repository = new();

    private EquipmentModel? equipment;

    private List<User> users =
        new();

    public EquipmentEditWindow()
    {
        InitializeComponent();

        LoadUsers();

        LoadStates();

        txtInventory.Text =
            repository.GenerateInventoryNumber();

        dpCommissioningDate.SelectedDate =
            DateTime.Today;
    }

    public EquipmentEditWindow(
      EquipmentModel equipment)
          : this()
    {
        this.equipment = equipment;

        txtTitle.Text =
            "Редактирование оборудования";

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

        dpCommissioningDate.SelectedDate =
            equipment.CommissioningDate;

        cmbState.SelectedIndex =
            equipment.StateId - 1;

        if (equipment.UserId != null)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Id ==
                    equipment.UserId)
                {
                    cmbUser.SelectedIndex =
                        i;

                    break;
                }
            }
        }
    }

    private void LoadUsers()
    {
        RequestRepository repository =
            new();

        users =
            repository.GetUsers();

        cmbUser.Items.Clear();

        cmbUser.Items.Add(
            "Не назначен");

        foreach (var user in users)
        {
            cmbUser.Items.Add(
                user.FullName);
        }

        cmbUser.SelectedIndex = 0;
    }

    private void LoadStates()
    {
        cmbState.Items.Clear();

        cmbState.Items.Add(
            "Исправно");

        cmbState.Items.Add(
            "Требует ремонта");

        cmbState.Items.Add(
            "На ремонте");

        cmbState.Items.Add(
            "Списано");

        cmbState.SelectedIndex = 0;
    }
    private void btnSave_Click(
    object sender,
    RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
            txtName.Text))
        {
            MessageBox.Show(
                "Введите наименование оборудования");

            return;
        }

        if (string.IsNullOrWhiteSpace(
            txtRoom.Text))
        {
            MessageBox.Show(
                "Введите кабинет");

            return;
        }

        if (dpCommissioningDate.SelectedDate == null)
        {
            MessageBox.Show(
                "Укажите дату ввода в эксплуатацию");

            return;
        }

        EquipmentModel item =
            equipment ?? new EquipmentModel();

        item.InventoryNumber =
            txtInventory.Text.Trim();

        item.EquipmentName =
            txtName.Text.Trim();

        item.Model =
            txtModel.Text.Trim();

        item.SerialNumber =
            txtSerial.Text.Trim();

        item.Manufacturer =
            txtManufacturer.Text.Trim();

        item.RoomNumber =
            txtRoom.Text.Trim();

        item.CommissioningDate =
            dpCommissioningDate
            .SelectedDate!.Value;

        item.StateId =
            cmbState.SelectedIndex + 1;

        item.Notes =
            txtNotes.Text.Trim();

        if (cmbUser.SelectedIndex <= 0)
        {
            item.UserId = null;
        }
        else
        {
            item.UserId =
                users[
                    cmbUser.SelectedIndex - 1]
                .Id;
        }

        if (equipment == null)
        {
            repository.Add(item);

            MessageBox.Show(
                "Оборудование успешно добавлено");
        }
        else
        {
            repository.Update(item);

            MessageBox.Show(
                "Оборудование успешно сохранено");
        }

        DialogResult = true;

        Close();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}