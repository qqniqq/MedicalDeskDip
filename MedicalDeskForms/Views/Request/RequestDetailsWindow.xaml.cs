using System.Windows;
using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;
using MedicalDeskLib.Services;
using MedicalDeskForms.Views.Materials;

namespace MedicalDeskForms.Views.Requests;

public partial class RequestDetailsWindow : Window
{
    private readonly Request request;

    private readonly RequestService service =
        new();

    private readonly RequestHistoryRepository
        historyRepository = new();

    public RequestDetailsWindow(
        Request request)
    {
        InitializeComponent();

        this.request = request;

        LoadRequest();

        LoadHistory();

        ConfigureButtons();
    }

    private void LoadRequest()
    {
        txtNumber.Text =
            request.RequestNumber;

        txtRoom.Text =
            request.RoomNumber;

        txtApplicant.Text =
            request.ApplicantName;

        txtPhone.Text =
            request.ApplicantPhone;

        txtType.Text =
            request.RequestTypeName;

        txtStatus.Text =
            request.StatusName;
        txtExecutor.Text =
    request.ExecutorName;

        txtCreated.Text =
            request.CreatedAt
            .ToString("dd.MM.yyyy HH:mm");

        txtAccepted.Text =
            request.AcceptedAt == null
            ? "-"
            : request.AcceptedAt.Value
                .ToString("dd.MM.yyyy HH:mm");

        txtCompleted.Text =
            request.CompletedAt == null
            ? "-"
            : request.CompletedAt.Value
                .ToString("dd.MM.yyyy HH:mm");

        txtDescription.Text =
            request.ProblemDescription;
    }

    private void LoadHistory()
    {
        gridHistory.ItemsSource =
            historyRepository.GetByRequest(
                request.Id);
    }

    private void ConfigureButtons()
    {
        string role =
            SessionManager
            .CurrentUser!
            .RoleName;

        if (role == "Пользователь")
        {
            btnTake.Visibility =
                Visibility.Collapsed;

            btnComplete.Visibility =
                Visibility.Collapsed;

            btnCancel.Visibility =
                Visibility.Collapsed;

            txtComment.IsReadOnly =
                true;
        }
        btnWriteOff.Visibility =
    Visibility.Collapsed;

        if (role == "Специалист")
        {
            btnCancel.Visibility =
                Visibility.Collapsed;
        }

        if (role == "Администратор")
        {
            txtComment.IsReadOnly =
                true;
        }
    }

    private void btnTake_Click(
        object sender,
        RoutedEventArgs e)
    {
        service.TakeToWork(
            request.Id);

        MessageBox.Show(
            "Заявка принята");

        Close();
    }

    private void btnComplete_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
            txtComment.Text))
        {
            MessageBox.Show(
                "Введите комментарий");

            return;
        }

        service.CompleteRequest(
            request.Id,
            txtComment.Text);

        MessageBox.Show(
            "Заявка завершена");

        Close();
    }
    private void btnWriteOff_Click(
    object sender,
    RoutedEventArgs e)
    {
        MaterialWriteOffWindow window =
            new(request.Id);

        window.ShowDialog();
    }
    private void btnCancel_Click(
        object sender,
        RoutedEventArgs e)
    {
        service.CancelRequest(
            request.Id);

        MessageBox.Show(
            "Заявка отменена");

        Close();
    }

    private void btnBack_Click(
        object sender,
        RoutedEventArgs e)
    {
        Close();
    }
}