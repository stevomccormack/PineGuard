using PineGuard.Common;

namespace PineGuard.Utils;

/// <summary>
/// Provides read-only dictionary access and query utility methods for <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/readonlydictionary">ReadOnlyDictionary Utility documentation</seealso>
public static class ReadOnlyDictionaryUtility
{
    /// <summary>
    /// Attempts to get the count of entries in the read-only dictionary.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="count">When this method returns, contains the count if successful; otherwise, 0.</param>
    /// <returns><see langword="true"/> if the dictionary is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetCount<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? dictionary, out int count)
    {
        count = 0;

        if (dictionary is null)
            return false;

        count = dictionary.Count;
        return true;
    }

    /// <summary>
    /// Attempts to retrieve a value by key from the read-only dictionary.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="key">The key to look up.</param>
    /// <param name="value">When this method returns, contains the value if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the key was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? dictionary, TKey key, out TValue? value)
    {
        value = default;

        return dictionary is not null && dictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// Attempts to retrieve a key-value pair by key from the read-only dictionary.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="key">The key to look up.</param>
    /// <param name="pair">When this method returns, contains the key-value pair if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the key was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetKeyValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? dictionary, TKey key, out KeyValuePair<TKey, TValue> pair)
    {
        pair = default;

        if (dictionary is null)
            return false;

        if (!dictionary.TryGetValue(key, out var value))
            return false;

        pair = new KeyValuePair<TKey, TValue>(key, value);
        return true;
    }

    /// <summary>
    /// Attempts to find the key associated with the specified value using default equality comparison.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="value">The value to search for.</param>
    /// <param name="key">When this method returns, contains the matching key if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if a matching key was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetKey<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? dictionary, TValue value, out TKey? key)
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

    /// <summary>
    /// Attempts to find the first key matching the specified predicate.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate to match keys against.</param>
    /// <param name="key">When this method returns, contains the matching key if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if a matching key was found; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static bool TryGetAnyKey<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? dictionary, Func<TKey, bool> predicate, out TKey? key)
    {
        ThrowHelper.ThrowIfNull(predicate);

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

    /// <summary>
    /// Attempts to find the first value matching the specified predicate.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate to match values against.</param>
    /// <param name="value">When this method returns, contains the matching value if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if a matching value was found; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static bool TryGetAnyValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? dictionary, Func<TValue, bool> predicate, out TValue? value)
    {
        ThrowHelper.ThrowIfNull(predicate);

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

    /// <summary>
    /// Attempts to find the first key-value pair matching the specified predicate.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate to match key-value pairs against.</param>
    /// <param name="pair">When this method returns, contains the matching pair if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if a matching pair was found; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static bool TryGetAnyItem<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? dictionary, Func<TKey, TValue, bool> predicate, out KeyValuePair<TKey, TValue> pair)
    {
        ThrowHelper.ThrowIfNull(predicate);

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
