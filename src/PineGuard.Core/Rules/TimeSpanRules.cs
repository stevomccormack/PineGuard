using PineGuard.Common;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="TimeSpan"/> validation predicates for duration comparisons and range checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/timespan">TimeSpan Rules documentation</seealso>
public static class TimeSpanRules
{
    /// <summary>
    /// Determines whether the specified duration falls within [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="value">The duration to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the duration range.</param>
    /// <param name="max">The upper bound of the duration range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public static bool IsInRange(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion = Inclusion.Inclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return value is not null && RuleComparison.IsInRange(value.Value, min, max, inclusion);
    }

    /// <summary>
    /// Determines whether the specified duration is longer than the reference duration.
    /// </summary>
    /// <param name="value">The duration to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference duration to compare against.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is longer than <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsLongerThan(TimeSpan? value, TimeSpan other, Inclusion inclusion = Inclusion.Exclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (value is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Value, other, inclusion);
    }

    /// <summary>
    /// Determines whether the specified duration is shorter than the reference duration.
    /// </summary>
    /// <param name="value">The duration to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference duration to compare against.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is shorter than <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsShorterThan(TimeSpan? value, TimeSpan other, Inclusion inclusion = Inclusion.Exclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (value is null)
            return false;

        return RuleComparison.IsLessThan(value.Value, other, inclusion);
    }
}
