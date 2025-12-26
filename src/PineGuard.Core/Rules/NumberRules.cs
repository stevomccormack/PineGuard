using PineGuard.Common;
using System.Numerics;

namespace PineGuard.Rules;

public static class NumberRules
{
    public static bool IsPositive<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value > T.Zero;

    public static bool IsNegative<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value < T.Zero;

    public static bool IsZero<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value == T.Zero;

    public static bool IsNotZero<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value != T.Zero;

    public static bool IsGreaterThan<T>(T? value, T min, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : struct, IComparable<T>
    {
        if (value is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Value, min, inclusion);
    }

    public static bool IsGreaterThanOrEqual<T>(T? value, T min) where T : struct, INumber<T> =>
        value is not null && value.Value >= min;

    public static bool IsLessThan<T>(T? value, T max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : struct, IComparable<T>
    {
        if (value is null)
            return false;

        return RuleComparison.IsLessThan(value.Value, max, inclusion);
    }

    public static bool IsLessThanOrEqual<T>(T? value, T max) where T : struct, INumber<T> =>
        value is not null && value.Value <= max;

    public static bool IsInRange<T>(T? value, T min, T max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        where T : struct, IComparable<T>
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsApproximately<T>(T? value, T target, T? tolerance) where T : struct, INumber<T>
    {
        if (value is null || tolerance is null)
            return false;

        if (tolerance.Value < T.Zero)
            return false;

        var diff = T.Abs(value.Value - target);
        return diff <= tolerance.Value;
    }
}
