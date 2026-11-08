using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Utils;

/// <summary>
/// Provides collection access and query utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/collection">Collection Utility documentation</seealso>
public static class CollectionUtility
{
    /// <summary>
    /// Attempts to get the count of items in the collection without enumerating.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="count">When this method returns, contains the count if successful; otherwise, 0.</param>
    /// <returns><see langword="true"/> if the count was determined in O(1); otherwise, <see langword="false"/>.</returns>
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
                // Returns false if count cannot be determined in O(1). Does not enumerate.
                return false;
        }
    }

    /// <summary>
    /// Attempts to retrieve an item at the specified index from the collection.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="index">The zero-based index of the item to retrieve. If negative, returns <see langword="false"/>.</param>
    /// <param name="item">When this method returns, contains the item if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the item was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryGet<T>(IEnumerable<T>? value, int index, out T? item)
    {
        item = default;

        if (value is null || index < 0)
            return false;

        switch (value)
        {
            case IList<T> list when index >= list.Count:
                return false;
            case IList<T> list:
                item = list[index];
                return true;
            case IReadOnlyList<T> roList when index >= roList.Count:
                return false;
            case IReadOnlyList<T> roList:
                item = roList[index];
                return true;
            case ICollection<T> c when index >= c.Count:
            case IReadOnlyCollection<T> rc when index >= rc.Count:
                return false;
        }

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

    /// <summary>
    /// Attempts to find the index of the specified item in the collection.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="index">When this method returns, contains the zero-based index if found; otherwise, -1.</param>
    /// <returns><see langword="true"/> if the item was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetIndex<T>(IEnumerable<T>? value, T item, out int index)
    {
        index = -1;

        switch (value)
        {
            case null:
                return false;
            case IList<T> list:
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (!EqualityComparer<T>.Default.Equals(list[i], item)) continue;
                        index = i;
                        return true;
                    }

                    return false;
                }
            case IReadOnlyList<T> roList:
                {
                    for (var i = 0; i < roList.Count; i++)
                    {
                        if (!EqualityComparer<T>.Default.Equals(roList[i], item)) continue;
                        index = i;
                        return true;
                    }

                    return false;
                }
            case ICollection<T> { Count: 0 }:
            case IReadOnlyCollection<T> { Count: 0 }:
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

    /// <summary>Validates that every element is non-null and creates a replayable, non-null-element snapshot.</summary>
    /// <typeparam name="T">The reference element type.</typeparam>
    /// <param name="value">The sequence to validate, enumerated at most once.</param>
    /// <param name="items">The validated snapshot on success, or null on failure. No null elements are filtered out.</param>
    /// <returns>False for a null sequence or any null element; true otherwise, including an empty sequence.</returns>
    public static bool TryValidateNonNullItems<T>(IEnumerable<T?>? value, [NotNullWhen(true)] out IReadOnlyList<T>? items) where T : class
    {
        items = null;
        if (value is null)
            return false;

        var result = new List<T>();
        foreach (var item in value)
        {
            if (item is null)
                return false;
            result.Add(item);
        }

        items = result;
        return true;
    }

    /// <summary>Validates that every element is non-null and creates a replayable, non-null-element snapshot.</summary>
    /// <typeparam name="T">The value element type.</typeparam>
    /// <param name="value">The sequence to validate, enumerated at most once.</param>
    /// <param name="items">The validated snapshot on success, or null on failure. No null elements are filtered out.</param>
    /// <returns>False for a null sequence or any null element; true otherwise, including an empty sequence.</returns>
    public static bool TryValidateNonNullItems<T>(IEnumerable<T?>? value, [NotNullWhen(true)] out IReadOnlyList<T>? items) where T : struct
    {
        items = null;
        if (value is null)
            return false;

        var result = new List<T>();
        foreach (var item in value)
        {
            if (!item.HasValue)
                return false;
            result.Add(item.GetValueOrDefault());
        }

        items = result;
        return true;
    }
}
