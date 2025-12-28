using PineGuard.Common;

namespace PineGuard.Rules;

public static class TimeOnlyRules
{
    public static bool IsBetween(TimeOnly? value, TimeOnly min, TimeOnly max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsChronological(TimeOnly? start, TimeOnly? end, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start is null || end is null)
            return false;

        return inclusion == RangeInclusion.Inclusive ? start.Value <= end.Value : start.Value < end.Value;
    }

    public static bool IsOverlapping(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (start1 is null || end1 is null || start2 is null || end2 is null)
            return false;

        return inclusion == RangeInclusion.Exclusive
            ? start1.Value < end2.Value && start2.Value < end1.Value
            : start1.Value <= end2.Value && start2.Value <= end1.Value;
    }
}
