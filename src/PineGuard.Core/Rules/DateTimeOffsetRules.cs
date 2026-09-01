using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="DateTimeOffset"/> validation predicates for temporal comparisons and range checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/datetimeoffset">DateTimeOffset Rules documentation</seealso>
public static class DateTimeOffsetRules
{
    /// <summary>
    /// Determines whether the specified date/time offset is in the past relative to UTC now.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the past; otherwise, <see langword="false"/>.</returns>
    public static bool IsInPast(DateTimeOffset? value, Inclusion inclusion = Inclusion.Exclusive, TimeProvider? timeProvider = null) =>
        value is not null && IsBefore(value, DateTimeUtility.GetUtcNow(timeProvider), inclusion);

    /// <summary>
    /// Determines whether the specified date/time offset is in the future relative to UTC now.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the future; otherwise, <see langword="false"/>.</returns>
    public static bool IsInFuture(DateTimeOffset? value, Inclusion inclusion = Inclusion.Exclusive, TimeProvider? timeProvider = null) =>
        value is not null && IsAfter(value, DateTimeUtility.GetUtcNow(timeProvider), inclusion);

    /// <summary>
    /// Determines whether the specified date/time offset falls within [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the range.</param>
    /// <param name="max">The upper bound of the range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public static bool IsBetween(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsBetween(value.Value, min, max, inclusion);

    /// <summary>
    /// Determines whether the specified date/time offset is before the given reference.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is before <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsBefore(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion = Inclusion.Exclusive, DateTimePrecision? precision = null)
    {
        if (value is null || other is null) return false;

        var v = value.Value;
        var o = other.Value;

        if (precision is null)
            return RuleComparison.IsLessThan(v, o, inclusion);

        if (!DateTimeUtility.TryTruncateToPrecisionUtc(v, precision.Value, out var truncatedV) ||
            !DateTimeUtility.TryTruncateToPrecisionUtc(o, precision.Value, out var truncatedO))
            return false;

        v = truncatedV!.Value;
        o = truncatedO!.Value;

        return RuleComparison.IsLessThan(v, o, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date/time offset is after the given reference.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is after <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAfter(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion = Inclusion.Exclusive, DateTimePrecision? precision = null)
    {
        if (value is null || other is null) return false;

        var v = value.Value;
        var o = other.Value;

        if (precision is null)
            return RuleComparison.IsGreaterThan(v, o, inclusion);

        if (!DateTimeUtility.TryTruncateToPrecisionUtc(v, precision.Value, out var truncatedV) ||
            !DateTimeUtility.TryTruncateToPrecisionUtc(o, precision.Value, out var truncatedO))
            return false;

        v = truncatedV!.Value;
        o = truncatedO!.Value;

        return RuleComparison.IsGreaterThan(v, o, inclusion);
    }

    /// <summary>
    /// Determines whether two date/time offsets are the same, optionally truncated to the given precision.
    /// </summary>
    /// <param name="value">The first date/time offset. If <see langword="null"/> and <paramref name="other"/> is also <see langword="null"/>, returns <see langword="true"/>.</param>
    /// <param name="other">The second date/time offset.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if the values are equal at the given precision; otherwise, <see langword="false"/>.</returns>
    public static bool IsSame(DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision = null)
    {
        if (value is null && other is null) return true;
        if (value is null || other is null) return false;

        var v = value.Value;
        var o = other.Value;

        if (precision is null)
            return RuleComparison.Equals(v, o);

        if (!DateTimeUtility.TryTruncateToPrecisionUtc(v, precision.Value, out var truncatedV) ||
            !DateTimeUtility.TryTruncateToPrecisionUtc(o, precision.Value, out var truncatedO))
            return false;

        v = truncatedV!.Value;
        o = truncatedO!.Value;

        return RuleComparison.Equals(v, o);
    }

    /// <summary>
    /// Determines whether the start date/time offset precedes the end date/time offset (equality permitted when <paramref name="inclusion"/> is <see cref="Inclusion.Inclusive"/>).
    /// </summary>
    /// <param name="start">The start date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="start"/> precedes <paramref name="end"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsChronological(DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsChronological(start, end, inclusion);

    /// <summary>
    /// Determines whether two date/time offset ranges overlap.
    /// </summary>
    /// <param name="start1">The start of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end1">The end of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="start2">The start of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end2">The end of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if the two ranges overlap; otherwise, <see langword="false"/>.</returns>
    public static bool IsOverlapping(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsOverlapping(start1, end1, start2, end2, inclusion);

    /// <summary>
    /// Determines whether the specified date/time offset is within a given time window of a reference.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="window">The maximum allowed time difference. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the time difference is within the window; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithin(DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window)
    {
        if (value is null || reference is null)
            return false;

        if (window < TimeSpan.Zero)
            return false;

        var diffTicks = (value.Value.ToUniversalTime() - reference.Value.ToUniversalTime()).Ticks;

        var absTicks = Math.Abs(diffTicks);
        return absTicks <= window.Ticks;
    }

    /// <summary>
    /// Determines whether the specified date/time offset is within the given number of calendar months from a reference.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="months">The maximum number of calendar months allowed. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the month difference is within the limit; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithinCalendarMonths(DateTimeOffset? value, DateTimeOffset? reference, int months)
    {
        if (value is null || reference is null)
            return false;

        if (months < 0)
            return false;

        var valueUtc = value.Value.ToUniversalTime();
        var referenceUtc = reference.Value.ToUniversalTime();

        var valueMonthIndex = (valueUtc.Year * 12) + valueUtc.Month;
        var referenceMonthIndex = (referenceUtc.Year * 12) + referenceUtc.Month;

        var monthDiff = Math.Abs(valueMonthIndex - referenceMonthIndex);
        return monthDiff <= months;
    }

    /// <summary>
    /// Determines whether the specified date/time offset falls on a weekday (Monday through Friday).
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a weekday; otherwise, <see langword="false"/>.</returns>
    /// <remarks>The calendar day is read from the value's own offset (its wall-clock date), not from UTC.</remarks>
    public static bool IsWeekday(DateTimeOffset? value)
    {
        // ReSharper disable once UseNullPropagation
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday;
    }

    /// <summary>
    /// Determines whether the specified date/time offset falls on a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a weekend day; otherwise, <see langword="false"/>.</returns>
    /// <remarks>The calendar day is read from the value's own offset (its wall-clock date), not from UTC.</remarks>
    public static bool IsWeekend(DateTimeOffset? value)
    {
        // ReSharper disable once UseNullPropagation
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    /// <summary>
    /// Determines whether the specified date/time offset falls on the first day of its month.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is the first day of the month; otherwise, <see langword="false"/>.</returns>
    /// <remarks>The calendar day is read from the value's own offset (its wall-clock date), not from UTC.</remarks>
    public static bool IsFirstDayOfMonth(DateTimeOffset? value)
    {
        if (value is null)
            return false;

        return value.Value.Day == 1;
    }

    /// <summary>
    /// Determines whether the specified date/time offset falls on the last day of its month.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is the last day of the month; otherwise, <see langword="false"/>.</returns>
    /// <remarks>The calendar day is read from the value's own offset (its wall-clock date), not from UTC.</remarks>
    public static bool IsLastDayOfMonth(DateTimeOffset? value)
    {
        if (value is null)
            return false;

        var date = value.Value;
        return date.Day == DateTime.DaysInMonth(date.Year, date.Month);
    }
}
