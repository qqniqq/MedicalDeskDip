using MedicalDeskLib.Models;

namespace MedicalDeskLib.Repositories;

public interface IRequestRepository
{
    List<Request> GetAll();

    Request? GetById(int id);

    void Create(Request request);

    void TakeToWork(
        int requestId,
        int specialistId);

    void Complete(
        int requestId,
        string comment);

    void Cancel(
        int requestId);

    List<Request> Search(string text);

    List<Request> GetByStatus(string status);

    List<Request> GetByAuthor(int authorId);
}