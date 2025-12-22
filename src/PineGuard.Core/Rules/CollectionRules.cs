namespace PineGuard.Rules;

public static class CollectionRules
{
    public static bool HasItems<T>(IEnumerable<T>? value)
    {
        if (value is null)
        {
            return false;
        }

        using var e = value.GetEnumerator();
        return e.MoveNext();
    }

    public static bool HasEmpty<T>(IEnumerable<T>? value)
    {
        if (value is null)
        {
            return false;
        }

        using var e = value.GetEnumerator();
        return !e.MoveNext();
    }

    public static bool HasExactCount<T>(IEnumerable<T>? value, int count)
    {
        if (value is null || count < 0)
        {
            return false;
        }

        if (value is ICollection<T> c)
        {
            return c.Count == count;
        }

        var i = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            i++;
            if (i > count)
            {
                return false;
            }
        }

        return i == count;
    }

    public static bool HasCountBetween<T>(IEnumerable<T>? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
        {
            return false;
        }

        if (min < 0 || max < 0 || min > max)
        {
            return false;
        }

        var count = TryGetCount(value);
        if (count is not null)
        {
            return RuleComparison.IsBetween(count.Value, min, max, inclusion);
        }

        var seen = 0;
        var upperBound = inclusion == RangeInclusion.Inclusive ? max : max - 1;
        if (upperBound < 0)
        {
            return false;
        }

        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            seen++;
            if (seen > upperBound)
            {
                return false;
            }
        }

        return RuleComparison.IsBetween(seen, min, max, inclusion);
    }

    public static bool HasAny<T>(IEnumerable<T>? value, Func<T, bool> predicate)
    {
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (value is null)
        {
            return false;
        }

        foreach (var item in value)
        {
            if (predicate(item))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasValidIndex<T>(IEnumerable<T>? value, int index)
    {
        if (value is null || index < 0)
        {
            return false;
        }

        if (value is IList<T> list)
        {
            return index < list.Count;
        }

        if (value is IReadOnlyList<T> roList)
        {
            return index < roList.Count;
        }

        var i = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            if (i == index)
            {
                return true;
            }

            i++;
        }

        return false;
    }

    public static bool HasNotItems<T>(IEnumerable<T>? value) => !HasItems(value);

    public static bool HasNotEmpty<T>(IEnumerable<T>? value) => !HasEmpty(value);

    public static bool HasNotExactCount<T>(IEnumerable<T>? value, int count) => !HasExactCount(value, count);

    public static bool HasNotCountBetween<T>(IEnumerable<T>? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        !HasCountBetween(value, min, max, inclusion);

    public static bool HasNotAny<T>(IEnumerable<T>? value, Func<T, bool> predicate) => !HasAny(value, predicate);

    public static bool HasNotValidIndex<T>(IEnumerable<T>? value, int index) => !HasValidIndex(value, index);

    private static int? TryGetCount<T>(IEnumerable<T> value)
    {
        return value switch
        {
            ICollection<T> c => c.Count,
            IReadOnlyCollection<T> rc => rc.Count,
            _ => null
        };
    }
}
