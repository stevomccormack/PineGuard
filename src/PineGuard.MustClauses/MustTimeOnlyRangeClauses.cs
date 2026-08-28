#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="TimeOnlyRange"/> values,
/// delegating to <see cref="TimeOnlyRangeRules"/> for core validation logic.
/// </summary>
/// <seealso cref="TimeOnlyRangeRules"/>
/// <seealso href="https://pineguard.ai/docs/must/time-only-range">Time Only Range Must Clauses documentation</seealso>
public static class MustTimeOnlyRangeClauses
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
    /// <seealso href="https://pineguard.ai/docs/must/time-only-range">Time Only Range Must Clauses documentation</seealso>
    public static MustResult<TimeOnlyRange> Chronological(this IMustClause _,
        TimeOnlyRange range,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be chronological.";

        var ok = TimeOnlyRangeRules.IsChronological(range, inclusion);
        return MustResult<TimeOnlyRange>.FromBool(ok, MustCodes.Range.Order.NotChronological, messageTemplate, paramName, range, range);
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
    /// <seealso href="https://pineguard.ai/docs/must/time-only-range">Time Only Range Must Clauses documentation</seealso>
    public static MustResult<TimeOnlyRange> Overlapping(this IMustClause _,
        TimeOnlyRange range1,
        TimeOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be overlapping.";

        var ok = TimeOnlyRangeRules.IsOverlapping(range1, range2, inclusion);
        return MustResult<TimeOnlyRange>.FromBool(ok, MustCodes.Range.Overlap.Missing, messageTemplate, paramName, range1, range1);
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
    /// <seealso href="https://pineguard.ai/docs/must/time-only-range">Time Only Range Must Clauses documentation</seealso>
    public static MustResult<TimeOnlyRange> NotOverlapping(this IMustClause _,
        TimeOnlyRange range1,
        TimeOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be overlapping.";

        var ok = !TimeOnlyRangeRules.IsOverlapping(range1, range2, inclusion);
        return MustResult<TimeOnlyRange>.FromBool(ok, MustCodes.Range.Overlap.Present, messageTemplate, paramName, range1, range1);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must contain the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only-range">Time Only Range Must Clauses documentation</seealso>
    public static MustResult<TimeOnlyRange> Contains(this IMustClause _,
        TimeOnlyRange range,
        TimeOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified time.";

        var ok = TimeOnlyRangeRules.Contains(range, value, inclusion);
        return MustResult<TimeOnlyRange>.FromBool(ok, MustCodes.Range.Bounds.NotContains, messageTemplate, paramName, range, range);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified time.
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
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified time."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/time-only-range">Time Only Range Must Clauses documentation</seealso>
    public static MustResult<TimeOnlyRange> NotContains(this IMustClause _,
        TimeOnlyRange range,
        TimeOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified time.";

        var ok = !TimeOnlyRangeRules.Contains(range, value, inclusion);
        return MustResult<TimeOnlyRange>.FromBool(ok, MustCodes.Range.Bounds.Contains, messageTemplate, paramName, range, range);
    }
}
#endif
