using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="IReadOnlyDictionary{TKey,TValue}"/> validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/readonly-dictionary">Fluent Read-Only Dictionary Extensions documentation</seealso>
public static class FluentReadOnlyDictionaryExtensions
{
    /// <summary>
    /// Validates that the property value is an empty read-only dictionary (overload for <see cref="IRuleBuilderInitial{TModel,TProperty}"/>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.Empty"/>, which delegates to
    /// <see cref="PineGuard.Rules.ReadOnlyDictionaryRules.IsEmpty{TKey,TValue}"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).Empty();
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.Empty"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> Empty<TModel, TKey, TValue>(
        this IRuleBuilderInitial<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe<TModel, IReadOnlyDictionary<TKey, TValue>?, IReadOnlyDictionary<TKey, TValue>?>(val => Must.Be.Empty(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is an empty read-only dictionary (overload for <see cref="IRuleBuilderOptions{TModel,TProperty}"/>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.Empty"/>, which delegates to
    /// <see cref="PineGuard.Rules.ReadOnlyDictionaryRules.IsEmpty{TKey,TValue}"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).Empty();
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.Empty"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> Empty<TModel, TKey, TValue>(
        this IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe<TModel, IReadOnlyDictionary<TKey, TValue>?, IReadOnlyDictionary<TKey, TValue>?>(val => Must.Be.Empty(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is an empty read-only dictionary (overload for <see cref="IRuleBuilder{TModel,TProperty}"/>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.Empty"/>, which delegates to
    /// <see cref="PineGuard.Rules.ReadOnlyDictionaryRules.IsEmpty{TKey,TValue}"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).Empty();
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.Empty"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> Empty<TModel, TKey, TValue>(
        this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Empty(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is a non-empty read-only dictionary (overload for <see cref="IRuleBuilderInitial{TModel,TProperty}"/>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotEmpty"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotEmpty"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotEmpty<TModel, TKey, TValue>(
        this IRuleBuilderInitial<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe<TModel, IReadOnlyDictionary<TKey, TValue>?, IReadOnlyDictionary<TKey, TValue>?>(val => Must.Be.NotEmpty(val, paramName: null)!,
            message);

    /// <summary>
    /// Validates that the property value is a non-empty read-only dictionary (overload for <see cref="IRuleBuilderOptions{TModel,TProperty}"/>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotEmpty"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotEmpty"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotEmpty<TModel, TKey, TValue>(
        this IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe<TModel, IReadOnlyDictionary<TKey, TValue>?, IReadOnlyDictionary<TKey, TValue>?>(val => Must.Be.NotEmpty(val, paramName: null)!,
            message);

    /// <summary>
    /// Validates that the property value is a non-empty read-only dictionary (overload for <see cref="IRuleBuilder{TModel,TProperty}"/>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotEmpty"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotEmpty"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotEmpty<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotEmpty(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains the specified key.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="key">The key that must be present in the dictionary.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.HasKey"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).HasKey("Content-Type");
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasKey"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> HasKey<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        TKey key,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasKey(val, key, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain the specified key.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="key">The key that must not be present in the dictionary.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotHasKey"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotHasKey("X-Forbidden");
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasKey"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotHasKey<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        TKey key,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasKey(val, key, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains the specified value.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value that must be present in the dictionary.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.HasValue"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).HasValue(new[] { "application/json" });
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasValue"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> HasValue<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        TValue value,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasValue(val, value, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain the specified value.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="value">The value that must not be present in the dictionary.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotHasValue"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotHasValue(new[] { "text/html" });
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasValue"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotHasValue<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        TValue value,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasValue(val, value, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains the specified key-value pair.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="key">The key of the required entry.</param>
    /// <param name="value">The value of the required entry.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.HasKeyValue"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).HasKeyValue("Content-Type", new[] { "application/json" });
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasKeyValue"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> HasKeyValue<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        TKey key,
        TValue value,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasKeyValue(val, key, value, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain the specified key-value pair.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="key">The key of the excluded entry.</param>
    /// <param name="value">The value of the excluded entry.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotHasKeyValue"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotHasKeyValue("X-Blocked", new[] { "true" });
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasKeyValue"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotHasKeyValue<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        TKey key,
        TValue value,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasKeyValue(val, key, value, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains at least one key matching the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the key match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.HasAnyKey"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).HasAnyKey(k => k.StartsWith("X-"));
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasAnyKey"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> HasAnyKey<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        Func<TKey, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasAnyKey(val, predicate, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains no keys matching the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the key match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotHasAnyKey"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotHasAnyKey(k => k.StartsWith("X-Blocked"));
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasAnyKey"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotHasAnyKey<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        Func<TKey, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasAnyKey(val, predicate, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains at least one value matching the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the value match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.HasAnyValue"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).HasAnyValue(v => v.Contains("application/json"));
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasAnyValue"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> HasAnyValue<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        Func<TValue, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasAnyValue(val, predicate, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains no values matching the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the value match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotHasAnyValue"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).NotHasAnyValue(v => v.Contains("text/html"));
    /// </code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasAnyValue"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotHasAnyValue<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        Func<TValue, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasAnyValue(val, predicate, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains at least one item matching the specified key-value predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that receives the key and value and defines the match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.HasAnyItem"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.ResponseHeaders).HasAnyItem((k, v) => k == "Accept" && v.Contains("json"));
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.HasAnyItem"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> HasAnyItem<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        Func<TKey, TValue, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasAnyItem(val, predicate, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value contains no items matching the specified key-value predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that receives the key and value and defines the match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDictionaryClauses.NotHasAnyItem"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.ResponseHeaders).NotHasAnyItem((k, v) => k == "X-Blocked" && v.Contains("true"));
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustDictionaryClauses.NotHasAnyItem"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<TKey, TValue>?> NotHasAnyItem<TModel, TKey, TValue>(this IRuleBuilder<TModel, IReadOnlyDictionary<TKey, TValue>?> ruleBuilder,
        Func<TKey, TValue, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasAnyItem(val, predicate, paramName: null),
            message);
}
