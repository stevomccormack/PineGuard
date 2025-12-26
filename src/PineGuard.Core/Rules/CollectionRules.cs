using PineGuard.Common;

namespace PineGuard.Rules;

public static class CollectionRules
{
    public static bool IsEmpty<T>(IEnumerable<T>? value)
    {
        if (value is null)
            return false;

        var count = TryGetCount(value);
        if (count is not null)
            return count.Value == 0;

        using var e = value.GetEnumerator();
        return !e.MoveNext();
    }

    public static bool HasItems<T>(IEnumerable<T>? value)
    {
        if (value is null)
            return false;

        var count = TryGetCount(value);
        if (count is not null)
            return count.Value != 0;

        using var e = value.GetEnumerator();
        return e.MoveNext();
    }

    public static bool HasExactCount<T>(IEnumerable<T>? value, int count)
    {
        if (value is null || count < 0)
            return false;

        var knownCount = TryGetCount(value);
        if (knownCount is not null)
            return knownCount.Value == count;

        var seen = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            seen++;
            if (seen > count)
                return false;
        }

        return seen == count;
    }

    public static bool HasMinCount<T>(IEnumerable<T>? value, int min)
    {
        if (value is null || min < 0)
            return false;

        var count = TryGetCount(value);
        if (count is not null)
            return count.Value >= min;

        var seen = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            seen++;
            if (seen >= min)
                return true;
        }

        return false;
    }

    public static bool HasMaxCount<T>(IEnumerable<T>? value, int max)
    {
        if (value is null || max < 0)
            return false;

        var count = TryGetCount(value);
        if (count is not null)
            return count.Value <= max;

        var seen = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            seen++;
            if (seen > max)
                return false;
        }

        return true;
    }

    public static bool HasCountBetween<T>(IEnumerable<T>? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        if (min < 0 || max < 0 || min > max)
            return false;

        var count = TryGetCount(value);
        if (count is not null)
            return RuleComparison.IsBetween(count.Value, min, max, inclusion);

        var upperBound = inclusion == RangeInclusion.Inclusive ? max : max - 1;
        if (upperBound < 0)
            return false;

        var seen = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            seen++;
            if (seen > upperBound)
                return false;
        }

        return RuleComparison.IsBetween(seen, min, max, inclusion);
    }

    public static bool HasAny<T>(IEnumerable<T>? value, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (value is null)
            return false;

        foreach (var item in value)
        {
            if (predicate(item))
                return true;
        }

        return false;
    }

    public static bool HasAll<T>(IEnumerable<T>? value, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (value is null)
            return false;

        foreach (var item in value)
        {
            if (!predicate(item))
                return false;
        }

        return true;
    }

    public static bool Contains<T>(IEnumerable<T>? value, T item)
    {
        if (value is null)
            return false;

        if (value is ICollection<T> c)
            return c.Contains(item);

        foreach (var element in value)
        {
            if (EqualityComparer<T>.Default.Equals(element, item))
                return true;
        }

        return false;
    }

    public static bool IsSubsetOf<T>(IEnumerable<T>? value, IEnumerable<T>? other)
    {
        if (value is null || other is null)
            return false;

        var otherSet = other as HashSet<T> ?? [.. other];
        foreach (var item in value)
        {
            if (!otherSet.Contains(item))
                return false;
        }

        return true;
    }

    public static bool HasValidIndex<T>(IEnumerable<T>? value, int index)
    {
        if (value is null || index < 0)
            return false;

        var count = TryGetCount(value);
        if (count is not null)
            return index < count.Value;

        var i = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            if (i == index)
                return true;

            i++;
        }

        return false;
    }

    private static int? TryGetCount<T>(IEnumerable<T> value) => value switch
    {
        ICollection<T> c => c.Count,
        IReadOnlyCollection<T> rc => rc.Count,
        _ => null
    };
}
