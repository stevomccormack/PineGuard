using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for validating boolean values represented as strings.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/bool">Fluent Bool Extensions documentation</seealso>
public static class FluentStringBoolExtensions
{
    /// <summary>
    /// Validates that the string property value parses as <see langword="true"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringBoolClauses.True"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.IsActiveString).True();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringBoolClauses.True"/>
    public static IRuleBuilderOptions<TModel, string?> True<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.True(val, paramName: null) : MustResult<bool>.Ok(false),
            message, MustCodes.Boolean.Value.False);

    /// <summary>
    /// Validates that the string property value parses as <see langword="false"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringBoolClauses.False"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.IsDeletedString).False();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringBoolClauses.False"/>
    public static IRuleBuilderOptions<TModel, string?> False<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.False(val, paramName: null) : MustResult<bool>.Ok(false),
            message, MustCodes.Boolean.Value.True);
}
