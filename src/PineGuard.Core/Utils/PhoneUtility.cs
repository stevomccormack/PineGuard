using PineGuard.Rules;

namespace PineGuard.Utils;

/// <summary>
/// Provides phone number parsing and validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/phone">Phone Utility documentation</seealso>
public static partial class PhoneUtility
{
    /// <summary>
    /// Attempts to parse the specified string as a phone number, extracting only digits.
    /// </summary>
    /// <param name="value">The phone number string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="digits">
    /// When this method returns <see langword="true"/>, contains the extracted digit-only phone number.
    /// When <see langword="false"/>, contains <see cref="string.Empty"/>.
    /// </param>
    /// <param name="minDigits">The minimum number of digits required. Defaults to <see cref="PhoneRules.DefaultMinDigits"/>.</param>
    /// <param name="maxDigits">The maximum number of digits allowed. Defaults to <see cref="PhoneRules.DefaultMaxDigits"/>.</param>
    /// <param name="allowedNonDigitCharacters">
    /// An optional set of non-digit characters to allow and strip.
    /// If <see langword="null"/>, uses <see cref="PhoneRules.DefaultAllowedNonDigitCharacters"/>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed and the digit count is within the specified range; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// PhoneUtility.TryParsePhone("+1-234-567-8901", out var digits); // true, digits = "12345678901"
    /// </code>
    /// </example>
    public static bool TryParsePhone(
        string? value,
        out string digits,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null)
    {
        digits = string.Empty;

        if (!StringUtility.TryGetTrimmed(value, out _))
            return false;

        if (minDigits < 1 || maxDigits < 1 || minDigits > maxDigits)
            return false;

        allowedNonDigitCharacters ??= PhoneRules.DefaultAllowedNonDigitCharacters;

        if (StringUtility.TryParseDigits(value, out digits, allowedNonDigitCharacters))
            return digits.Length >= minDigits && digits.Length <= maxDigits;

        digits = string.Empty;
        return false;

    }
}
