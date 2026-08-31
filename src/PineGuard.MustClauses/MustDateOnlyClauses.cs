#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="DateOnly"/> values,
/// delegating to <see cref="DateOnlyRules"/> for core validation logic.
/// </summary>
/// <seealso cref="DateOnlyRules"/>
/// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
public static class MustDateOnlyClauses
{
    /// <summary>
    /// Validates that the specified value must be in the past.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies today's date. If <see langword="null"/>, the system clock is used.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Past(this IMustClause _,
        DateOnly value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the past.";

        var ok = DateOnlyRules.IsInPast(value, timeProvider: timeProvider);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.NotPast, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the past or present.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies today's date. If <see langword="null"/>, the system clock is used.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> PastOrPresent(this IMustClause _,
        DateOnly value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the past or present.";

        var ok = DateOnlyRules.IsInPast(value, Inclusion.Inclusive, timeProvider);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.Future, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the future.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies today's date. If <see langword="null"/>, the system clock is used.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Future(this IMustClause _,
        DateOnly value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the future.";

        var ok = DateOnlyRules.IsInFuture(value, timeProvider: timeProvider);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.NotFuture, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be in the future or present.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="timeProvider">The clock that supplies today's date. If <see langword="null"/>, the system clock is used.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> FutureOrPresent(this IMustClause _,
        DateOnly value,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be in the future or present.";

        var ok = DateOnlyRules.IsInFuture(value, Inclusion.Inclusive, timeProvider);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Relative.Past, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Between(this IMustClause _,
        DateOnly value,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected range.";

        var ok = DateOnlyRules.IsBetween(value, min, max, inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Range.OutOfRange, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotBetween(this IMustClause _,
        DateOnly value,
        DateOnly min,
        DateOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected range.";

        var ok = !DateOnlyRules.IsBetween(value, min, max, inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Range.InRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be before the specified date.
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
    /// The failure message follows the pattern <c>"{paramName} must be before the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Before(this IMustClause _,
        DateOnly value,
        DateOnly other,
        DatePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be before the specified date.";

        var ok = DateOnlyRules.IsBefore(value, other, Inclusion.Exclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotBefore, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be on or before the specified date.
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
    /// The failure message follows the pattern <c>"{paramName} must be on or before the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> OnOrBefore(this IMustClause _,
        DateOnly value,
        DateOnly other,
        DatePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be on or before the specified date.";

        var ok = DateOnlyRules.IsBefore(value, other, Inclusion.Inclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.After, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be after the specified date.
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
    /// The failure message follows the pattern <c>"{paramName} must be after the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> After(this IMustClause _,
        DateOnly value,
        DateOnly other,
        DatePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be after the specified date.";

        var ok = DateOnlyRules.IsAfter(value, other, Inclusion.Exclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotAfter, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be on or after the specified date.
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
    /// The failure message follows the pattern <c>"{paramName} must be on or after the specified date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> OnOrAfter(this IMustClause _,
        DateOnly value,
        DateOnly other,
        DatePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be on or after the specified date.";

        var ok = DateOnlyRules.IsAfter(value, other, Inclusion.Inclusive, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.Before, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be the same date.
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
    /// The failure message follows the pattern <c>"{paramName} must be the same date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Same(this IMustClause _,
        DateOnly value,
        DateOnly other,
        DatePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the same date.";

        var ok = DateOnlyRules.IsSame(value, other, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Equality.NotEqual, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be the same date.
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
    /// The failure message follows the pattern <c>"{paramName} must not be the same date."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotSame(this IMustClause _,
        DateOnly value,
        DateOnly other,
        DatePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the same date.";

        var ok = !DateOnlyRules.IsSame(value, other, precision);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Equality.Equal, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Chronological(this IMustClause _,
        DateOnly start,
        DateOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be chronological.";

        var ok = DateOnlyRules.IsChronological(start, end, inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.NotChronological, messageTemplate, paramName, start, start);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Overlapping(this IMustClause _,
        DateOnly start1,
        DateOnly end1,
        DateOnly start2,
        DateOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be overlapping.";

        var range1Ok = DateOnlyRange.TryCreate(start1, end1, out var _);
        var range2Ok = DateOnlyRange.TryCreate(start2, end2, out var _);

        var ok = range1Ok && range2Ok && DateOnlyRules.IsOverlapping(start1, end1, start2, end2, inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Overlap.Missing, messageTemplate, paramName, start1, start1);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotOverlapping(this IMustClause _,
        DateOnly start1,
        DateOnly end1,
        DateOnly start2,
        DateOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be overlapping.";

        var range1Ok = DateOnlyRange.TryCreate(start1, end1, out var _);
        var range2Ok = DateOnlyRange.TryCreate(start2, end2, out var _);

        var ok = !(range1Ok && range2Ok && DateOnlyRules.IsOverlapping(start1, end1, start2, end2, inclusion));
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Overlap.Present, messageTemplate, paramName, start1, start1);
    }

    /// <summary>
    /// Validates that the specified value must be within the expected number of days.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="days">The number of days within which the value must fall.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be within the expected number of days."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> WithinDays(this IMustClause _,
        DateOnly value,
        DateOnly reference,
        int days,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected number of days.";
        var ok = DateOnlyRules.IsWithin(value, reference, days);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.NotWithin, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be within the expected number of days.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="reference">The reference value to compare against.</param>
    /// <param name="days">The number of days within which the value must fall.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be within the expected number of days."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotWithinDays(this IMustClause _,
        DateOnly value,
        DateOnly reference,
        int days,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected number of days.";
        var ok = !DateOnlyRules.IsWithin(value, reference, days);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.Within, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> WithinCalendarMonths(this IMustClause _,
        DateOnly value,
        DateOnly reference,
        int months,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the expected number of calendar months.";
        var ok = DateOnlyRules.IsWithinCalendarMonths(value, reference, months);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.NotWithinCalendarMonths,
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotWithinCalendarMonths(this IMustClause _,
        DateOnly value,
        DateOnly reference,
        int months,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be within the expected number of calendar months.";
        var ok = !DateOnlyRules.IsWithinCalendarMonths(value, reference, months);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Proximity.WithinCalendarMonths, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Weekday(this IMustClause _,
        DateOnly value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a weekday.";

        var ok = DateOnlyRules.IsWeekday(value);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Calendar.NotWeekday, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> Weekend(this IMustClause _,
        DateOnly value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a weekend day.";

        var ok = DateOnlyRules.IsWeekend(value);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Calendar.NotWeekend, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> FirstDayOfMonth(this IMustClause _,
        DateOnly value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the first day of the month.";

        var ok = DateOnlyRules.IsFirstDayOfMonth(value);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Calendar.NotFirstDayOfMonth, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotFirstDayOfMonth(this IMustClause _,
        DateOnly value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the first day of the month.";

        var ok = !DateOnlyRules.IsFirstDayOfMonth(value);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Calendar.FirstDayOfMonth, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> LastDayOfMonth(this IMustClause _,
        DateOnly value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be the last day of the month.";

        var ok = DateOnlyRules.IsLastDayOfMonth(value);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Calendar.NotLastDayOfMonth, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotLastDayOfMonth(this IMustClause _,
        DateOnly value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be the last day of the month.";

        var ok = !DateOnlyRules.IsLastDayOfMonth(value);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Calendar.LastDayOfMonth, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be chronological.
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
    /// The failure message follows the pattern <c>"{paramName} must not be chronological."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> NotChronological(this IMustClause _,
        DateOnly start,
        DateOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be chronological.";

        var ok = !DateOnlyRules.IsChronological(start, end, inclusion);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Order.Chronological, messageTemplate, paramName, start, start);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-only">Date Only Must Clauses documentation</seealso>
    public static MustResult<DateOnly> MinimumAge(this IMustClause _,
        DateOnly value,
        int years,
        TimeProvider? timeProvider = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (years < 0)
            return MustResult<DateOnly>.Fail(MustCodes.Date.Age.BelowMinimum,
                "{paramName} requires a non-negative number of years.", nameof(years), years);

        const string messageTemplate = "{paramName} must meet the expected minimum age.";

        var ok = DateOnlyRules.HasMinimumAge(value, years, timeProvider);
        return MustResult<DateOnly>.FromBool(ok, MustCodes.Date.Age.BelowMinimum, messageTemplate, paramName, value, value);
    }
}
#endif
