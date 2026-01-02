using PineGuard.Common;

namespace PineGuard.Rules;

internal static class RangeRules
{
    internal static bool IsChronological<T>(T? start, T? end, RangeInclusion inclusion)
        where T : struct, IComparable<T>
    {
        if (start is null || end is null)
            return false;

        return inclusion == RangeInclusion.Inclusive
            ? start.Value.CompareTo(end.Value) <= 0
            : start.Value.CompareTo(end.Value) < 0;
    }

    internal static bool IsOverlapping<T>(T? start1, T? end1, T? start2, T? end2, RangeInclusion inclusion)
        where T : struct, IComparable<T>
    {
        if (start1 is null || end1 is null || start2 is null || end2 is null)
            return false;

        if (inclusion == RangeInclusion.Exclusive)
            return start1.Value.CompareTo(end2.Value) < 0 && start2.Value.CompareTo(end1.Value) < 0;

        return start1.Value.CompareTo(end2.Value) <= 0 && start2.Value.CompareTo(end1.Value) <= 0;
    }
}
