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
    public static bool IsBetween(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsBetween(value.Value, min, max, inclusion);

    /// <summary>
    /// Determines whether the specified time is before the given reference time.
    /// </summary>
    /// <param name="value">The time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="precision">Optional precision for time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is before <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsBefore(TimeOnly? value, TimeOnly? other, Inclusion inclusion = Inclusion.Inclusive, TimePrecision? precision = null)
    {
        if (value is null || other is null)
            return false;

        var left = value.Value;
        var right = other.Value;

        if (precision is null)
            return RuleComparison.IsLessThan(left, right, inclusion);

        if (!TimeOnlyUtility.TryTruncateToPrecision(left, precision.Value, out var tLeft) ||
            !TimeOnlyUtility.TryTruncateToPrecision(right, precision.Value, out var tRight))
            return false;

        left = tLeft;
        right = tRight;

        return RuleComparison.IsLessThan(left, right, inclusion);
    }

    /// <summary>
    /// Determines whether the specified time is after the given reference time.
    /// </summary>
    /// <param name="value">The time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="precision">Optional precision for time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is after <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAfter(TimeOnly? value, TimeOnly? other, Inclusion inclusion = Inclusion.Inclusive, TimePrecision? precision = null)
    {
        if (value is null || other is null)
            return false;

        var left = value.Value;
        var right = other.Value;

        if (precision is null)
            return RuleComparison.IsGreaterThan(left, right, inclusion);

        if (!TimeOnlyUtility.TryTruncateToPrecision(left, precision.Value, out var tLeft) ||
            !TimeOnlyUtility.TryTruncateToPrecision(right, precision.Value, out var tRight))
            return false;

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
        if (value is null && other is null)
            return true;

        if (value is null || other is null)
            return false;

        var left = value.Value;
        var right = other.Value;

        if (precision is null)
            return RuleComparison.Equals(left, right);

        if (!TimeOnlyUtility.TryTruncateToPrecision(left, precision.Value, out var tLeft) ||
            !TimeOnlyUtility.TryTruncateToPrecision(right, precision.Value, out var tRight))
            return false;

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
    /// <returns><see langword="true"/> if the time difference is within the window; otherwise, <see langword="false"/>.</returns>
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
        var absTicks = diffTicks < 0 ? -diffTicks : diffTicks;
        return absTicks <= window.Ticks;
    }

    /// <summary>
    /// Determines whether the start time is chronologically before or equal to the end time.
    /// </summary>
    /// <param name="start">The start time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="start"/> precedes <paramref name="end"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsChronological(TimeOnly? start, TimeOnly? end, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (start is null || end is null)
            return false;

        return inclusion == Inclusion.Inclusive ? start.Value <= end.Value : start.Value < end.Value;
    }

    /// <summary>
    /// Determines whether two time ranges overlap.
    /// </summary>
    /// <param name="start1">The start of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end1">The end of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="start2">The start of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end2">The end of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if the two time ranges overlap; otherwise, <see langword="false"/>.</returns>
    public static bool IsOverlapping(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (start1 is null || end1 is null || start2 is null || end2 is null)
            return false;

        return inclusion == Inclusion.Exclusive
            ? start1.Value < end2.Value && start2.Value < end1.Value
            : start1.Value <= end2.Value && start2.Value <= end1.Value;
    }
}
#endif
