namespace PineGuard.Rules;

internal static class RuleComparison
{
    internal static bool IsBetween<T>(T value, T min, T max, RangeInclusion inclusion)
        where T : IComparable<T>
    {
        if (min.CompareTo(max) > 0)
        {
            return false;
        }

        var minOk = inclusion == RangeInclusion.Inclusive
            ? value.CompareTo(min) >= 0
            : value.CompareTo(min) > 0;

        var maxOk = inclusion == RangeInclusion.Inclusive
            ? value.CompareTo(max) <= 0
            : value.CompareTo(max) < 0;

        return minOk && maxOk;
    }

    internal static bool IsGreaterThan<T>(T value, T min, RangeInclusion inclusion)
        where T : IComparable<T>
    {
        return inclusion == RangeInclusion.Inclusive
            ? value.CompareTo(min) >= 0
            : value.CompareTo(min) > 0;
    }

    internal static bool IsLessThan<T>(T value, T max, RangeInclusion inclusion)
        where T : IComparable<T>
    {
        return inclusion == RangeInclusion.Inclusive
            ? value.CompareTo(max) <= 0
            : value.CompareTo(max) < 0;
    }
}
