using System.Windows;
using EquipmentModel = MedicalDeskLib.Models.Equipment;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Equipment;

public partial class EquipmentListWindow : Window
{
    private readonly EquipmentRepository
        repository = new();

    public EquipmentListWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        gridEquipment.ItemsSource =
            repository.GetAll();
    }

    private EquipmentModel? Selected()
    {
        return gridEquipment.SelectedItem
            as EquipmentModel;
    }

    private void txtSearch_TextChanged(
        object sender,
        System.Windows.Controls.TextChangedEventArgs e)
    {
        string text =
            txtSearch.Text.Trim();

        if (string.IsNullOrWhiteSpace(text))
        {
            LoadData();
            return;
        }

        gridEquipment.ItemsSource =
            repository
            .GetAll()
            .Where(x =>
                x.InventoryNumber.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.EquipmentName.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.RoomNumber.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.Manufacturer.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase)
                ||
                x.Model.Contains(
                    text,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private void btnAdd_Click(
        object sender,
        RoutedEventArgs e)
    {
        EquipmentEditWindow window =
            new();

        window.ShowDialog();

        LoadData();
    }

    private void btnEdit_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (Selected() == null)
        {
            MessageBox.Show(
                "Выберите оборудование");

            return;
        }

        EquipmentEditWindow window =
            new(
                Selected()!);

        window.ShowDialog();

        LoadData();
    }

    private void btnHistory_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (Selected() == null)
        {
            MessageBox.Show(
                "Выберите оборудование");

            return;
        }

        EquipmentHistoryWindow window =
            new(
                Selected()!.Id);

        window.ShowDialog();
    }

    private void btnDelete_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (Selected() == null)
        {
            MessageBox.Show(
                "Выберите оборудование");

            return;
        }

        if (MessageBox.Show(
            "Удалить выбранное оборудование?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question)
            != MessageBoxResult.Yes)
        {
            return;
        }

        repository.Delete(
            Selected()!.Id);

        MessageBox.Show(
            "Оборудование удалено");

        LoadData();
    }

    private void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
    {
        txtSearch.Clear();

        LoadData();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}