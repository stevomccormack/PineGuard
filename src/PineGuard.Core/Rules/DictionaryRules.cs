using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure validation predicates for <see cref="IDictionary{TKey,TValue}"/> collections.
/// </summary>
/// <remarks>
/// All methods return <see langword="false"/> when the dictionary is <see langword="null"/>.
/// The validated dictionary parameter is named <c>value</c>, except in <see cref="HasValue{TKey,TValue}"/> and
/// <see cref="HasKeyValue{TKey,TValue}"/>, where it remains <c>dictionary</c> because <c>value</c> already
/// names the dictionary value being searched for.
/// For read-only dictionary validation, see <see cref="ReadOnlyDictionaryRules"/>.
/// </remarks>
/// <seealso cref="ReadOnlyDictionaryRules"/>
/// <seealso href="https://pineguard.ai/docs/rules/dictionary">Dictionary Rules documentation</seealso>
public static class DictionaryRules
{
    /// <summary>
    /// Determines whether the specified dictionary is empty (contains no entries).
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the dictionary is non-null and has zero entries; otherwise, <see langword="false"/>.</returns>
    public static bool IsEmpty<TKey, TValue>(IDictionary<TKey, TValue>? value) =>
        DictionaryUtility.TryGetCount(value, out var count) && count == 0;

    /// <summary>
    /// Determines whether the specified dictionary is not empty (contains at least one entry).
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the dictionary is non-null and has at least one entry; otherwise, <see langword="false"/>.</returns>
    public static bool IsNotEmpty<TKey, TValue>(IDictionary<TKey, TValue>? value) =>
        DictionaryUtility.TryGetCount(value, out var count) && count != 0;

    /// <summary>
    /// Determines whether the specified dictionary contains the given key.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="key">The key to look up. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> contains <paramref name="key"/>; otherwise, <see langword="false"/>.</returns>
    public static bool HasKey<TKey, TValue>(IDictionary<TKey, TValue>? value, TKey key) =>
        value is not null && key is not null && value.ContainsKey(key);

    /// <summary>
    /// Determines whether the specified dictionary contains the given value.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="value">The value to search for.</param>
    /// <returns>
    /// <see langword="true"/> if any entry in <paramref name="dictionary"/> has a value equal to <paramref name="value"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TValue value) =>
        DictionaryUtility.TryGetKey(dictionary, value, out _);

    /// <summary>
    /// Determines whether the specified dictionary contains an entry with the given key and value.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="dictionary">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="key">The key to look up. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="value">The expected value for the given <paramref name="key"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="dictionary"/> contains <paramref name="key"/> and its
    /// associated value equals <paramref name="value"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasKeyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key, TValue value) =>
        key is not null &&
        DictionaryUtility.TryGetValue(dictionary, key, out var actual) &&
        EqualityComparer<TValue>.Default.Equals(actual!, value!);

    /// <summary>
    /// Determines whether the specified dictionary contains any key satisfying the given predicate.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate applied to each key.</param>
    /// <returns>
    /// <see langword="true"/> if at least one key satisfies <paramref name="predicate"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasAnyKey<TKey, TValue>(IDictionary<TKey, TValue>? value, Func<TKey, bool> predicate) =>
        DictionaryUtility.TryGetAnyKey(value, predicate, out _);

    /// <summary>
    /// Determines whether the specified dictionary contains any value satisfying the given predicate.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate applied to each value.</param>
    /// <returns>
    /// <see langword="true"/> if at least one value satisfies <paramref name="predicate"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasAnyValue<TKey, TValue>(IDictionary<TKey, TValue>? value, Func<TValue, bool> predicate) =>
        DictionaryUtility.TryGetAnyValue(value, predicate, out _);

    /// <summary>
    /// Determines whether the specified dictionary contains any key-value pair satisfying the given predicate.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="value">The dictionary to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate applied to each key-value pair.</param>
    /// <returns>
    /// <see langword="true"/> if at least one pair satisfies <paramref name="predicate"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasAnyItem<TKey, TValue>(IDictionary<TKey, TValue>? value, Func<TKey, TValue, bool> predicate) =>
        DictionaryUtility.TryGetAnyItem(value, predicate, out _);
}
