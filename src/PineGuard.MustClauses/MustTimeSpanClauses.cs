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
    /// Returns a failed result immediately if <paramref name="min"/> is longer than <paramref name="max"/>.
    /// Delegates to <see cref="TimeSpanRules.IsInRange"/>. The failure message follows the pattern
    /// <c>"{paramName} must be within the expected duration range."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.InRange(elapsed, TimeSpan.Zero, TimeSpan.FromHours(1));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsInRange"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> InRange(this IMustClause _,
        TimeSpan value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.OutOfRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (min > max)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.OutOfRange, "{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must be within the expected duration range.";

        var ok = TimeSpanRules.IsInRange(value, min, max, inclusion);
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
    /// Returns a failed result immediately if <paramref name="min"/> is longer than <paramref name="max"/>.
    /// Delegates to <see cref="TimeSpanRules.IsInRange"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be within the expected duration range."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.OutOfRange(timeout, TimeSpan.Zero, TimeSpan.FromSeconds(30));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsInRange"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> OutOfRange(this IMustClause _,
        TimeSpan value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.InRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (min > max)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.InRange, "{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must not be within the expected duration range.";

        var ok = !TimeSpanRules.IsInRange(value, min, max, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.InRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="TimeSpan"/> duration is longer than the given other.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="TimeSpan"/> value to validate.</param>
    /// <param name="other">The other duration to compare against.</param>
    /// <param name="inclusion">Whether the comparison is exclusive or inclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> exceeds <paramref name="other"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TimeSpanRules.IsLongerThan"/>. The failure message follows the pattern
    /// <c>"{paramName} must be longer than the other."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.LongerThan(elapsed, TimeSpan.FromSeconds(1));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsLongerThan"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> LongerThan(this IMustClause _,
        TimeSpan value,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.NotLonger, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        const string messageTemplate = "{paramName} must be longer than the other.";

        var ok = TimeSpanRules.IsLongerThan(value, other, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.NotLonger, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="TimeSpan"/> duration is shorter than the given other.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="TimeSpan"/> value to validate.</param>
    /// <param name="other">The other duration to compare against.</param>
    /// <param name="inclusion">Whether the comparison is exclusive or inclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is shorter than <paramref name="other"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TimeSpanRules.IsShorterThan"/>. The failure message follows the pattern
    /// <c>"{paramName} must be shorter than the other."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.ShorterThan(timeout, TimeSpan.FromMinutes(5));
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpanRules.IsShorterThan"/>
    /// <seealso href="https://pineguard.ai/docs/must/timespan">TimeSpan Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> ShorterThan(this IMustClause _,
        TimeSpan value,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.NotShorter, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        const string messageTemplate = "{paramName} must be shorter than the other.";

        var ok = TimeSpanRules.IsShorterThan(value, other, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.NotShorter, messageTemplate, paramName, value, value);
    }
}
