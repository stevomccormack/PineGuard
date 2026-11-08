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
    /// Delegates to <see cref="MustTimeSpanClauses.InRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Duration).InRangeTimeSpan(TimeSpan.Zero, TimeSpan.FromHours(8));</code></example>
    /// <seealso cref="MustTimeSpanClauses.InRange"/>
    public static IRuleBuilderOptions<TModel, string?> InRangeTimeSpan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.InRangeTimeSpan(val, min, max, inclusion, paramName: null)),
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
    /// Delegates to <see cref="MustTimeSpanClauses.OutOfRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Duration).OutOfRangeTimeSpan(TimeSpan.FromHours(1), TimeSpan.FromHours(2));</code></example>
    /// <seealso cref="MustTimeSpanClauses.OutOfRange"/>
    public static IRuleBuilderOptions<TModel, string?> OutOfRangeTimeSpan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OutOfRangeTimeSpan(val, min, max, inclusion, paramName: null)),
            message, MustCodes.Time.Duration.InRange);

    /// <summary>
    /// Validates that the string value represents a <see cref="TimeSpan"/> greater than the specified threshold.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the threshold itself is included or excluded.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTimeSpanClauses.GreaterThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Timeout).GreaterThan(TimeSpan.FromSeconds(30));</code></example>
    /// <seealso cref="MustTimeSpanClauses.GreaterThan"/>
    public static IRuleBuilderOptions<TModel, string?> GreaterThanTimeSpan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.GreaterThanTimeSpan(val, other, inclusion, paramName: null)),
            message, MustCodes.Time.Duration.NotGreater);

    /// <summary>
    /// Validates that the string value represents a <see cref="TimeSpan"/> less than the specified threshold.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the threshold itself is included or excluded.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTimeSpanClauses.LessThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Delay).LessThan(TimeSpan.FromMinutes(5));</code></example>
    /// <seealso cref="MustTimeSpanClauses.LessThan"/>
    public static IRuleBuilderOptions<TModel, string?> LessThanTimeSpan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.LessThanTimeSpan(val, other, inclusion, paramName: null)),
            message, MustCodes.Time.Duration.NotLess);
    private static MustResult<T> SkipAbsentValue<T>(string? value, MustResult<T> result) =>
        value is null && result.ParamName is null ? MustResult<T>.Ok(default) : result;
}
