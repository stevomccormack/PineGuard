#if NET8_0_OR_GREATER
using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate time-only string representations,
/// parsing the input string before delegating to time-only rules.
/// </summary>
/// <seealso cref="TimeOnlyRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
public static class MustStringTimeOnlyClauses
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    private const string NullMessage = "{paramName} must not be null.";

    private const string InvalidTimePrecisionMessage = "{paramName} has an invalid time precision.";

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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> BetweenTimeOnly(this IMustClause _,
        string? value,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Range.OutOfRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Range.OutOfRange, "{paramName} must be less than or equal to " + nameof(max) + ".", nameof(min), min);

        const string messageTemplate = "{paramName} must be a time within the expected range.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeOnlyRules.IsBetween(parsedValue, min, max, inclusion);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Range.OutOfRange, messageTemplate, paramName, value, parsedValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotBetweenTimeOnly(this IMustClause _,
        string? value,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Range.InRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Range.InRange, "{paramName} must be less than or equal to " + nameof(max) + ".", nameof(min), min);

        const string messageTemplate = "{paramName} must be a time not within the expected range.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsBetween(parsedValue, min, max, inclusion);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Range.InRange, messageTemplate, paramName, value, parsedValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> WithinTimeOnly(this IMustClause _,
        string? value,
        string reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Proximity.NotWithin, NullMessage, paramName, value);

        if (reference is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Proximity.NotWithin, NullMessage, nameof(reference), reference);

        if (window < TimeSpan.Zero)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Proximity.NotWithin, "{paramName} requires a non-negative time window.", nameof(window), window);

        const string messageTemplate = "{paramName} must be a time within the expected time window.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsedValue, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        if (!StringUtility.TimeOnly.TryParse(reference, out var parsedReference, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsed = parsedValue.GetValueOrDefault();
        var ok = TimeOnlyRules.IsWithin(parsed, parsedReference, window);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Proximity.NotWithin, messageTemplate, paramName, value, parsed);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotWithinTimeOnly(this IMustClause _,
        string? value,
        string reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Proximity.Within, NullMessage, paramName, value);

        if (reference is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Proximity.Within, NullMessage, nameof(reference), reference);

        if (window < TimeSpan.Zero)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Proximity.Within, "{paramName} requires a non-negative time window.", nameof(window), window);

        const string messageTemplate = "{paramName} must be a time not within the expected time window.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsedValue, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        if (!StringUtility.TimeOnly.TryParse(reference, out var parsedReference, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsed = parsedValue.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsWithin(parsed, parsedReference, window);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Proximity.Within, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must be a time before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be a time before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> BeforeTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotBefore, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotBefore, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be a time before the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeOnlyRules.IsBefore(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.NotBefore, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a time on or before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be a time on or before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> OnOrBeforeTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.After, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.After, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be a time on or before the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeOnlyRules.IsBefore(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.After, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a time after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be a time after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> AfterTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotAfter, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotAfter, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be a time after the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeOnlyRules.IsAfter(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.NotAfter, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a time on or after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be a time on or after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> OnOrAfterTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.Before, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.Before, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be a time on or after the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeOnlyRules.IsAfter(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.Before, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a time the same as the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be a time the same as the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> SameTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Equality.NotEqual, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Equality.NotEqual, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be a time the same as the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeOnlyRules.IsSame(parsedValue, other, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Equality.NotEqual, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a time not the same as the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be a time not the same as the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotSameTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Equality.Equal, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Equality.Equal, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be a time not the same as the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsSame(parsedValue, other, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Equality.Equal, messageTemplate, paramName, value, parsedValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> ChronologicalTimeOnly(this IMustClause _,
        string? start,
        string? end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        if (start is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotChronological, NullMessage, paramName, start);

        if (end is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotChronological, NullMessage, nameof(end), end);

        const string messageTemplate = "{paramName} must be chronological.";

        if (!StringUtility.TimeOnly.TryParse(start, out var parsedStart, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, start, result: default);

        if (!StringUtility.TimeOnly.TryParse(end, out var parsedEnd, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(end), end, result: default);

        var parsedStartValue = parsedStart.GetValueOrDefault();
        var ok = TimeOnlyRules.IsChronological(parsedStartValue, parsedEnd.GetValueOrDefault(), inclusion);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.NotChronological, messageTemplate, paramName, start, parsedStartValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> OverlappingTimeOnly(this IMustClause _,
        string? start1,
        string? end1,
        string? start2,
        string? end2,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        if (start1 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Missing, NullMessage, paramName, start1);

        if (end1 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Missing, NullMessage, nameof(end1), end1);

        if (start2 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Missing, NullMessage, nameof(start2), start2);

        if (end2 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Missing, NullMessage, nameof(end2), end2);

        const string messageTemplate = "{paramName} must be overlapping.";

        if (!StringUtility.TimeOnly.TryParse(start1, out var s1, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, start1, result: default);

        if (!StringUtility.TimeOnly.TryParse(end1, out var e1, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(end1), end1, result: default);

        if (!StringUtility.TimeOnly.TryParse(start2, out var s2, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(start2), start2, result: default);

        if (!StringUtility.TimeOnly.TryParse(end2, out var e2, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(end2), end2, result: default);

        var parsedStartValue = s1.GetValueOrDefault();
        var ok = TimeOnlyRules.IsOverlapping(
            parsedStartValue,
            e1.GetValueOrDefault(),
            s2.GetValueOrDefault(),
            e2.GetValueOrDefault(),
            inclusion);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Overlap.Missing, messageTemplate, paramName, start1, parsedStartValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotOverlappingTimeOnly(this IMustClause _,
        string? start1,
        string? end1,
        string? start2,
        string? end2,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        if (start1 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Present, NullMessage, paramName, start1);

        if (end1 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Present, NullMessage, nameof(end1), end1);

        if (start2 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Present, NullMessage, nameof(start2), start2);

        if (end2 is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Overlap.Present, NullMessage, nameof(end2), end2);

        const string messageTemplate = "{paramName} must not be overlapping.";

        if (!StringUtility.TimeOnly.TryParse(start1, out var s1, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, start1, result: default);

        if (!StringUtility.TimeOnly.TryParse(end1, out var e1, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(end1), end1, result: default);

        if (!StringUtility.TimeOnly.TryParse(start2, out var s2, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(start2), start2, result: default);

        if (!StringUtility.TimeOnly.TryParse(end2, out var e2, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(end2), end2, result: default);

        var parsedStartValue = s1.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsOverlapping(
            parsedStartValue,
            e1.GetValueOrDefault(),
            s2.GetValueOrDefault(),
            e2.GetValueOrDefault(),
            inclusion);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Overlap.Present, messageTemplate, paramName, start1, parsedStartValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a time before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be a time before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotBeforeTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.Before, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.Before, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be a time before the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsBefore(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.Before, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a time on or before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be a time on or before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotOnOrBeforeTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotAfter, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotAfter, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be a time on or before the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsBefore(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.NotAfter, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a time after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be a time after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotAfterTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.After, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.After, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be a time after the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsAfter(parsedValue, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.After, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must not be a time on or after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be a time on or after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotOnOrAfterTimeOnly(this IMustClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotBefore, NullMessage, paramName, value);

        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.NotBefore, InvalidTimePrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be a time on or after the specified time.";

        if (!StringUtility.TimeOnly.TryParse(value, out var parsed, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsAfter(parsedValue, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.NotBefore, messageTemplate, paramName, value, parsedValue);
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
    /// <seealso href="https://pineguard.ai/docs/must/string-time-only">String Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotChronologicalTimeOnly(this IMustClause _,
        string? start,
        string? end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        if (start is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.Chronological, NullMessage, paramName, start);

        if (end is null)
            return MustResult<TimeOnly>.Fail(MustCodes.Time.Order.Chronological, NullMessage, nameof(end), end);

        const string messageTemplate = "{paramName} must not be chronological.";

        if (!StringUtility.TimeOnly.TryParse(start, out var parsedStart, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, start, result: default);

        if (!StringUtility.TimeOnly.TryParse(end, out var parsedEnd, styles))
            return MustResult<TimeOnly>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, nameof(end), end, result: default);

        var parsedStartValue = parsedStart.GetValueOrDefault();
        var ok = !TimeOnlyRules.IsChronological(parsedStartValue, parsedEnd.GetValueOrDefault(), inclusion);
        return MustResult<TimeOnly>.FromBool(ok, MustCodes.Time.Order.Chronological, messageTemplate, paramName, start, parsedStartValue);
    }
}
#endif
