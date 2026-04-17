#if NET8_0_OR_GREATER
using System.Globalization;
using FluentValidation;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string-to-<see cref="DateOnly"/> property validation including
/// temporal comparisons, range checks, proximity constraints, and chronological ordering on string-encoded dates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-date-only">Fluent String DateOnly Extensions documentation</seealso>
public static class FluentStringDateOnlyExtensions
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.BirthDate).PastDateOnly();</code></example>
    public static IRuleBuilderOptions<TModel, string?> PastDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.PastDateOnly(val, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> in the past or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> PastOrPresentDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.PastOrPresentDateOnly(val, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> FutureDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.FutureDateOnly(val, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> in the future or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> FutureOrPresentDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.FutureOrPresentDateOnly(val, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> BetweenDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.BetweenDateOnly(val, min, max, inclusion, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotBetweenDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotBetweenDateOnly(val, min, max, inclusion, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> within the specified number of days from a reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date.</param>
    /// <param name="days">The maximum number of days allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> WithinDaysDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly? reference,
        int days,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.WithinDaysDateOnly(val, reference, days, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not within the specified number of days from a reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date.</param>
    /// <param name="days">The day threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotWithinDaysDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly? reference,
        int days,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotWithinDaysDateOnly(val, reference, days, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> within the specified number of calendar months from a reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> WithinCalendarMonthsDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly? reference,
        int months,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.WithinCalendarMonthsDateOnly(val, reference, months, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not within the specified number of calendar months from a reference date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference date.</param>
    /// <param name="months">The calendar months threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotWithinCalendarMonthsDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly? reference,
        int months,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotWithinCalendarMonthsDateOnly(val, reference, months, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> BeforeDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.BeforeDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotBeforeDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotBeforeDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> on or before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OnOrBeforeDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.OnOrBeforeDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not on or before the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotOnOrBeforeDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotOnOrBeforeDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> AfterDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.AfterDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotAfterDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotAfterDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> on or after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OnOrAfterDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.OnOrAfterDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not on or after the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotOnOrAfterDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotOnOrAfterDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents the same <see cref="DateOnly"/> as the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> SameDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.SameDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> not the same as the specified date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The date to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotSameDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateOnly other,
        DatePrecision? precision = null,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotSameDateOnly(val, other, precision, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> that is chronologically before the specified end date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end date as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> ChronologicalDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ChronologicalDateOnly(val, end, inclusion, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> that is not chronologically before the specified end date.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end date as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotChronologicalDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotChronologicalDateOnly(val, end, inclusion, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> range that overlaps with another range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range as a string.</param>
    /// <param name="start2">The start of the second range as a string.</param>
    /// <param name="end2">The end of the second range as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OverlappingDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end1,
        string start2,
        string end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.OverlappingDateOnly(val, end1, start2, end2, inclusion, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateOnly"/> range that does not overlap with another range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range as a string.</param>
    /// <param name="start2">The start of the second range as a string.</param>
    /// <param name="end2">The end of the second range as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotOverlappingDateOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end1,
        string start2,
        string end2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotOverlappingDateOnly(val, end1, start2, end2, inclusion, styles, paramName: null) : MustResult<DateOnly>.Ok(default),
            message);
}
#endif
