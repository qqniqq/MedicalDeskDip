using System.Windows;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;
using MedicalDeskLib.Services;

namespace MedicalDeskForms.Views.Requests;

public partial class RequestEditWindow : Window
{
    private readonly RequestService service =
        new();

    private readonly UserRepository
        userRepository =
            new();

    public RequestEditWindow()
    {
        InitializeComponent();

        LoadTypes();

        LoadUsers();

        ConfigureRoleMode();
    }

    private void ConfigureRoleMode()
    {
        if (SessionManager.CurrentUser == null)
            return;

        if (SessionManager.CurrentUser.RoleName ==
            "Пользователь")
        {
            panelUser.Visibility =
                Visibility.Collapsed;
        }
    }

    private void LoadUsers()
    {
        cmbUser.ItemsSource =
            userRepository.GetAll();

        cmbUser.DisplayMemberPath =
            "FullName";

        cmbUser.SelectedValuePath =
            "Id";

        if (cmbUser.Items.Count > 0)
        {
            cmbUser.SelectedIndex = 0;
        }
    }

    private void cmbUser_SelectionChanged(
        object sender,
        System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (cmbUser.SelectedItem == null)
            return;

        User user =
            (User)cmbUser.SelectedItem;

        txtPhone.Text =
            user.Phone;
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
            if (cmbUser.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите пользователя");

                return;
            }

            User user =
                (User)cmbUser.SelectedItem;

            service.CreateRequestForSelectedUser(
                user.Id,
                txtRoom.Text,
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