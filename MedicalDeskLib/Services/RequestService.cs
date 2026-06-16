using MedicalDeskLib.Models;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Security;

namespace MedicalDeskLib.Services;

public class RequestService
{
    private readonly RequestRepository repository =
        new();

    private readonly RequestHistoryRepository
        historyRepository =
            new();

    public void CreateRequestByUser(
        string room,
        int typeId,
        string description)
    {
        repository.Create(
            new Request
            {
                RoomNumber = room,

                ApplicantName =
                    SessionManager.CurrentUser!.FullName,

                ApplicantPhone =
                    SessionManager.CurrentUser.Phone,

                RequestTypeId =
                    typeId,

                ProblemDescription =
                    description,

                AuthorId =
                    SessionManager.CurrentUser.Id
            });

        NotificationService.Create(
            "Заявка",
            "Создана новая заявка");

        AuditService.Log(
            SessionManager.CurrentUser.Id,
            "Создание заявки",
            description);
    }

    public void CreateRequestByOperator(
        string room,
        string applicant,
        string phone,
        int typeId,
        string description)
    {
        repository.Create(
            new Request
            {
                RoomNumber =
                    room,

                ApplicantName =
                    applicant,

                ApplicantPhone =
                    phone,

                RequestTypeId =
                    typeId,

                ProblemDescription =
                    description,

                AuthorId =
                    SessionManager.CurrentUser!.Id
            });

        NotificationService.Create(
            "Заявка",
            "Создана новая заявка");

        AuditService.Log(
            SessionManager.CurrentUser.Id,
            "Создание заявки",
            description);
    }

    public void TakeToWork(
        int requestId)
    {
        repository.TakeToWork(
            requestId,
            SessionManager.CurrentUser!.Id);

        historyRepository.Add(
            requestId,
            "В работе",
            "Заявка принята специалистом",
            SessionManager.CurrentUser.Id);

        NotificationService.Create(
            "Заявка",
            $"Заявка №{requestId} принята в работу");

        AuditService.Log(
            SessionManager.CurrentUser.Id,
            "Взятие заявки",
            $"Заявка №{requestId}");
    }

    public void CompleteRequest(
        int requestId,
        string comment)
    {
        repository.Complete(
            requestId,
            comment);

        historyRepository.Add(
            requestId,
            "Завершена",
            comment,
            SessionManager.CurrentUser!.Id);

        NotificationService.Create(
            "Заявка",
            $"Заявка №{requestId} завершена");

        AuditService.Log(
            SessionManager.CurrentUser.Id,
            "Завершение заявки",
            comment);
    }

    public void CancelRequest(
        int requestId)
    {
        repository.Cancel(
            requestId);

        historyRepository.Add(
            requestId,
            "Отменена",
            "Заявка отменена",
            SessionManager.CurrentUser!.Id);

        NotificationService.Create(
            "Заявка",
            $"Заявка №{requestId} отменена");

        AuditService.Log(
            SessionManager.CurrentUser.Id,
            "Отмена заявки",
            $"Заявка №{requestId}");
        MaterialService materialService =
    new();

        materialService.CheckMinimumStock();
    }
    public void CreateRequestForSelectedUser(
    int userId,
    string room,
    int typeId,
    string description)
    {
        UserRepository users =
            new();

        User? user =
            users.GetById(
                userId);

        if (user == null)
            return;

        repository.Create(
            new Request
            {
                RoomNumber = room,

                ApplicantName =
                    user.FullName,

                ApplicantPhone =
                    user.Phone,

                RequestTypeId =
                    typeId,

                ProblemDescription =
                    description,

                AuthorId =
                    user.Id
            });

        NotificationService.Create(
            "Заявка",
            $"Для пользователя {user.FullName} создана заявка");

        AuditService.Log(
            SessionManager.CurrentUser!.Id,
            "Создание заявки",
            user.FullName);
    }
}