#if NET8_0_OR_GREATER
using System.Globalization;
using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string-to-<see cref="TimeOnly"/> property validation including
/// temporal comparisons, range checks, proximity constraints, and chronological ordering on string-encoded times.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-time-only">Fluent String TimeOnly Extensions documentation</seealso>
public static class FluentStringTimeOnlyExtensions
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.StartTime).InRangeTimeOnly(new TimeOnly(9, 0), new TimeOnly(17, 0));</code></example>
    public static IRuleBuilderOptions<TModel, string?> InRangeTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.InRangeTimeOnly(val, min, max, inclusion, styles, paramName: null)),
            message, MustCodes.Time.Range.OutOfRange);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OutOfRangeTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OutOfRangeTimeOnly(val, min, max, inclusion, styles, paramName: null)),
            message, MustCodes.Time.Range.InRange);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference time as a string.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> WithinTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.WithinTimeOnly(val, reference, window, styles, paramName: null)),
            message, MustCodes.Time.Proximity.NotWithin);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference time as a string.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotWithinTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotWithinTimeOnly(val, reference, window, styles, paramName: null)),
            message, MustCodes.Time.Proximity.Within);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> BeforeTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.BeforeTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> not before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotBeforeTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotBeforeTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> on or before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OnOrBeforeTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OnOrBeforeTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> not on or before the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotOnOrBeforeTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotOnOrBeforeTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> AfterTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.AfterTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.NotAfter);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> not after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotAfterTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotAfterTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.After);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> on or after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OnOrAfterTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OnOrAfterTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.Before);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> not on or after the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotOnOrAfterTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotOnOrAfterTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Order.NotBefore);

    /// <summary>Validates that the string value represents the same <see cref="TimeOnly"/> as the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> SameTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.SameTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Equality.NotEqual);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> not the same as the specified time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The time to compare against.</param>
    /// <param name="precision">The optional precision for comparison.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotSameTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotSameTimeOnly(val, other, precision, styles, paramName: null)),
            message, MustCodes.Time.Equality.Equal);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> that is chronologically before the specified end time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end time as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> ChronologicalTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.ChronologicalTimeOnly(val, end, inclusion, styles, paramName: null)),
            message, MustCodes.Time.Order.NotChronological);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> that is not chronologically before the specified end time.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end">The end time as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotChronologicalTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotChronologicalTimeOnly(val, end, inclusion, styles, paramName: null)),
            message, MustCodes.Time.Order.Chronological);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> range that overlaps with another range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range as a string.</param>
    /// <param name="start2">The start of the second range as a string.</param>
    /// <param name="end2">The end of the second range as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OverlappingTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end1,
        string start2,
        string end2,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OverlappingTimeOnly(val, end1, start2, end2, inclusion, styles, paramName: null)),
            message, MustCodes.Time.Overlap.Missing);

    /// <summary>Validates that the string value represents a <see cref="TimeOnly"/> range that does not overlap with another range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="end1">The end of the first range as a string.</param>
    /// <param name="start2">The start of the second range as a string.</param>
    /// <param name="end2">The end of the second range as a string.</param>
    /// <param name="inclusion">Whether the comparison is inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotOverlappingTimeOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string end1,
        string start2,
        string end2,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotOverlappingTimeOnly(val, end1, start2, end2, inclusion, styles, paramName: null)),
            message, MustCodes.Time.Overlap.Present);
    private static MustResult<T> SkipAbsentValue<T>(string? value, MustResult<T> result) =>
        value is null && result.ParamName is null ? MustResult<T>.Ok(default) : result;
}
#endif
