#if NET8_0_OR_GREATER
using FluentValidation;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="DateOnlyRange"/> property validation including
/// chronological ordering, overlap detection, and containment checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/date-only-range">Fluent DateOnly Range Extensions documentation</seealso>
public static class FluentDateOnlyRangeExtensions
{
    /// <summary>Validates that the nullable <see cref="DateOnlyRange"/> is chronologically ordered (start before end).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnlyRange?> Chronological<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange?> ruleBuilder,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Chronological(val.Value, inclusion, paramName: null) : MustResult<DateOnlyRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateOnlyRange"/> is chronologically ordered (start before end).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.DateRange).Chronological();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnlyRange> Chronological<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange> ruleBuilder,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Chronological(val, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateOnlyRange"/> overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test for overlap.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnlyRange?> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange?> ruleBuilder,
        DateOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Overlapping(val.Value, range2, inclusion, paramName: null) : MustResult<DateOnlyRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateOnlyRange"/> overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test for overlap.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.DateRange).Overlapping(otherRange);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnlyRange> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange> ruleBuilder,
        DateOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Overlapping(val, range2, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateOnlyRange"/> does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test against.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnlyRange?> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange?> ruleBuilder,
        DateOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOverlapping(val.Value, range2, inclusion, paramName: null) : MustResult<DateOnlyRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateOnlyRange"/> does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="range2">The range to test against.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnlyRange> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange> ruleBuilder,
        DateOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOverlapping(val, range2, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateOnlyRange"/> contains the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The date to check for containment.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnlyRange?> Contains<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange?> ruleBuilder,
        DateOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Contains(val.Value, value, inclusion, paramName: null) : MustResult<DateOnlyRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateOnlyRange"/> contains the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The date to check for containment.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.DateRange).Contains(targetDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnlyRange> Contains<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange> ruleBuilder,
        DateOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Contains(val, value, inclusion, paramName: null),
            message);

    /// <summary>Validates that the nullable <see cref="DateOnlyRange"/> does not contain the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The date to check.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnlyRange?> NotContains<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange?> ruleBuilder,
        DateOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotContains(val.Value, value, inclusion, paramName: null) : MustResult<DateOnlyRange>.Ok(default),
            message);

    /// <summary>Validates that the <see cref="DateOnlyRange"/> does not contain the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The date to check.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnlyRange> NotContains<TModel>(
        this IRuleBuilder<TModel, DateOnlyRange> ruleBuilder,
        DateOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotContains(val, value, inclusion, paramName: null),
            message);
}
#endif
