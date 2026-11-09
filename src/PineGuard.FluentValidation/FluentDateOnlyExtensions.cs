#if NET8_0_OR_GREATER
using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="DateOnly"/> property validation including
/// temporal comparisons, range checks, and proximity constraints.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/date-only">Fluent DateOnly Extensions documentation</seealso>
public static class FluentDateOnlyExtensions
{
    /// <summary>Validates that the <see cref="DateOnly"/> value is in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.BirthDate).Past();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> Past<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Past(val, timeProvider, paramName: null),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Past<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Past(val.GetValueOrDefault(), timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the <see cref="DateOnly"/> value is in the past or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartDate).PastOrPresent();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> PastOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PastOrPresent(val, timeProvider, paramName: null),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the past or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> PastOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.PastOrPresent(val.GetValueOrDefault(), timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the <see cref="DateOnly"/> value is in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.ExpiryDate).Future();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> Future<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Future(val, timeProvider, paramName: null),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Future<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Future(val.GetValueOrDefault(), timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the <see cref="DateOnly"/> value is in the future or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.ValidUntil).FutureOrPresent();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> FutureOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FutureOrPresent(val, timeProvider, paramName: null),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the future or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> FutureOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.FutureOrPresent(val.GetValueOrDefault(), timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the <see cref="DateOnly"/> value falls between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the date range.</param>
    /// <param name="max">The upper bound of the date range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EventDate).InRange(min, max);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> InRange<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.InRange(val, min, max, inclusion, paramName: null),
            message, MustCodes.Date.Range.OutOfRange);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value falls between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the date range.</param>
    /// <param name="max">The upper bound of the date range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> InRange<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.InRange(val.GetValueOrDefault(), min, max, inclusion, paramName: null)),
            message, MustCodes.Date.Range.OutOfRange);

    /// <summary>Validates that the <see cref="DateOnly"/> value does not fall between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> OutOfRange<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OutOfRange(val, min, max, inclusion, paramName: null),
            message, MustCodes.Date.Range.InRange);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value does not fall between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> OutOfRange<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OutOfRange(val.GetValueOrDefault(), min, max, inclusion, paramName: null)),
            message, MustCodes.Date.Range.InRange);

    /// <summary>Validates that the <see cref="DateOnly"/> value is before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartDate).Before(cutoffDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> Before<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Before(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the <see cref="DateOnly"/> value is before the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartDate).Before(x => x.EndDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> Before<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.Before(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Before<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Before(val.GetValueOrDefault(), other, precision, paramName: null)),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is before the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Before<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.Before(val.Value, other(model), precision, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the <see cref="DateOnly"/> value is on or before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrBefore(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the <see cref="DateOnly"/> value is on or before the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartDate).OnOrBefore(x => x.EndDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrBefore(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is on or before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OnOrBefore(val.GetValueOrDefault(), other, precision, paramName: null)),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is on or before the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrBefore(val.Value, other(model), precision, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the <see cref="DateOnly"/> value is after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EndDate).After(startDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> After<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.After(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the <see cref="DateOnly"/> value is after the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EndDate).After(x => x.StartDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> After<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.After(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> After<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.After(val.GetValueOrDefault(), other, precision, paramName: null)),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is after the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> After<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.After(val.Value, other(model), precision, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the <see cref="DateOnly"/> value is on or after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrAfter(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the <see cref="DateOnly"/> value is on or after the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EndDate).OnOrAfter(x => x.StartDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrAfter(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is on or after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OnOrAfter(val.GetValueOrDefault(), other, precision, paramName: null)),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is on or after the date returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        Func<TModel, DateOnly> other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrAfter(val.Value, other(model), precision, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the <see cref="DateOnly"/> value is the same as the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> Same<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Same(val, other, precision, paramName: null),
            message, MustCodes.Date.Equality.NotEqual);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is the same as the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Same<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Same(val.GetValueOrDefault(), other, precision, paramName: null)),
            message, MustCodes.Date.Equality.NotEqual);

    /// <summary>Validates that the <see cref="DateOnly"/> value is not the same as the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotSame<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotSame(val, other, precision, paramName: null),
            message, MustCodes.Date.Equality.Equal);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is not the same as the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for the comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> NotSame<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotSame(val.GetValueOrDefault(), other, precision, paramName: null)),
            message, MustCodes.Date.Equality.Equal);

    /// <summary>Validates that the <see cref="DateOnly"/> value is chronologically before the end date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end date that the value must precede.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> Chronological<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Chronological(val, end, inclusion, paramName: null),
            message, MustCodes.Date.Order.NotChronological);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is chronologically before the end date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end date that the value must precede.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Chronological<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Chronological(val.GetValueOrDefault(), end, inclusion, paramName: null)),
            message, MustCodes.Date.Order.NotChronological);

    /// <summary>Validates that the <see cref="DateOnly"/> value is not chronologically before the end date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end date.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotChronological<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotChronological(val, end, inclusion, paramName: null),
            message, MustCodes.Date.Order.Chronological);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is not chronologically before the end date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end date.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> NotChronological<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotChronological(val.GetValueOrDefault(), end, inclusion, paramName: null)),
            message, MustCodes.Date.Order.Chronological);

    /// <summary>Validates that the date range <c>[value..end1]</c> overlaps with <c>[start2..end2]</c>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range (value is the start).</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly end1,
        DateOnly start2,
        DateOnly end2,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Overlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Missing);

    /// <summary>Validates that the nullable date range <c>[value..end1]</c> overlaps with <c>[start2..end2]</c>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range (value is the start).</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly end1,
        DateOnly start2,
        DateOnly end2,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Overlapping(val.GetValueOrDefault(), end1, start2, end2, inclusion, paramName: null)),
            message, MustCodes.Date.Overlap.Missing);

    /// <summary>Validates that the date range <c>[value..end1]</c> does not overlap with <c>[start2..end2]</c>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range (value is the start).</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly end1,
        DateOnly start2,
        DateOnly end2,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOverlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Present);

    /// <summary>Validates that the nullable date range <c>[value..end1]</c> does not overlap with <c>[start2..end2]</c>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range (value is the start).</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly end1,
        DateOnly start2,
        DateOnly end2,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotOverlapping(val.GetValueOrDefault(), end1, start2, end2, inclusion, paramName: null)),
            message, MustCodes.Date.Overlap.Present);

    /// <summary>Validates that the <see cref="DateOnly"/> value is within the specified number of days from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="days">The maximum number of days allowed from the reference.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> WithinDays<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly reference,
        int days,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.WithinDays(val, reference, days, paramName: null),
            message, MustCodes.Date.Proximity.NotWithin);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is within the specified number of days from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="days">The maximum number of days allowed from the reference.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> WithinDays<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly reference,
        int days,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.WithinDays(val.GetValueOrDefault(), reference, days, paramName: null)),
            message, MustCodes.Date.Proximity.NotWithin);

    /// <summary>Validates that the <see cref="DateOnly"/> value is not within the specified number of days from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="days">The number of days threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotWithinDays<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly reference,
        int days,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithinDays(val, reference, days, paramName: null),
            message, MustCodes.Date.Proximity.Within);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is not within the specified number of days from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="days">The number of days threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> NotWithinDays<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly reference,
        int days,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotWithinDays(val.GetValueOrDefault(), reference, days, paramName: null)),
            message, MustCodes.Date.Proximity.Within);

    /// <summary>Validates that the <see cref="DateOnly"/> value is within the specified number of calendar months from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> WithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.WithinCalendarMonths(val, reference, months, paramName: null),
            message, MustCodes.Date.Proximity.NotWithinCalendarMonths);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is within the specified number of calendar months from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> WithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.WithinCalendarMonths(val.GetValueOrDefault(), reference, months, paramName: null)),
            message, MustCodes.Date.Proximity.NotWithinCalendarMonths);

    /// <summary>Validates that the <see cref="DateOnly"/> value is not within the specified number of calendar months from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="months">The number of calendar months threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotWithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithinCalendarMonths(val, reference, months, paramName: null),
            message, MustCodes.Date.Proximity.WithinCalendarMonths);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is not within the specified number of calendar months from the reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date to measure from.</param>
    /// <param name="months">The number of calendar months threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> NotWithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotWithinCalendarMonths(val.GetValueOrDefault(), reference, months, paramName: null)),
            message, MustCodes.Date.Proximity.WithinCalendarMonths);

    /// <summary>Validates that the <see cref="DateOnly"/> value falls on a weekday (Monday through Friday).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> Weekday<TModel>(this IRuleBuilder<TModel, DateOnly> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Weekday(val, paramName: null),
            message, MustCodes.Date.Calendar.NotWeekday);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value falls on a weekday (Monday through Friday).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Weekday<TModel>(this IRuleBuilder<TModel, DateOnly?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Weekday(val.GetValueOrDefault(), paramName: null)),
            message, MustCodes.Date.Calendar.NotWeekday);

    /// <summary>Validates that the <see cref="DateOnly"/> value falls on a weekend (Saturday or Sunday).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> Weekend<TModel>(this IRuleBuilder<TModel, DateOnly> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Weekend(val, paramName: null),
            message, MustCodes.Date.Calendar.NotWeekend);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value falls on a weekend (Saturday or Sunday).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Weekend<TModel>(this IRuleBuilder<TModel, DateOnly?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Weekend(val.GetValueOrDefault(), paramName: null)),
            message, MustCodes.Date.Calendar.NotWeekend);

    /// <summary>Validates that the <see cref="DateOnly"/> value is the first day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> FirstDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FirstDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.NotFirstDayOfMonth);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is the first day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> FirstDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.FirstDayOfMonth(val.GetValueOrDefault(), paramName: null)),
            message, MustCodes.Date.Calendar.NotFirstDayOfMonth);

    /// <summary>Validates that the <see cref="DateOnly"/> value is not the first day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotFirstDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotFirstDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.FirstDayOfMonth);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is not the first day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> NotFirstDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotFirstDayOfMonth(val.GetValueOrDefault(), paramName: null)),
            message, MustCodes.Date.Calendar.FirstDayOfMonth);

    /// <summary>Validates that the <see cref="DateOnly"/> value is the last day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> LastDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.LastDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.NotLastDayOfMonth);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is the last day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> LastDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.LastDayOfMonth(val.GetValueOrDefault(), paramName: null)),
            message, MustCodes.Date.Calendar.NotLastDayOfMonth);

    /// <summary>Validates that the <see cref="DateOnly"/> value is not the last day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotLastDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotLastDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.LastDayOfMonth);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is not the last day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> NotLastDayOfMonth<TModel>(this IRuleBuilder<TModel, DateOnly?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotLastDayOfMonth(val.GetValueOrDefault(), paramName: null)),
            message, MustCodes.Date.Calendar.LastDayOfMonth);

    /// <summary>Validates that the <see cref="DateOnly"/> date of birth meets the expected minimum age.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="years">The minimum age in whole years.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>A negative <paramref name="years"/> is a configuration error, reported against that parameter.</remarks>
    /// <example><code>RuleFor(x => x.DateOfBirth).MinimumAge(18);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> MinimumAge<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        int years,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.MinimumAge(val, years, timeProvider, paramName: null),
            message, MustCodes.Date.Age.BelowMinimum);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> date of birth meets the expected minimum age.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="years">The minimum age in whole years.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes. A negative <paramref name="years"/> is a configuration error, reported against that parameter.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> MinimumAge<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        int years,
        TimeProvider? timeProvider = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.MinimumAge(val.GetValueOrDefault(), years, timeProvider, paramName: null)),
            message, MustCodes.Date.Age.BelowMinimum);
    private static MustResult<T> SkipAbsentValue<T>(T? value, MustResult<T> result) where T : struct =>
        value is null && result.ParamName is null ? MustResult<T>.Ok(default) : result;
}
#endif
