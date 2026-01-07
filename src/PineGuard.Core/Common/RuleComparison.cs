namespace PineGuard.Common;

internal static class RuleComparison
{
    internal static bool Equals<T>(T value, T other)
        where T : IComparable<T> =>
        value.CompareTo(other) == 0;

    internal static bool IsBetween<T>(T value, T min, T max, Inclusion inclusion = Inclusion.Inclusive)
        where T : IComparable<T>
    {
        if (min.CompareTo(max) > 0)
            return false;

        var inclusive = inclusion switch
        {
            Inclusion.Inclusive => true,
            Inclusion.Exclusive => false,
            _ => throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null)
        };

        var minOk = inclusive
            ? value.CompareTo(min) >= 0
            : value.CompareTo(min) > 0;

        var maxOk = inclusive
            ? value.CompareTo(max) <= 0
            : value.CompareTo(max) < 0;

        return minOk && maxOk;
    }

    internal static bool IsGreaterThan<T>(T value, T min, Inclusion inclusion = Inclusion.Inclusive)
        where T : IComparable<T>
    {
        return inclusion switch
        {
            Inclusion.Inclusive => value.CompareTo(min) >= 0,
            Inclusion.Exclusive => value.CompareTo(min) > 0,
            _ => throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null)
        };
    }

    internal static bool IsLessThan<T>(T value, T max, Inclusion inclusion = Inclusion.Inclusive)
        where T : IComparable<T>
    {
        return inclusion switch
        {
            Inclusion.Inclusive => value.CompareTo(max) <= 0,
            Inclusion.Exclusive => value.CompareTo(max) < 0,
            _ => throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null)
        };
    }
}
