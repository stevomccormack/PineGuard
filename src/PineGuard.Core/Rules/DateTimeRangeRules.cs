using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateTimeRangeRules
{
    public static bool IsChronological(DateTimeRange? range, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range is null)
            return false;

        var r = range.Value;
        return RangeRules.IsChronological<DateTime>(r.Start, r.End, inclusion);
    }

    public static bool IsOverlapping(DateTimeRange? range1, DateTimeRange? range2, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range1 is null || range2 is null)
            return false;

        return range1.Value.Overlaps(range2.Value, inclusion);
    }
}
