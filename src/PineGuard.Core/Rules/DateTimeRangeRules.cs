using PineGuard.Common;

namespace PineGuard.Rules;

/// <summary>
/// Provides validation predicates for <see cref="DateTimeRange"/> structs.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/datetime">DateTime Rules documentation</seealso>
public static class DateTimeRangeRules
{
    /// <summary>
    /// Determines whether the specified date-time range is chronological (start &lt;= end).
    /// </summary>
    /// <param name="range">The range to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the endpoints are inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <c>range.Start</c> is before (or equal to, if inclusive) <c>range.End</c>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsChronological(DateTimeRange? range, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range is null)
            return false;

        var r = range.Value;
        return RangeRules.IsChronological<DateTime>(r.Start, r.End, inclusion);
    }

    /// <summary>
    /// Determines whether two date-time ranges overlap.
    /// </summary>
    /// <param name="range1">The first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="range2">The second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the endpoints are inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="range1"/> and <paramref name="range2"/> overlap;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsOverlapping(DateTimeRange? range1, DateTimeRange? range2, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (range1 is null || range2 is null)
            return false;

        return range1.Value.Overlaps(range2.Value, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date-time falls within the given range.
    /// </summary>
    /// <param name="range">The range to test against. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="value">The date-time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the endpoints are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> falls within <paramref name="range"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Contains(DateTimeRange? range, DateTime? value, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (range is null || value is null)
            return false;

        var r = range.Value;
        var v = value.Value;

        return inclusion == Inclusion.Exclusive ? v > r.Start && v < r.End : v >= r.Start && v <= r.End;
    }
}
