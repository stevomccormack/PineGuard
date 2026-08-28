using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate date-time offset string representations,
/// parsing the input string before delegating to date-time offset rules.
/// </summary>
/// <seealso cref="DateTimeOffsetRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
public static class MustStringDateTimeOffsetClauses
{
    private const string NullMessage = "{paramName} must not be null.";
    private const DateTimeStyles DefaultStyles = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;

    /// <summary>
    /// Validates that the specified value must be a date/time in the past.
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
    /// The failure message follows the pattern <c>"{paramName} must be a date/time in the past."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> PastDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Relative.NotPast, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date/time in the past.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateTimeOffsetRules.IsInPast(parsedValue);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.NotPast, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date/time in the past or present.
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
    /// The failure message follows the pattern <c>"{paramName} must be a date/time in the past or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> PastOrPresentDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Relative.Future, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date/time in the past or present.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateTimeOffsetRules.IsInPast(parsedValue, Inclusion.Inclusive);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.Future, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date/time in the future.
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
    /// The failure message follows the pattern <c>"{paramName} must be a date/time in the future."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> FutureDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Relative.NotFuture, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date/time in the future.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateTimeOffsetRules.IsInFuture(parsedValue);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.NotFuture, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a date/time in the future or present.
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
    /// The failure message follows the pattern <c>"{paramName} must be a date/time in the future or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> FutureOrPresentDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Relative.Past, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a date/time in the future or present.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateTimeOffsetRules.IsInFuture(parsedValue, Inclusion.Inclusive);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.Past, messageTemplate, paramName, value, parsedValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> BetweenDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Range.OutOfRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Range.OutOfRange,
                "{paramName} must be less than or equal to " + nameof(max) + ".", nameof(min), min);

        const string messageTemplate = "{paramName} must be a date/time within the expected range.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateTimeOffsetRules.IsBetween(parsedValue, min, max, inclusion);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Range.OutOfRange, messageTemplate, paramName, value, parsedValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotBetweenDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Range.InRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Range.InRange,
                "{paramName} must be less than or equal to " + nameof(max) + ".", nameof(min), min);

        const string messageTemplate = "{paramName} must be a date/time not within the expected range.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateTimeOffsetRules.IsBetween(parsedValue, min, max, inclusion);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Range.InRange, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative time window.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="window">The time window to check against.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative time window."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> WithinDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeOffset? reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.NotWithin, NullMessage, paramName, value);

        if (window < TimeSpan.Zero)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.NotWithin,
                "{paramName} requires a non-negative time window.", nameof(window), window);

        const string messageTemplate = "{paramName} must be a date/time within the expected time window.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateTimeOffsetRules.IsWithin(parsedValue, reference, window);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.NotWithin, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative time window.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="window">The time window to check against.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative time window."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotWithinDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeOffset? reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.Within, NullMessage, paramName, value);

        if (window < TimeSpan.Zero)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.Within,
                "{paramName} requires a non-negative time window.", nameof(window), window);

        const string messageTemplate = "{paramName} must be a date/time not within the expected time window.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateTimeOffsetRules.IsWithin(parsedValue, reference, window);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.Within, messageTemplate, paramName, value, parsedValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> WithinCalendarMonthsDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeOffset? reference,
        int months,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.NotWithinCalendarMonths, NullMessage, paramName, value);

        if (months < 0)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.NotWithinCalendarMonths,
                "{paramName} requires a non-negative number of months.", nameof(months), months);

        const string messageTemplate = "{paramName} must be a date/time within the expected number of calendar months.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = DateTimeOffsetRules.IsWithinCalendarMonths(parsedValue, reference, months);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.NotWithinCalendarMonths,
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
    /// <seealso href="https://pineguard.ai/docs/must/string-date-time-offset">String Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotWithinCalendarMonthsDateTimeOffset(this IMustClause _,
        string? value,
        DateTimeOffset? reference,
        int months,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.WithinCalendarMonths, NullMessage, paramName, value);

        if (months < 0)
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Proximity.WithinCalendarMonths,
                "{paramName} requires a non-negative number of months.", nameof(months), months);

        const string messageTemplate = "{paramName} must be a date/time not within the expected number of calendar months.";

        if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles))
            return MustResult<DateTimeOffset>.Fail(MustCodes.Date.Format.Invalid, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !DateTimeOffsetRules.IsWithinCalendarMonths(parsedValue, reference, months);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.WithinCalendarMonths,
            messageTemplate, paramName, value, parsedValue);
    }
}
