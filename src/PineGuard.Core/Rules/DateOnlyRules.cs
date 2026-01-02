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

    public static bool IsChronological(DateOnly? start, DateOnly? end, RangeInclusion inclusion = RangeInclusion.Exclusive) =>
        RangeRules.IsChronological(start, end, inclusion);

    public static bool IsOverlapping(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, RangeInclusion inclusion = RangeInclusion.Exclusive) =>
        RangeRules.IsOverlapping(start1, end1, start2, end2, inclusion);
}
