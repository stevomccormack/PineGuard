using PineGuard.Common;

namespace PineGuard.Rules;

internal static class RangeRules
{
    internal static bool IsChronological<T>(T? start, T? end, Inclusion inclusion)
        where T : struct, IComparable<T>
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return start is not null && end is not null && RuleComparison.IsLessThan(start.Value, end.Value, inclusion);
    }

    internal static bool IsOverlapping(long? start1, long? end1, long? start2, long? end2, Inclusion inclusion)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (start1 is null || end1 is null || start2 is null || end2 is null)
            return false;

        if (start1.Value > end1.Value || start2.Value > end2.Value)
            return false;

        return TemporalRangeComparison.Overlaps(start1.Value, end1.Value, start2.Value, end2.Value, inclusion);
    }
}
