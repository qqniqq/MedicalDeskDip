namespace MedicalDeskLib.Helpers;

public static class PasswordGenerator
{
    public static string Generate()
    {
        return
            $"MD-{Random.Shared.Next(1000, 9999)}";
    }
}