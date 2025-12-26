using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateTimeOffsetRules
{
    public static bool IsInPast(DateTimeOffset? value)
    {
        if (value is null)
            return false;

        return value.Value < DateTimeOffset.UtcNow;
    }

    public static bool IsInFuture(DateTimeOffset? value)
    {
        if (value is null)
            return false;

        return value.Value > DateTimeOffset.UtcNow;
    }

    public static bool IsBetween(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsChronological(DateTimeOffset? start, DateTimeOffset? end, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start is null || end is null)
            return false;

        return inclusion == RangeInclusion.Inclusive ? start.Value <= end.Value : start.Value < end.Value;
    }

    public static bool IsOverlapping(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start1 is null || end1 is null || start2 is null || end2 is null)
            return false;

        return inclusion == RangeInclusion.Exclusive
            ? start1.Value < end2.Value && start2.Value < end1.Value
            : start1.Value <= end2.Value && start2.Value <= end1.Value;
    }
}
