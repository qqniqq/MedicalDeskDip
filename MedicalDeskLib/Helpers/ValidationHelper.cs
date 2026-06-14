using System.Text.RegularExpressions;

namespace MedicalDeskLib.Helpers;

public static class ValidationHelper
{
    public static bool IsValidFullName(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Regex.IsMatch(
            value,
            @"^[А-Яа-яЁёA-Za-z\s\-]+$");
    }

    public static bool IsValidPhone(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Regex.IsMatch(
            value,
            @"^\+\d{11,15}$");
    }

    public static bool IsValidLogin(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Regex.IsMatch(
            value,
            @"^[A-Za-z0-9]+$");
    }

    public static bool IsValidPassword(
        string value)
    {
        return !string.IsNullOrWhiteSpace(value)
               && value.Length >= 6;
    }
}