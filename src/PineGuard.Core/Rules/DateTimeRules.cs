namespace PineGuard.Rules;

public static class DateTimeRules
{
    public static bool IsInPast(DateTime value) => value < DateTime.UtcNow;

    public static bool IsNotInPast(DateTime value) => !IsInPast(value);

    public static bool IsInFuture(DateTime value) => value > DateTime.UtcNow;

    public static bool IsNotInFuture(DateTime value) => !IsInFuture(value);

    public static bool IsBetween(DateTime value, DateTime min, DateTime max, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        RuleComparison.IsBetween(value, min, max, inclusion);

    public static bool IsNotBetween(DateTime value, DateTime min, DateTime max, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        !IsBetween(value, min, max, inclusion);

    public static bool IsChronological(DateTime start, DateTime end, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        return inclusion == RangeInclusion.Inclusive
            ? start <= end
            : start < end;
    }

    public static bool IsNotChronological(DateTime start, DateTime end, RangeInclusion inclusion = RangeInclusion.Exclusive) =>
        !IsChronological(start, end, inclusion);

    public static bool IsOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2, RangeInclusion inclusion = RangeInclusion.Exclusive)
    {
        if (inclusion == RangeInclusion.Exclusive)
        {
            return start1 < end2 && start2 < end1;
        }

        return start1 <= end2 && start2 <= end1;
    }

    public static bool IsNotOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2, RangeInclusion inclusion = RangeInclusion.Exclusive) =>
        !IsOverlapping(start1, end1, start2, end2, inclusion);
}
