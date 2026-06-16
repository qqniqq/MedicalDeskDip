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
    private readonly NotificationRepository
        notificationRepository = new();

    private readonly RequestRepository
        requestRepository = new();

    public MainWindow()
    {
        InitializeComponent();

        LoadUserInfo();

        ConfigurePermissions();

        BindEvents();

        LoadDashboard();
    }

    private void BindEvents()
    {
        btnRequests.Click +=
            btnRequests_Click;

        btnEquipment.Click +=
            btnEquipment_Click;

        btnMaterials.Click +=
            btnMaterials_Click;

        btnUsers.Click +=
            btnUsers_Click;

        btnNotifications.Click +=
            btnNotifications_Click;

        btnReports.Click +=
            btnReports_Click;
    }

    private void LoadDashboard()
    {
        if (SessionManager.CurrentUser == null)
            return;

        string role =
            SessionManager.CurrentUser.RoleName;

        if (role == "Администратор")
        {
            var requests =
                requestRepository.GetAll();

            txtMyActive.Text =
                requests.Count.ToString();

            txtNew.Text =
                requests
                .Count(x =>
                    x.StatusName == "Новая")
                .ToString();

            txtWork.Text =
                requests
                .Count(x =>
                    x.StatusName == "В работе")
                .ToString();

            txtNotifications.Text =
                notificationRepository
                .GetAll()
                .Count(x =>
                    !x.IsRead)
                .ToString();
        }
        else if (role == "Специалист")
        {
            var requests =
                requestRepository.GetAll();

            txtMyActive.Text =
                requests
                .Count(x =>
                    x.StatusName == "Новая")
                .ToString();

            txtNew.Text =
                requests
                .Count(x =>
                    x.StatusName == "Новая")
                .ToString();

            txtWork.Text =
                requests
                .Count(x =>
                    x.StatusName == "В работе")
                .ToString();

            txtNotifications.Text =
                notificationRepository
                .GetUnreadCount(
                    SessionManager.CurrentUser.Id)
                .ToString();
        }
        else
        {
            var requests =
                requestRepository.GetMyRequests(
                    SessionManager.CurrentUser.Id);

            txtMyActive.Text =
                requests
                .Count(x =>
                    x.StatusName == "Новая"
                    ||
                    x.StatusName == "В работе")
                .ToString();

            txtNew.Text =
                requests
                .Count(x =>
                    x.StatusName == "Новая")
                .ToString();

            txtWork.Text =
                requests
                .Count(x =>
                    x.StatusName == "В работе")
                .ToString();

            txtNotifications.Text =
                notificationRepository
                .GetUnreadCount(
                    SessionManager.CurrentUser.Id)
                .ToString();
        }
    }

    private void LoadUserInfo()
    {
        if (SessionManager.CurrentUser == null)
            return;

        txtCurrentUser.Text =
            $"{SessionManager.CurrentUser.FullName} ({SessionManager.CurrentUser.RoleName})";

        int count =
            notificationRepository
            .GetUnreadCount(
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

    private void btnReports_Click(
        object? sender,
        RoutedEventArgs e)
    {
        ReportsWindow window =
            new();

        window.ShowDialog();
    }

    private void btnRequests_Click(
        object? sender,
        RoutedEventArgs e)
    {
        RequestListWindow window =
            new();

        window.ShowDialog();

        LoadDashboard();
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

        LoadUserInfo();

        LoadDashboard();
    }

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