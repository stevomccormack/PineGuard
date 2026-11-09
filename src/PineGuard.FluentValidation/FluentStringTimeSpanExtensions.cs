using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string-to-<see cref="TimeSpan"/> property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-timespan">Fluent String TimeSpan Extensions documentation</seealso>
public static class FluentStringTimeSpanExtensions
{
    /// <summary>
    /// Validates that the string value represents a <see cref="TimeSpan"/> duration between the specified bounds.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum allowed duration.</param>
    /// <param name="max">The maximum allowed duration.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTimeSpanClauses.DurationBetween"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Duration).DurationBetween(TimeSpan.Zero, TimeSpan.FromHours(8));</code></example>
    /// <seealso cref="MustTimeSpanClauses.DurationBetween"/>
    public static IRuleBuilderOptions<TModel, string?> DurationBetween<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.DurationBetween(val, min, max, inclusion, paramName: null) : MustResult<TimeSpan>.Ok(TimeSpan.Zero),
            message, MustCodes.Time.Duration.OutOfRange);

    /// <summary>
    /// Validates that the string value represents a <see cref="TimeSpan"/> duration not between the specified bounds.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTimeSpanClauses.NotDurationBetween"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Duration).NotDurationBetween(TimeSpan.FromHours(1), TimeSpan.FromHours(2));</code></example>
    /// <seealso cref="MustTimeSpanClauses.NotDurationBetween"/>
    public static IRuleBuilderOptions<TModel, string?> NotDurationBetween<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotDurationBetween(val, min, max, inclusion, paramName: null) : MustResult<TimeSpan>.Ok(TimeSpan.Zero),
            message, MustCodes.Time.Duration.InRange);

    /// <summary>
    /// Validates that the string value represents a <see cref="TimeSpan"/> longer than the specified threshold.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="threshold">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the threshold itself is included or excluded.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTimeSpanClauses.LongerThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Timeout).LongerThan(TimeSpan.FromSeconds(30));</code></example>
    /// <seealso cref="MustTimeSpanClauses.LongerThan"/>
    public static IRuleBuilderOptions<TModel, string?> LongerThan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.LongerThan(val, threshold, inclusion, paramName: null) : MustResult<TimeSpan>.Ok(TimeSpan.Zero),
            message, MustCodes.Time.Duration.NotLonger);

    /// <summary>
    /// Validates that the string value represents a <see cref="TimeSpan"/> shorter than the specified threshold.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="threshold">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the threshold itself is included or excluded.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTimeSpanClauses.ShorterThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Delay).ShorterThan(TimeSpan.FromMinutes(5));</code></example>
    /// <seealso cref="MustTimeSpanClauses.ShorterThan"/>
    public static IRuleBuilderOptions<TModel, string?> ShorterThan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ShorterThan(val, threshold, inclusion, paramName: null) : MustResult<TimeSpan>.Ok(TimeSpan.Zero),
            message, MustCodes.Time.Duration.NotShorter);
}
