using PineGuard.Common;

namespace PineGuard.Rules;

public static class TimeOnlyRangeRules
{
    public static bool IsChronological(TimeOnlyRange? range, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range is null)
            return false;

        var r = range.Value;
        return RangeRules.IsChronological<TimeOnly>(r.Start, r.End, inclusion);
    }

    public static bool IsOverlapping(TimeOnlyRange? range1, TimeOnlyRange? range2, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range1 is null || range2 is null)
            return false;

        return range1.Value.Overlaps(range2.Value, inclusion);
    }
}
