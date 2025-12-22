namespace PineGuard.Rules;

public static class NumberRules
{
    public static bool IsPositive<T>(T value) where T : IComparable<T> => value.CompareTo(default!) > 0;

    public static bool IsNotPositive<T>(T value) where T : IComparable<T> => !IsPositive(value);

    public static bool IsNegative<T>(T value) where T : IComparable<T> => value.CompareTo(default!) < 0;

    public static bool IsNotNegative<T>(T value) where T : IComparable<T> => !IsNegative(value);

    public static bool IsZero<T>(T value) where T : IComparable<T> => value.CompareTo(default!) == 0;

    public static bool IsNotZero<T>(T value) where T : IComparable<T> => !IsZero(value);

    public static bool IsGreaterThan<T>(T value, T min, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : IComparable<T> =>
        RuleComparison.IsGreaterThan(value, min, inclusion);

    public static bool IsNotGreaterThan<T>(T value, T min, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : IComparable<T> =>
        !IsGreaterThan(value, min, inclusion);

    public static bool IsLessThan<T>(T value, T max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : IComparable<T> =>
        RuleComparison.IsLessThan(value, max, inclusion);

    public static bool IsNotLessThan<T>(T value, T max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : IComparable<T> =>
        !IsLessThan(value, max, inclusion);

    public static bool IsInRange<T>(T value, T min, T max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : IComparable<T> =>
        RuleComparison.IsBetween(value, min, max, inclusion);

    public static bool IsNotInRange<T>(T value, T min, T max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : IComparable<T> =>
        !IsInRange(value, min, max, inclusion);
}
