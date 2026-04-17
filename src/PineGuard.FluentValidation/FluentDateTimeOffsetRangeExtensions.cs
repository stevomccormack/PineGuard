using FluentValidation;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="DateTimeOffsetRange"/> property validation including
/// chronological ordering, overlap detection, and containment checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/datetimeoffset-range">Fluent DateTimeOffset Range Extensions documentation</seealso>
public static class FluentDateTimeOffsetRangeExtensions
{
    /// <summary>Validates that the nullable <see cref="DateTimeOffsetRange"/> is chronologically ordered.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange?> Chronological<TModel>(
        this IRuleBuilder<TModel, DateTimeOffsetRange?> ruleBuilder,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Chronological(val.Value, inclusion, paramName: null) : MustResult<DateTimeOffsetRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateTimeOffsetRange"/> is chronologically ordered.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.OffsetRange).Chronological();</code></example>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange> Chronological<TModel>(
        this IRuleBuilder<TModel, DateTimeOffsetRange> ruleBuilder,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Chronological(val, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateTimeOffsetRange"/> overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test for overlap.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange?> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffsetRange?> ruleBuilder,
        DateTimeOffsetRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Overlapping(val.Value, range2, inclusion, paramName: null) : MustResult<DateTimeOffsetRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateTimeOffsetRange"/> overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test for overlap.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffsetRange> ruleBuilder,
        DateTimeOffsetRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Overlapping(val, range2, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateTimeOffsetRange"/> does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test against.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange?> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffsetRange?> ruleBuilder,
        DateTimeOffsetRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOverlapping(val.Value, range2, inclusion, paramName: null) : MustResult<DateTimeOffsetRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateTimeOffsetRange"/> does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test against.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffsetRange> ruleBuilder,
        DateTimeOffsetRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOverlapping(val, range2, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateTimeOffsetRange"/> contains the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check for containment.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange?> Contains<TModel>(this IRuleBuilder<TModel, DateTimeOffsetRange?> ruleBuilder,
        DateTimeOffset value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Contains(val.Value, value, inclusion, paramName: null) : MustResult<DateTimeOffsetRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateTimeOffsetRange"/> contains the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check for containment.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange> Contains<TModel>(this IRuleBuilder<TModel, DateTimeOffsetRange> ruleBuilder,
        DateTimeOffset value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Contains(val, value, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateTimeOffsetRange"/> does not contain the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange?> NotContains<TModel>(this IRuleBuilder<TModel, DateTimeOffsetRange?> ruleBuilder,
        DateTimeOffset value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotContains(val.Value, value, inclusion, paramName: null) : MustResult<DateTimeOffsetRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateTimeOffsetRange"/> does not contain the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value to check.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffsetRange> NotContains<TModel>(this IRuleBuilder<TModel, DateTimeOffsetRange> ruleBuilder,
        DateTimeOffset value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotContains(val, value, inclusion, paramName: null),
            message);
}
