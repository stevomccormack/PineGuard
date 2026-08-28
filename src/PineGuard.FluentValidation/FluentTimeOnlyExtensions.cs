#if NET8_0_OR_GREATER
using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="TimeOnly"/> property validation including
/// temporal comparisons, range checks, proximity constraints, and chronological ordering.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/time-only">Fluent TimeOnly Extensions documentation</seealso>
public static class FluentTimeOnlyExtensions
{
    /// <summary>Validates that the <see cref="TimeOnly"/> value is between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartTime).Between(new TimeOnly(9, 0), new TimeOnly(17, 0));</code></example>
    public static IRuleBuilderOptions<TModel, TimeOnly> Between<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Between(val, min, max, inclusion, paramName: null),
            message, MustCodes.Time.Range.OutOfRange);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> Between<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Between(val.Value, min, max, inclusion, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Range.OutOfRange);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotBetween<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotBetween(val, min, max, inclusion, paramName: null),
            message, MustCodes.Time.Range.InRange);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotBetween<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotBetween(val.Value, min, max, inclusion, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Range.InRange);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> Before<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Before(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is before the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartTime).Before(x => x.EndTime);</code></example>
    public static IRuleBuilderOptions<TModel, TimeOnly> Before<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.Before(val, other(model), precision, paramName: null),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> Before<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Before(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is before the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> Before<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.Before(val.Value, other(model), precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotBefore(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotBefore(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is on or before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrBefore(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is on or before the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartTime).OnOrBefore(x => x.EndTime);</code></example>
    public static IRuleBuilderOptions<TModel, TimeOnly> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrBefore(val, other(model), precision, paramName: null),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is on or before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.OnOrBefore(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is on or before the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrBefore(val.Value, other(model), precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not on or before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotOnOrBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOnOrBefore(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not on or before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotOnOrBefore<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOnOrBefore(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> After<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.After(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is after the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EndTime).After(x => x.StartTime);</code></example>
    public static IRuleBuilderOptions<TModel, TimeOnly> After<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.After(val, other(model), precision, paramName: null),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> After<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.After(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is after the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> After<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.After(val.Value, other(model), precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotAfter(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotAfter(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is on or after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrAfter(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is on or after the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EndTime).OnOrAfter(x => x.StartTime);</code></example>
    public static IRuleBuilderOptions<TModel, TimeOnly> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrAfter(val, other(model), precision, paramName: null),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is on or after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.OnOrAfter(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is on or after the time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        Func<TModel, TimeOnly> other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrAfter(val.Value, other(model), precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not on or after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotOnOrAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOnOrAfter(val, other, precision, paramName: null),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not on or after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotOnOrAfter<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOnOrAfter(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is the same as the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> Same<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Same(val, other, precision, paramName: null),
            message, MustCodes.Time.Equality.NotEqual);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is the same as the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> Same<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Same(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Equality.NotEqual);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not the same as the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotSame<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotSame(val, other, precision, paramName: null),
            message, MustCodes.Time.Equality.Equal);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not the same as the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotSame<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotSame(val.Value, other, precision, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Equality.Equal);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference time.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> Within<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Within(val, reference, window, paramName: null),
            message, MustCodes.Time.Proximity.NotWithin);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference time.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> Within<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Within(val.Value, reference, window, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Proximity.NotWithin);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference time.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotWithin<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithin(val, reference, window, paramName: null),
            message, MustCodes.Time.Proximity.Within);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference time.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotWithin<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotWithin(val.Value, reference, window, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Proximity.Within);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is chronologically before the specified end time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end time.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> Chronological<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Chronological(val, end, inclusion, paramName: null),
            message, MustCodes.Time.Order.NotChronological);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is chronologically before the specified end time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end time.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> Chronological<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Chronological(val.Value, end, inclusion, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.NotChronological);

    /// <summary>Validates that the <see cref="TimeOnly"/> value is not chronologically before the specified end time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end time.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotChronological<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotChronological(val, end, inclusion, paramName: null),
            message, MustCodes.Time.Order.Chronological);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> value is not chronologically before the specified end time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end time.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotChronological<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotChronological(val.Value, end, inclusion, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Order.Chronological);

    /// <summary>Validates that the <see cref="TimeOnly"/> range overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> Overlapping<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly end1,
        TimeOnly start2,
        TimeOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Overlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Time.Overlap.Missing);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> range overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> Overlapping<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly end1,
        TimeOnly start2,
        TimeOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Overlapping(val.Value, end1, start2, end2, inclusion, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Overlap.Missing);

    /// <summary>Validates that the <see cref="TimeOnly"/> range does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, TimeOnly> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, TimeOnly> ruleBuilder,
        TimeOnly end1,
        TimeOnly start2,
        TimeOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOverlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Time.Overlap.Present);

    /// <summary>Validates that the nullable <see cref="TimeOnly"/> range does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, TimeOnly?> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, TimeOnly?> ruleBuilder,
        TimeOnly end1,
        TimeOnly start2,
        TimeOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOverlapping(val.Value, end1, start2, end2, inclusion, paramName: null) : MustResult<TimeOnly>.Ok(default),
            message, MustCodes.Time.Overlap.Present);
}
#endif
