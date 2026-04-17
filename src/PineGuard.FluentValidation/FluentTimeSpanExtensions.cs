using FluentValidation;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="TimeSpan"/> property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/timespan">Fluent TimeSpan Extensions documentation</seealso>
public static class FluentTimeSpanExtensions
{
    /// <summary>
    /// Validates that the <see cref="TimeSpan"/> duration falls between the specified bounds.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum allowed duration.</param>
    /// <param name="max">The maximum allowed duration.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustTimeSpanClauses.DurationBetween"/>.</remarks>
    /// <example><code>RuleFor(x => x.Duration).DurationBetween(TimeSpan.Zero, TimeSpan.FromHours(8));</code></example>
    /// <seealso cref="MustTimeSpanClauses.DurationBetween"/>
    public static IRuleBuilderOptions<TModel, TimeSpan> DurationBetween<TModel>(this IRuleBuilder<TModel, TimeSpan> ruleBuilder,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.DurationBetween(val, min, max, inclusion, paramName: null),
            message);

    /// <summary>
    /// Validates that the <see cref="TimeSpan"/> duration does not fall between the specified bounds.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustTimeSpanClauses.NotDurationBetween"/>.</remarks>
    /// <example><code>RuleFor(x => x.Duration).NotDurationBetween(TimeSpan.FromHours(1), TimeSpan.FromHours(2));</code></example>
    /// <seealso cref="MustTimeSpanClauses.NotDurationBetween"/>
    public static IRuleBuilderOptions<TModel, TimeSpan> NotDurationBetween<TModel>(this IRuleBuilder<TModel, TimeSpan> ruleBuilder,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotDurationBetween(val, min, max, inclusion, paramName: null),
            message);

    /// <summary>
    /// Validates that the <see cref="TimeSpan"/> duration is greater than the specified threshold.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="threshold">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the threshold itself is included or excluded.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustTimeSpanClauses.GreaterThan"/>.</remarks>
    /// <example><code>RuleFor(x => x.Timeout).GreaterThan(TimeSpan.FromSeconds(30));</code></example>
    /// <seealso cref="MustTimeSpanClauses.GreaterThan"/>
    public static IRuleBuilderOptions<TModel, TimeSpan> GreaterThan<TModel>(this IRuleBuilder<TModel, TimeSpan> ruleBuilder,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.GreaterThan(val, threshold, inclusion, paramName: null),
            message);

    /// <summary>
    /// Validates that the <see cref="TimeSpan"/> duration is less than the specified threshold.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="threshold">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the threshold itself is included or excluded.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustTimeSpanClauses.LessThan"/>.</remarks>
    /// <example><code>RuleFor(x => x.Delay).LessThan(TimeSpan.FromMinutes(5));</code></example>
    /// <seealso cref="MustTimeSpanClauses.LessThan"/>
    public static IRuleBuilderOptions<TModel, TimeSpan> LessThan<TModel>(this IRuleBuilder<TModel, TimeSpan> ruleBuilder,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.LessThan(val, threshold, inclusion, paramName: null),
            message);
}
