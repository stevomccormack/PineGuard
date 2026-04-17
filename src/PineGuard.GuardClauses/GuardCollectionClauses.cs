using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="IEnumerable{T}"/> collections.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/collection">Guard Collection Clauses documentation</seealso>
public static class GuardCollectionClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not empty (i.e., contains any elements).
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotEmpty{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not empty and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotEmpty{T}"/>:
    /// <c>Guard.Against.Empty</c> passes when the collection is empty.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Empty(items);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotEmpty{T}"/>
    public static IEnumerable<T> Empty<T>(this IGuardClause _,
        IEnumerable<T>? value,
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
    /// Throws if <paramref name="value"/> is empty (contains no elements).
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.Empty{T}"/>.
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
    /// This guard is the complement of <see cref="MustCollectionClauses.Empty{T}"/>:
    /// <c>Guard.Against.NotEmpty</c> passes when the collection contains elements.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotEmpty(items);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.Empty{T}"/>
    public static IEnumerable<T> NotEmpty<T>(this IGuardClause _,
        IEnumerable<T>? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Empty(value, paramName); // Guard.Against.NotEmpty => Must.Be.Empty (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have exactly <paramref name="count"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="count">The exact number of elements required.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasExactCount{T}"/>.
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
    /// Thrown when <paramref name="value"/> does not have exactly <paramref name="count"/> elements and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasExactCount{T}"/>:
    /// <c>Guard.Against.NotHasExactCount</c> passes when the count matches exactly.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasExactCount(coordinates, count: 2);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasExactCount{T}"/>
    public static IEnumerable<T> NotHasExactCount<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int count,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasExactCount(value, count, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has fewer than <paramref name="min"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="min">The minimum number of elements required.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasMinCount{T}"/>.
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
    /// Thrown when <paramref name="value"/> has fewer than <paramref name="min"/> elements and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasMinCount{T}"/>:
    /// <c>Guard.Against.NotHasMinCount</c> passes when the collection meets the minimum count.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasMinCount(items, min: 1);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasMinCount{T}"/>
    public static IEnumerable<T> NotHasMinCount<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int min,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasMinCount(value, min, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has more than <paramref name="max"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="max">The maximum number of elements allowed.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasMaxCount{T}"/>.
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
    /// Thrown when <paramref name="value"/> has more than <paramref name="max"/> elements and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasMaxCount{T}"/>:
    /// <c>Guard.Against.NotHasMaxCount</c> passes when the collection does not exceed the maximum.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasMaxCount(tags, max: 10);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasMaxCount{T}"/>
    public static IEnumerable<T> NotHasMaxCount<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int max,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasMaxCount(value, max, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if the element count of <paramref name="value"/> is not between <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="min">The minimum count (inclusive by default).</param>
    /// <param name="max">The maximum count (inclusive by default).</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasCountBetween{T}"/>.
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
    /// Thrown when the count is outside the range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasCountBetween{T}"/>:
    /// <c>Guard.Against.NotHasCountBetween</c> passes when the count is within the range.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasCountBetween(items, min: 1, max: 5);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasCountBetween{T}"/>
    public static IEnumerable<T> NotHasCountBetween<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasCountBetween(value, min, max, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if any element of <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="predicate">The predicate that must not match any element.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasAny{T}"/>.
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
    /// Thrown when any element satisfies the predicate and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasAny{T}"/>:
    /// <c>Guard.Against.NotHasAny</c> passes when at least one element matches the predicate.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasAny(items, x => x.IsActive);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasAny{T}"/>
    public static IEnumerable<T> NotHasAny<T>(this IGuardClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasAny(value, predicate, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if no element of <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="predicate">The predicate that at least one element must not satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotHasAny{T}"/>.
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
    /// Thrown when any element satisfies the predicate (i.e., any match exists) and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotHasAny{T}"/>:
    /// <c>Guard.Against.HasAny</c> passes when no element matches the predicate.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasAny(items, x => x.IsDeleted);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasAny{T}"/>
    public static IEnumerable<T> HasAny<T>(this IGuardClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasAny(value, predicate, paramName); // Guard.Against.HasAny => Must.Be.NotHasAny (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if not all elements of <paramref name="value"/> satisfy <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="predicate">The predicate that all elements must satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasAll{T}"/>.
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
    /// Thrown when not all elements satisfy the predicate and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasAll{T}"/>:
    /// <c>Guard.Against.NotHasAll</c> passes when all elements match the predicate.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasAll(items, x => x.IsValid);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasAll{T}"/>
    public static IEnumerable<T> NotHasAll<T>(this IGuardClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasAll(value, predicate, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if all elements of <paramref name="value"/> satisfy <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="predicate">The predicate that not all elements may satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotHasAll{T}"/>.
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
    /// Thrown when all elements satisfy the predicate and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotHasAll{T}"/>:
    /// <c>Guard.Against.HasAll</c> passes when at least one element does not match the predicate.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasAll(items, x => x.IsDeleted);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasAll{T}"/>
    public static IEnumerable<T> HasAll<T>(this IGuardClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasAll(value, predicate, paramName); // Guard.Against.HasAll => Must.Be.NotHasAll (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains any duplicate elements.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="comparer">An optional equality comparer for element comparison.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasDistinctItems{T}"/>.
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
    /// Thrown when <paramref name="value"/> contains duplicates and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasDistinctItems{T}"/>:
    /// <c>Guard.Against.DuplicateItems</c> passes when all elements are distinct.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.DuplicateItems(ids);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasDistinctItems{T}"/>
    public static IEnumerable<T> DuplicateItems<T>(this IGuardClause _,
        IEnumerable<T>? value,
        IEqualityComparer<T>? comparer = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasDistinctItems(value, comparer, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains all distinct elements (no duplicates).
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="comparer">An optional equality comparer for element comparison.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasDuplicateItems{T}"/>.
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
    /// Thrown when <paramref name="value"/> has no duplicates and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasDuplicateItems{T}"/>:
    /// <c>Guard.Against.DistinctItems</c> passes when at least one duplicate exists.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.DistinctItems(collection);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasDuplicateItems{T}"/>
    public static IEnumerable<T> DistinctItems<T>(this IGuardClause _,
        IEnumerable<T>? value,
        IEqualityComparer<T>? comparer = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasDuplicateItems(value, comparer, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains any <see langword="null"/> elements.
    /// </summary>
    /// <typeparam name="T">The reference element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotContainsNullItems{T}"/>.
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
    /// Thrown when <paramref name="value"/> has <see langword="null"/> elements and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotContainsNullItems{T}"/>:
    /// <c>Guard.Against.ContainsNullItems</c> passes when no element is <see langword="null"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.ContainsNullItems(names);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotContainsNullItems{T}"/>
    public static IEnumerable<T> ContainsNullItems<T>(this IGuardClause _,
        IEnumerable<T?>? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : class
    {
        var result = Must.Be.NotContainsNullItems(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not contain <paramref name="item"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="item">The item that must be present in the collection.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.Contains{T}"/>.
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
    /// Thrown when <paramref name="value"/> does not contain <paramref name="item"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.Contains{T}"/>:
    /// <c>Guard.Against.NotContains</c> passes when the item is present.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotContains(roles, "Admin");
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.Contains{T}"/>
    public static IEnumerable<T> NotContains<T>(this IGuardClause _,
        IEnumerable<T>? value,
        T item,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Contains(value, item, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains <paramref name="item"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="item">The item that must not be present in the collection.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotContains{T}"/>.
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
    /// Thrown when <paramref name="value"/> contains <paramref name="item"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotContains{T}"/>:
    /// <c>Guard.Against.Contains</c> passes when the item is absent.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Contains(blockedIds, userId);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotContains{T}"/>
    public static IEnumerable<T> Contains<T>(this IGuardClause _,
        IEnumerable<T>? value,
        T item,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotContains(value, item, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a subset of <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="other">The superset collection.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.SubsetOf{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not a subset of <paramref name="other"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.SubsetOf{T}"/>:
    /// <c>Guard.Against.NotSubsetOf</c> passes when all elements of <paramref name="value"/> exist in <paramref name="other"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotSubsetOf(selectedRoles, availableRoles);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.SubsetOf{T}"/>
    public static IEnumerable<T> NotSubsetOf<T>(this IGuardClause _,
        IEnumerable<T>? value,
        IEnumerable<T>? other,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.SubsetOf(value, other, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is a subset of <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="other">The superset to check against.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotSubsetOf{T}"/>.
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
    /// Thrown when <paramref name="value"/> is a subset of <paramref name="other"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotSubsetOf{T}"/>:
    /// <c>Guard.Against.SubsetOf</c> passes when at least one element is outside <paramref name="other"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.SubsetOf(extras, baseSet);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotSubsetOf{T}"/>
    public static IEnumerable<T> SubsetOf<T>(this IGuardClause _,
        IEnumerable<T>? value,
        IEnumerable<T>? other,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotSubsetOf(value, other, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have a valid element at <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="index">The zero-based index that must exist in the collection.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.HasIndex{T}"/>.
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
    /// Thrown when <paramref name="index"/> is out of range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.HasIndex{T}"/>:
    /// <c>Guard.Against.NotHasIndex</c> passes when the index is valid.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasIndex(items, index: 0);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.HasIndex{T}"/>
    public static IEnumerable<T> NotHasIndex<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int index,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasIndex(value, index, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has a valid element at <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="index">The zero-based index that must not exist in the collection.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotHasIndex{T}"/>.
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
    /// Thrown when <paramref name="index"/> is valid and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotHasIndex{T}"/>:
    /// <c>Guard.Against.HasIndex</c> passes when the index is out of range.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasIndex(items, index: 5);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasIndex{T}"/>
    public static IEnumerable<T> HasIndex<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int index,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasIndex(value, index, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has exactly <paramref name="count"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="count">The exact count that is forbidden.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotHasExactCount{T}"/>.
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
    /// Thrown when <paramref name="value"/> has exactly <paramref name="count"/> elements and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotHasExactCount{T}"/>:
    /// <c>Guard.Against.HasExactCount</c> passes when the count does not match exactly.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasExactCount(items, count: 0);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasExactCount{T}"/>
    public static IEnumerable<T> HasExactCount<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int count,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasExactCount(value, count, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has at least <paramref name="min"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="min">The minimum count that is forbidden.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotHasMinCount{T}"/>.
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
    /// Thrown when <paramref name="value"/> meets or exceeds the minimum count and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotHasMinCount{T}"/>:
    /// <c>Guard.Against.HasMinCount</c> passes when the count is below the minimum.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasMinCount(items, min: 100);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasMinCount{T}"/>
    public static IEnumerable<T> HasMinCount<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int min,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasMinCount(value, min, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has at most <paramref name="max"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="max">The maximum count that is forbidden.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotHasMaxCount{T}"/>.
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
    /// Thrown when <paramref name="value"/> does not exceed the maximum count and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotHasMaxCount{T}"/>:
    /// <c>Guard.Against.HasMaxCount</c> passes when the count exceeds the maximum.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasMaxCount(items, max: 1);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasMaxCount{T}"/>
    public static IEnumerable<T> HasMaxCount<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int max,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasMaxCount(value, max, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if the element count of <paramref name="value"/> is between <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The collection to guard.</param>
    /// <param name="min">The lower bound of the forbidden range.</param>
    /// <param name="max">The upper bound of the forbidden range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCollectionClauses.NotHasCountBetween{T}"/>.
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
    /// Thrown when the count is within the range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCollectionClauses.NotHasCountBetween{T}"/>:
    /// <c>Guard.Against.HasCountBetween</c> passes when the count is outside the range.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasCountBetween(items, min: 0, max: 5);
    /// </code>
    /// </example>
    /// <seealso cref="MustCollectionClauses.NotHasCountBetween{T}"/>
    public static IEnumerable<T> HasCountBetween<T>(this IGuardClause _,
        IEnumerable<T>? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasCountBetween(value, min, max, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }
}
