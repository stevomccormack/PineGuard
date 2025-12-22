namespace PineGuard.Rules;

public static class DictionaryRules
{
    public static bool HasEntries<TKey, TValue>(IDictionary<TKey, TValue>? dictionary)
    {
        if (dictionary is null)
        {
            return false;
        }

        return dictionary.Count > 0;
    }

    public static bool HasNotEntries<TKey, TValue>(IDictionary<TKey, TValue>? dictionary) =>
        !HasEntries(dictionary);

    public static bool HasKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key)
    {
        if (dictionary is null)
        {
            return false;
        }

        return dictionary.ContainsKey(key);
    }

    public static bool HasNotKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key) =>
        !HasKey(dictionary, key);

    public static bool HasValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TValue value)
    {
        if (dictionary is null)
        {
            return false;
        }

        foreach (var pair in dictionary)
        {
            if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasNotValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TValue value) =>
        !HasValue(dictionary, value);

    public static bool HasKeyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key, TValue value)
    {
        if (dictionary is null)
        {
            return false;
        }

        return dictionary.TryGetValue(key, out var actual)
               && EqualityComparer<TValue>.Default.Equals(actual, value);
    }

    public static bool HasNotKeyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key, TValue value) =>
        !HasKeyValue(dictionary, key, value);

    public static bool HasKeyMatching<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, bool> predicate)
    {
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (dictionary is null)
        {
            return false;
        }

        foreach (var pair in dictionary)
        {
            if (predicate(pair.Key))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasNotKeyMatching<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, bool> predicate) =>
        !HasKeyMatching(dictionary, predicate);

    public static bool HasValueMatching<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TValue, bool> predicate)
    {
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (dictionary is null)
        {
            return false;
        }

        foreach (var pair in dictionary)
        {
            if (predicate(pair.Value))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasNotValueMatching<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TValue, bool> predicate) =>
        !HasValueMatching(dictionary, predicate);

    public static bool HasEntryMatching<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, TValue, bool> predicate)
    {
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (dictionary is null)
        {
            return false;
        }

        foreach (var pair in dictionary)
        {
            if (predicate(pair.Key, pair.Value))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasNotEntryMatching<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, TValue, bool> predicate) =>
        !HasEntryMatching(dictionary, predicate);
}
