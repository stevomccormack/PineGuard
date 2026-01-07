using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static class DateTimeRules
{
    public static bool IsInPast(DateTime value) => 
        DateTimeUtility.ToUtc(value) < DateTime.UtcNow;

    public static bool IsInFuture(DateTime value) => 
        DateTimeUtility.ToUtc(value) > DateTime.UtcNow;

    public static bool IsBetween(DateTime value, DateTime min, DateTime max, Inclusion inclusion = Inclusion.Inclusive) =>
        RuleComparison.IsBetween(value, min, max, inclusion);

    public static bool IsChronological(DateTime? start, DateTime? end, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsChronological(start, end, inclusion);

    public static bool IsOverlapping(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsOverlapping(start1, end1, start2, end2, inclusion);

    public static bool IsBefore(DateTime value, DateTime other, Inclusion inclusion = Inclusion.Inclusive) =>
        RuleComparison.IsLessThan(DateTimeUtility.ToUtc(value), DateTimeUtility.ToUtc(other), inclusion);

    public static bool IsAfter(DateTime value, DateTime other, Inclusion inclusion = Inclusion.Inclusive) =>
        RuleComparison.IsGreaterThan(DateTimeUtility.ToUtc(value), DateTimeUtility.ToUtc(other), inclusion);

    public static bool IsSame(DateTime value, DateTime other) =>
        DateTimeUtility.ToUtc(value) == DateTimeUtility.ToUtc(other);

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


    public static bool IsUtc(DateTime value) => value.Kind == DateTimeKind.Utc;

    public static bool IsLocal(DateTime value) => value.Kind == DateTimeKind.Local;

    public static bool IsUnspecified(DateTime value) => value.Kind == DateTimeKind.Unspecified;

    public static bool HasExplicitKind(DateTime value) => value.Kind != DateTimeKind.Unspecified;

}
