#if NET8_0_OR_GREATER
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="DateOnly"/> validation predicates for temporal comparisons and range checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/dateonly">DateOnly Rules documentation</seealso>
public static class DateOnlyRules
{
    /// <summary>
    /// Determines whether the specified date is in the past relative to UTC now.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether today is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the past; otherwise, <see langword="false"/>.</returns>
    public static bool IsInPast(DateOnly? value, Inclusion inclusion = Inclusion.Exclusive) =>
        value is not null && IsBefore(value, DateOnly.FromDateTime(DateTime.UtcNow), inclusion);

    /// <summary>
    /// Determines whether the specified date is in the future relative to UTC now.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether today is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the future; otherwise, <see langword="false"/>.</returns>
    public static bool IsInFuture(DateOnly? value, Inclusion inclusion = Inclusion.Exclusive) =>
        value is not null && IsAfter(value, DateOnly.FromDateTime(DateTime.UtcNow), inclusion);

    /// <summary>
    /// Determines whether the specified date falls within [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the date range.</param>
    /// <param name="max">The upper bound of the date range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public static bool IsBetween(DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsBetween(value.Value, min, max, inclusion);

    /// <summary>
    /// Determines whether the specified date is before the given reference date.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="precision">Optional precision for date truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is before <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsBefore(DateOnly? value, DateOnly? other, Inclusion inclusion = Inclusion.Inclusive, DatePrecision? precision = null)
    {
        if (value is null || other is null)
            return false;

        var v = value.Value;
        var o = other.Value;

        if (precision is null)
            return RuleComparison.IsLessThan(v, o, inclusion);

        if (!DateTimeUtility.TryTruncateToPrecision(v, precision.Value, out var tV) ||
            !DateTimeUtility.TryTruncateToPrecision(o, precision.Value, out var tO))
            return false;

        v = tV!.Value;
        o = tO!.Value;

        return RuleComparison.IsLessThan(v, o, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date is after the given reference date.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="precision">Optional precision for date truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is after <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAfter(DateOnly? value, DateOnly? other, Inclusion inclusion = Inclusion.Inclusive, DatePrecision? precision = null)
    {
        if (value is null || other is null)
            return false;

        var v = value.Value;
        var o = other.Value;

        if (precision is null)
            return RuleComparison.IsGreaterThan(v, o, inclusion);

        if (!DateTimeUtility.TryTruncateToPrecision(v, precision.Value, out var tV) ||
            !DateTimeUtility.TryTruncateToPrecision(o, precision.Value, out var tO))
            return false;

        v = tV!.Value;
        o = tO!.Value;

        return RuleComparison.IsGreaterThan(v, o, inclusion);
    }

    /// <summary>
    /// Determines whether two dates are the same, optionally truncated to the given precision.
    /// </summary>
    /// <param name="value">The first date. If <see langword="null"/> and <paramref name="other"/> is also <see langword="null"/>, returns <see langword="true"/>.</param>
    /// <param name="other">The second date.</param>
    /// <param name="precision">Optional precision for date truncation before comparison.</param>
    /// <returns><see langword="true"/> if the dates are equal at the given precision; otherwise, <see langword="false"/>.</returns>
    public static bool IsSame(DateOnly? value, DateOnly? other, DatePrecision? precision = null)
    {
        if (value is null && other is null)
            return true;

        if (value is null || other is null)
            return false;

        var v = value.Value;
        var o = other.Value;

        if (precision is null)
            return RuleComparison.Equals(v, o);

        if (!DateTimeUtility.TryTruncateToPrecision(v, precision.Value, out var tV) ||
            !DateTimeUtility.TryTruncateToPrecision(o, precision.Value, out var tO))
            return false;

        v = tV!.Value;
        o = tO!.Value;

        return RuleComparison.Equals(v, o);
    }

    /// <summary>
    /// Determines whether the start date is chronologically before or equal to the end date.
    /// </summary>
    /// <param name="start">The start date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="start"/> precedes <paramref name="end"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsChronological(DateOnly? start, DateOnly? end, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsChronological(start, end, inclusion);

    /// <summary>
    /// Determines whether two date ranges overlap.
    /// </summary>
    /// <param name="start1">The start of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end1">The end of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="start2">The start of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end2">The end of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if the two ranges overlap; otherwise, <see langword="false"/>.</returns>
    public static bool IsOverlapping(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsOverlapping(start1, end1, start2, end2, inclusion);

    /// <summary>
    /// Determines whether the specified date is within the given number of calendar months from a reference date.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="months">The maximum number of calendar months allowed. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the month difference is within the limit; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithinCalendarMonths(DateOnly? value, DateOnly? reference, int months)
    {
        if (value is null || reference is null)
            return false;

        if (months < 0)
            return false;

        var v = value.Value;
        var r = reference.Value;

        var valueMonthIndex = (v.Year * 12) + v.Month;
        var referenceMonthIndex = (r.Year * 12) + r.Month;

        var monthDiff = Math.Abs(valueMonthIndex - referenceMonthIndex);
        return monthDiff <= months;
    }

    /// <summary>
    /// Determines whether the specified date is within the given number of days from a reference date.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="days">The maximum number of days allowed. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the absolute day difference is within the limit; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithin(DateOnly? value, DateOnly? reference, int days)
    {
        if (value is null || reference is null)
            return false;

        if (days < 0)
            return false;

        var diffDays = Math.Abs(value.Value.DayNumber - reference.Value.DayNumber);
        return diffDays <= days;
    }
}
#endif
