using PineGuard.Rules;

namespace PineGuard.Utils;

public static class PhoneUtility
{
    public static bool TrySanitizePhoneDigits(
        string? value,
        out string digits,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null)
    {
        digits = string.Empty;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (minDigits < 1 || maxDigits < 1 || minDigits > maxDigits)
            return false;

        allowedNonDigitCharacters ??= PhoneRules.DefaultAllowedNonDigitCharacters;

        if (!StringNumberRules.TrySanitizeDigits(trimmed, out digits, allowedNonDigitCharacters))
        {
            digits = string.Empty;
            return false;
        }

        return digits.Length >= minDigits && digits.Length <= maxDigits;
    }
}
