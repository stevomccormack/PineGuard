using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateTimeRules
{
    public static bool IsInPast(DateTime value) => ToUtc(value) < DateTime.UtcNow;

    public static bool IsInFuture(DateTime value) => ToUtc(value) > DateTime.UtcNow;

    public static bool IsBetween(DateTime value, DateTime min, DateTime max, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        RuleComparison.IsBetween(value, min, max, inclusion);

    public static bool IsChronological(DateTime? start, DateTime? end, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start is null || end is null)
            return false;

        return inclusion == RangeInclusion.Inclusive ? start.Value <= end.Value : start.Value < end.Value;
    }

    public static bool IsOverlapping(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start1 is null || end1 is null || start2 is null || end2 is null)
            return false;

        return inclusion == RangeInclusion.Exclusive
            ? start1.Value < end2.Value && start2.Value < end1.Value
            : start1.Value <= end2.Value && start2.Value <= end1.Value;
    }

    public static bool IsWithinDaysFromNow(DateTime? value, int days)
    {
        if (value is null || days < 0)
            return false;

        var diffDays = Math.Abs((value.Value - DateTime.UtcNow).TotalDays);
        return diffDays <= days;
    }

    public static bool IsWeekday(DateTime? value)
    {
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday;
    }

    public static bool IsWeekend(DateTime? value)
    {
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    public static bool IsFirstDayOfMonth(DateTime? value)
    {
        if (value is null)
            return false;

        return value.Value.Day == 1;
    }

    public static bool IsLastDayOfMonth(DateTime? value)
    {
        if (value is null)
            return false;

        var date = value.Value;
        return date.Day == DateTime.DaysInMonth(date.Year, date.Month);
    }

    public static bool IsSameDay(DateTime? value, DateTime? other)
    {
        if (value is null || other is null)
            return false;

        return value.Value.Date == other.Value.Date;
    }

    public static bool HasExplicitKind(DateTime value) => value.Kind != DateTimeKind.Unspecified;

    public static bool IsUtc(DateTime value) => value.Kind == DateTimeKind.Utc;

    public static bool IsLocal(DateTime value) => value.Kind == DateTimeKind.Local;

    public static bool IsUnspecified(DateTime value) => value.Kind == DateTimeKind.Unspecified;

    private static DateTime ToUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Utc => value,
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
}
