using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="Guid"/> property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/guid">Fluent GUID Extensions documentation</seealso>
public static class FluentGuidExtensions
{
    /// <summary>
    /// Validates that the property value is not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustGuidClauses.NotEmpty"/>. Use this overload for non-nullable <see cref="Guid"/> properties.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CorrelationId).NotEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustGuidClauses.NotEmpty"/>
    public static IRuleBuilderOptions<TModel, Guid> NotEmpty<TModel>(
        this IRuleBuilder<TModel, Guid> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotEmpty(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is not <see langword="null"/> and not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustGuidClauses.NotEmpty"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.OptionalId).NotEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustGuidClauses.NotEmpty"/>
    public static IRuleBuilderOptions<TModel, Guid?> NotEmpty<TModel>(
        this IRuleBuilder<TModel, Guid?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotEmpty(val.Value, paramName: null) : MustResult<Guid>.Ok(Guid.Empty),
            message);
}
