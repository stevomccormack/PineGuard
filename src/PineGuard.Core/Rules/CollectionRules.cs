using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static class CollectionRules
{
    public static bool IsEmpty<T>(IEnumerable<T>? value)
    {
        if (value is null)
            return false;

        if (value is ICollection<T> c)
            return c.Count == 0;

        if (value is IReadOnlyCollection<T> rc)
            return rc.Count == 0;

        using var e = value.GetEnumerator();
        return !e.MoveNext();
    }

    public static bool HasItems<T>(IEnumerable<T>? value)
    {
        if (value is null)
            return false;

        if (value is ICollection<T> c)
            return c.Count != 0;

        if (value is IReadOnlyCollection<T> rc)
            return rc.Count != 0;

        using var e = value.GetEnumerator();
        return e.MoveNext();
    }

    public static bool HasExactCount<T>(IEnumerable<T>? value, int count)
    {
        if (value is null || count < 0)
            return false;

        if (value is ICollection<T> c)
            return c.Count == count;

        if (value is IReadOnlyCollection<T> rc)
            return rc.Count == count;

        return TryGetCountUpTo(value, maxInclusive: count, out var seen) && seen == count;
    }

    public static bool HasMinCount<T>(IEnumerable<T>? value, int min)
    {
        if (value is null || min < 0)
            return false;

        if (value is ICollection<T> c)
            return c.Count >= min;

        if (value is IReadOnlyCollection<T> rc)
            return rc.Count >= min;

        // Has at least min items if there exists an element at index (min-1)
        return min == 0 || HasIndex(value, min - 1);
    }

    public static bool HasMaxCount<T>(IEnumerable<T>? value, int max)
    {
        if (value is null || max < 0)
            return false;

        if (value is ICollection<T> c)
            return c.Count <= max;

        if (value is IReadOnlyCollection<T> rc)
            return rc.Count <= max;

        // Has at most max items if it does NOT have an element at index max
        return !HasIndex(value, max);
    }

    public static bool HasCountBetween<T>(IEnumerable<T>? value, int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        if (min < 0 || max < 0 || min > max)
            return false;

        if (value is ICollection<T> c)
            return RuleComparison.IsBetween(c.Count, min, max, inclusion);

        if (value is IReadOnlyCollection<T> rc)
            return RuleComparison.IsBetween(rc.Count, min, max, inclusion);

        var upperBound = inclusion == Inclusion.Inclusive ? max : max - 1;
        if (upperBound < 0)
            return false;

        if (!TryGetCountUpTo(value, maxInclusive: upperBound, out var seen))
            return false;

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

    public static bool HasIndex<T>(IEnumerable<T>? value, int index)
    {
        if (value is null || index < 0)
            return false;

        if (value is ICollection<T> c)
            return index < c.Count;

        if (value is IReadOnlyCollection<T> rc)
            return index < rc.Count;

        return CollectionUtility.TryGet(value, index, out _);
    }

    private static bool TryGetCountUpTo<T>(IEnumerable<T> value, int maxInclusive, out int seen)
    {
        seen = 0;

        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            seen++;

            if (seen > maxInclusive)
                return false;
        }

        return true;
    }
}
