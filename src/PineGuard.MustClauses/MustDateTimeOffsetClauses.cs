using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="DateTimeOffset"/> values,
/// delegating to <see cref="DateTimeOffsetRules"/> for core validation logic.
/// </summary>
/// <seealso cref="DateTimeOffsetRules"/>
/// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
public static class MustDateTimeOffsetClauses
{
    /// <summary>
    /// Validates that the specified value must be in the past.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be in the past."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Past(this IMustClause _,
        DateTimeOffset value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the past.";

        var ok = DateTimeOffsetRules.IsInPast(value, timeProvider: timeProvider);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.NotPast, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the past or present.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be in the past or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> PastOrPresent(this IMustClause _,
        DateTimeOffset value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the past or present.";

        var ok = DateTimeOffsetRules.IsInPast(value, Inclusion.Inclusive, timeProvider);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.Future, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the future.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be in the future."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Future(this IMustClause _,
        DateTimeOffset value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the future.";

        var ok = DateTimeOffsetRules.IsInFuture(value, timeProvider: timeProvider);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.NotFuture, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the future or present.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be in the future or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> FutureOrPresent(this IMustClause _,
        DateTimeOffset value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the future or present.";

        var ok = DateTimeOffsetRules.IsInFuture(value, Inclusion.Inclusive, timeProvider);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Relative.Past, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be within the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Between(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected range.";

        var ok = DateTimeOffsetRules.IsBetween(value, min, max, inclusion);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Range.OutOfRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be within the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotBetween(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset min,
        DateTimeOffset max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected range.";

        var ok = !DateTimeOffsetRules.IsBetween(value, min, max, inclusion);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Range.InRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be before the specified date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be before the specified date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Before(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be before the specified date/time.";

        var ok = DateTimeOffsetRules.IsBefore(value, other, Inclusion.Exclusive, precision);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Order.NotBefore, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be on or before the specified date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be on or before the specified date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> OnOrBefore(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be on or before the specified date/time.";

        var ok = DateTimeOffsetRules.IsBefore(value, other, Inclusion.Inclusive, precision);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Order.After, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be after the specified date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be after the specified date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> After(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be after the specified date/time.";

        var ok = DateTimeOffsetRules.IsAfter(value, other, Inclusion.Exclusive, precision);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Order.NotAfter, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be on or after the specified date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be on or after the specified date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> OnOrAfter(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be on or after the specified date/time.";

        var ok = DateTimeOffsetRules.IsAfter(value, other, Inclusion.Inclusive, precision);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Order.Before, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be the same date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be the same date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Same(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the same date/time.";

        var ok = DateTimeOffsetRules.IsSame(value, other, precision);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Equality.NotEqual, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be the same date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The precision level for comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be the same date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotSame(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the same date/time.";

        var ok = !DateTimeOffsetRules.IsSame(value, other, precision);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Equality.Equal, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be chronological.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Chronological(this IMustClause _,
        DateTimeOffset start,
        DateTimeOffset end,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be chronological.";

        var ok = DateTimeOffsetRules.IsChronological(start, end, inclusion);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Order.NotChronological, messageTemplate, paramName, start, start);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Overlapping(this IMustClause _,
        DateTimeOffset start1,
        DateTimeOffset end1,
        DateTimeOffset start2,
        DateTimeOffset end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be overlapping.";

        var range1Ok = DateTimeOffsetRange.TryCreate(start1, end1, out var _);
        var range2Ok = DateTimeOffsetRange.TryCreate(start2, end2, out var _);

        var ok = range1Ok && range2Ok && DateTimeOffsetRules.IsOverlapping(start1, end1, start2, end2, inclusion);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Overlap.Missing, messageTemplate, paramName, start1, start1);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotOverlapping(this IMustClause _,
        DateTimeOffset start1,
        DateTimeOffset end1,
        DateTimeOffset start2,
        DateTimeOffset end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be overlapping.";

        var range1Ok = DateTimeOffsetRange.TryCreate(start1, end1, out var _);
        var range2Ok = DateTimeOffsetRange.TryCreate(start2, end2, out var _);

        var ok = !(range1Ok && range2Ok && DateTimeOffsetRules.IsOverlapping(start1, end1, start2, end2, inclusion));
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Overlap.Present, messageTemplate, paramName, start1, start1);
    }

    /// <summary>
    /// Validates that the specified value must be within the expected time window.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="window">The time window to check against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be within the expected time window."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Within(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset reference,
        TimeSpan window,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected time window.";

        var ok = DateTimeOffsetRules.IsWithin(value, reference, window);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.NotWithin, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be within the expected time window.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="window">The time window to check against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be within the expected time window."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotWithin(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset reference,
        TimeSpan window,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected time window.";

        var ok = !DateTimeOffsetRules.IsWithin(value, reference, window);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.Within, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be within the expected number of calendar months.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="months">The number of calendar months within which the value must fall.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be within the expected number of calendar months."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> WithinCalendarMonths(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset reference,
        int months,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected number of calendar months.";

        var ok = DateTimeOffsetRules.IsWithinCalendarMonths(value, reference, months);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.NotWithinCalendarMonths,
            messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be within the expected number of calendar months.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="months">The number of calendar months within which the value must fall.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be within the expected number of calendar months."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotWithinCalendarMonths(this IMustClause _,
        DateTimeOffset value,
        DateTimeOffset reference,
        int months,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected number of calendar months.";

        var ok = !DateTimeOffsetRules.IsWithinCalendarMonths(value, reference, months);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Proximity.WithinCalendarMonths,
            messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be a weekday.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a weekday."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Weekday(this IMustClause _,
        DateTimeOffset value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a weekday.";

        var ok = DateTimeOffsetRules.IsWeekday(value);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Calendar.NotWeekday, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be a weekend day.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a weekend day."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> Weekend(this IMustClause _,
        DateTimeOffset value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a weekend day.";

        var ok = DateTimeOffsetRules.IsWeekend(value);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Calendar.NotWeekend, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be the first day of the month.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be the first day of the month."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> FirstDayOfMonth(this IMustClause _,
        DateTimeOffset value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the first day of the month.";

        var ok = DateTimeOffsetRules.IsFirstDayOfMonth(value);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Calendar.NotFirstDayOfMonth, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be the first day of the month.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be the first day of the month."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotFirstDayOfMonth(this IMustClause _,
        DateTimeOffset value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the first day of the month.";

        var ok = !DateTimeOffsetRules.IsFirstDayOfMonth(value);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Calendar.FirstDayOfMonth, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be the last day of the month.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be the last day of the month."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> LastDayOfMonth(this IMustClause _,
        DateTimeOffset value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the last day of the month.";

        var ok = DateTimeOffsetRules.IsLastDayOfMonth(value);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Calendar.NotLastDayOfMonth, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be the last day of the month.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be the last day of the month."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset">Date Time Offset Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> NotLastDayOfMonth(this IMustClause _,
        DateTimeOffset value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the last day of the month.";

        var ok = !DateTimeOffsetRules.IsLastDayOfMonth(value);
        return MustResult<DateTimeOffset>.FromBool(ok, MustCodes.Date.Calendar.LastDayOfMonth, messageTemplate, paramName, value, value);
    }
}
