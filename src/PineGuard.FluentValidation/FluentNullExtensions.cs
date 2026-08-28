using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

// Renamed from Null/NotNull to NotRequired/Required to avoid FluentValidation naming collisions.
/// <summary>
/// Provides FluentValidation extension methods for <see langword="null"/> presence validation.
/// </summary>
/// <remarks>
/// These methods are named <c>Required</c> and <c>NotRequired</c> rather than <c>NotNull</c>/<c>Null</c>
/// to avoid naming collisions with FluentValidation's built-in null checks.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/fluent/null">Fluent Null Extensions documentation</seealso>
public static class FluentNullExtensions
{
    /// <summary>
    /// Validates that the property value is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNullClauses.Null"/>. This method is named <c>NotRequired</c>
    /// to avoid collision with FluentValidation's built-in <c>Null()</c> rule.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DeletedAt).NotRequired();
    /// </code>
    /// </example>
    /// <seealso cref="MustNullClauses.Null"/>
    public static IRuleBuilderOptions<TModel, T?> NotRequired<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Null(val, paramName: null),
            message, MustCodes.Value.State.NotNull);

    /// <summary>
    /// Validates that the property value is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNullClauses.NotNull"/>. This method is named <c>Required</c>
    /// to avoid collision with FluentValidation's built-in <c>NotNull()</c> rule.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UserId).Required();
    /// </code>
    /// </example>
    /// <seealso cref="MustNullClauses.NotNull"/>
    public static IRuleBuilderOptions<TModel, T?> Required<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotNull(val, paramName: null),
            message, MustCodes.Value.State.Null);
}
