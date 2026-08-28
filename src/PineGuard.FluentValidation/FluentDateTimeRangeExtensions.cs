using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="DateTimeRange"/> property validation including
/// chronological ordering, overlap detection, and containment checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/datetime-range">Fluent DateTime Range Extensions documentation</seealso>
public static class FluentDateTimeRangeExtensions
{
    /// <summary>Validates that the nullable <see cref="DateTimeRange"/> is chronologically ordered.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeRange?> Chronological<TModel>(
        this IRuleBuilder<TModel, DateTimeRange?> ruleBuilder,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Chronological(val.Value, inclusion, paramName: null) : MustResult<DateTimeRange>.Ok(default),
            message, MustCodes.Range.Order.NotChronological);

    /// <summary>Validates that the <see cref="DateTimeRange"/> is chronologically ordered.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.TimeRange).Chronological();</code></example>
    public static IRuleBuilderOptions<TModel, DateTimeRange> Chronological<TModel>(
        this IRuleBuilder<TModel, DateTimeRange> ruleBuilder,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Chronological(val, inclusion, paramName: null),
            message, MustCodes.Range.Order.NotChronological);

    /// <summary>Validates that the nullable <see cref="DateTimeRange"/> overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test for overlap.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeRange?> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeRange?> ruleBuilder,
        DateTimeRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Overlapping(val.Value, range2, inclusion, paramName: null) : MustResult<DateTimeRange>.Ok(default),
            message, MustCodes.Range.Overlap.Missing);

    /// <summary>Validates that the <see cref="DateTimeRange"/> overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test for overlap.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeRange> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeRange> ruleBuilder,
        DateTimeRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Overlapping(val, range2, inclusion, paramName: null),
            message, MustCodes.Range.Overlap.Missing);

    /// <summary>Validates that the nullable <see cref="DateTimeRange"/> does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test against.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeRange?> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeRange?> ruleBuilder,
        DateTimeRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOverlapping(val.Value, range2, inclusion, paramName: null) : MustResult<DateTimeRange>.Ok(default),
            message, MustCodes.Range.Overlap.Present);

    /// <summary>Validates that the <see cref="DateTimeRange"/> does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test against.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeRange> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeRange> ruleBuilder,
        DateTimeRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOverlapping(val, range2, inclusion, paramName: null),
            message, MustCodes.Range.Overlap.Present);

    /// <summary>Validates that the nullable <see cref="DateTimeRange"/> contains the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check for containment.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeRange?> Contains<TModel>(
        this IRuleBuilder<TModel, DateTimeRange?> ruleBuilder,
        DateTime value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Contains(val.Value, value, inclusion, paramName: null) : MustResult<DateTimeRange>.Ok(default),
            message, MustCodes.Range.Bounds.NotContains);

    /// <summary>Validates that the <see cref="DateTimeRange"/> contains the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check for containment.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeRange> Contains<TModel>(
        this IRuleBuilder<TModel, DateTimeRange> ruleBuilder,
        DateTime value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Contains(val, value, inclusion, paramName: null),
            message, MustCodes.Range.Bounds.NotContains);

    /// <summary>Validates that the nullable <see cref="DateTimeRange"/> does not contain the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeRange?> NotContains<TModel>(
        this IRuleBuilder<TModel, DateTimeRange?> ruleBuilder,
        DateTime value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotContains(val.Value, value, inclusion, paramName: null) : MustResult<DateTimeRange>.Ok(default),
            message, MustCodes.Range.Bounds.Contains);

    /// <summary>Validates that the <see cref="DateTimeRange"/> does not contain the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeRange> NotContains<TModel>(
        this IRuleBuilder<TModel, DateTimeRange> ruleBuilder,
        DateTime value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotContains(val, value, inclusion, paramName: null),
            message, MustCodes.Range.Bounds.Contains);
}
