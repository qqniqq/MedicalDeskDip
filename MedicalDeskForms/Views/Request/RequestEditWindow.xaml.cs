using System.Windows;
using MedicalDeskLib.Security;
using MedicalDeskLib.Services;

namespace MedicalDeskForms.Views.Requests;

public partial class RequestEditWindow : Window
{
    private readonly RequestService service =
        new();

    public RequestEditWindow()
    {
        InitializeComponent();

        LoadTypes();

        ConfigureRole();
    }

    private void ConfigureRole()
    {
        if (SessionManager.CurrentUser == null)
            return;

        if (SessionManager.CurrentUser.RoleName ==
            "Пользователь")
        {
            panelApplicant.Visibility =
                Visibility.Collapsed;
        }
    }

    private void LoadTypes()
    {
        cmbType.Items.Add("Компьютер");
        cmbType.Items.Add("Ноутбук");
        cmbType.Items.Add("Принтер");
        cmbType.Items.Add("МФУ");
        cmbType.Items.Add("Интернет");
        cmbType.Items.Add("Локальная сеть");
        cmbType.Items.Add("Программное обеспечение");
        cmbType.Items.Add("Телефон");
        cmbType.Items.Add("Электронная почта");
        cmbType.Items.Add("Другое");

        cmbType.SelectedIndex = 0;
    }

    private void btnSave_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
            txtRoom.Text))
        {
            MessageBox.Show(
                "Укажите кабинет");

            return;
        }

        if (string.IsNullOrWhiteSpace(
            txtDescription.Text))
        {
            MessageBox.Show(
                "Введите описание проблемы");

            return;
        }

        if (SessionManager.CurrentUser!
            .RoleName == "Пользователь")
        {
            service.CreateRequestByUser(
                txtRoom.Text,
                cmbType.SelectedIndex + 1,
                txtDescription.Text);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(
                txtApplicant.Text))
            {
                MessageBox.Show(
                    "Введите ФИО заявителя");

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtPhone.Text))
            {
                MessageBox.Show(
                    "Введите телефон");

                return;
            }

            service.CreateRequestByOperator(
                txtRoom.Text,
                txtApplicant.Text,
                txtPhone.Text,
                cmbType.SelectedIndex + 1,
                txtDescription.Text);
        }

        MessageBox.Show(
            "Заявка успешно создана");

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