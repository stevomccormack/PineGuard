namespace PineGuard.Common;

/// <summary>
/// Compares temporal coordinates measured in nonnegative days or ticks.
/// </summary>
internal static class TemporalRangeComparison
{
    internal static void ValidateInclusion(Inclusion inclusion)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null);
    }

    internal static bool Overlaps(long start, long end, long otherStart, long otherEnd, Inclusion inclusion)
    {
        ValidateInclusion(inclusion);

        var lower = Math.Max(start, otherStart);
        var upper = Math.Min(end, otherEnd);

        // Subtraction is safe for the nonnegative, bounded temporal coordinates.
        // An open intersection needs a representable point strictly between its endpoints.
        return inclusion == Inclusion.Inclusive ? lower <= upper : upper - lower > 1;
    }

    internal static bool IsAdjacent(long start, long end, long otherStart, long otherEnd) =>
        start - otherEnd == 1 || otherStart - end == 1;
}
