using MedicalDeskLib.Repositories;
using System.Text;

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

        string first =
            Transliterate(
                parts[0]);

        string second =
            Transliterate(
                parts[1]);

        string third =
            Transliterate(
                parts[2]);

        string prefix =
            $"{first[0]}" +
            $"{second[0]}" +
            $"{third[0]}"
            .ToUpper();

        UserRepository repository =
            new();

        int number = 1;

        string login =
            $"{prefix}{number:000}";

        while (
            repository.LoginExists(
                login))
        {
            number++;

            login =
                $"{prefix}{number:000}";
        }

        return login;
    }
    private static string Transliterate(
    string value)
    {
        Dictionary<char, string> map =
            new()
            {
            {'А',"A"},
            {'Б',"B"},
            {'В',"V"},
            {'Г',"G"},
            {'Д',"D"},
            {'Е',"E"},
            {'Ё',"E"},
            {'Ж',"ZH"},
            {'З',"Z"},
            {'И',"I"},
            {'Й',"I"},
            {'К',"K"},
            {'Л',"L"},
            {'М',"M"},
            {'Н',"N"},
            {'О',"O"},
            {'П',"P"},
            {'Р',"R"},
            {'С',"S"},
            {'Т',"T"},
            {'У',"U"},
            {'Ф',"F"},
            {'Х',"H"},
            {'Ц',"C"},
            {'Ч',"CH"},
            {'Ш',"SH"},
            {'Щ',"SCH"},
            {'Ы',"Y"},
            {'Э',"E"},
            {'Ю',"YU"},
            {'Я',"YA"}
            };

        StringBuilder result =
            new();

        foreach (char c in
            value.ToUpper())
        {
            if (map.ContainsKey(c))
            {
                result.Append(
                    map[c]);
            }
        }

        return result.ToString();
    }
}