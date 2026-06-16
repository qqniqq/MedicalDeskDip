using System.Windows;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;

namespace MedicalDeskForms.Views;

public partial class NotificationWindow : Window
{
    private readonly NotificationRepository repository =
        new();

    public NotificationWindow()
    {
        InitializeComponent();

        LoadData();
    }

    private void LoadData()
    {
        if (SessionManager.CurrentUser == null)
            return;

        List<Notification> notifications;

        if (SessionManager.CurrentUser.RoleName ==
            "Администратор")
        {
            notifications =
                repository.GetAll();
        }
        else
        {
            notifications =
                repository.GetForUser(
                    SessionManager.CurrentUser.Id);
        }

        gridNotifications.ItemsSource =
            notifications;

        int unreadCount =
            notifications.Count(
                x => !x.IsRead);

        txtInfo.Text =
            $"Всего уведомлений: {notifications.Count} | Непрочитанных: {unreadCount}";
    }

    private void btnRead_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (gridNotifications.SelectedItem == null)
        {
            MessageBox.Show(
                "Выберите уведомление",
                "Уведомления",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            return;
        }

        Notification item =
            (Notification)
            gridNotifications.SelectedItem;

        repository.MarkAsRead(
            item.Id);

        LoadData();
    }

    private void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
    {
        LoadData();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }

    private void gridNotifications_MouseDoubleClick(
        object sender,
        System.Windows.Input.MouseButtonEventArgs e)
    {
        if (gridNotifications.SelectedItem == null)
            return;

        Notification item =
            (Notification)
            gridNotifications.SelectedItem;

        repository.MarkAsRead(
            item.Id);

        LoadData();
    }
}