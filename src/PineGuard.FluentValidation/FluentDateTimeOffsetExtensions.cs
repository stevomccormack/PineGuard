using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="DateTimeOffset"/> property validation including
/// temporal comparisons, range checks, proximity constraints, and cross-property model expression overloads.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/datetimeoffset">Fluent DateTimeOffset Extensions documentation</seealso>
public static class FluentDateTimeOffsetExtensions
{
    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.CreatedAt).Past();</code></example>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Past<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Past(val, paramName: null),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Past<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Past(val.Value, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is in the past or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> PastOrPresent<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PastOrPresent(val, paramName: null),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is in the past or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> PastOrPresent<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.PastOrPresent(val.Value, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Future<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Future(val, paramName: null),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Future<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Future(val.Value, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is in the future or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> FutureOrPresent<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FutureOrPresent(val, paramName: null),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is in the future or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> FutureOrPresent<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.FutureOrPresent(val.Value, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Between<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Between(val, min, max, inclusion, paramName: null),
            message, MustCodes.Date.Range.OutOfRange);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Between<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Between(val.Value, min, max, inclusion, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Range.OutOfRange);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> NotBetween<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotBetween(val, min, max, inclusion, paramName: null),
            message, MustCodes.Date.Range.InRange);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> NotBetween<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotBetween(val.Value, min, max, inclusion, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Range.InRange);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is before the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Before<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Before(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is before the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartedAt).Before(x => x.CompletedAt);</code></example>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Before<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.Before(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is before the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Before<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Before(val.Value, other, precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is before the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Before<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.Before(val.Value, other(model), precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is on or before the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrBefore(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is on or before the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartedAt).OnOrBefore(x => x.CompletedAt);</code></example>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrBefore(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is on or before the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.OnOrBefore(val.Value, other, precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is on or before the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> OnOrBefore<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrBefore(val.Value, other(model), precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is after the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> After<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.After(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is after the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.CompletedAt).After(x => x.StartedAt);</code></example>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> After<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.After(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is after the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> After<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.After(val.Value, other, precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is after the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> After<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.After(val.Value, other(model), precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is on or after the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrAfter(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is on or after the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.CompletedAt).OnOrAfter(x => x.StartedAt);</code></example>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrAfter(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is on or after the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.OnOrAfter(val.Value, other, precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is on or after the timestamp returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the timestamp to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> OnOrAfter<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        Func<TModel, DateTimeOffset> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrAfter(val.Value, other(model), precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is the same as the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Same<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Same(val, other, precision, paramName: null),
            message, MustCodes.Date.Equality.NotEqual);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is the same as the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Same<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Same(val.Value, other, precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Equality.NotEqual);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is not the same as the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> NotSame<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotSame(val, other, precision, paramName: null),
            message, MustCodes.Date.Equality.Equal);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is not the same as the specified timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The timestamp to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> NotSame<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotSame(val.Value, other, precision, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Equality.Equal);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is chronologically before the specified end timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end timestamp.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Chronological<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Chronological(val, end, inclusion, paramName: null),
            message, MustCodes.Date.Order.NotChronological);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is chronologically before the end timestamp resolved from the model.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="endExpression">A function that resolves the end timestamp from the model.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Chronological<TModel>(this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        Func<TModel, DateTimeOffset> endExpression,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.Chronological(val, endExpression(model), inclusion, paramName: null),
            message, MustCodes.Date.Order.NotChronological);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is chronologically before the specified end timestamp.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end timestamp.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Chronological<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Chronological(val.Value, end, inclusion, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Order.NotChronological);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> range overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset end1,
        DateTimeOffset start2,
        DateTimeOffset end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Overlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Missing);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> range overlaps with the range resolved from the model.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1Expression">A function that resolves the end of the first range from the model.</param>
    /// <param name="start2Expression">A function that resolves the start of the second range from the model.</param>
    /// <param name="end2Expression">A function that resolves the end of the second range from the model.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Overlapping<TModel>(this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        Func<TModel, DateTimeOffset> end1Expression,
        Func<TModel, DateTimeOffset> start2Expression,
        Func<TModel, DateTimeOffset> end2Expression,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.Overlapping(val, end1Expression(model), start2Expression(model), end2Expression(model), inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Missing);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> range overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Overlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset end1,
        DateTimeOffset start2,
        DateTimeOffset end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Overlapping(val.Value, end1, start2, end2, inclusion, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Overlap.Missing);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> range does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset end1,
        DateTimeOffset start2,
        DateTimeOffset end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOverlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Present);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> range does not overlap with the range resolved from the model.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1Expression">A function that resolves the end of the first range from the model.</param>
    /// <param name="start2Expression">A function that resolves the start of the second range from the model.</param>
    /// <param name="end2Expression">A function that resolves the end of the second range from the model.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> NotOverlapping<TModel>(this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        Func<TModel, DateTimeOffset> end1Expression,
        Func<TModel, DateTimeOffset> start2Expression,
        Func<TModel, DateTimeOffset> end2Expression,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.NotOverlapping(val, end1Expression(model), start2Expression(model), end2Expression(model), inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Present);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> range does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> NotOverlapping<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset end1,
        DateTimeOffset start2,
        DateTimeOffset end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotOverlapping(val.Value, end1, start2, end2, inclusion, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Overlap.Present);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> Within<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Within(val, reference, window, paramName: null),
            message, MustCodes.Date.Proximity.NotWithin);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> Within<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Within(val.Value, reference, window, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Proximity.NotWithin);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> NotWithin<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithin(val, reference, window, paramName: null),
            message, MustCodes.Date.Proximity.Within);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> NotWithin<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotWithin(val.Value, reference, window, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Proximity.Within);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is within the specified number of calendar months from a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> WithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.WithinCalendarMonths(val, reference, months, paramName: null),
            message, MustCodes.Date.Proximity.NotWithinCalendarMonths);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is within the specified number of calendar months from a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> WithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.WithinCalendarMonths(val.Value, reference, months, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Proximity.NotWithinCalendarMonths);

    /// <summary>Validates that the <see cref="DateTimeOffset"/> value is not within the specified number of calendar months from a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The calendar months threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> NotWithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        DateTimeOffset reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithinCalendarMonths(val, reference, months, paramName: null),
            message, MustCodes.Date.Proximity.WithinCalendarMonths);

    /// <summary>Validates that the nullable <see cref="DateTimeOffset"/> value is not within the specified number of calendar months from a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The calendar months threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> NotWithinCalendarMonths<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        DateTimeOffset reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotWithinCalendarMonths(val.Value, reference, months, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Proximity.WithinCalendarMonths);

}
