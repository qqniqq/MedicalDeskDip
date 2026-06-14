using System.Windows;
using MedicalDeskLib.Helpers;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;
using MedicalDeskLib.Services;
using System.Windows.Input;

namespace MedicalDeskForms.Views.Users;

public partial class UserEditWindow : Window
{
    private readonly UserRepository repository =
        new();

    private User? currentUser;

    public UserEditWindow()
    {
        InitializeComponent();

        LoadRoles();

        txtPhone.Text = "+";
    }

    public UserEditWindow(
     User user)
     : this()
    {
        currentUser = user;

        txtFullName.Text =
            user.FullName;

        txtPhone.Text =
            user.Phone;

        txtLogin.Text =
            user.Login;

        chkIsActive.IsChecked =
            user.IsActive;

        cmbRole.SelectedIndex =
            user.RoleId - 1;

        txtPassword.Password =
            "";

        txtPasswordConfirm.Password =
            "";
    }

    private void LoadRoles()
    {
        cmbRole.Items.Add("Администратор");
        cmbRole.Items.Add("Специалист");
        cmbRole.Items.Add("Пользователь");

        cmbRole.SelectedIndex = 0;
    }

    private void txtPhone_TextChanged(
        object sender,
        System.Windows.Controls.TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(txtPhone.Text))
        {
            txtPhone.Text = "+";
            txtPhone.SelectionStart =
                txtPhone.Text.Length;
        }
    }

    private void btnGenerateLogin_Click(
        object sender,
        RoutedEventArgs e)
    {
        txtLogin.Text =
            LoginGenerator.Generate(
                txtFullName.Text);
    }

    private bool ValidateData()
    {
        if (!ValidationHelper.IsValidFullName(
            txtFullName.Text))
        {
            MessageBox.Show(
                "Некорректное ФИО");

            return false;
        }

        if (!ValidationHelper.IsValidPhone(
            txtPhone.Text))
        {
            MessageBox.Show(
                "Некорректный телефон");

            return false;
        }

        if (!ValidationHelper.IsValidLogin(
            txtLogin.Text))
        {
            MessageBox.Show(
                "Некорректный логин");

            return false;
        }

        if (!ValidationHelper.IsValidPassword(
            txtPassword.Password))
        {
            MessageBox.Show(
                "Пароль должен содержать минимум 6 символов");

            return false;
        }

        if (txtPassword.Password !=
            txtPasswordConfirm.Password)
        {
            MessageBox.Show(
                "Пароли не совпадают");

            return false;
        }

        User? loginOwner =
            repository.GetByLogin(
                txtLogin.Text);

        if (loginOwner != null)
        {
            if (currentUser == null ||
                loginOwner.Id != currentUser.Id)
            {
                MessageBox.Show(
                    "Такой логин уже существует");

                return false;
            }
        }

        return true;
    }

    private void btnSave_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (!ValidateData())
            return;

        User user =
            currentUser ?? new User();

        user.FullName =
            txtFullName.Text.Trim();

        user.Phone =
            txtPhone.Text.Trim();

        user.Login =
            txtLogin.Text.Trim();

        user.PasswordHash =
            txtPassword.Password;

        user.RoleId =
            cmbRole.SelectedIndex + 1;

        user.IsActive =
            chkIsActive.IsChecked ?? true;

        if (currentUser == null)
        {
            repository.Add(user);

            AuditService.Log(
                SessionManager.CurrentUser?.Id,
                "Создание пользователя",
                user.FullName);
        }
        else
        {
            repository.Update(user);

            AuditService.Log(
                SessionManager.CurrentUser?.Id,
                "Редактирование пользователя",
                user.FullName);
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
    private void txtFullName_PreviewTextInput(
    object sender,
    TextCompositionEventArgs e)
    {
        InputValidation.OnlyLetters(
            sender,
            e);
    }
    private void txtPhone_PreviewTextInput(
    object sender,
    TextCompositionEventArgs e)
    {
        InputValidation.PhoneInput(
            sender,
            e);
    }
    private void txtLogin_PreviewTextInput(
    object sender,
    TextCompositionEventArgs e)
    {
        InputValidation.LoginInput(
            sender,
            e);
    }
}