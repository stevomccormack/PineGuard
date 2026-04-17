using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="DateTimeRange"/> values,
/// delegating to <see cref="DateTimeRangeRules"/> for core validation logic.
/// </summary>
/// <seealso cref="DateTimeRangeRules"/>
/// <seealso href="https://pineguard.ai/docs/must/date-time-range">Date Time Range Must Clauses documentation</seealso>
public static class MustDateTimeRangeClauses
{
    /// <summary>
    /// Validates that the specified value must be chronological.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="range">The range to validate.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-range">Date Time Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeRange> Chronological(this IMustClause _,
        DateTimeRange range,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be chronological.";

        var ok = DateTimeRangeRules.IsChronological(range, inclusion);
        return MustResult<DateTimeRange>.FromBool(ok, messageTemplate, paramName, range, range);
    }

    /// <summary>
    /// Validates that the specified value must be overlapping.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="range1">The first range to validate.</param>
    /// <param name="range2">The second range to validate.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-range">Date Time Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeRange> Overlapping(this IMustClause _,
        DateTimeRange range1,
        DateTimeRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be overlapping.";

        var ok = DateTimeRangeRules.IsOverlapping(range1, range2, inclusion);
        return MustResult<DateTimeRange>.FromBool(ok, messageTemplate, paramName, range1, range1);
    }

    /// <summary>
    /// Validates that the specified value must not be overlapping.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="range1">The first range to validate.</param>
    /// <param name="range2">The second range to validate.</param>
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-range">Date Time Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeRange> NotOverlapping(this IMustClause _,
        DateTimeRange range1,
        DateTimeRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be overlapping.";

        var ok = !DateTimeRangeRules.IsOverlapping(range1, range2, inclusion);
        return MustResult<DateTimeRange>.FromBool(ok, messageTemplate, paramName, range1, range1);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="range">The range to validate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain the specified date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-range">Date Time Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeRange> Contains(this IMustClause _,
        DateTimeRange range,
        DateTime value,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified date/time.";

        var ok = DateTimeRangeRules.Contains(range, value, inclusion);
        return MustResult<DateTimeRange>.FromBool(ok, messageTemplate, paramName, range, range);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified date/time.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="range">The range to validate.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified date/time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/date-time-range">Date Time Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeRange> NotContains(this IMustClause _,
        DateTimeRange range,
        DateTime value,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified date/time.";

        var ok = !DateTimeRangeRules.Contains(range, value, inclusion);
        return MustResult<DateTimeRange>.FromBool(ok, messageTemplate, paramName, range, range);
    }
}
