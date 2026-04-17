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
    public static bool IsDurationBetween(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsBetween(value.Value, min, max, inclusion);

    /// <summary>
    /// Determines whether the specified duration is greater than the given threshold.
    /// </summary>
    /// <param name="value">The duration to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="threshold">The threshold to compare against. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is greater than <paramref name="threshold"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsGreaterThan(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (value is null || threshold is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Value, threshold.Value, inclusion);
    }

    /// <summary>
    /// Determines whether the specified duration is less than the given threshold.
    /// </summary>
    /// <param name="value">The duration to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="threshold">The threshold to compare against. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is less than <paramref name="threshold"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsLessThan(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (value is null || threshold is null)
            return false;

        return RuleComparison.IsLessThan(value.Value, threshold.Value, inclusion);
    }
}
