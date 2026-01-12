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
        switch (value)
        {
            case null:
                return false;
            case ICollection<T> c:
                return c.Count != 0;
            case IReadOnlyCollection<T> rc:
                return rc.Count != 0;
            default:
            {
                using var e = value.GetEnumerator();
                return e.MoveNext();
            }
        }
    }

    public static bool HasExactCount<T>(IEnumerable<T>? value, int count)
    {
        if (value is null || count < 0)
            return false;

        return value switch
        {
            ICollection<T> c => c.Count == count,
            IReadOnlyCollection<T> rc => rc.Count == count,
            _ => TryGetCountUpTo(value, maxInclusive: count, out var seen) && seen == count
        };
    }

    public static bool HasMinCount<T>(IEnumerable<T>? value, int min)
    {
        if (value is null || min < 0)
            return false;

        return value switch
        {
            ICollection<T> c => c.Count >= min,
            IReadOnlyCollection<T> rc => rc.Count >= min,
            _ => min == 0 || HasIndex(value, min - 1)
        };

        // Has at least min items if there exists an element at index (min-1)
    }

    public static bool HasMaxCount<T>(IEnumerable<T>? value, int max)
    {
        if (value is null || max < 0)
            return false;

        return value switch
        {
            ICollection<T> c => c.Count <= max,
            IReadOnlyCollection<T> rc => rc.Count <= max,
            _ => !HasIndex(value, max)
        };

        // Has at most max items if it does NOT have an element at index max
    }

    public static bool HasCountBetween<T>(IEnumerable<T>? value, int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        if (min < 0 || max < 0 || min > max)
            return false;

        switch (value)
        {
            case ICollection<T> c:
                return RuleComparison.IsBetween(c.Count, min, max, inclusion);
            case IReadOnlyCollection<T> rc:
                return RuleComparison.IsBetween(rc.Count, min, max, inclusion);
        }

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

        return value.Any(predicate);
    }

    public static bool HasAll<T>(IEnumerable<T>? value, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (value is null)
            return false;

        return value.All(predicate);
    }

    public static bool Contains<T>(IEnumerable<T>? value, T item)
    {
        return value switch
        {
            null => false,
            ICollection<T> c => c.Contains(item),
            _ => value.Any(element => EqualityComparer<T>.Default.Equals(element, item))
        };
    }

    public static bool IsSubsetOf<T>(IEnumerable<T>? value, IEnumerable<T>? other)
    {
        if (value is null || other is null)
            return false;

        var otherSet = other as HashSet<T> ?? [.. other];
        return value.All(item => otherSet.Contains(item));
    }

    public static bool HasIndex<T>(IEnumerable<T>? value, int index)
    {
        if (value is null || index < 0)
            return false;

        return value switch
        {
            ICollection<T> c => index < c.Count,
            IReadOnlyCollection<T> rc => index < rc.Count,
            _ => CollectionUtility.TryGet(value, index, out _)
        };
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
