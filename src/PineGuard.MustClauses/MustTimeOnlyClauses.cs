#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="TimeOnly"/> values,
/// delegating to time-only rules for core validation logic.
/// </summary>
/// <seealso cref="TimeOnlyRules"/>
/// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
public static class MustTimeOnlyClauses
{
    private const string InvalidPrecisionMessage = "{paramName} requires a valid precision.";

    /// <summary>
    /// Validates that the specified value requires a valid range.
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
    /// The failure message follows the pattern <c>"{paramName} requires a valid range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> Between(this IMustClause _,
        TimeOnly value,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (min > max)
            return MustResult<TimeOnly>.Fail("{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must be within the expected range.";

        var ok = TimeOnlyRules.IsBetween(value, min, max, inclusion);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value requires a valid range.
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
    /// The failure message follows the pattern <c>"{paramName} requires a valid range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotBetween(this IMustClause _,
        TimeOnly value,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (min > max)
            return MustResult<TimeOnly>.Fail("{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must not be within the expected range.";

        var ok = !TimeOnlyRules.IsBetween(value, min, max, inclusion);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> Before(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be before the specified time.";

        var ok = TimeOnlyRules.IsBefore(value, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be on or before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be on or before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> OnOrBefore(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be on or before the specified time.";

        var ok = TimeOnlyRules.IsBefore(value, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> After(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be after the specified time.";

        var ok = TimeOnlyRules.IsAfter(value, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be on or after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must be on or after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> OnOrAfter(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be on or after the specified time.";

        var ok = TimeOnlyRules.IsAfter(value, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be the same time.
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
    /// The failure message follows the pattern <c>"{paramName} must be the same time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> Same(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must be the same time.";

        var ok = TimeOnlyRules.IsSame(value, other, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be the same time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be the same time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotSame(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be the same time.";

        var ok = !TimeOnlyRules.IsSame(value, other, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative window.
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
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative window."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> Within(this IMustClause _,
        TimeOnly value,
        TimeOnly reference,
        TimeSpan window,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (window < TimeSpan.Zero)
            return MustResult<TimeOnly>.Fail("{paramName} requires a non-negative window.", nameof(window), window);

        const string messageTemplate = "{paramName} must be within the expected time window.";

        var ok = TimeOnlyRules.IsWithin(value, reference, window);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative window.
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
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative window."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotWithin(this IMustClause _,
        TimeOnly value,
        TimeOnly reference,
        TimeSpan window,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (window < TimeSpan.Zero)
            return MustResult<TimeOnly>.Fail("{paramName} requires a non-negative window.", nameof(window), window);

        const string messageTemplate = "{paramName} must not be within the expected time window.";

        var ok = !TimeOnlyRules.IsWithin(value, reference, window);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> Chronological(this IMustClause _,
        TimeOnly start,
        TimeOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be chronological.";

        var ok = TimeOnlyRules.IsChronological(start, end, inclusion);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, start, start);
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
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> Overlapping(this IMustClause _,
        TimeOnly start1,
        TimeOnly end1,
        TimeOnly start2,
        TimeOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be overlapping.";

        var range1Ok = TimeOnlyRange.TryCreate(start1, end1, out var _);
        var range2Ok = TimeOnlyRange.TryCreate(start2, end2, out var _);

        var ok = range1Ok && range2Ok && TimeOnlyRangeRules.IsOverlapping(new TimeOnlyRange(start1, end1), new TimeOnlyRange(start2, end2), inclusion);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, start1, start1);
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
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotOverlapping(this IMustClause _,
        TimeOnly start1,
        TimeOnly end1,
        TimeOnly start2,
        TimeOnly end2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be overlapping.";

        var range1Ok = TimeOnlyRange.TryCreate(start1, end1, out var _);
        var range2Ok = TimeOnlyRange.TryCreate(start2, end2, out var _);

        var ok = !(range1Ok && range2Ok && TimeOnlyRangeRules.IsOverlapping(new TimeOnlyRange(start1, end1), new TimeOnlyRange(start2, end2), inclusion));
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, start1, start1);
    }

    /// <summary>
    /// Validates that the specified value must not be before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotBefore(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be before the specified time.";

        var ok = !TimeOnlyRules.IsBefore(value, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be on or before the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be on or before the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotOnOrBefore(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be on or before the specified time.";

        var ok = !TimeOnlyRules.IsBefore(value, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotAfter(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be after the specified time.";

        var ok = !TimeOnlyRules.IsAfter(value, other, Inclusion.Exclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be on or after the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not be on or after the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotOnOrAfter(this IMustClause _,
        TimeOnly value,
        TimeOnly other,
        TimePrecision? precision = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is not null && !Enum.IsDefined(precision.Value))
            return MustResult<TimeOnly>.Fail(InvalidPrecisionMessage, nameof(precision), precision);

        const string messageTemplate = "{paramName} must not be on or after the specified time.";

        var ok = !TimeOnlyRules.IsAfter(value, other, Inclusion.Inclusive, precision);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, value, value);
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
    /// <seealso href="https://pineguard.ai/docs/must/time-only">Time Only Must Clauses documentation</seealso>
    public static MustResult<TimeOnly> NotChronological(this IMustClause _,
        TimeOnly start,
        TimeOnly end,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be chronological.";

        var ok = !TimeOnlyRules.IsChronological(start, end, inclusion);
        return MustResult<TimeOnly>.FromBool(ok, messageTemplate, paramName, start, start);
    }
}
#endif
