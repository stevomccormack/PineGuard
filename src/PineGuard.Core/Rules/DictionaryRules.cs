namespace PineGuard.Rules;

public static class DictionaryRules
{
    public static bool IsEmpty<TKey, TValue>(IDictionary<TKey, TValue>? dictionary) =>
        dictionary is null || dictionary.Count == 0;

    public static bool HasKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key)
    {
        if (dictionary is null)
            return false;

        return dictionary.ContainsKey(key);
    }

    public static bool HasItems<TKey, TValue>(IDictionary<TKey, TValue>? dictionary)
    {
        if (dictionary is null)
            return false;

        return dictionary.Count != 0;
    }

    public static bool HasValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TValue value)
    {
        if (dictionary is null)
            return false;

        foreach (var pair in dictionary)
        {
            if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                return true;
        }

        return false;
    }

    public static bool HasKeyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key, TValue value)
    {
        if (dictionary is null)
            return false;

        return dictionary.TryGetValue(key, out var actual)
            && EqualityComparer<TValue>.Default.Equals(actual, value);
    }

    public static bool HasAnyKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (dictionary is null)
            return false;

        foreach (var pair in dictionary)
        {
            if (predicate(pair.Key))
                return true;
        }

        return false;
    }

    public static bool HasAnyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TValue, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (dictionary is null)
            return false;

        foreach (var pair in dictionary)
        {
            if (predicate(pair.Value))
                return true;
        }

        return false;
    }

    public static bool HasAnyItem<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, TValue, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (dictionary is null)
            return false;

        foreach (var pair in dictionary)
        {
            if (predicate(pair.Key, pair.Value))
                return true;
        }

        return false;
    }
}
