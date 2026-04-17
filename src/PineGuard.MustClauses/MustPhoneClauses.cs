using System.Runtime.CompilerServices;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate generic phone number strings.
/// </summary>
/// <seealso cref="PhoneRules"/>
/// <seealso href="https://pineguard.ai/docs/must/phone">Phone Must Clauses documentation</seealso>
public static class MustPhoneClauses
{
    /// <summary>
    /// Validates that the specified string is a valid phone number and returns the normalised digit string.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The phone number string to validate.</param>
    /// <param name="minDigits">The minimum number of digits required. Defaults to <see cref="PhoneRules.DefaultMinDigits"/>.</param>
    /// <param name="maxDigits">The maximum number of digits allowed. Defaults to <see cref="PhoneRules.DefaultMaxDigits"/>.</param>
    /// <param name="allowedNonDigitCharacters">
    /// Optional array of non-digit characters that are permitted in the input (e.g., <c>+</c>, <c>-</c>).
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid phone number, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// extracted digit string.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="PhoneUtility.TryParsePhone"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid phone number."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.PhoneNumber(phoneNumber);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="PhoneRules"/>
    /// <seealso href="https://pineguard.ai/docs/must/phone">Phone Must Clauses documentation</seealso>
    public static MustResult<string> PhoneNumber(this IMustClause _,
        string? value,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a valid phone number.";

        var ok = PhoneUtility.TryParsePhone(value, out var digits, minDigits, maxDigits, allowedNonDigitCharacters);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: digits);
    }

    /// <summary>
    /// Validates that the specified string satisfies phone number format rules and returns the original string.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The phone number string to validate.</param>
    /// <param name="minDigits">The minimum number of digits required. Defaults to <see cref="PhoneRules.DefaultMinDigits"/>.</param>
    /// <param name="maxDigits">The maximum number of digits allowed. Defaults to <see cref="PhoneRules.DefaultMaxDigits"/>.</param>
    /// <param name="allowedNonDigitCharacters">
    /// Optional array of non-digit characters that are permitted in the input (e.g., <c>+</c>, <c>-</c>).
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> passes phone number format validation, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/>
    /// contains the original input string.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="PhoneRules.IsPhoneNumber"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid phone number."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.PhoneNumberString(rawPhone);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="PhoneRules.IsPhoneNumber"/>
    /// <seealso href="https://pineguard.ai/docs/must/phone">Phone Must Clauses documentation</seealso>
    public static MustResult<string> PhoneNumberString(this IMustClause _,
        string? value,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a valid phone number.";

        var ok = PhoneRules.IsPhoneNumber(value, minDigits, maxDigits, allowedNonDigitCharacters);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value!);
    }
}
