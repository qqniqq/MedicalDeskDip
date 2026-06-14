using MedicalDeskLib.Repositories;

namespace MedicalDeskLib.Helpers;

public static class LoginGenerator
{
    public static string Generate(
        string fullName)
    {
        string[] parts =
            fullName
                .Trim()
                .Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3)
            return "";

        string prefix =
            $"{char.ToUpper(parts[0][0])}" +
            $"{char.ToUpper(parts[1][0])}" +
            $"{char.ToUpper(parts[2][0])}";

        UserRepository repository =
            new();

        int number = 1;

        string login =
            $"{prefix}{number}";

        while (
            repository.LoginExists(
                login))
        {
            number++;

            login =
                $"{prefix}{number}";
        }

        return login;
    }
}