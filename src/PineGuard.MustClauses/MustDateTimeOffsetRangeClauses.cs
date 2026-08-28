using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="DateTimeOffsetRange"/> values,
/// delegating to <see cref="DateTimeOffsetRangeRules"/> for core validation logic.
/// </summary>
/// <seealso cref="DateTimeOffsetRangeRules"/>
/// <seealso href="https://pineguard.ai/docs/must/date-time-offset-range">Date Time Offset Range Must Clauses documentation</seealso>
public static class MustDateTimeOffsetRangeClauses
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset-range">Date Time Offset Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffsetRange> Chronological(this IMustClause _,
        DateTimeOffsetRange range,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be chronological.";

        var ok = DateTimeOffsetRangeRules.IsChronological(range, inclusion);
        return MustResult<DateTimeOffsetRange>.FromBool(ok, MustCodes.Range.Order.NotChronological, messageTemplate, paramName, range, range);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset-range">Date Time Offset Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffsetRange> Overlapping(this IMustClause _,
        DateTimeOffsetRange range1,
        DateTimeOffsetRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be overlapping.";

        var ok = DateTimeOffsetRangeRules.IsOverlapping(range1, range2, inclusion);
        return MustResult<DateTimeOffsetRange>.FromBool(ok, MustCodes.Range.Overlap.Missing, messageTemplate, paramName, range1, range1);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset-range">Date Time Offset Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffsetRange> NotOverlapping(this IMustClause _,
        DateTimeOffsetRange range1,
        DateTimeOffsetRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be overlapping.";

        var ok = !DateTimeOffsetRangeRules.IsOverlapping(range1, range2, inclusion);
        return MustResult<DateTimeOffsetRange>.FromBool(ok, MustCodes.Range.Overlap.Present, messageTemplate, paramName, range1, range1);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset-range">Date Time Offset Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffsetRange> Contains(this IMustClause _,
        DateTimeOffsetRange range,
        DateTimeOffset value,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified date/time.";

        var ok = DateTimeOffsetRangeRules.Contains(range, value, inclusion);
        return MustResult<DateTimeOffsetRange>.FromBool(ok, MustCodes.Range.Bounds.NotContains, messageTemplate, paramName, range, range);
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
    /// <seealso href="https://pineguard.ai/docs/must/date-time-offset-range">Date Time Offset Range Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffsetRange> NotContains(this IMustClause _,
        DateTimeOffsetRange range,
        DateTimeOffset value,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified date/time.";

        var ok = !DateTimeOffsetRangeRules.Contains(range, value, inclusion);
        return MustResult<DateTimeOffsetRange>.FromBool(ok, MustCodes.Range.Bounds.Contains, messageTemplate, paramName, range, range);
    }
}
