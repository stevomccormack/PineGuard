using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="TimeSpan"/> duration values.
/// </summary>
/// <seealso cref="TimeSpanRules"/>
/// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
public static class MustTimeSpanClauses
{
    /// <summary>
    /// Validates that the specified <see cref="TimeSpan"/> duration falls within the given range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="TimeSpan"/> value to validate.</param>
    /// <param name="min">The minimum allowed duration.</param>
    /// <param name="max">The maximum allowed duration.</param>
    /// <param name="inclusion">Whether the range boundaries are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is within the specified duration range, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="min"/> is greater than <paramref name="max"/>.
    /// Delegates to <see cref="TimeSpanRules.IsDurationBetween"/>. The failure message follows the pattern
    /// <c>"{paramName} must be within the expected duration range."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.DurationBetween(elapsed, TimeSpan.Zero, TimeSpan.FromHours(1));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsDurationBetween"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> DurationBetween(this IMustClause _,
        TimeSpan value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (min > max)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.OutOfRange, "{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must be within the expected duration range.";

        var ok = TimeSpanRules.IsDurationBetween(value, min, max, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.OutOfRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="TimeSpan"/> duration does not fall within the given range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="TimeSpan"/> value to validate.</param>
    /// <param name="min">The minimum boundary of the excluded range.</param>
    /// <param name="max">The maximum boundary of the excluded range.</param>
    /// <param name="inclusion">Whether the range boundaries are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is outside the specified duration range, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="min"/> is greater than <paramref name="max"/>.
    /// Delegates to <see cref="TimeSpanRules.IsDurationBetween"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be within the expected duration range."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotDurationBetween(timeout, TimeSpan.Zero, TimeSpan.FromSeconds(30));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsDurationBetween"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> NotDurationBetween(this IMustClause _,
        TimeSpan value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (min > max)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.InRange, "{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must not be within the expected duration range.";

        var ok = !TimeSpanRules.IsDurationBetween(value, min, max, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.InRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="TimeSpan"/> duration is greater than the given threshold.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="TimeSpan"/> value to validate.</param>
    /// <param name="threshold">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the comparison is exclusive or inclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> exceeds <paramref name="threshold"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TimeSpanRules.IsGreaterThan"/>. The failure message follows the pattern
    /// <c>"{paramName} must be greater than the threshold."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.GreaterThan(elapsed, TimeSpan.FromSeconds(1));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsGreaterThan"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> GreaterThan(this IMustClause _,
        TimeSpan value,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be greater than the threshold.";

        var ok = TimeSpanRules.IsGreaterThan(value, threshold, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.NotGreater, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="TimeSpan"/> duration is less than the given threshold.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="TimeSpan"/> value to validate.</param>
    /// <param name="threshold">The threshold duration to compare against.</param>
    /// <param name="inclusion">Whether the comparison is exclusive or inclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is less than <paramref name="threshold"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TimeSpanRules.IsLessThan"/>. The failure message follows the pattern
    /// <c>"{paramName} must be less than the threshold."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.LessThan(timeout, TimeSpan.FromMinutes(5));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsLessThan"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> LessThan(this IMustClause _,
        TimeSpan value,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be less than the threshold.";

        var ok = TimeSpanRules.IsLessThan(value, threshold, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.NotLess, messageTemplate, paramName, value, value);
    }
}
