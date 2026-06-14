using System.Windows;
using EquipmentModel = MedicalDeskLib.Models.Equipment;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Equipment;

public partial class EquipmentListWindow : Window
{
    private readonly EquipmentRepository repository =
        new();

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

    private void btnAdd_Click(
        object sender,
        RoutedEventArgs e)
    {
        new EquipmentEditWindow()
            .ShowDialog();

        LoadData();
    }

    private void btnEdit_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (Selected() == null)
            return;

        new EquipmentEditWindow(
            Selected()!)
            .ShowDialog();

        LoadData();
    }

    private void btnHistory_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (Selected() == null)
            return;

        new EquipmentHistoryWindow(
            Selected()!.Id)
            .ShowDialog();
    }

    private void btnDelete_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (Selected() == null)
            return;

        repository.Delete(
            Selected()!.Id);

        LoadData();
    }
}