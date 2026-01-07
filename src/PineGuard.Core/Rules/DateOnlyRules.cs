using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateOnlyRules
{
    public static bool IsInPast(DateOnly? value)
    {
        if (value is null)
            return false;

        return value.Value < DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static bool IsInFuture(DateOnly? value)
    {
        if (value is null)
            return false;

        return value.Value > DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public static bool IsBetween(DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsChronological(DateOnly? start, DateOnly? end, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsChronological(start, end, inclusion);

    public static bool IsOverlapping(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsOverlapping(start1, end1, start2, end2, inclusion);

    public static bool IsBefore(DateOnly? value, DateOnly other, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsLessThan(value.Value, other, inclusion);
    }

    public static bool IsAfter(DateOnly? value, DateOnly other, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Value, other, inclusion);
    }

    public static bool IsSame(DateOnly? value, DateOnly other)
    {
        if (value is null) 
            return false;

        return RuleComparison.Equals(value.Value, other);
    }
}
