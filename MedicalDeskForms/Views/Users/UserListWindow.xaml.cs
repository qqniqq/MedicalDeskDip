using System.Windows;
using System.Windows.Controls;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;
using MedicalDeskLib.Services;

namespace MedicalDeskForms.Views.Users;

public partial class UserListWindow : Window
{
    private readonly UserRepository repository =
        new();

    public UserListWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        gridUsers.ItemsSource =
            repository.GetAll();

        UpdateBlockButton();
    }

    private User? SelectedUser()
    {
        return gridUsers.SelectedItem as User;
    }

    private void UpdateBlockButton()
    {
        User? user =
            SelectedUser();

        if (user == null)
        {
            btnBlock.Content =
                "Блокировать";

            return;
        }

        btnBlock.Content =
            user.IsActive
                ? "Блокировать"
                : "Разблокировать";
    }

    private void gridUsers_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        UpdateBlockButton();
    }

    private void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
    {
        LoadData();
    }

    private void btnAdd_Click(
        object sender,
        RoutedEventArgs e)
    {
        UserEditWindow window =
            new();

        window.ShowDialog();

        LoadData();
    }

    private void btnEdit_Click(
        object sender,
        RoutedEventArgs e)
    {
        User? user =
            SelectedUser();

        if (user == null)
            return;

        UserEditWindow window =
            new(user);

        window.ShowDialog();

        LoadData();
    }

    private void btnDelete_Click(
        object sender,
        RoutedEventArgs e)
    {
        User? user =
            SelectedUser();

        if (user == null)
            return;

        if (user.Login.ToLower() == "admin")
        {
            MessageBox.Show(
                "Нельзя удалить администратора.");

            return;
        }

        if (SessionManager.CurrentUser != null &&
            user.Id == SessionManager.CurrentUser.Id)
        {
            MessageBox.Show(
                "Нельзя удалить самого себя.");

            return;
        }

        MessageBoxResult result =
            MessageBox.Show(
                $"Удалить пользователя {user.FullName} ?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        repository.Delete(user.Id);

        AuditService.Log(
            SessionManager.CurrentUser?.Id,
            "Удаление пользователя",
            $"Удален пользователь {user.FullName}");

        LoadData();
    }

    private void btnBlock_Click(
        object sender,
        RoutedEventArgs e)
    {
        User? user =
            SelectedUser();

        if (user == null)
            return;

        if (SessionManager.CurrentUser != null &&
            user.Id == SessionManager.CurrentUser.Id)
        {
            MessageBox.Show(
                "Нельзя блокировать самого себя.");

            return;
        }

        repository.ToggleBlock(
            user.Id,
            !user.IsActive);

        AuditService.Log(
            SessionManager.CurrentUser?.Id,
            "Блокировка пользователя",
            user.FullName);

        LoadData();
    }

    private void btnResetPassword_Click(
        object sender,
        RoutedEventArgs e)
    {
        User? user =
            SelectedUser();

        if (user == null)
            return;

        repository.ResetPassword(
            user.Id,
            "123456");

        MessageBox.Show(
            "Пароль сброшен на 123456");

        AuditService.Log(
            SessionManager.CurrentUser?.Id,
            "Сброс пароля",
            user.FullName);
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}