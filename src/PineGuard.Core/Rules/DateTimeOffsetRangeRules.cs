using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateTimeOffsetRangeRules
{
    public static bool IsChronological(DateTimeOffsetRange? range, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (range is null)
            return false;

        var r = range.Value;
        return RangeRules.IsChronological<DateTimeOffset>(r.Start, r.End, inclusion);
    }

    public static bool IsOverlapping(DateTimeOffsetRange? range1, DateTimeOffsetRange? range2, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (range1 is null || range2 is null)
            return false;

        return range1.Value.Overlaps(range2.Value, inclusion);
    }
}
