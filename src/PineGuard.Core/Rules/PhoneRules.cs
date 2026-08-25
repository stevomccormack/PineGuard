using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides phone number validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/phone">Phone Rules documentation</seealso>
public static partial class PhoneRules
{
    private static readonly char[] DefaultAllowedNonDigitCharactersSource = ['+', '(', ')', '-', '.', '/'];

    /// <summary>
    /// The default set of allowed non-digit characters in phone numbers: <c>+</c>, <c>(</c>, <c>)</c>, <c>-</c>, <c>.</c>, <c>/</c>.
    /// </summary>
    /// <remarks>
    /// Returns a fresh copy on every access. <see cref="PhoneUtility.TryParsePhone"/> and every caller that
    /// receives this default hold their own array instance, so mutating the returned array cannot change the
    /// default used by other callers or by later calls.
    /// </remarks>
    public static char[] DefaultAllowedNonDigitCharacters => [.. DefaultAllowedNonDigitCharactersSource];

    /// <summary>
    /// The default minimum number of digits in a phone number (7).
    /// </summary>
    public const int DefaultMinDigits = 7;

    /// <summary>
    /// The default maximum number of digits in a phone number (15, per ITU-T E.164).
    /// </summary>
    public const int DefaultMaxDigits = 15;

    /// <summary>
    /// Determines whether the specified value is a valid phone number using the given digit count constraints.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="minDigits">The minimum number of digits required. Defaults to <see cref="DefaultMinDigits"/>.</param>
    /// <param name="maxDigits">The maximum number of digits allowed. Defaults to <see cref="DefaultMaxDigits"/>.</param>
    /// <param name="allowedNonDigitCharacters">
    /// Optional set of non-digit characters allowed between digits (e.g., spaces, dashes).
    /// If <see langword="null"/>, uses <see cref="DefaultAllowedNonDigitCharacters"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains between <paramref name="minDigits"/> and
    /// <paramref name="maxDigits"/> digits with only allowed non-digit characters; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = PhoneRules.IsPhoneNumber("+1 (555) 123-4567"); // true
    /// bool invalid = PhoneRules.IsPhoneNumber("123");             // false (too few digits)
    /// </code>
    /// </example>
    public static bool IsPhoneNumber(
        string? value,
        int minDigits = DefaultMinDigits,
        int maxDigits = DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null) =>
        PhoneUtility.TryParsePhone(value, out _, minDigits, maxDigits, allowedNonDigitCharacters);
}
