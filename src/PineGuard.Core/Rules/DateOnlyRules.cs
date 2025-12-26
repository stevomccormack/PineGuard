using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateOnlyRules
{
    public static bool IsInPast(DateOnly? value)
    {
        if (value is null)
            return false;

        return value.Value < DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static bool IsInFuture(DateOnly? value)
    {
        if (value is null)
            return false;

        return value.Value > DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static bool IsBetween(DateOnly? value, DateOnly min, DateOnly max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsChronological(DateOnly? start, DateOnly? end, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start is null || end is null)
            return false;

        return inclusion == RangeInclusion.Inclusive ? start.Value <= end.Value : start.Value < end.Value;
    }

    public static bool IsOverlapping(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start1 is null || end1 is null || start2 is null || end2 is null)
            return false;

        return inclusion == RangeInclusion.Exclusive
            ? start1.Value < end2.Value && start2.Value < end1.Value
            : start1.Value <= end2.Value && start2.Value <= end1.Value;
    }
}
