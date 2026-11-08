using System.Globalization;
using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string-to-<see cref="DateTimeOffset"/> property validation including
/// temporal comparisons, range checks, and proximity constraints on string-encoded timestamps.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-datetimeoffset">Fluent String DateTimeOffset Extensions documentation</seealso>
public static class FluentStringDateTimeOffsetExtensions
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.CreatedAt).PastDateTimeOffset();</code></example>
    public static IRuleBuilderOptions<TModel, string?> PastDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeProvider? timeProvider = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.PastDateTimeOffset(val, styles, timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.NotPast);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the past or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> PastOrPresentDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeProvider? timeProvider = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.PastOrPresentDateTimeOffset(val, styles, timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.Future);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> FutureDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeProvider? timeProvider = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.FutureDateTimeOffset(val, styles, timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.NotFuture);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the future or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> FutureOrPresentDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        TimeProvider? timeProvider = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.FutureOrPresentDateTimeOffset(val, styles, timeProvider, paramName: null)),
            message, MustCodes.Date.Relative.Past);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> InRangeDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.InRangeDateTimeOffset(val, min, max, inclusion, styles, paramName: null)),
            message, MustCodes.Date.Range.OutOfRange);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> OutOfRangeDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OutOfRangeDateTimeOffset(val, min, max, inclusion, styles, paramName: null)),
            message, MustCodes.Date.Range.InRange);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> WithinDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.WithinDateTimeOffset(val, reference, window, styles, paramName: null)),
            message, MustCodes.Date.Proximity.NotWithin);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotWithinDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotWithinDateTimeOffset(val, reference, window, styles, paramName: null)),
            message, MustCodes.Date.Proximity.Within);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> within the specified calendar months of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> WithinCalendarMonthsDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        int months,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.WithinCalendarMonthsDateTimeOffset(val, reference, months, styles, paramName: null)),
            message, MustCodes.Date.Proximity.NotWithinCalendarMonths);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> not within the specified calendar months of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The calendar months threshold.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotWithinCalendarMonthsDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        int months,
        DateTimeStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotWithinCalendarMonthsDateTimeOffset(val, reference, months, styles, paramName: null)),
            message, MustCodes.Date.Proximity.WithinCalendarMonths);
    private static MustResult<T> SkipAbsentValue<T>(string? value, MustResult<T> result) =>
        value is null && result.ParamName is null ? MustResult<T>.Ok(default) : result;
}
