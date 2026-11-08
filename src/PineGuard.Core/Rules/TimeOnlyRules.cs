#if NET8_0_OR_GREATER
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="TimeOnly"/> validation predicates for temporal comparisons and range checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/timeonly">TimeOnly Rules documentation</seealso>
public static class TimeOnlyRules
{
    /// <summary>
    /// Determines whether the specified time falls within [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="value">The time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the time range.</param>
    /// <param name="max">The upper bound of the time range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public static bool IsInRange(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion = Inclusion.Inclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return value is not null && RuleComparison.IsInRange(value.Value, min, max, inclusion);
    }

    /// <summary>
    /// Determines whether the specified time is before the given reference time.
    /// </summary>
    /// <param name="value">The time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference time.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="precision">Optional precision for time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is before <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsBefore(TimeOnly? value, TimeOnly other, Inclusion inclusion = Inclusion.Exclusive, TimePrecision? precision = null)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (precision.HasValue && !EnumRules.IsDefined<TimePrecision>(precision.Value))
            return false;

        if (value is null)
            return false;

        var left = value.Value;
        var right = other;

        if (precision is null)
            return RuleComparison.IsLessThan(left, right, inclusion);

        _ = TimeOnlyUtility.TryTruncateToPrecision(left, precision.Value, out var tLeft);
        _ = TimeOnlyUtility.TryTruncateToPrecision(right, precision.Value, out var tRight);

        left = tLeft;
        right = tRight;

        return RuleComparison.IsLessThan(left, right, inclusion);
    }

    /// <summary>
    /// Determines whether the specified time is after the given reference time.
    /// </summary>
    /// <param name="value">The time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference time.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="precision">Optional precision for time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is after <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAfter(TimeOnly? value, TimeOnly other, Inclusion inclusion = Inclusion.Exclusive, TimePrecision? precision = null)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (precision.HasValue && !EnumRules.IsDefined<TimePrecision>(precision.Value))
            return false;

        if (value is null)
            return false;

        var left = value.Value;
        var right = other;

        if (precision is null)
            return RuleComparison.IsGreaterThan(left, right, inclusion);

        _ = TimeOnlyUtility.TryTruncateToPrecision(left, precision.Value, out var tLeft);
        _ = TimeOnlyUtility.TryTruncateToPrecision(right, precision.Value, out var tRight);

        left = tLeft;
        right = tRight;

        return RuleComparison.IsGreaterThan(left, right, inclusion);
    }

    /// <summary>
    /// Determines whether two times are the same, optionally truncated to the given precision.
    /// </summary>
    /// <param name="value">The first time. If <see langword="null"/> and <paramref name="other"/> is also <see langword="null"/>, returns <see langword="true"/>.</param>
    /// <param name="other">The second time.</param>
    /// <param name="precision">Optional precision for time truncation before comparison.</param>
    /// <returns><see langword="true"/> if the times are equal at the given precision; otherwise, <see langword="false"/>.</returns>
    public static bool IsSame(TimeOnly? value, TimeOnly? other, TimePrecision? precision = null)
    {
        if (precision.HasValue && !EnumRules.IsDefined<TimePrecision>(precision.Value))
            return false;

        if (value is null && other is null)
            return true;

        if (value is null || other is null)
            return false;

        var left = value.Value;
        var right = other.Value;

        if (precision is null)
            return RuleComparison.Equals(left, right);

        _ = TimeOnlyUtility.TryTruncateToPrecision(left, precision.Value, out var tLeft);
        _ = TimeOnlyUtility.TryTruncateToPrecision(right, precision.Value, out var tRight);

        left = tLeft;
        right = tRight;

        return RuleComparison.Equals(left, right);
    }

    /// <summary>
    /// Determines whether the specified time is within a given time window of a reference time.
    /// </summary>
    /// <param name="value">The time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="window">The maximum allowed time difference. If negative or greater than one day, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the minimal clock distance between the two times, wrapping across midnight, is within the window; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithin(TimeOnly? value, TimeOnly? reference, TimeSpan window)
    {
        if (value is null || reference is null)
            return false;

        if (window < TimeSpan.Zero)
            return false;

        // windows larger than a day are not meaningful.
        if (window > TimeSpan.FromDays(1))
            return false;

        var diffTicks = (value.Value.ToTimeSpan() - reference.Value.ToTimeSpan()).Ticks;
        var linearTicks = diffTicks < 0 ? -diffTicks : diffTicks;
        var circularTicks = Math.Min(linearTicks, TimeSpan.TicksPerDay - linearTicks);
        return circularTicks <= window.Ticks;
    }

    /// <summary>
    /// Determines whether the start time precedes the end time (equality permitted when <paramref name="inclusion"/> is <see cref="Inclusion.Inclusive"/>).
    /// </summary>
    /// <param name="start">The start time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="start"/> precedes <paramref name="end"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsChronological(TimeOnly? start, TimeOnly? end, Inclusion inclusion = Inclusion.Exclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return RangeRules.IsChronological(start, end, inclusion);
    }

    /// <summary>
    /// Determines whether two time ranges overlap.
    /// </summary>
    /// <param name="start1">The start of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end1">The end of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="start2">The start of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end2">The end of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether to intersect closed ranges or open interiors. Singleton and one-step interiors are empty. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if the two time ranges overlap; otherwise, <see langword="false"/>. Returns <see langword="false"/> if either range is inverted (start after end).</returns>
    public static bool IsOverlapping(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion = Inclusion.Inclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return RangeRules.IsOverlapping(start1?.Ticks, end1?.Ticks, start2?.Ticks, end2?.Ticks, inclusion);
    }
}
#endif
