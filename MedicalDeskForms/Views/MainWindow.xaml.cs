using System.Windows;
using MedicalDeskLib.Security;
using MedicalDeskForms.Views.Requests;
using MedicalDeskForms.Views.Equipment;
using MedicalDeskForms.Views.Users;
using MedicalDeskForms.Views.Materials;
using MedicalDeskLib.Repositories;
using MedicalDeskForms.Views.Reports;

namespace MedicalDeskForms.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        LoadUserInfo();

        ConfigurePermissions();

        BindEvents();
    }

    private void BindEvents()
    {
        btnDashboard_Click(
    null,
    null!);
        btnRequests.Click += btnRequests_Click;
        btnEquipment.Click += btnEquipment_Click;
        btnMaterials.Click += btnMaterials_Click;
        btnUsers.Click += btnUsers_Click;
        btnNotifications.Click += btnNotifications_Click;
        btnDashboard.Click += btnDashboard_Click;
        btnReports.Click += btnReports_Click;
    }
    private void btnReports_Click(
    object? sender,
    RoutedEventArgs e)
    {
        ReportsWindow window =
            new();

        window.ShowDialog();
    }

    private void btnDashboard_Click(
    object? sender,
    RoutedEventArgs e)
    {
        DashboardWindow window =
            new();

        window.ShowDialog();
    }
    private void LoadUserInfo()
    {
        if (SessionManager.CurrentUser == null)
            return;

        txtCurrentUser.Text =
            $"{SessionManager.CurrentUser.FullName} ({SessionManager.CurrentUser.RoleName})";

        int count =
            notificationRepository.GetUnreadCount(
                SessionManager.CurrentUser.Id);

        btnNotifications.Content =
            $"Уведомления ({count})";
    }

    private void ConfigurePermissions()
    {
        if (SessionManager.CurrentUser == null)
            return;

        string role =
            SessionManager
            .CurrentUser
            .RoleName;

        switch (role)
        {
            case "Администратор":
                break;

            case "Специалист":

                btnUsers.Visibility =
                    Visibility.Collapsed;

                btnSettings.Visibility =
                    Visibility.Collapsed;

                break;

            case "Пользователь":

                btnUsers.Visibility =
                    Visibility.Collapsed;

                btnReports.Visibility =
                    Visibility.Collapsed;

                btnSettings.Visibility =
                    Visibility.Collapsed;

                btnEquipment.Visibility =
                    Visibility.Collapsed;

                btnNotifications.Visibility =
                    Visibility.Collapsed;

                btnMaterials.Visibility =
                    Visibility.Collapsed;

                break;
        }
    }

    private void btnRequests_Click(
        object? sender,
        RoutedEventArgs e)
    {
        RequestListWindow window =
            new();

        window.ShowDialog();
    }

    private void btnEquipment_Click(
        object? sender,
        RoutedEventArgs e)
    {
        EquipmentListWindow window =
            new();

        window.ShowDialog();
    }

    private void btnMaterials_Click(
        object? sender,
        RoutedEventArgs e)
    {
        MaterialListWindow window =
            new();

        window.ShowDialog();
    }

    private void btnUsers_Click(
        object? sender,
        RoutedEventArgs e)
    {
        UserListWindow window =
            new();

        window.ShowDialog();
    }

    private void btnNotifications_Click(
        object? sender,
        RoutedEventArgs e)
    {
        NotificationWindow window =
            new();

        window.ShowDialog();
    }
    private readonly NotificationRepository
    notificationRepository = new();
    private void btnExit_Click(
        object sender,
        RoutedEventArgs e)
    {
        SessionManager.Logout();

        LoginWindow login =
            new();

        login.Show();

        Close();
    }

}