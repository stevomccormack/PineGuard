using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="bool"/> property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/bool">Fluent Bool Extensions documentation</seealso>
public static class FluentBoolExtensions
{
    /// <summary>
    /// Validates that the property value is <see langword="true"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBoolClauses.True"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.IsActive).True();
    /// RuleFor(x => x.IsActive).True("Must be active to proceed.");
    /// </code>
    /// </example>
    /// <seealso cref="MustBoolClauses.True"/>
    public static IRuleBuilderOptions<TModel, bool?> True<TModel>(this IRuleBuilder<TModel, bool?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.True(val.Value, paramName: null) : MustResult<bool>.Ok(false),
            message);

    /// <summary>
    /// Validates that the property value is <see langword="false"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBoolClauses.False"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.IsDeleted).False();
    /// RuleFor(x => x.IsDeleted).False("Item must not be deleted.");
    /// </code>
    /// </example>
    /// <seealso cref="MustBoolClauses.False"/>
    public static IRuleBuilderOptions<TModel, bool?> False<TModel>(this IRuleBuilder<TModel, bool?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.False(val.Value, paramName: null) : MustResult<bool>.Ok(false),
            message);
}
