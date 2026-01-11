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

        if (value is IList<T> list)
        {
            if (index >= list.Count)
                return false;

            item = list[index];
            return true;
        }

        if (value is IReadOnlyList<T> roList)
        {
            if (index >= roList.Count)
                return false;

            item = roList[index];
            return true;
        }

        if (value is ICollection<T> c && index >= c.Count)
            return false;

        if (value is IReadOnlyCollection<T> rc && index >= rc.Count)
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

        if (value is IList<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], item))
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        var idx = 0;
        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            if (EqualityComparer<T>.Default.Equals(e.Current, item))
            {
                index = idx;
                return true;
            }

            idx++;
        }

        return false;
    }
}
