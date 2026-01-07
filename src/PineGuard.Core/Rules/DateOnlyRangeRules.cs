using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateOnlyRangeRules
{
    public static bool IsChronological(DateOnlyRange? range, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range is null)
            return false;

        var r = range.Value;
        return RangeRules.IsChronological<DateOnly>(r.Start, r.End, inclusion);
    }

    public static bool IsOverlapping(DateOnlyRange? range1, DateOnlyRange? range2, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range1 is null || range2 is null)
            return false;

        return range1.Value.Overlaps(range2.Value, inclusion);
    }
}
