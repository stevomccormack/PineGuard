using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="DateTime"/> property validation including
/// temporal comparisons, range checks, proximity constraints, day-of-week checks, and <see cref="DateTimeKind"/> validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/datetime">Fluent DateTime Extensions documentation</seealso>
public static class FluentDateTimeExtensions
{
    /// <summary>Validates that the <see cref="DateTime"/> value is in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.CreatedAt).Past();</code></example>
    public static IRuleBuilderOptions<TModel, DateTime> Past<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Past(val, paramName: null),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the <see cref="DateTime"/> value is in the past or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> PastOrPresent<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PastOrPresent(val, paramName: null),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the <see cref="DateTime"/> value is in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Future<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Future(val, paramName: null),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the <see cref="DateTime"/> value is in the future or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> FutureOrPresent<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FutureOrPresent(val, paramName: null),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the <see cref="DateTime"/> value is between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Between<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime min,
        DateTime max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Between(val, min, max, inclusion, paramName: null),
            message, MustCodes.Date.Range.OutOfRange);

    /// <summary>Validates that the <see cref="DateTime"/> value is not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotBetween<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime min,
        DateTime max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotBetween(val, min, max, inclusion, paramName: null),
            message, MustCodes.Date.Range.InRange);

    /// <summary>Validates that the <see cref="DateTime"/> value is before the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Before<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Before(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the <see cref="DateTime"/> value is before the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartDate).Before(x => x.EndDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateTime> Before<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.Before(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the nullable <see cref="DateTime"/> value is before the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTime?> Before<TModel>(this IRuleBuilder<TModel, DateTime?> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.Before(val.Value, other(model), precision, paramName: null) : MustResult<DateTime>.Ok(default),
            message, MustCodes.Date.Order.NotBefore);

    /// <summary>Validates that the <see cref="DateTime"/> value is on or before the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> OnOrBefore<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrBefore(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the <see cref="DateTime"/> value is on or before the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.StartDate).OnOrBefore(x => x.EndDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateTime> OnOrBefore<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrBefore(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the nullable <see cref="DateTime"/> value is on or before the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTime?> OnOrBefore<TModel>(this IRuleBuilder<TModel, DateTime?> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrBefore(val.Value, other(model), precision, paramName: null) : MustResult<DateTime>.Ok(default),
            message, MustCodes.Date.Order.After);

    /// <summary>Validates that the <see cref="DateTime"/> value is after the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> After<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.After(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the <see cref="DateTime"/> value is after the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EndDate).After(x => x.StartDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateTime> After<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.After(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the nullable <see cref="DateTime"/> value is after the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTime?> After<TModel>(this IRuleBuilder<TModel, DateTime?> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.After(val.Value, other(model), precision, paramName: null) : MustResult<DateTime>.Ok(default),
            message, MustCodes.Date.Order.NotAfter);

    /// <summary>Validates that the <see cref="DateTime"/> value is on or after the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> OnOrAfter<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OnOrAfter(val, other, precision, paramName: null),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the <see cref="DateTime"/> value is on or after the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <example><code>RuleFor(x => x.EndDate).OnOrAfter(x => x.StartDate);</code></example>
    public static IRuleBuilderOptions<TModel, DateTime> OnOrAfter<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => Must.Be.OnOrAfter(val, other(model), precision, paramName: null),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the nullable <see cref="DateTime"/> value is on or after the date-time returned by <paramref name="other"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">A function that returns the date-time to compare against, evaluated against the model instance.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, DateTime?> OnOrAfter<TModel>(this IRuleBuilder<TModel, DateTime?> ruleBuilder,
        Func<TModel, DateTime> other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe((model, val) => val.HasValue ? Must.Be.OnOrAfter(val.Value, other(model), precision, paramName: null) : MustResult<DateTime>.Ok(default),
            message, MustCodes.Date.Order.Before);

    /// <summary>Validates that the <see cref="DateTime"/> value is the same as the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Same<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Same(val, other, precision, paramName: null),
            message, MustCodes.Date.Equality.NotEqual);

    /// <summary>Validates that the <see cref="DateTime"/> value is not the same as the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotSame<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        DateTimePrecision? precision = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotSame(val, other, precision, paramName: null),
            message, MustCodes.Date.Equality.Equal);

    /// <summary>Validates that the <see cref="DateTime"/> value is chronologically before the specified end date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end date-time.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Chronological<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Chronological(val, end, inclusion, paramName: null),
            message, MustCodes.Date.Order.NotChronological);

    /// <summary>Validates that the <see cref="DateTime"/> range overlaps with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Overlapping<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime end1,
        DateTime start2,
        DateTime end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Overlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Missing);

    /// <summary>Validates that the <see cref="DateTime"/> range does not overlap with the specified range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotOverlapping<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime end1,
        DateTime start2,
        DateTime end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOverlapping(val, end1, start2, end2, inclusion, paramName: null),
            message, MustCodes.Date.Overlap.Present);

    /// <summary>Validates that the <see cref="DateTime"/> value is within the specified number of days from now.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="days">The maximum number of days allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> WithinDaysFromNow<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        int days,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.WithinDaysFromNow(val, days, paramName: null),
            message, MustCodes.Date.Proximity.NotWithin);

    /// <summary>Validates that the <see cref="DateTime"/> value is not within the specified number of days from now.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="days">The day threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotWithinDaysFromNow<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        int days,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithinDaysFromNow(val, days, paramName: null),
            message, MustCodes.Date.Proximity.Within);

    /// <summary>Validates that the <see cref="DateTime"/> value is within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date-time.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Within<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Within(val, reference, window, paramName: null),
            message, MustCodes.Date.Proximity.NotWithin);

    /// <summary>Validates that the <see cref="DateTime"/> value is not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date-time.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotWithin<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime reference,
        TimeSpan window,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithin(val, reference, window, paramName: null),
            message, MustCodes.Date.Proximity.Within);

    /// <summary>Validates that the <see cref="DateTime"/> value is within the specified number of calendar months from a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date-time.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> WithinCalendarMonths<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.WithinCalendarMonths(val, reference, months, paramName: null),
            message, MustCodes.Date.Proximity.NotWithinCalendarMonths);

    /// <summary>Validates that the <see cref="DateTime"/> value is not within the specified number of calendar months from a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date-time.</param>
    /// <param name="months">The calendar months threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotWithinCalendarMonths<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime reference,
        int months,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotWithinCalendarMonths(val, reference, months, paramName: null),
            message, MustCodes.Date.Proximity.WithinCalendarMonths);

    /// <summary>Validates that the <see cref="DateTime"/> value falls on a weekday (Monday through Friday).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Weekday<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Weekday(val, paramName: null),
            message, MustCodes.Date.Calendar.NotWeekday);

    /// <summary>Validates that the <see cref="DateTime"/> value falls on a weekend (Saturday or Sunday).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Weekend<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Weekend(val, paramName: null),
            message, MustCodes.Date.Calendar.NotWeekend);

    /// <summary>Validates that the <see cref="DateTime"/> value is the first day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> FirstDayOfMonth<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FirstDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.NotFirstDayOfMonth);

    /// <summary>Validates that the <see cref="DateTime"/> value is not the first day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotFirstDayOfMonth<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotFirstDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.FirstDayOfMonth);

    /// <summary>Validates that the <see cref="DateTime"/> value is the last day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> LastDayOfMonth<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.LastDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.NotLastDayOfMonth);

    /// <summary>Validates that the <see cref="DateTime"/> value is not the last day of its month.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotLastDayOfMonth<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotLastDayOfMonth(val, paramName: null),
            message, MustCodes.Date.Calendar.LastDayOfMonth);

    /// <summary>Validates that the <see cref="DateTime"/> value falls on the same calendar day as the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> SameDay<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.SameDay(val, other, paramName: null),
            message, MustCodes.Date.Equality.NotSameDay);

    /// <summary>Validates that the <see cref="DateTime"/> value does not fall on the same calendar day as the specified date-time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date-time to compare against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotSameDay<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder,
        DateTime other,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotSameDay(val, other, paramName: null),
            message, MustCodes.Date.Equality.SameDay);

    /// <summary>Validates that the <see cref="DateTime"/> value has <see cref="DateTimeKind.Utc"/> kind.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Utc<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Utc(val, paramName: null),
            message, MustCodes.Date.Kind.NotUtc);

    /// <summary>Validates that the <see cref="DateTime"/> value does not have <see cref="DateTimeKind.Utc"/> kind.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotUtc<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotUtc(val, paramName: null),
            message, MustCodes.Date.Kind.Utc);

    /// <summary>Validates that the <see cref="DateTime"/> value has <see cref="DateTimeKind.Local"/> kind.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Local<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Local(val, paramName: null),
            message, MustCodes.Date.Kind.NotLocal);

    /// <summary>Validates that the <see cref="DateTime"/> value does not have <see cref="DateTimeKind.Local"/> kind.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotLocal<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotLocal(val, paramName: null),
            message, MustCodes.Date.Kind.Local);

    /// <summary>Validates that the <see cref="DateTime"/> value has <see cref="DateTimeKind.Unspecified"/> kind.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> Unspecified<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Unspecified(val, paramName: null),
            message, MustCodes.Date.Kind.NotUnspecified);

    /// <summary>Validates that the <see cref="DateTime"/> value does not have <see cref="DateTimeKind.Unspecified"/> kind.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotUnspecified<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotUnspecified(val, paramName: null),
            message, MustCodes.Date.Kind.Unspecified);

    /// <summary>Validates that the <see cref="DateTime"/> value has an explicit <see cref="DateTimeKind"/> (not <see cref="DateTimeKind.Unspecified"/>).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> ExplicitKind<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.ExplicitKind(val, paramName: null),
            message, MustCodes.Date.Kind.Unspecified);

    /// <summary>Validates that the <see cref="DateTime"/> value does not have an explicit <see cref="DateTimeKind"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, DateTime> NotExplicitKind<TModel>(this IRuleBuilder<TModel, DateTime> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotExplicitKind(val, paramName: null),
            message, MustCodes.Date.Kind.NotUnspecified);
}
