using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="DateTime"/> validation predicates for temporal comparisons, range checks, and calendar queries.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/datetime">DateTime Rules documentation</seealso>
public static class DateTimeRules
{
    /// <summary>
    /// Determines whether the specified date/time is in the past relative to UTC now.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the past; otherwise, <see langword="false"/>.</returns>
    public static bool IsInPast(DateTime? value, Inclusion inclusion = Inclusion.Exclusive) =>
        IsBefore(value, DateTime.UtcNow, inclusion);

    /// <summary>
    /// Determines whether the specified date/time is in the future relative to UTC now.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the future; otherwise, <see langword="false"/>.</returns>
    public static bool IsInFuture(DateTime? value, Inclusion inclusion = Inclusion.Exclusive) =>
        IsAfter(value, DateTime.UtcNow, inclusion);

    /// <summary>
    /// Determines whether the specified date/time falls within [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the date/time range.</param>
    /// <param name="max">The upper bound of the date/time range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public static bool IsBetween(DateTime? value, DateTime min, DateTime max,
        Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsBetween(value.Value, min, max, inclusion);

    /// <summary>
    /// Determines whether the specified date/time is before the given reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is before <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsBefore(DateTime? value, DateTime? other, Inclusion inclusion = Inclusion.Inclusive,
        DateTimePrecision? precision = null)
    {
        if (value is null || other is null)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var otherUtc = DateTimeUtility.ToUtc(other)!.Value;

        if (precision is null)
            return RuleComparison.IsLessThan(valueUtc, otherUtc, inclusion);

        if (!DateTimeUtility.TryTruncateToPrecisionUtc(valueUtc, precision.Value, out var tValue) ||
            !DateTimeUtility.TryTruncateToPrecisionUtc(otherUtc, precision.Value, out var tOther))
            return false;

        valueUtc = tValue!.Value;
        otherUtc = tOther!.Value;

        return RuleComparison.IsLessThan(valueUtc, otherUtc, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date/time is after the given reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is after <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAfter(DateTime? value, DateTime? other, Inclusion inclusion = Inclusion.Inclusive,
        DateTimePrecision? precision = null)
    {
        if (value is null || other is null)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var otherUtc = DateTimeUtility.ToUtc(other)!.Value;

        if (precision is null)
            return RuleComparison.IsGreaterThan(valueUtc, otherUtc, inclusion);

        if (!DateTimeUtility.TryTruncateToPrecisionUtc(valueUtc, precision.Value, out var tValue) ||
            !DateTimeUtility.TryTruncateToPrecisionUtc(otherUtc, precision.Value, out var tOther))
            return false;

        valueUtc = tValue!.Value;
        otherUtc = tOther!.Value;

        return RuleComparison.IsGreaterThan(valueUtc, otherUtc, inclusion);
    }

    /// <summary>
    /// Determines whether two date/times are the same, optionally truncated to the given precision.
    /// </summary>
    /// <param name="value">The first date/time. If <see langword="null"/> and <paramref name="other"/> is also <see langword="null"/>, returns <see langword="true"/>.</param>
    /// <param name="other">The second date/time.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if the date/times are equal at the given precision; otherwise, <see langword="false"/>.</returns>
    public static bool IsSame(DateTime? value, DateTime? other, DateTimePrecision? precision = null)
    {
        if (value is null && other is null)
            return true;

        if (value is null || other is null)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var otherUtc = DateTimeUtility.ToUtc(other)!.Value;

        if (precision is null)
            return RuleComparison.Equals(valueUtc, otherUtc);

        if (!DateTimeUtility.TryTruncateToPrecisionUtc(valueUtc, precision.Value, out var tValue) ||
            !DateTimeUtility.TryTruncateToPrecisionUtc(otherUtc, precision.Value, out var tOther))
            return false;

        valueUtc = tValue!.Value;
        otherUtc = tOther!.Value;

        return RuleComparison.Equals(valueUtc, otherUtc);
    }

    /// <summary>
    /// Determines whether the start date/time is chronologically before or equal to the end date/time.
    /// </summary>
    /// <param name="start">The start date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="start"/> precedes <paramref name="end"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsChronological(DateTime? start, DateTime? end, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsChronological(start, end, inclusion);

    /// <summary>
    /// Determines whether two date/time ranges overlap.
    /// </summary>
    /// <param name="start1">The start of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end1">The end of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="start2">The start of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end2">The end of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if the two ranges overlap; otherwise, <see langword="false"/>.</returns>
    public static bool IsOverlapping(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2,
        Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsOverlapping(start1, end1, start2, end2, inclusion);

    /// <summary>
    /// Determines whether the specified date/time is within the given number of days from UTC now.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="days">The maximum number of days allowed. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the absolute day difference from UTC now is within the limit; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithinDaysFromNow(DateTime? value, int days)
    {
        if (value is null || days < 0)
            return false;

        var diffDays = Math.Abs((value.Value - DateTime.UtcNow).TotalDays);
        return diffDays <= days;
    }

    /// <summary>
    /// Determines whether the specified date/time is within a given time window of a reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="window">The maximum allowed time difference. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the time difference is within the window; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithin(DateTime? value, DateTime? reference, TimeSpan window)
    {
        if (value is null || reference is null)
            return false;

        if (window < TimeSpan.Zero)
            return false;

        var diff = DateTimeUtility.Diff(value, reference);
        var diffTicks = diff!.Value.Ticks;
        var absTicks = Math.Abs(diffTicks);

        return absTicks <= window.Ticks;
    }

    /// <summary>
    /// Determines whether the specified date/time is within the given number of calendar months from a reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="months">The maximum number of calendar months allowed. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the month difference is within the limit; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithinCalendarMonths(DateTime? value, DateTime? reference, int months)
    {
        if (value is null || reference is null)
            return false;

        if (months < 0)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var referenceUtc = DateTimeUtility.ToUtc(reference)!.Value;
        var valueMonthIndex = (valueUtc.Year * 12) + valueUtc.Month;
        var referenceMonthIndex = (referenceUtc.Year * 12) + referenceUtc.Month;

        var monthDiff = Math.Abs(valueMonthIndex - referenceMonthIndex);
        return monthDiff <= months;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on a weekday (Monday through Friday).
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a weekday; otherwise, <see langword="false"/>.</returns>
    public static bool IsWeekday(DateTime? value)
    {
        // ReSharper disable once UseNullPropagation
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a weekend day; otherwise, <see langword="false"/>.</returns>
    public static bool IsWeekend(DateTime? value)
    {
        // ReSharper disable once UseNullPropagation
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on the first day of its month.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is the first day of the month; otherwise, <see langword="false"/>.</returns>
    public static bool IsFirstDayOfMonth(DateTime? value)
    {
        if (value is null)
            return false;

        return value.Value.Day == 1;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on the last day of its month.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is the last day of the month; otherwise, <see langword="false"/>.</returns>
    public static bool IsLastDayOfMonth(DateTime? value)
    {
        if (value is null)
            return false;

        var date = value.Value;
        return date.Day == DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    /// Determines whether two date/times fall on the same calendar day (ignoring time components).
    /// </summary>
    /// <param name="value">The first date/time. If <see langword="null"/> and <paramref name="other"/> is also <see langword="null"/>, returns <see langword="true"/>.</param>
    /// <param name="other">The second date/time.</param>
    /// <returns><see langword="true"/> if both values have the same date component; otherwise, <see langword="false"/>.</returns>
    public static bool IsSameDay(DateTime? value, DateTime? other)
    {
        if (value is null && other is null) return true;
        if (value is null || other is null) return false;

        return value.Value.Date == other.Value.Date;
    }

    /// <summary>
    /// Determines whether the specified date/time has <see cref="DateTimeKind.Utc"/> kind.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is UTC; otherwise, <see langword="false"/>.</returns>
    public static bool IsUtc(DateTime? value) => value is not null && value.Value.Kind == DateTimeKind.Utc;

    /// <summary>
    /// Determines whether the specified date/time has <see cref="DateTimeKind.Local"/> kind.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is local; otherwise, <see langword="false"/>.</returns>
    public static bool IsLocal(DateTime? value) => value is not null && value.Value.Kind == DateTimeKind.Local;

    /// <summary>
    /// Determines whether the specified date/time has <see cref="DateTimeKind.Unspecified"/> kind.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> has unspecified kind; otherwise, <see langword="false"/>.</returns>
    public static bool IsUnspecified(DateTime? value) =>
        value is not null && value.Value.Kind == DateTimeKind.Unspecified;

    /// <summary>
    /// Checks whether the specified date/time has an explicit <see cref="DateTimeKind"/> (not <see cref="DateTimeKind.Unspecified"/>).
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> has an explicit kind; otherwise, <see langword="false"/>.</returns>
    public static bool HasExplicitKind(DateTime? value) =>
        value is not null && value.Value.Kind != DateTimeKind.Unspecified;
}
