using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for check-digit validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/checksum">Fluent Checksum Extensions documentation</seealso>
public static class FluentChecksumExtensions
{
    /// <summary>
    /// Validates that the property value satisfies the Luhn checksum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustChecksumClauses.Luhn"/>, which strips spaces and hyphens before verifying.
    /// A passing rule proves only that the digits are internally consistent — never that the sequence identifies
    /// a real account, device or person. If the value is <see langword="null"/>, validation passes (null values
    /// should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CardNumber).Luhn();
    /// </code>
    /// </example>
    /// <seealso cref="MustChecksumClauses.Luhn"/>
    public static IRuleBuilderOptions<TModel, string?> Luhn<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Luhn(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Checksum.Luhn.Invalid);
}
