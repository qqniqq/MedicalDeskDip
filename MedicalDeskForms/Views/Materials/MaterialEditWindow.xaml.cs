using System.Windows;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;
using MedicalDeskLib.Services;

namespace MedicalDeskForms.Views.Materials;

public partial class MaterialEditWindow : Window
{
    private readonly MaterialRepository repository =
        new();

    private Material? material;

    public MaterialEditWindow()
    {
        InitializeComponent();

        LoadCategories();

        dpReceiptDate.SelectedDate =
            DateTime.Today;
    }

    public MaterialEditWindow(
        Material material)
        : this()
    {
        this.material = material;

        txtName.Text =
            material.Name;

        txtQuantity.Text =
            material.Quantity.ToString();

        txtMinimum.Text =
            material.MinimumQuantity.ToString();

        txtNotes.Text =
            material.Notes;

        dpReceiptDate.SelectedDate =
            material.ReceiptDate;

        for (int i = 0; i < categories.Count; i++)
        {
            if (categories[i].Id ==
                material.CategoryId)
            {
                cmbCategory.SelectedIndex =
                    i;

                break;
            }
        }
    }

    private void LoadCategories()
    {
        categories =
            repository.GetCategories();

        cmbCategory.Items.Clear();

        foreach (var category in categories)
        {
            cmbCategory.Items.Add(
                category.Name);
        }

        if (cmbCategory.Items.Count > 0)
        {
            cmbCategory.SelectedIndex = 0;
        }
    }

    private void btnSave_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
            txtName.Text))
        {
            MessageBox.Show(
                "Введите название материала");

            return;
        }

        if (!int.TryParse(
            txtQuantity.Text,
            out int quantity))
        {
            MessageBox.Show(
                "Количество указано неверно");

            return;
        }

        if (!int.TryParse(
            txtMinimum.Text,
            out int minimum))
        {
            MessageBox.Show(
                "Минимальный остаток указан неверно");

            return;
        }

        Material item =
            material ?? new Material();

        item.Name =
            txtName.Text.Trim();

        item.CategoryId =
            categories[
                cmbCategory.SelectedIndex]
            .Id;

        item.Quantity =
            quantity;

        item.MinimumQuantity =
            minimum;

        item.ReceiptDate =
            dpReceiptDate.SelectedDate ??
            DateTime.Today;

        item.Notes =
            txtNotes.Text.Trim();

        if (material == null)
        {
            repository.Add(item);

            AuditService.Log(
                SessionManager.CurrentUser?.Id,
                "Добавление расходного материала",
                item.Name);
        }
        else
        {
            repository.Update(item);

            AuditService.Log(
                SessionManager.CurrentUser?.Id,
                "Редактирование расходного материала",
                item.Name);
        }

        DialogResult = true;

        Close();
    }
    private List<MaterialCategory>
    categories = new();
    private void btnCancel_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}