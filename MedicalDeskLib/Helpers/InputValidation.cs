using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MedicalDeskLib.Helpers;

public static class InputValidation
{
    public static void OnlyDigits(
        object sender,
        TextCompositionEventArgs e)
    {
        e.Handled =
            !Regex.IsMatch(
                e.Text,
                @"^[0-9]+$");
    }

    public static void OnlyLetters(
        object sender,
        TextCompositionEventArgs e)
    {
        e.Handled =
            !Regex.IsMatch(
                e.Text,
                @"^[А-Яа-яЁёA-Za-z\s\-]+$");
    }

    public static void LoginInput(
        object sender,
        TextCompositionEventArgs e)
    {
        e.Handled =
            !Regex.IsMatch(
                e.Text,
                @"^[A-Za-z0-9]+$");
    }

    public static void PhoneInput(
        object sender,
        TextCompositionEventArgs e)
    {
        e.Handled =
            !Regex.IsMatch(
                e.Text,
                @"^[0-9]+$");
    }
}