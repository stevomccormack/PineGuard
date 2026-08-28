using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for predicate-based property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/predicate">Fluent Predicate Extensions documentation</seealso>
public static class FluentPredicateExtensions
{
    /// <summary>
    /// Validates that the property value satisfies the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">The predicate the value must satisfy.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustPredicateClauses.Satisfies"/>. If the value is <see langword="null"/>,
    /// validation fails without invoking <paramref name="predicate"/>; use a separate <c>.NotNull()</c> rule
    /// beforehand if <see langword="null"/> should be reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Age).Satisfies(age => age >= 18);
    /// </code>
    /// </example>
    /// <seealso cref="MustPredicateClauses.Satisfies"/>
    public static IRuleBuilderOptions<TModel, T?> Satisfies<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        Func<T, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Satisfies(val, predicate, paramName: null),
            message, MustCodes.Predicate.Result.False);

    /// <summary>
    /// Validates that the property value does not satisfy the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">The predicate the value must not satisfy.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustPredicateClauses.NotSatisfies"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotSatisfies(s => s == "Banned");
    /// </code>
    /// </example>
    /// <seealso cref="MustPredicateClauses.NotSatisfies"/>
    public static IRuleBuilderOptions<TModel, T?> NotSatisfies<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        Func<T, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotSatisfies(val, predicate, paramName: null),
            message, MustCodes.Predicate.Result.True);
}
