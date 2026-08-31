using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="DateTime"/> values,
/// delegating to <see cref="DateTimeRules"/> for core validation logic.
/// </summary>
/// <seealso cref="DateTimeRules"/>
/// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
public static class MustDateTimeClauses
{
    /// <summary>
    /// Validates that the specified value must be in the past.
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
    /// The failure message follows the pattern <c>"{paramName} must be in the past."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Past(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the past.";

        var ok = DateTimeRules.IsInPast(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Relative.NotPast, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the past or present.
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
    /// The failure message follows the pattern <c>"{paramName} must be in the past or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> PastOrPresent(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the past or present.";

        var ok = DateTimeRules.IsInPast(value, Inclusion.Inclusive);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Relative.Future, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the future.
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
    /// The failure message follows the pattern <c>"{paramName} must be in the future."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Future(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the future.";

        var ok = DateTimeRules.IsInFuture(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Relative.NotFuture, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the future or present.
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
    /// The failure message follows the pattern <c>"{paramName} must be in the future or present."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> FutureOrPresent(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the future or present.";

        var ok = DateTimeRules.IsInFuture(value, Inclusion.Inclusive);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Relative.Past, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Between(this IMustClause _,
        DateTime value,
        DateTime min,
        DateTime max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected range.";

        var ok = DateTimeRules.IsBetween(value, min, max, inclusion);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Range.OutOfRange, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotBetween(this IMustClause _,
        DateTime value,
        DateTime min,
        DateTime max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected range.";

        var ok = !DateTimeRules.IsBetween(value, min, max, inclusion);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Range.InRange, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Before(this IMustClause _,
        DateTime value,
        DateTime other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be before the specified date/time.";

        var ok = DateTimeRules.IsBefore(value, other, Inclusion.Exclusive, precision);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Order.NotBefore, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> OnOrBefore(this IMustClause _,
        DateTime value,
        DateTime other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be on or before the specified date/time.";

        var ok = DateTimeRules.IsBefore(value, other, Inclusion.Inclusive, precision);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Order.After, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> After(this IMustClause _,
        DateTime value,
        DateTime other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be after the specified date/time.";

        var ok = DateTimeRules.IsAfter(value, other, Inclusion.Exclusive, precision);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Order.NotAfter, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> OnOrAfter(this IMustClause _,
        DateTime value,
        DateTime other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be on or after the specified date/time.";

        var ok = DateTimeRules.IsAfter(value, other, Inclusion.Inclusive, precision);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Order.Before, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Same(this IMustClause _,
        DateTime value,
        DateTime other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the same date/time.";

        var ok = DateTimeRules.IsSame(value, other, precision);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Equality.NotEqual, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotSame(this IMustClause _,
        DateTime value,
        DateTime other,
        DateTimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the same date/time.";

        var ok = !DateTimeRules.IsSame(value, other, precision);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Equality.Equal, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Chronological(this IMustClause _,
        DateTime start,
        DateTime end,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be chronological.";

        var ok = DateTimeRules.IsChronological(start, end, inclusion);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Order.NotChronological, messageTemplate, paramName, start, start);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Overlapping(this IMustClause _,
        DateTime start1,
        DateTime end1,
        DateTime start2,
        DateTime end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be overlapping.";

        var range1Ok = DateTimeRange.TryCreate(start1, end1, out var _);
        var range2Ok = DateTimeRange.TryCreate(start2, end2, out var _);

        var ok = range1Ok && range2Ok && DateTimeRules.IsOverlapping(start1, end1, start2, end2, inclusion);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Overlap.Missing, messageTemplate, paramName, start1, start1);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotOverlapping(this IMustClause _,
        DateTime start1,
        DateTime end1,
        DateTime start2,
        DateTime end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be overlapping.";

        var range1Ok = DateTimeRange.TryCreate(start1, end1, out var _);
        var range2Ok = DateTimeRange.TryCreate(start2, end2, out var _);

        var ok = !(range1Ok && range2Ok && DateTimeRules.IsOverlapping(start1, end1, start2, end2, inclusion));
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Overlap.Present, messageTemplate, paramName, start1, start1);
    }

    /// <summary>
    /// Validates that the specified value must be within the expected number of days from now.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="days">The number of days within which the value must fall.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be within the expected number of days from now."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> WithinDaysFromNow(this IMustClause _,
        DateTime value,
        int days,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected number of days from now.";

        var ok = DateTimeRules.IsWithinDaysFromNow(value, days);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Proximity.NotWithin, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be within the expected number of days from now.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="days">The number of days within which the value must fall.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be within the expected number of days from now."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotWithinDaysFromNow(this IMustClause _,
        DateTime value,
        int days,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected number of days from now.";

        var ok = !DateTimeRules.IsWithinDaysFromNow(value, days);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Proximity.Within, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Within(this IMustClause _,
        DateTime value,
        DateTime reference,
        TimeSpan window,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected time window.";

        var ok = DateTimeRules.IsWithin(value, reference, window);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Proximity.NotWithin, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotWithin(this IMustClause _,
        DateTime value,
        DateTime reference,
        TimeSpan window,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected time window.";

        var ok = !DateTimeRules.IsWithin(value, reference, window);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Proximity.Within, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> WithinCalendarMonths(this IMustClause _,
        DateTime value,
        DateTime reference,
        int months,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected number of calendar months.";

        var ok = DateTimeRules.IsWithinCalendarMonths(value, reference, months);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Proximity.NotWithinCalendarMonths,
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotWithinCalendarMonths(this IMustClause _,
        DateTime value,
        DateTime reference,
        int months,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected number of calendar months.";

        var ok = !DateTimeRules.IsWithinCalendarMonths(value, reference, months);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Proximity.WithinCalendarMonths, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Weekday(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a weekday.";

        var ok = DateTimeRules.IsWeekday(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Calendar.NotWeekday, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Weekend(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a weekend day.";

        var ok = DateTimeRules.IsWeekend(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Calendar.NotWeekend, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> FirstDayOfMonth(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the first day of the month.";

        var ok = DateTimeRules.IsFirstDayOfMonth(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Calendar.NotFirstDayOfMonth, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotFirstDayOfMonth(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the first day of the month.";

        var ok = !DateTimeRules.IsFirstDayOfMonth(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Calendar.FirstDayOfMonth, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> LastDayOfMonth(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the last day of the month.";

        var ok = DateTimeRules.IsLastDayOfMonth(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Calendar.NotLastDayOfMonth, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotLastDayOfMonth(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the last day of the month.";

        var ok = !DateTimeRules.IsLastDayOfMonth(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Calendar.LastDayOfMonth, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be the same day.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be the same day."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> SameDay(this IMustClause _,
        DateTime value,
        DateTime other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the same day.";

        var ok = DateTimeRules.IsSameDay(value, other);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Equality.NotSameDay, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be the same day.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be the same day."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotSameDay(this IMustClause _,
        DateTime value,
        DateTime other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the same day.";

        var ok = !DateTimeRules.IsSameDay(value, other);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Equality.SameDay, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be UTC.
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
    /// The failure message follows the pattern <c>"{paramName} must be UTC."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Utc(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be UTC.";

        var ok = DateTimeRules.IsUtc(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.NotUtc, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be UTC.
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
    /// The failure message follows the pattern <c>"{paramName} must not be UTC."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotUtc(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be UTC.";

        var ok = !DateTimeRules.IsUtc(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.Utc, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be local.
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
    /// The failure message follows the pattern <c>"{paramName} must be local."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Local(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be local.";

        var ok = DateTimeRules.IsLocal(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.NotLocal, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be local.
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
    /// The failure message follows the pattern <c>"{paramName} must not be local."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotLocal(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be local.";

        var ok = !DateTimeRules.IsLocal(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.Local, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must have an unspecified kind.
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
    /// The failure message follows the pattern <c>"{paramName} must have an unspecified kind."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> Unspecified(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must have an unspecified kind.";

        var ok = DateTimeRules.IsUnspecified(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.NotUnspecified, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not have an unspecified kind.
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
    /// The failure message follows the pattern <c>"{paramName} must not have an unspecified kind."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotUnspecified(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not have an unspecified kind.";

        var ok = !DateTimeRules.IsUnspecified(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.Unspecified, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must have an explicit kind.
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
    /// The failure message follows the pattern <c>"{paramName} must have an explicit kind."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> ExplicitKind(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must have an explicit kind.";

        var ok = DateTimeRules.HasExplicitKind(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.Unspecified, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not have an explicit kind.
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
    /// The failure message follows the pattern <c>"{paramName} must not have an explicit kind."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> NotExplicitKind(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not have an explicit kind.";

        var ok = !DateTimeRules.HasExplicitKind(value);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Kind.NotUnspecified, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified date of birth must meet the expected minimum age.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The date of birth to validate.</param>
    /// <param name="years">The minimum age in whole years.</param>
    /// <param name="timeProvider">The clock that supplies today's date. If <see langword="null"/>, the system clock is used.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must meet the expected minimum age."</c>
    /// A negative <paramref name="years"/> is a configuration error, reported against that parameter.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time">Date Time Must Clauses documentation</seealso>
    public static MustResult<DateTime> MinimumAge(this IMustClause _,
        DateTime value,
        int years,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (years < 0)
            return MustResult<DateTime>.Fail(MustCodes.Date.Age.BelowMinimum,
                "{paramName} requires a non-negative number of years.", nameof(years), years);

        const string messageTemplate = "{paramName} must meet the expected minimum age.";

        var ok = DateTimeRules.HasMinimumAge(value, years, timeProvider);
        return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Age.BelowMinimum, messageTemplate, paramName, value, value);
    }
}
