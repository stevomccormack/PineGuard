namespace PineGuard.Common;

/// <summary>
/// Provides generic, <see cref="IComparable{T}"/>-based comparison helpers shared across PineGuard rules.
/// </summary>
public static class RuleComparison
{
    /// <summary>
    /// Determines whether <paramref name="value"/> and <paramref name="other"/> are equal, as determined by <see cref="IComparable{T}.CompareTo(T)"/>.
    /// </summary>
    /// <typeparam name="T">The comparable type being compared.</typeparam>
    /// <param name="value">The first value to compare.</param>
    /// <param name="other">The second value to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> compares as equal to <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool Equals<T>(T value, T other)
        where T : IComparable<T> =>
        value.CompareTo(other) == 0;

    /// <summary>
    /// Determines whether <paramref name="value"/> falls within the range bounded by <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">The comparable type being compared.</typeparam>
    /// <param name="value">The value to test.</param>
    /// <param name="min">The lower bound of the range.</param>
    /// <param name="max">The upper bound of the range.</param>
    /// <param name="inclusion">
    /// Whether <paramref name="min"/> and <paramref name="max"/> are included in the range. Defaults to <see cref="Inclusion.Inclusive"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> falls within the range; <see langword="false"/> if it falls outside the range,
    /// or if <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </returns>
    public static bool IsInRange<T>(T value, T min, T max, Inclusion inclusion = Inclusion.Inclusive)
        where T : IComparable<T>
    {
        var inclusive = inclusion switch
        {
            Inclusion.Inclusive => true,
            Inclusion.Exclusive => false,
            _ => throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null)
        };

        if (min.CompareTo(max) > 0)
            return false;

        var minOk = inclusive
            ? value.CompareTo(min) >= 0
            : value.CompareTo(min) > 0;

        var maxOk = inclusive
            ? value.CompareTo(max) <= 0
            : value.CompareTo(max) < 0;

        return minOk && maxOk;
    }

    /// <summary>
    /// Determines whether <paramref name="value"/> is greater than (or, when inclusive, equal to) <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="T">The comparable type being compared.</typeparam>
    /// <param name="value">The value to test.</param>
    /// <param name="other">The lower bound to compare against.</param>
    /// <param name="inclusion">
    /// Whether <paramref name="other"/> itself satisfies the comparison. Defaults to <see cref="Inclusion.Inclusive"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than <paramref name="other"/> (or equal to it, when <paramref name="inclusion"/>
    /// is <see cref="Inclusion.Inclusive"/>); otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsGreaterThan<T>(T value, T other, Inclusion inclusion = Inclusion.Inclusive)
        where T : IComparable<T> =>
        inclusion switch
        {
            Inclusion.Inclusive => value.CompareTo(other) >= 0,
            Inclusion.Exclusive => value.CompareTo(other) > 0,
            _ => throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null)
        };

    /// <summary>
    /// Determines whether <paramref name="value"/> is less than (or, when inclusive, equal to) <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="T">The comparable type being compared.</typeparam>
    /// <param name="value">The value to test.</param>
    /// <param name="other">The upper bound to compare against.</param>
    /// <param name="inclusion">
    /// Whether <paramref name="other"/> itself satisfies the comparison. Defaults to <see cref="Inclusion.Inclusive"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is less than <paramref name="other"/> (or equal to it, when <paramref name="inclusion"/>
    /// is <see cref="Inclusion.Inclusive"/>); otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsLessThan<T>(T value, T other, Inclusion inclusion = Inclusion.Inclusive)
        where T : IComparable<T> =>
        inclusion switch
        {
            Inclusion.Inclusive => value.CompareTo(other) <= 0,
            Inclusion.Exclusive => value.CompareTo(other) < 0,
            _ => throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null)
        };
}
