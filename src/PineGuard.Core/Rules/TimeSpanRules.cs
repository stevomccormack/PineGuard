using PineGuard.Common;

namespace PineGuard.Rules;

public static class TimeSpanRules
{
    public static bool IsDurationBetween(TimeSpan? value, TimeSpan min, TimeSpan max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsGreaterThan(TimeSpan? value, TimeSpan threshold)
    {
        if (value is null)
            return false;

        return value.Value > threshold;
    }

    public static bool IsLessThan(TimeSpan? value, TimeSpan threshold)
    {
        if (value is null)
            return false;

        return value.Value < threshold;
    }
}