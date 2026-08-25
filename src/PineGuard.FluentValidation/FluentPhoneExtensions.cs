using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for phone number property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/phone">Fluent Phone Extensions documentation</seealso>
public static class FluentPhoneExtensions
{
    /// <summary>
    /// Validates that the string value is a well-formed phone number.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="minDigits">The minimum number of digits required. Defaults to <see cref="PhoneRules.DefaultMinDigits"/>.</param>
    /// <param name="maxDigits">The maximum number of digits allowed. Defaults to <see cref="PhoneRules.DefaultMaxDigits"/>.</param>
    /// <param name="allowedNonDigitCharacters">Optional array of allowed non-digit characters (e.g., spaces, dashes).</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustPhoneClauses.PhoneNumber"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example><code>RuleFor(x => x.Phone).PhoneNumber();</code></example>
    /// <seealso cref="MustPhoneClauses.PhoneNumber"/>
    public static IRuleBuilderOptions<TModel, string?> PhoneNumber<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PhoneNumber(val, minDigits, maxDigits, allowedNonDigitCharacters, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a well-formed phone number string representation.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="minDigits">The minimum number of digits required. Defaults to <see cref="PhoneRules.DefaultMinDigits"/>.</param>
    /// <param name="maxDigits">The maximum number of digits allowed. Defaults to <see cref="PhoneRules.DefaultMaxDigits"/>.</param>
    /// <param name="allowedNonDigitCharacters">Optional array of allowed non-digit characters (e.g., spaces, dashes).</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustPhoneClauses.PhoneNumberString"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example><code>RuleFor(x => x.Mobile).PhoneNumberString();</code></example>
    /// <seealso cref="MustPhoneClauses.PhoneNumberString"/>
    public static IRuleBuilderOptions<TModel, string?> PhoneNumberString<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int minDigits = PhoneRules.DefaultMinDigits,
        int maxDigits = PhoneRules.DefaultMaxDigits,
        char[]? allowedNonDigitCharacters = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PhoneNumberString(val, minDigits, maxDigits, allowedNonDigitCharacters, paramName: null),
            message);
}
