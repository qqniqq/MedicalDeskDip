using System.Windows;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Materials;

public partial class MaterialListWindow : Window
{
    private readonly MaterialRepository repository =
        new();

    public MaterialListWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private Material? SelectedMaterial =>
        gridMaterials.SelectedItem
        as Material;

    private void LoadData()
    {
        gridMaterials.ItemsSource =
            repository.GetAll();
    }

    private void btnAdd_Click(
        object sender,
        RoutedEventArgs e)
    {
        MaterialEditWindow form =
            new();

        form.ShowDialog();

        LoadData();
    }

    private void btnEdit_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (SelectedMaterial == null)
            return;

        MaterialEditWindow form =
            new(SelectedMaterial);

        form.ShowDialog();

        LoadData();
    }

    private void btnDelete_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (SelectedMaterial == null)
            return;

        if (MessageBox.Show(
            "Удалить материал?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question)
            != MessageBoxResult.Yes)
            return;

        repository.Delete(
            SelectedMaterial.Id);

        LoadData();
    }

    private void btnIncome_Click(
     object sender,
     RoutedEventArgs e)
    {
        if (SelectedMaterial == null)
            return;

        MaterialIncomeWindow window =
            new(SelectedMaterial);

        window.ShowDialog();

        LoadData();
    }

    private void btnHistory_Click(
        object sender,
        RoutedEventArgs e)
    {
        MaterialHistoryWindow window =
            new();

        window.ShowDialog();
    }

    private void btnLowStock_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridMaterials.ItemsSource =
            repository.GetLowStock();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
    private void btnAll_Click(
    object sender,
    RoutedEventArgs e)
    {
        LoadData();
    }

}