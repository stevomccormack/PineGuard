namespace PineGuard.Rules;

public static class PhoneRules
{
    public const int DefaultMinDigits = 7;
    public const int DefaultMaxDigits = 15;

    public static readonly char[] DefaultAllowedNonDigitCharacters = ['+', '(', ')', '-', '.', '/'];

    public static bool IsPhoneNumber(
        string? value,
        int minDigits = DefaultMinDigits,
        int maxDigits = DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (minDigits < 1 || maxDigits < 1 || minDigits > maxDigits)
            return false;

        HashSet<char>? allowed = null;

        if (allowedNonDigitCharacters is { Length: > 0 })
            allowed = [.. allowedNonDigitCharacters];
        else if (allowedNonDigitCharacters is null)
            allowed = [.. DefaultAllowedNonDigitCharacters];

        var digits = 0;

        foreach (var ch in value)
        {
            if (ch is >= '0' and <= '9')
            {
                digits++;
                continue;
            }

            if (char.IsWhiteSpace(ch))
                continue;

            if (allowed is not null && allowed.Contains(ch))
                continue;

            return false;
        }

        return digits >= minDigits && digits <= maxDigits;
    }
}
