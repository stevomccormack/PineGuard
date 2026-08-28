using System.Runtime.CompilerServices;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for phone number validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/phone">Guard Phone documentation</seealso>
public static class GuardPhoneClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotPhoneNumber constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="minDigits">The minimum number of digits.</param>
    /// <param name="maxDigits">The maximum number of digits.</param>
    /// <param name="allowedNonDigitCharacters">The allowed non-digit characters.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustPhoneClauses.PhoneNumber"/>
    public static string NotPhoneNumber(this IGuardClause _,
        string? value,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.PhoneNumber(value, minDigits, maxDigits, allowedNonDigitCharacters, paramName); // Guard.Against.NotPhoneNumber => Must.Be.PhoneNumber
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotPhoneNumberString constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="minDigits">The minimum number of digits.</param>
    /// <param name="maxDigits">The maximum number of digits.</param>
    /// <param name="allowedNonDigitCharacters">The allowed non-digit characters.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustPhoneClauses.PhoneNumberString"/>
    public static string NotPhoneNumberString(this IGuardClause _,
        string? value,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.PhoneNumberString(value, minDigits, maxDigits, allowedNonDigitCharacters, paramName); // Guard.Against.NotPhoneNumberString => Must.Be.PhoneNumberString
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
