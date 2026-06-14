using System.Windows;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;

namespace MedicalDeskForms.Views.Materials;

public partial class MaterialIncomeWindow : Window
{
    private readonly Material material;

    private readonly MaterialRepository repository =
        new();

    public MaterialIncomeWindow(
        Material material)
    {
        InitializeComponent();

        this.material =
            material;
    }

    private void btnSave_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (!int.TryParse(
            txtQuantity.Text,
            out int quantity))
        {
            MessageBox.Show(
                "Введите количество");

            return;
        }

        if (quantity <= 0)
        {
            MessageBox.Show(
                "Количество должно быть больше нуля");

            return;
        }

        repository.AddQuantity(
            material.Id,
            quantity);
        MaterialHistoryRepository history =
    new();

        history.AddIncome(
            material.Id,
            quantity,
            txtComment.Text);

        MessageBox.Show(
            "Склад пополнен");

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