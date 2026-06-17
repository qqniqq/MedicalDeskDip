using System.Windows;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;
using MedicalDeskLib.Models;

namespace MedicalDeskForms.Views.Requests;

public partial class RequestListWindow : Window
{
    private readonly RequestRepository repository =
        new();

    public RequestListWindow()
    {
        InitializeComponent();

        LoadStatuses();

        ConfigurePermissions();

        LoadData();
    }

    private void ConfigurePermissions()
    {
        if (SessionManager.CurrentUser == null)
            return;

        string role =
            SessionManager.CurrentUser.RoleName;

        if (role == "Пользователь")
        {
            btnAll.Visibility =
                Visibility.Collapsed;

            btnNew.Visibility =
                Visibility.Collapsed;

            btnWork.Visibility =
                Visibility.Collapsed;

            btnCompleted.Visibility =
                Visibility.Collapsed;

            btnCanceled.Visibility =
                Visibility.Collapsed;

            cmbStatus.Visibility =
                Visibility.Collapsed;


        }
    }

    private void LoadStatuses()
    {
        cmbStatus.Items.Add("Все");
        cmbStatus.Items.Add("Новая");
        cmbStatus.Items.Add("В работе");
        cmbStatus.Items.Add("Завершена");
        cmbStatus.Items.Add("Отменена");

        cmbStatus.SelectedIndex = 0;
    }

    private void LoadData()
    {
        if (SessionManager.CurrentUser == null)
            return;

        if (SessionManager.CurrentUser.RoleName ==
            "Пользователь")
        {
            gridRequests.ItemsSource =
                repository.GetMyRequests(
                    SessionManager.CurrentUser.Id);
        }
        else
        {
            gridRequests.ItemsSource =
                repository.GetAll();
        }
    }

    private void txtSearch_TextChanged(
        object sender,
        System.Windows.Controls.TextChangedEventArgs e)
    {
        if (SessionManager.CurrentUser == null)
            return;

        string search =
            txtSearch.Text.Trim();

        if (SessionManager.CurrentUser.RoleName ==
            "Пользователь")
        {
            gridRequests.ItemsSource =
                repository
                .GetMyRequests(
                    SessionManager.CurrentUser.Id)
                .Where(x =>
                    x.RequestNumber.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    x.RoomNumber.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    x.ApplicantName.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

            return;
        }

        gridRequests.ItemsSource =
            repository.Search(search);
    }

    private void cmbStatus_SelectionChanged(
        object sender,
        System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (cmbStatus.SelectedItem == null)
            return;

        string status =
            cmbStatus.SelectedItem.ToString()!;

        if (status == "Все")
        {
            LoadData();
            return;
        }

        gridRequests.ItemsSource =
            repository.GetByStatus(
                status);
    }

    private void btnRefresh_Click(
        object sender,
        RoutedEventArgs e)
    {
        txtSearch.Clear();

        cmbStatus.SelectedIndex = 0;

        LoadData();
    }

    private void btnCreate_Click(
        object sender,
        RoutedEventArgs e)
    {
        RequestEditWindow form =
            new();

        bool? result =
            form.ShowDialog();

        if (result == true)
        {
            LoadData();
        }
    }

    private void gridRequests_MouseDoubleClick(
        object sender,
        System.Windows.Input.MouseButtonEventArgs e)
    {
        if (gridRequests.SelectedItem == null)
            return;

        Request request =
            (Request)
            gridRequests.SelectedItem;

        RequestDetailsWindow details =
            new(request);

        details.ShowDialog();

        LoadData();
    }

    private void btnAll_Click(
        object sender,
        RoutedEventArgs e)
    {
        LoadData();
    }

    private void btnNew_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridRequests.ItemsSource =
            repository.GetByStatus(
                "Новая");
    }

    private void btnWork_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridRequests.ItemsSource =
            repository.GetByStatus(
                "В работе");
    }

    private void btnCompleted_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridRequests.ItemsSource =
            repository.GetByStatus(
                "Завершена");
    }

    private void btnCanceled_Click(
        object sender,
        RoutedEventArgs e)
    {
        gridRequests.ItemsSource =
            repository.GetByStatus(
                "Отменена");
    }

  
    private void btnDelete_Click(
    object sender,
    RoutedEventArgs e)
    {
        if (gridRequests.SelectedItem == null)
        {
            MessageBox.Show(
                "Выберите заявку");

            return;
        }

        Request request =
            (Request)
            gridRequests.SelectedItem;

        string role =
            SessionManager
            .CurrentUser!
            .RoleName;

        if (role == "Пользователь"
            &&
            request.StatusName != "Новая")
        {
            MessageBox.Show(
                "Можно удалить только новую заявку");

            return;
        }

        if (role == "Специалист")
        {
            MessageBox.Show(
                "Удаление заявок недоступно");

            return;
        }

        if (MessageBox.Show(
            $"Удалить заявку № {request.RequestNumber}?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question)
            != MessageBoxResult.Yes)
        {
            return;
        }

        repository.Delete(
            request.Id);

        MessageBox.Show(
            "Заявка удалена");

        LoadData();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}