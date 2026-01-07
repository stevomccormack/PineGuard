using PineGuard.Common;

namespace PineGuard.Rules;

public static class DateTimeOffsetRules
{
    public static bool IsInPast(DateTimeOffset? value)
    {
        if (value is null)
            return false;

        return value.Value < DateTimeOffset.UtcNow;
    }

    public static bool IsInFuture(DateTimeOffset? value)
    {
        if (value is null)
            return false;

        return value.Value > DateTimeOffset.UtcNow;
    }

    public static bool IsBetween(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Value, min, max, inclusion);
    }

    public static bool IsChronological(DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsChronological(start, end, inclusion);

    public static bool IsOverlapping(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion = Inclusion.Exclusive) =>
        RangeRules.IsOverlapping(start1, end1, start2, end2, inclusion);

    public static bool IsBefore(DateTimeOffset? value, DateTimeOffset other, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsLessThan(value.Value, other, inclusion);
    }

    public static bool IsAfter(DateTimeOffset? value, DateTimeOffset other, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Value, other, inclusion);
    }

    public static bool IsSame(DateTimeOffset? value, DateTimeOffset other)
    {
        if (value is null)
            return false;

        return RuleComparison.Equals(value.Value, other);
    }
}
