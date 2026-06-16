using MedicalDeskLib.Security;
using MedicalDeskLib.Repositories;
using MedicalDeskLib.Models;

namespace MedicalDeskLib.Services;

public class AuthService
{
    private readonly UserRepository _repository =
        new();

    public User? Login(
        string login,
        string password)
    {
        var user =
            _repository.GetAll()
            .FirstOrDefault(x =>
                x.Login == login);

        if (user == null)
            return null;

        if (!user.IsActive)
            return null;

        string hash =
     PasswordHasher.Hash(
         password);

        if (user.PasswordHash != hash)
        {
            return null;
        }

        SessionManager.CurrentUser = user;

        AuditService.Log(
            user.Id,
            "Вход",
            $"Пользователь {user.Login} вошел");

        return user;
    }
}