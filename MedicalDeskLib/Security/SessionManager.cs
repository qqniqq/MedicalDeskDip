using MedicalDeskLib.Models;

namespace MedicalDeskLib.Security;

public static class SessionManager
{
    public static User? CurrentUser { get; set; }

    public static bool IsLogged =>
        CurrentUser != null;

    public static void Logout()
    {
        CurrentUser = null;
    }
}