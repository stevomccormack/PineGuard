namespace PineGuard.Utils;

public static class CollectionUtility
{
    public static bool TryGetCount<T>(IEnumerable<T>? value, out int count)
    {
        count = 0;

        if (value is null)
            return false;

        switch (value)
        {
            case ICollection<T> c:
                count = c.Count;
                return true;
            case IReadOnlyCollection<T> rc:
                count = rc.Count;
                return true;
            default:
                return false;
        }
    }

    public static bool TryGet<T>(IEnumerable<T>? value, int index, out T? item)
    {
        item = default;

        if (value is null || index < 0)
            return false;

        if (TryGetCount(value, out var count) && index >= count)
            return false;

        var i = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            if (i == index)
            {
                item = e.Current;
                return true;
            }

            i++;
        }

        item = default;
        return false;
    }

    public static bool TryGetIndex<T>(IEnumerable<T>? value, T item, out int index)
    {
        index = -1;

        if (value is null)
            return false;

        var i = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            if (EqualityComparer<T>.Default.Equals(e.Current, item))
            {
                index = i;
                return true;
            }

            i++;
        }

        return false;
    }
}
