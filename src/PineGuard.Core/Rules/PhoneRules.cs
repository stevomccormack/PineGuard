using PineGuard.Utils;

namespace PineGuard.Rules;

public static class PhoneRules
{
    public static readonly char[] DefaultAllowedNonDigitCharacters = ['+', '(', ')', '-', '.', '/'];

    public const int DefaultMinDigits = 7;
    public const int DefaultMaxDigits = 15;

    public static bool IsPhoneNumber(
        string? value,
        int minDigits = DefaultMinDigits,
        int maxDigits = DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null)
    {
        return PhoneUtility.TryParsePhone(value, out _, minDigits, maxDigits, allowedNonDigitCharacters);
    }
}
