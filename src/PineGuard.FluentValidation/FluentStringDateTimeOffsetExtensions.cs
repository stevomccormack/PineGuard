using System.Globalization;
using FluentValidation;
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
    private const DateTimeStyles DefaultStyles = DateTimeStyles.RoundtripKind | DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the past.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.CreatedAt).PastDateTimeOffset();</code></example>
    public static IRuleBuilderOptions<TModel, string?> PastDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.PastDateTimeOffset(val, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the past or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> PastOrPresentDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.PastOrPresentDateTimeOffset(val, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the future.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> FutureDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.FutureDateTimeOffset(val, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> in the future or present.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> FutureOrPresentDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.FutureOrPresentDateTimeOffset(val, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> BetweenDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.BetweenDateTimeOffset(val, min, max, inclusion, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> not between the specified bounds.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotBetweenDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotBetweenDateTimeOffset(val, min, max, inclusion, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The allowed time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> WithinDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        TimeSpan window,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.WithinDateTimeOffset(val, reference, window, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> not within the specified time window of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="window">The excluded time window.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotWithinDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        TimeSpan window,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotWithinDateTimeOffset(val, reference, window, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> within the specified calendar months of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The maximum number of calendar months allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> WithinCalendarMonthsDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        int months,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.WithinCalendarMonthsDateTimeOffset(val, reference, months, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);

    /// <summary>Validates that the string value represents a <see cref="DateTimeOffset"/> not within the specified calendar months of a reference.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="reference">The reference timestamp.</param>
    /// <param name="months">The calendar months threshold.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="DateTimeStyles"/> used for parsing.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotWithinCalendarMonthsDateTimeOffset<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        DateTimeOffset? reference,
        int months,
        string? message = null,
        DateTimeStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotWithinCalendarMonthsDateTimeOffset(val, reference, months, styles, paramName: null) : MustResult<DateTimeOffset>.Ok(default),
            message);
}
