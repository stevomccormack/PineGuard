using FluentValidation;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for collection validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/collection">Fluent Collection Extensions documentation</seealso>
public static class FluentCollectionExtensions
{
    /// <summary>
    /// Validates that the property value is an empty collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.Empty"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Tags).Empty();
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.Empty"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> Empty<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Empty(val, paramName: null), message);

    /// <summary>
    /// Validates that the property value is a non-empty collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotEmpty"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).NotEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotEmpty"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotEmpty<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotEmpty(val, paramName: null), message);

    /// <summary>
    /// Validates that the property value has exactly the specified number of elements.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="count">The exact number of elements required.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasExactCount"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Coordinates).HasExactCount(2);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasExactCount"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasExactCount<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int count,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasExactCount(val, count, paramName: null), message);

    /// <summary>
    /// Validates that the property value does not have exactly the specified number of elements.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="count">The number of elements the collection must not have.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotHasExactCount"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).NotHasExactCount(0);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasExactCount"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotHasExactCount<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int count,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasExactCount(val, count, paramName: null), message);

    /// <summary>
    /// Validates that the property value has at least the specified minimum number of elements.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum required number of elements.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasMinCount"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Tags).HasMinCount(1);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasMinCount"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasMinCount<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int min,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasMinCount(val, min, paramName: null), message);

    /// <summary>
    /// Validates that the property value does not have at least the specified minimum number of elements.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum element count the collection must not reach.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotHasMinCount"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Tags).NotHasMinCount(10);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasMinCount"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotHasMinCount<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int min,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasMinCount(val, min, paramName: null), message);

    /// <summary>
    /// Validates that the property value has at most the specified maximum number of elements.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="max">The maximum allowed number of elements.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasMaxCount"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Tags).HasMaxCount(10);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasMaxCount"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasMaxCount<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int max,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasMaxCount(val, max, paramName: null), message);

    /// <summary>
    /// Validates that the property value exceeds the specified maximum number of elements.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="max">The maximum element count the collection must exceed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotHasMaxCount"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).NotHasMaxCount(0);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasMaxCount"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotHasMaxCount<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int max,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasMaxCount(val, max, paramName: null), message);

    /// <summary>
    /// Validates that the property value has an element count within the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum allowed element count.</param>
    /// <param name="max">The maximum allowed element count.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasCountBetween"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Tags).HasCountBetween(1, 5);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasCountBetween"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasCountBetween<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasCountBetween(val, min, max, inclusion, paramName: null), message);

    /// <summary>
    /// Validates that the property value has an element count outside the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded count range.</param>
    /// <param name="max">The upper bound of the excluded count range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotHasCountBetween"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Tags).NotHasCountBetween(0, 0);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasCountBetween"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotHasCountBetween<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasCountBetween(val, min, max, inclusion, paramName: null), message);

    /// <summary>
    /// Validates that the property value contains at least one element matching the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasAny"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).HasAny(item => item.IsActive);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasAny"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasAny<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        Func<T, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasAny(val, predicate, paramName: null), message);

    /// <summary>
    /// Validates that the property value contains no elements matching the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the match condition.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotHasAny"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).NotHasAny(item => item.IsDeleted);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasAny"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotHasAny<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        Func<T, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasAny(val, predicate, paramName: null), message);

    /// <summary>
    /// Validates that all elements in the property value match the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the condition all elements must satisfy.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasAll"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).HasAll(item => item.IsValid);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasAll"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasAll<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        Func<T, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasAll(val, predicate, paramName: null), message);

    /// <summary>
    /// Validates that not all elements in the property value match the specified predicate.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="predicate">A function that defines the condition that must not be satisfied by all elements.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotHasAll"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).NotHasAll(item => item.IsDeleted);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasAll"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotHasAll<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        Func<T, bool> predicate,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasAll(val, predicate, paramName: null), message);

    /// <summary>
    /// Validates that the property value contains only distinct elements (no duplicates).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="comparer">An optional equality comparer to use for duplicate detection.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasDistinctItems"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Tags).HasDistinctItems();
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasDistinctItems"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasDistinctItems<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        IEqualityComparer<T>? comparer = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasDistinctItems(val, comparer, paramName: null), message);

    /// <summary>
    /// Validates that the property value contains at least one duplicate element.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="comparer">An optional equality comparer to use for duplicate detection.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasDuplicateItems"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DuplicatedEntries).HasDuplicateItems();
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasDuplicateItems"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasDuplicateItems<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        IEqualityComparer<T>? comparer = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasDuplicateItems(val, comparer, paramName: null), message);

    /// <summary>
    /// Validates that the property value does not contain any <see langword="null"/> elements.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The reference element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotContainsNullItems"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Names).NotContainsNullItems();
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotContainsNullItems"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T?>?> NotContainsNullItems<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T?>?> ruleBuilder,
        string? message = null)
        where T : class =>
        ruleBuilder.MustBe(val => Must.Be.NotContainsNullItems(val, paramName: null), message);

    /// <summary>
    /// Validates that the property value contains the specified item.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="item">The item that must be present in the collection.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.Contains"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Roles).Contains("admin");
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.Contains"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> Contains<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        T item,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Contains(val, item, paramName: null), message);

    /// <summary>
    /// Validates that the property value does not contain the specified item.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="item">The item that must not be present in the collection.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotContains"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Roles).NotContains("superadmin");
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotContains"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotContains<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        T item,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotContains(val, item, paramName: null), message);

    /// <summary>
    /// Validates that the property value is a subset of the specified collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The superset collection to validate against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.SubsetOf"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SelectedRoles).SubsetOf(allowedRoles);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.SubsetOf"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> SubsetOf<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        IEnumerable<T>? other,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.SubsetOf(val, other, paramName: null), message);

    /// <summary>
    /// Validates that the property value is not a subset of the specified collection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The collection to compare against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotSubsetOf"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Roles).NotSubsetOf(restrictedRoles);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotSubsetOf"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotSubsetOf<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        IEnumerable<T>? other,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotSubsetOf(val, other, paramName: null), message);

    /// <summary>
    /// Validates that the property value has an element at the specified index.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="index">The zero-based index that must exist within the collection.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.HasIndex"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).HasIndex(0);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasIndex"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> HasIndex<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int index,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasIndex(val, index, paramName: null), message);

    /// <summary>
    /// Validates that the property value does not have an element at the specified index.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="index">The zero-based index that must not exist within the collection.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCollectionClauses.NotHasIndex"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Items).NotHasIndex(5);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasIndex"/>
    public static IRuleBuilderOptions<TModel, IEnumerable<T>?> NotHasIndex<TModel, T>(
        this IRuleBuilder<TModel, IEnumerable<T>?> ruleBuilder,
        int index,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasIndex(val, index, paramName: null), message);
}
