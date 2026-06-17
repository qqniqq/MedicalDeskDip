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

    private readonly UserRepository
        userRepository =
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
            SessionManager.CurrentUser.Id,
            "Заявка",
            "Ваша заявка успешно зарегистрирована");
        UserRepository users =
    new();

        foreach (var user in users.GetAll())
        {
            if (user.RoleName == "Специалист")
            {
                NotificationService.Create(
                    user.Id,
                    "Новая заявка",
                    "Поступила новая заявка");
            }
        }
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
            SessionManager.CurrentUser.Id,
            "Заявка",
            "Заявка успешно зарегистрирована");

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

        Request? request =
            repository.GetById(
                requestId);

        if (request != null)
        {
            NotificationService.Create(
                request.AuthorId,
                "Заявка",
                $"Ваша заявка №{request.RequestNumber} принята в работу");
        }

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

        Request? request =
            repository.GetById(
                requestId);

        if (request != null)
        {
            NotificationService.Create(
                request.AuthorId,
                "Заявка",
                $"Ваша заявка №{request.RequestNumber} завершена");
        }

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

        Request? request =
            repository.GetById(
                requestId);

        if (request != null)
        {
            NotificationService.Create(
                request.AuthorId,
                "Заявка",
                $"Ваша заявка №{request.RequestNumber} отменена");
        }

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
            user.Id,
            "Заявка",
            "Для вас зарегистрирована новая заявка");
        foreach (var specialist in users.GetAll())
        {
            if (specialist.RoleName == "Специалист")
            {
                NotificationService.Create(
                    specialist.Id,
                    "Новая заявка",
                    "Поступила новая заявка");
            }
        }
        AuditService.Log(
            SessionManager.CurrentUser!.Id,
            "Создание заявки",
            user.FullName);
    }
}