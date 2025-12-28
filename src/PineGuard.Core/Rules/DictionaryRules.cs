using PineGuard.Utils;

namespace PineGuard.Rules;

public static class DictionaryRules
{
    public static bool IsEmpty<TKey, TValue>(IDictionary<TKey, TValue>? dictionary) =>
        !DictionaryUtility.TryGetCount(dictionary, out var count) || count == 0;

    public static bool HasItems<TKey, TValue>(IDictionary<TKey, TValue>? dictionary) =>
        DictionaryUtility.TryGetCount(dictionary, out var count) && count != 0;

    public static bool HasKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key) =>
        dictionary is not null && dictionary.ContainsKey(key);

    public static bool HasValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TValue value) =>
        DictionaryUtility.TryGetKeyForValue(dictionary, value, out _);

    public static bool HasKeyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, TKey key, TValue value) => 
        DictionaryUtility.TryGetValue(dictionary, key, out var actual) && 
        EqualityComparer<TValue>.Default.Equals(actual, value);

    public static bool HasAnyKey<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, bool> predicate) =>
        DictionaryUtility.TryGetAnyKey(dictionary, predicate, out _);

    public static bool HasAnyValue<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TValue, bool> predicate) =>
        DictionaryUtility.TryGetAnyValue(dictionary, predicate, out _);

    public static bool HasAnyItem<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, Func<TKey, TValue, bool> predicate) =>
        DictionaryUtility.TryGetAnyItem(dictionary, predicate, out _);
}
