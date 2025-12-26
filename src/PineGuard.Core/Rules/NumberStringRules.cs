namespace PineGuard.Rules;

public static class NumberStringRules
{
    public static bool IsDigitsOnly(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        value = value.Trim();

        foreach (var ch in value)
        {
            if (ch is < '0' or > '9')
                return false;
        }

        return value.Length != 0;
    }
}
