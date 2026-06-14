using System.Windows;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Services;

namespace MedicalDeskForms.Views.Materials;

public partial class MaterialWriteOffWindow : Window
{
    private readonly MaterialRepository
        repository = new();

    private readonly MaterialService
        service = new();

    private readonly int requestId;

    public MaterialWriteOffWindow(
        int requestId)
    {
        InitializeComponent();

        this.requestId =
            requestId;

        LoadMaterials();
    }

    private void LoadMaterials()
    {
        cmbMaterial.ItemsSource =
            repository.GetAll();

        cmbMaterial.DisplayMemberPath =
            "Name";

        cmbMaterial.SelectedValuePath =
            "Id";
    }

    private void btnSave_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (cmbMaterial.SelectedItem == null)
            return;

        if (!int.TryParse(
            txtQuantity.Text,
            out int quantity))
        {
            MessageBox.Show(
                "Введите количество");

            return;
        }

        Material material =
            (Material)
            cmbMaterial.SelectedItem;

        if (quantity > material.Quantity)
        {
            MessageBox.Show(
                "Недостаточно расходников на складе");

            return;
        }

        service.WriteOff(
            material.Id,
            requestId,
            quantity,
            txtReason.Text);

        MessageBox.Show(
            "Списание выполнено");

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