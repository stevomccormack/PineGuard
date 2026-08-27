using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate read-only dictionaries,
/// delegating to <see cref="ReadOnlyDictionaryRules"/> for core validation logic.
/// </summary>
/// <seealso cref="ReadOnlyDictionaryRules"/>
/// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
public static class MustReadOnlyDictionaryClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified value must be empty.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be empty."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>?> Empty<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be empty.";

        var ok = ReadOnlyDictionaryRules.IsEmpty(dictionary);
        return MustResult<IReadOnlyDictionary<TKey, TValue>?>.FromBool(ok, MustCodes.Dictionary.Items.NotEmpty, messageTemplate, paramName, dictionary, dictionary);
    }

    /// <summary>
    /// Validates that the specified value must not be empty and have items.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be empty and have items."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> NotEmpty<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be empty and have items.";

        var ok = ReadOnlyDictionaryRules.IsNotEmpty(dictionary);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Items.Empty, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified key.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain the specified key."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> HasKey<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        TKey key,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified key.";

        var ok = ReadOnlyDictionaryRules.HasKey(dictionary, key);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Keys.Missing, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified key.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified key."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> NotHasKey<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        TKey key,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified key.";

        var ok = !ReadOnlyDictionaryRules.HasKey(dictionary, key);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Keys.Present, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain the specified value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> HasValue<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        TValue value,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified value.";

        var ok = ReadOnlyDictionaryRules.HasValue(dictionary, value);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Values.Missing, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> NotHasValue<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        TValue value,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified value.";

        var ok = !ReadOnlyDictionaryRules.HasValue(dictionary, value);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Values.Present, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified key/value pair.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain the specified key/value pair."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> HasKeyValue<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        TKey key,
        TValue value,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified key/value pair.";

        var ok = ReadOnlyDictionaryRules.HasKeyValue(dictionary, key, value);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Items.Missing, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified key/value pair.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified key/value pair."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> NotHasKeyValue<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        TKey key,
        TValue value,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified key/value pair.";

        var ok = !ReadOnlyDictionaryRules.HasKeyValue(dictionary, key, value);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Items.Present, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must contain a key that matches the predicate.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a key that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> HasAnyKey<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        Func<TKey, bool> predicate,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<IReadOnlyDictionary<TKey, TValue>>.Fail(MustCodes.Dictionary.Keys.NoMatch, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must contain a key that matches the predicate.";

        var ok = ReadOnlyDictionaryRules.HasAnyKey(dictionary, predicate);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Keys.NoMatch, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must not contain a key that matches the predicate.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a key that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> NotHasAnyKey<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        Func<TKey, bool> predicate,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<IReadOnlyDictionary<TKey, TValue>>.Fail(MustCodes.Dictionary.Keys.Match, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must not contain a key that matches the predicate.";

        var ok = !ReadOnlyDictionaryRules.HasAnyKey(dictionary, predicate);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Keys.Match, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must contain a value that matches the predicate.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a value that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> HasAnyValue<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        Func<TValue, bool> predicate,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<IReadOnlyDictionary<TKey, TValue>>.Fail(MustCodes.Dictionary.Values.NoMatch, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must contain a value that matches the predicate.";

        var ok = ReadOnlyDictionaryRules.HasAnyValue(dictionary, predicate);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Values.NoMatch, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must not contain a value that matches the predicate.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a value that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> NotHasAnyValue<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        Func<TValue, bool> predicate,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<IReadOnlyDictionary<TKey, TValue>>.Fail(MustCodes.Dictionary.Values.Match, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must not contain a value that matches the predicate.";

        var ok = !ReadOnlyDictionaryRules.HasAnyValue(dictionary, predicate);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Values.Match, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must contain an item that matches the predicate.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an item that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> HasAnyItem<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        Func<TKey, TValue, bool> predicate,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<IReadOnlyDictionary<TKey, TValue>>.Fail(MustCodes.Dictionary.Items.NoMatch, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must contain an item that matches the predicate.";

        var ok = ReadOnlyDictionaryRules.HasAnyItem(dictionary, predicate);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Items.NoMatch, messageTemplate, paramName, dictionary, dictionary!);
    }

    /// <summary>
    /// Validates that the specified value must not contain an item that matches the predicate.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain an item that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/read-only-dictionary">Read Only Dictionary Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<TKey, TValue>> NotHasAnyItem<TKey, TValue>(this IMustClause _,
        IReadOnlyDictionary<TKey, TValue>? dictionary,
        Func<TKey, TValue, bool> predicate,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
    {
        if (predicate is null)
            return MustResult<IReadOnlyDictionary<TKey, TValue>>.Fail(MustCodes.Dictionary.Items.Match, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must not contain an item that matches the predicate.";

        var ok = !ReadOnlyDictionaryRules.HasAnyItem(dictionary, predicate);
        return MustResult<IReadOnlyDictionary<TKey, TValue>>.FromBool(ok, MustCodes.Dictionary.Items.Match, messageTemplate, paramName, dictionary, dictionary!);
    }
}
