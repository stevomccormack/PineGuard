using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string-to-GUID property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-guid">Fluent String GUID Extensions documentation</seealso>
public static class FluentStringGuidExtensions
{
    /// <summary>
    /// Validates that the string value is a valid GUID representation.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGuidClauses.Guid"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.ExternalId).Guid();</code></example>
    /// <seealso cref="MustStringGuidClauses.Guid"/>
    public static IRuleBuilderOptions<TModel, string?> Guid<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Guid(val, paramName: null) : MustResult<Guid>.Ok(System.Guid.Empty),
            message);

    /// <summary>
    /// Validates that the string value is a valid non-empty GUID representation.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGuidClauses.NotEmptyGuid"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.CorrelationId).NotEmptyGuid();</code></example>
    /// <seealso cref="MustStringGuidClauses.NotEmptyGuid"/>
    public static IRuleBuilderOptions<TModel, string?> NotEmptyGuid<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotEmptyGuid(val, paramName: null) : MustResult<Guid>.Ok(System.Guid.Empty),
            message);
}
