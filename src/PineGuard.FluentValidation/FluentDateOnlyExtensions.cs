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
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.BirthDate).Past();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> Past<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Past(val, paramName: null),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Past<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Past(val.Value, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the <see cref="DateOnly"/> value is in the past or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartDate).PastOrPresent();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> PastOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PastOrPresent(val, paramName: null),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the past or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> PastOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.PastOrPresent(val.Value, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the <see cref="DateOnly"/> value is in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.ExpiryDate).Future();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> Future<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Future(val, paramName: null),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> Future<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Future(val.Value, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the <see cref="DateOnly"/> value is in the future or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.ValidUntil).FutureOrPresent();</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> FutureOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FutureOrPresent(val, paramName: null),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the nullable <see cref="DateOnly"/> value is in the future or is today.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateOnly?> FutureOrPresent<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.FutureOrPresent(val.Value, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the <see cref="DateOnly"/> value falls between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the date range.</param>
    /// <param name="max">The upper bound of the date range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EventDate).Between(min, max);</code></example>
    public static IRuleBuilderOptions<TModel, DateOnly> Between<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Between(val, min, max, inclusion, paramName: null),
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
    public static IRuleBuilderOptions<TModel, DateOnly?> Between<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Between(val.Value, min, max, inclusion, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Range.OutOfRange);

    /// <summary>Validates that the <see cref="DateOnly"/> value does not fall between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateOnly> NotBetween<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotBetween(val, min, max, inclusion, paramName: null),
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
    public static IRuleBuilderOptions<TModel, DateOnly?> NotBetween<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotBetween(val.Value, min, max, inclusion, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Before(val.Value, other, precision, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.OnOrBefore(val.Value, other, precision, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.After(val.Value, other, precision, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.OnOrAfter(val.Value, other, precision, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Same(val.Value, other, precision, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotSame(val.Value, other, precision, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Chronological(val.Value, end, inclusion, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotChronological(val.Value, end, inclusion, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        Inclusion inclusion = Inclusion.Exclusive,
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
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Overlapping(val.Value, end1, start2, end2, inclusion, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        Inclusion inclusion = Inclusion.Exclusive,
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
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOverlapping(val.Value, end1, start2, end2, inclusion, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.WithinDays(val.Value, reference, days, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotWithinDays(val.Value, reference, days, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.WithinCalendarMonths(val.Value, reference, months, paramName: null) : MustResult<DateOnly>.Ok(default),
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
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotWithinCalendarMonths(val.Value, reference, months, paramName: null) : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Proximity.WithinCalendarMonths);
}
#endif
