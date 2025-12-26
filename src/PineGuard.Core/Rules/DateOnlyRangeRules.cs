using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateOnlyRangeRules
{
    public static bool IsChronological(DateOnlyRange? range, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (range is null)
            return false;

        var r = range.Value;

        return inclusion == RangeInclusion.Inclusive
            ? r.Start <= r.End
            : r.Start < r.End;
    }

    public static bool IsOverlapping(DateOnlyRange? range1, DateOnlyRange? range2, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (range1 is null || range2 is null)
            return false;

        return range1.Value.Overlaps(range2.Value, inclusion);
    }
}
