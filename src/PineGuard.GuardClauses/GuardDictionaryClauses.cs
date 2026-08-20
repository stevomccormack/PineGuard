using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="IDictionary{TKey,TValue}"/> values.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/dictionary">Guard Dictionary Clauses documentation</seealso>
public static class GuardDictionaryClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is <see langword="null"/> or contains entries (is not empty).
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.Empty{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains entries and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.Empty{TKey,TValue}"/>:
    /// <c>Guard.Against.NotEmpty</c> passes when the dictionary is empty.
    /// Note: a <see langword="null"/> dictionary always throws <see cref="ArgumentNullException"/> regardless of <paramref name="exceptionCreator"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotEmpty(headers);
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.Empty{TKey,TValue}"/>
    public static IDictionary<TKey, TValue>? NotEmpty<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            GuardFailure.Throw(message ?? Must.Be.NotNull(value, paramName).Message, paramName, value, exceptionCreator);

        var result = Must.Be.Empty(value, paramName); // Guard.Against.NotEmpty => Must.Be.Empty (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is empty (contains no entries).
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.NotEmpty{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is empty and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.NotEmpty{TKey,TValue}"/>:
    /// <c>Guard.Against.Empty</c> passes when the dictionary has entries.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Empty(settings);
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotEmpty{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> Empty<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotEmpty(value, paramName); // Guard.Against.Empty => Must.Be.NotEmpty (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not contain <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="key">The key that must be present.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.HasKey{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the key is missing and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.HasKey{TKey,TValue}"/>:
    /// <c>Guard.Against.NotHasKey</c> passes when the key exists.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasKey(config, "ApiKey");
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasKey{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> NotHasKey<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        TKey key,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasKey(value, key, paramName); // Guard.Against.NotHasKey => Must.Be.HasKey (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains <paramref name="key"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="key">The key that must not be present.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.NotHasKey{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the key is present and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.NotHasKey{TKey,TValue}"/>:
    /// <c>Guard.Against.HasKey</c> passes when the key is absent.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasKey(cache, "deprecated-key");
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasKey{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> HasKey<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        TKey key,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasKey(value, key, paramName); // Guard.Against.HasKey => Must.Be.NotHasKey (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not contain <paramref name="dictionaryValue"/> among its values.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="dictionaryValue">The value that must be present.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.HasValue{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the value is absent and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.HasValue{TKey,TValue}"/>:
    /// <c>Guard.Against.NotHasValue</c> passes when the value exists in the dictionary.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasValue(statusMap, HttpStatusCode.OK);
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasValue{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> NotHasValue<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        TValue dictionaryValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasValue(value, dictionaryValue, paramName); // Guard.Against.NotHasValue => Must.Be.HasValue (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains <paramref name="dictionaryValue"/> among its values.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="dictionaryValue">The value that must not be present.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.NotHasValue{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the value is present and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.NotHasValue{TKey,TValue}"/>:
    /// <c>Guard.Against.HasValue</c> passes when the value is absent from the dictionary.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasValue(statusMap, HttpStatusCode.InternalServerError);
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasValue{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> HasValue<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        TValue dictionaryValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasValue(value, dictionaryValue, paramName); // Guard.Against.HasValue => Must.Be.NotHasValue (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not contain the entry with <paramref name="key"/> mapped to <paramref name="dictionaryValue"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="key">The key to look up.</param>
    /// <param name="dictionaryValue">The value expected at <paramref name="key"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.HasKeyValue{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the key-value pair is absent and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.HasKeyValue{TKey,TValue}"/>:
    /// <c>Guard.Against.NotHasKeyValue</c> passes when the specific key-value pair is present.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasKeyValue(config, "Env", "Production");
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasKeyValue{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> NotHasKeyValue<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        TKey key,
        TValue dictionaryValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasKeyValue(value, key, dictionaryValue, paramName); // Guard.Against.NotHasKeyValue => Must.Be.HasKeyValue (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains the entry with <paramref name="key"/> mapped to <paramref name="dictionaryValue"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="key">The key to look up.</param>
    /// <param name="dictionaryValue">The value that must not be at <paramref name="key"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.NotHasKeyValue{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the key-value pair is present and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.NotHasKeyValue{TKey,TValue}"/>:
    /// <c>Guard.Against.HasKeyValue</c> passes when that key-value pair is absent.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasKeyValue(config, "Mode", "Debug");
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasKeyValue{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> HasKeyValue<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        TKey key,
        TValue dictionaryValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasKeyValue(value, key, dictionaryValue, paramName); // Guard.Against.HasKeyValue => Must.Be.NotHasKeyValue (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if no key in <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="predicate">The predicate that at least one key must satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.HasAnyKey{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when no key satisfies the predicate and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.HasAnyKey{TKey,TValue}"/>:
    /// <c>Guard.Against.NotHasAnyKey</c> passes when at least one key matches.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasAnyKey(config, k => k.StartsWith("Api"));
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasAnyKey{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> NotHasAnyKey<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        Func<TKey, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasAnyKey(value, predicate, paramName); // Guard.Against.NotHasAnyKey => Must.Be.HasAnyKey (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if any key in <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="predicate">The predicate that no key may satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.NotHasAnyKey{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when any key satisfies the predicate and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.NotHasAnyKey{TKey,TValue}"/>:
    /// <c>Guard.Against.HasAnyKey</c> passes when no key matches.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasAnyKey(config, k => k.StartsWith("Deprecated"));
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasAnyKey{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> HasAnyKey<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        Func<TKey, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasAnyKey(value, predicate, paramName); // Guard.Against.HasAnyKey => Must.Be.NotHasAnyKey (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if no value in <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="predicate">The predicate that at least one value must satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.HasAnyValue{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when no value satisfies the predicate and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.HasAnyValue{TKey,TValue}"/>:
    /// <c>Guard.Against.NotHasAnyValue</c> passes when at least one value matches.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasAnyValue(settings, v => v.IsEnabled);
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasAnyValue{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> NotHasAnyValue<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        Func<TValue, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasAnyValue(value, predicate, paramName); // Guard.Against.NotHasAnyValue => Must.Be.HasAnyValue (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if any value in <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="predicate">The predicate that no value may satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.NotHasAnyValue{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when any value satisfies the predicate and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.NotHasAnyValue{TKey,TValue}"/>:
    /// <c>Guard.Against.HasAnyValue</c> passes when no value matches.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasAnyValue(settings, v => v.IsDeprecated);
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasAnyValue{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> HasAnyValue<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        Func<TValue, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasAnyValue(value, predicate, paramName); // Guard.Against.HasAnyValue => Must.Be.NotHasAnyValue (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if no key-value pair in <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="predicate">The predicate receiving both key and value; at least one pair must satisfy it.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.HasAnyItem{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when no pair satisfies the predicate and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.HasAnyItem{TKey,TValue}"/>:
    /// <c>Guard.Against.NotHasAnyItem</c> passes when at least one pair matches.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.NotHasAnyItem(config, (k, v) => k == "Timeout" && v > 0);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasAnyItem{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> NotHasAnyItem<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        Func<TKey, TValue, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasAnyItem(value, predicate, paramName); // Guard.Against.NotHasAnyItem => Must.Be.HasAnyItem (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if any key-value pair in <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The dictionary to guard.</param>
    /// <param name="predicate">The predicate receiving both key and value; no pair may satisfy it.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDictionaryClauses.NotHasAnyItem{TKey,TValue}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when any pair satisfies the predicate and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDictionaryClauses.NotHasAnyItem{TKey,TValue}"/>:
    /// <c>Guard.Against.HasAnyItem</c> passes when no pair matches.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasAnyItem(config, (k, v) => k.StartsWith("Deprecated"));
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasAnyItem{TKey,TValue}"/>
    public static IDictionary<TKey, TValue> HasAnyItem<TKey, TValue>(this IGuardClause _,
        IDictionary<TKey, TValue>? value,
        Func<TKey, TValue, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasAnyItem(value, predicate, paramName); // Guard.Against.HasAnyItem => Must.Be.NotHasAnyItem (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }
}
