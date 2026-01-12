namespace PineGuard.Utils;

public static class DictionaryUtility
{
    public static bool TryGetCount<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, out int count)
    {
        count = 0;

        if (dictionary is null)
            return false;

        count = dictionary.Count;
        return true;
    }

    public static bool TryGetValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key, out TValue? value)
    {
        value = default;

        if (dictionary is null)
            return false;

        return dictionary.TryGetValue(key, out value);
    }

    public static bool TryGetKeyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key, out KeyValuePair<TKey, TValue> pair)
    {
        pair = default;

        if (dictionary is null)
            return false;

        if (!dictionary.TryGetValue(key, out var value))
            return false;

        pair = new KeyValuePair<TKey, TValue>(key, value);
        return true;
    }

    public static bool TryGetKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TValue value, out TKey? key)
    {
        key = default;

        if (dictionary is null)
            return false;

        foreach (var pair in dictionary)
        {
            if (!EqualityComparer<TValue>.Default.Equals(pair.Value, value)) continue;

            key = pair.Key;
            return true;
        }

        return false;
    }

    public static bool TryGetAnyKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, bool> predicate, out TKey? key)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        key = default;

        if (dictionary is null)
            return false;

        foreach (var pair in dictionary)
        {
            if (!predicate(pair.Key)) continue;

            key = pair.Key;
            return true;
        }

        return false;
    }

    public static bool TryGetAnyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TValue, bool> predicate, out TValue? value)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        value = default;

        if (dictionary is null)
            return false;

        foreach (var pair in dictionary)
        {
            if (!predicate(pair.Value)) continue;

            value = pair.Value;
            return true;
        }

        return false;
    }

    public static bool TryGetAnyItem<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, TValue, bool> predicate, out KeyValuePair<TKey, TValue> pair)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        pair = default;

        if (dictionary is null)
            return false;

        foreach (var item in dictionary)
        {
            if (!predicate(item.Key, item.Value)) continue;

            pair = item;
            return true;
        }

        return false;
    }
}
