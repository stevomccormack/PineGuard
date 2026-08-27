#if NET8_0_OR_GREATER
using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate date-only string representations,
/// parsing the input string before delegating to date-only rules.
/// </summary>
/// <seealso cref="DateOnlyRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
public static class MustStringDateOnlyClauses
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified value must be a date in the past.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date in the past."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> PastDateOnly(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Relative.NotPast, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date in the past.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsInPast(parsedValue);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.NotPast, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date in the past or present.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date in the past or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> PastOrPresentDateOnly(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Relative.Future, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date in the past or present.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsInPast(parsedValue, Inclusion.Inclusive);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.Future, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date in the future.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date in the future."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> FutureDateOnly(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Relative.NotFuture, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date in the future.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsInFuture(parsedValue);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.NotFuture, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date in the future or present.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date in the future or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> FutureOrPresentDateOnly(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Relative.Past, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date in the future or present.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsInFuture(parsedValue, Inclusion.Inclusive);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.Past, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be less than or equal to .
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be less than or equal to "</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> BetweenDateOnly(this IMustClause _,
        string? value,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Range.OutOfRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Range.OutOfRange,
                "{paramName} must be less than or equal to " + nameof(max) + ".", nameof(min), min);

        const string messageTemplate = "{paramName} must be a date within the expected range.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsBetween(parsedValue, min, max, inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Range.OutOfRange, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be less than or equal to .
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be less than or equal to "</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotBetweenDateOnly(this IMustClause _,
        string? value,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Range.InRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Range.InRange,
                "{paramName} must be less than or equal to " + nameof(max) + ".", nameof(min), min);

        const string messageTemplate = "{paramName} must be a date not within the expected range.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsBetween(parsedValue, min, max, inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Range.InRange, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative number of days.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="days">The number of days within which the value must fall.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative number of days."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> WithinDaysDateOnly(this IMustClause _,
        string? value,
        DateOnly? reference,
        int days,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.NotWithin, NullMessage, paramName, value);

        if (days < 0)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.NotWithin,
                "{paramName} requires a non-negative number of days.", nameof(days), days);

        const string messageTemplate = "{paramName} must be a date within the expected number of days.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsWithin(parsedValue, reference, days);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.NotWithin, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative number of days.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="days">The number of days within which the value must fall.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative number of days."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotWithinDaysDateOnly(this IMustClause _,
        string? value,
        DateOnly? reference,
        int days,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.Within, NullMessage, paramName, value);

        if (days < 0)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.Within,
                "{paramName} requires a non-negative number of days.", nameof(days), days);

        const string messageTemplate = "{paramName} must be a date not within the expected number of days.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsWithin(parsedValue, reference, days);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.Within, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative number of months.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="months">The number of calendar months within which the value must fall.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative number of months."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> WithinCalendarMonthsDateOnly(this IMustClause _,
        string? value,
        DateOnly? reference,
        int months,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.NotWithinCalendarMonths, NullMessage, paramName, value);

        if (months < 0)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.NotWithinCalendarMonths,
                "{paramName} requires a non-negative number of months.", nameof(months), months);

        const string messageTemplate = "{paramName} must be a date within the expected number of calendar months.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsWithinCalendarMonths(parsedValue, reference, months);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.NotWithinCalendarMonths,
            messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative number of months.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="months">The number of calendar months within which the value must fall.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative number of months."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotWithinCalendarMonthsDateOnly(this IMustClause _,
        string? value,
        DateOnly? reference,
        int months,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.WithinCalendarMonths, NullMessage, paramName, value);

        if (months < 0)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Proximity.WithinCalendarMonths,
                "{paramName} requires a non-negative number of months.", nameof(months), months);

        const string messageTemplate = "{paramName} must be a date not within the expected number of calendar months.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsWithinCalendarMonths(parsedValue, reference, months);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.WithinCalendarMonths,
            messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date before the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date before the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> BeforeDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.NotBefore, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date before the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsBefore(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotBefore, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a date before the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a date before the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotBeforeDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.Before, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a date before the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsBefore(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.Before, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date on or before the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date on or before the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> OnOrBeforeDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.After, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date on or before the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsBefore(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.After, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a date on or before the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a date on or before the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotOnOrBeforeDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.NotAfter, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a date on or before the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsBefore(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotAfter, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date after the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date after the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> AfterDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.NotAfter, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date after the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsAfter(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotAfter, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a date after the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a date after the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotAfterDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.After, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a date after the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsAfter(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.After, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date on or after the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a date on or after the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> OnOrAfterDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.Before, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date on or after the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsAfter(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.Before, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a date on or after the specified date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a date on or after the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotOnOrAfterDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.NotBefore, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a date on or after the specified date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsAfter(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotBefore, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be the same date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be the same date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> SameDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Equality.NotEqual, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be the same date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateOnlyRules.IsSame(parsedValue, other, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Equality.NotEqual, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be the same date.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be the same date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotSameDateOnly(this IMustClause _,
        string? value,
        DateOnly other,
        DatePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Equality.Equal, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be the same date.";

        if (!StringUtility.DateOnly.TryParse(value, out var parsed, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateOnlyRules.IsSame(parsedValue, other, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Equality.Equal, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be chronological.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be chronological."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> ChronologicalDateOnly(this IMustClause _,
        string? start,
        string? end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        if (start is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.NotChronological, NullMessage, paramName, start);

        if (end is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.NotChronological, NullMessage, nameof(end), end);

        const string messageTemplate = "{paramName} must be chronological.";

        if (!StringUtility.DateOnly.TryParse(start, out var parsedStart, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, start);

        if (!StringUtility.DateOnly.TryParse(end, out var parsedEnd, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(end), end);

        var parsedStartValue = parsedStart.GetValueOrDefault();
        var ok = DateOnlyRules.IsChronological(parsedStartValue, parsedEnd.GetValueOrDefault(), inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotChronological,
            messageTemplate, paramName, start, parsedStartValue);
    }

    /// <summary>
    /// Validates that the specified value must not be chronological.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be chronological."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotChronologicalDateOnly(this IMustClause _,
        string? start,
        string? end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        if (start is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.Chronological, NullMessage, paramName, start);

        if (end is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Order.Chronological, NullMessage, nameof(end), end);

        const string messageTemplate = "{paramName} must not be chronological.";

        if (!StringUtility.DateOnly.TryParse(start, out var parsedStart, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, start);

        if (!StringUtility.DateOnly.TryParse(end, out var parsedEnd, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(end), end);

        var parsedStartValue = parsedStart.GetValueOrDefault();
        var ok = !DateOnlyRules.IsChronological(parsedStartValue, parsedEnd.GetValueOrDefault(), inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.Chronological, messageTemplate, paramName, start, parsedStartValue);
    }

    /// <summary>
    /// Validates that the specified value must be overlapping.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="start1">The start of the first range.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be overlapping."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> OverlappingDateOnly(this IMustClause _,
        string? start1,
        string? end1,
        string? start2,
        string? end2,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        if (start1 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Missing, NullMessage, paramName, start1);

        if (end1 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Missing, NullMessage, nameof(end1), end1);

        if (start2 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Missing, NullMessage, nameof(start2), start2);

        if (end2 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Missing, NullMessage, nameof(end2), end2);

        const string messageTemplate = "{paramName} must be overlapping.";

        if (!StringUtility.DateOnly.TryParse(start1, out var s1, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, start1);

        if (!StringUtility.DateOnly.TryParse(end1, out var e1, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(end1), end1);

        if (!StringUtility.DateOnly.TryParse(start2, out var s2, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(start2), start2);

        if (!StringUtility.DateOnly.TryParse(end2, out var e2, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(end2), end2);

        var parsedStartValue = s1.GetValueOrDefault();
        var ok = DateOnlyRules.IsOverlapping(
            parsedStartValue,
            e1.GetValueOrDefault(),
            s2.GetValueOrDefault(),
            e2.GetValueOrDefault(),
            inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Overlap.Missing, messageTemplate, paramName, start1, parsedStartValue);
    }

    /// <summary>
    /// Validates that the specified value must not be overlapping.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="start1">The start of the first range.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be overlapping."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-only">String Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotOverlappingDateOnly(this IMustClause _,
        string? start1,
        string? end1,
        string? start2,
        string? end2,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        if (start1 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Present, NullMessage, paramName, start1);

        if (end1 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Present, NullMessage, nameof(end1), end1);

        if (start2 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Present, NullMessage, nameof(start2), start2);

        if (end2 is null)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Overlap.Present, NullMessage, nameof(end2), end2);

        const string messageTemplate = "{paramName} must not be overlapping.";

        if (!StringUtility.DateOnly.TryParse(start1, out var s1, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, start1);

        if (!StringUtility.DateOnly.TryParse(end1, out var e1, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(end1), end1);

        if (!StringUtility.DateOnly.TryParse(start2, out var s2, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(start2), start2);

        if (!StringUtility.DateOnly.TryParse(end2, out var e2, styles))
            return MustResult<DateOnly>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, nameof(end2), end2);

        var parsedStartValue = s1.GetValueOrDefault();
        var ok = !DateOnlyRules.IsOverlapping(
            parsedStartValue,
            e1.GetValueOrDefault(),
            s2.GetValueOrDefault(),
            e2.GetValueOrDefault(),
            inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Overlap.Present, messageTemplate, paramName, start1, parsedStartValue);
    }
}
#endif
