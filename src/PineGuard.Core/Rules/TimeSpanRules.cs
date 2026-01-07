using PineGuard.Common;

namespace PineGuard.Rules;

public static class TimeSpanRules
{
    public static bool IsDurationBetween(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsGreaterThan(TimeSpan? value, TimeSpan threshold, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Value, threshold, inclusion);
    }

    public static bool IsLessThan(TimeSpan? value, TimeSpan threshold, Inclusion inclusion = Inclusion.Exclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsLessThan(value.Value, threshold, inclusion);
    }
}