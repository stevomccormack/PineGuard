using PineGuard.Rules;
using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static bool TryGetTrimmed(string? value, out string trimmed)
    {
        trimmed = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        trimmed = value.Trim();
        return trimmed.Length != 0;
    }
    public static bool TryParseDigitsOnly(string? value, out string digitsOnly)
        => TryParseDigits(value, out digitsOnly, allowedNonDigitChars: []);

    public static bool TryParseDigits(string? value, out string digits, char[]? allowedNonDigitChars = null)
    {
        digits = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        allowedNonDigitChars ??= StringRules.NumberTypes.DefaultAllowedDigitSeparators;

        var trimmed = value.Trim();

        if (allowedNonDigitChars.Length == 0)
        {
            foreach (var ch in trimmed)
            {
                if (ch is < '0' or > '9')
                    return false;
            }

            digits = trimmed;
            return true;
        }

        var allowed = new HashSet<char>(allowedNonDigitChars);

        Span<char> buffer = stackalloc char[trimmed.Length];
        var written = 0;

        foreach (var ch in trimmed)
        {
            if (ch is >= '0' and <= '9')
            {
                buffer[written++] = ch;
                continue;
            }

            if (allowed.Contains(ch))
                continue;

            return false;
        }

        if (written == 0)
            return false;

        digits = new string(buffer[..written]);
        return true;
    }

    public static bool TitleCase(string? value, out string titleCased)
    {
        titleCased = string.Empty;

        if (value is null)
            return false;

        if (!TryGetTrimmed(value, out var trimmed))
            return false;

        var textInfo = CultureInfo.InvariantCulture.TextInfo;
        titleCased = textInfo.ToTitleCase(trimmed.ToLowerInvariant());

        return titleCased.Length != 0;
    }

    public static bool TitleCase(string? value)
        => TitleCase(value, out _);
}
