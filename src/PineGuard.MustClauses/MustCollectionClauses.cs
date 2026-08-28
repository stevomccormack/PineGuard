using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate collections,
/// delegating to <see cref="CollectionRules"/> for core validation logic.
/// </summary>
/// <seealso cref="CollectionRules"/>
/// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
public static class MustCollectionClauses
{
    private const string NullMessage = "{paramName} must not be null.";
    private const string NonNegativeMinMessage = "{paramName} requires a non-negative minimum count.";
    private const string NonNegativeMaxMessage = "{paramName} requires a non-negative maximum count.";

    /// <summary>
    /// Validates that the specified value must be empty.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be empty."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> Empty<T>(this IMustClause _,
        IEnumerable<T>? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.NotEmpty, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be empty.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.IsEmpty(enumerable);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.NotEmpty, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not be empty.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be empty."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotEmpty<T>(this IMustClause _,
        IEnumerable<T>? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Empty, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be empty.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.IsNotEmpty(enumerable);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.Empty, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative count.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="count">The expected count.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative count."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasExactCount<T>(this IMustClause _,
        IEnumerable<T>? value,
        int count,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.Mismatch, NullMessage, paramName, value);

        if (count < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.Mismatch, "{paramName} requires a non-negative count.", nameof(count), count);

        const string messageTemplate = "{paramName} must have the expected count.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasExactCount(enumerable, count);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.Mismatch, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must have at least the minimum count.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have at least the minimum count."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasMinCount<T>(this IMustClause _,
        IEnumerable<T>? value,
        int min,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooFew, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooFew, NonNegativeMinMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must have at least the minimum count.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasMinCount(enumerable, min);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.TooFew, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must have at most the maximum count.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have at most the maximum count."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasMaxCount<T>(this IMustClause _,
        IEnumerable<T>? value,
        int max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooMany, NullMessage, paramName, value);

        if (max < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooMany, NonNegativeMaxMessage, nameof(max), max);

        const string messageTemplate = "{paramName} must have at most the maximum count.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasMaxCount(enumerable, max);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.TooMany, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value requires a valid count range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a valid count range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasCountBetween<T>(this IMustClause _,
        IEnumerable<T>? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.OutOfRange, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.OutOfRange, NonNegativeMinMessage, nameof(min), min);

        if (max < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.OutOfRange, NonNegativeMaxMessage, nameof(max), max);

        if (min > max)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.OutOfRange, "{paramName} requires a valid count range.", nameof(min), min);

        const string messageTemplate = "{paramName} must have a count within the expected range.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasCountBetween(enumerable, min, max, inclusion);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.OutOfRange, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must contain an item that matches the predicate.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="predicate">The predicate function to evaluate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an item that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasAny<T>(this IMustClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.NoMatch, NullMessage, paramName, value);

        if (predicate is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.NoMatch, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must contain an item that matches the predicate.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasAny(enumerable, predicate);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.NoMatch, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not contain an item that matches the predicate.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="predicate">The predicate function to evaluate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain an item that matches the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotHasAny<T>(this IMustClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Match, NullMessage, paramName, value);

        if (predicate is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Match, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must not contain an item that matches the predicate.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.HasAny(enumerable, predicate);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.Match, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must have all items match the predicate.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="predicate">The predicate function to evaluate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have all items match the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasAll<T>(this IMustClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.NotAllMatch, NullMessage, paramName, value);

        if (predicate is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.NotAllMatch, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must have all items match the predicate.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasAll(enumerable, predicate);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.NotAllMatch, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not have all items match the predicate.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="predicate">The predicate function to evaluate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have all items match the predicate."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotHasAll<T>(this IMustClause _,
        IEnumerable<T>? value,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.AllMatch, NullMessage, paramName, value);

        if (predicate is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.AllMatch, NullMessage, nameof(predicate), predicate);

        const string messageTemplate = "{paramName} must not have all items match the predicate.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.HasAll(enumerable, predicate);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.AllMatch, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must have distinct items.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="comparer">An optional equality comparer to use.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have distinct items."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasDistinctItems<T>(this IMustClause _,
        IEnumerable<T>? value,
        IEqualityComparer<T>? comparer = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Duplicate, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must have distinct items.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasDistinctItems(enumerable, comparer);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.Duplicate, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must have duplicate items.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="comparer">An optional equality comparer to use.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must have duplicate items."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasDuplicateItems<T>(this IMustClause _,
        IEnumerable<T>? value,
        IEqualityComparer<T>? comparer = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Distinct, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must have duplicate items.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasDuplicateItems(enumerable, comparer);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.Distinct, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not contain any null items.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain any null items."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotContainsNullItems<T>(this IMustClause _,
        IEnumerable<T?>? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : class
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.ContainsNull, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not contain any null items.";

        var enumerable = value as T?[] ?? [.. value];
        var ok = !CollectionRules.ContainsNullItems(enumerable);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.ContainsNull, messageTemplate, paramName, enumerable, enumerable!);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified item.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain the specified item."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> Contains<T>(this IMustClause _,
        IEnumerable<T>? value,
        T item,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Missing, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must contain the specified item.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.Contains(enumerable, item);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.Missing, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified item.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="item">The item to search for.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified item."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotContains<T>(this IMustClause _,
        IEnumerable<T>? value,
        T item,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Present, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not contain the specified item.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.Contains(enumerable, item);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.Present, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must be a subset of the other collection.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a subset of the other collection."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> SubsetOf<T>(this IMustClause _,
        IEnumerable<T>? value,
        IEnumerable<T>? other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.NotSubset, NullMessage, paramName, value);

        if (other is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.NotSubset, NullMessage, nameof(other), other);

        const string messageTemplate = "{paramName} must be a subset of the other collection.";

        var enumerable = value as T[] ?? [.. value];
        var otherEnumerable = other as T[] ?? [.. other];
        var ok = CollectionRules.IsSubsetOf(enumerable, otherEnumerable);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.NotSubset, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not be a subset of the other collection.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a subset of the other collection."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotSubsetOf<T>(this IMustClause _,
        IEnumerable<T>? value,
        IEnumerable<T>? other,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Subset, NullMessage, paramName, value);

        if (other is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Items.Subset, NullMessage, nameof(other), other);

        const string messageTemplate = "{paramName} must not be a subset of the other collection.";

        var enumerable = value as T[] ?? [.. value];
        var otherEnumerable = other as T[] ?? [.. other];
        var ok = !CollectionRules.IsSubsetOf(enumerable, otherEnumerable);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Items.Subset, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative index.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="index">The index to check.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative index."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> HasIndex<T>(this IMustClause _,
        IEnumerable<T>? value,
        int index,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Index.OutOfRange, NullMessage, paramName, value);

        if (index < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Index.OutOfRange, "{paramName} requires a non-negative index.", nameof(index), index);

        const string messageTemplate = "{paramName} must have an item at the specified index.";

        var enumerable = value as T[] ?? [.. value];
        var ok = CollectionRules.HasIndex(enumerable, index);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Index.OutOfRange, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative index.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="index">The index to check.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative index."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotHasIndex<T>(this IMustClause _,
        IEnumerable<T>? value,
        int index,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Index.InRange, NullMessage, paramName, value);

        if (index < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Index.InRange, "{paramName} requires a non-negative index.", nameof(index), index);

        const string messageTemplate = "{paramName} must not have an item at the specified index.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.HasIndex(enumerable, index);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Index.InRange, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative count.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="count">The expected count.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative count."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotHasExactCount<T>(this IMustClause _,
        IEnumerable<T>? value,
        int count,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.Match, NullMessage, paramName, value);

        if (count < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.Match, "{paramName} requires a non-negative count.", nameof(count), count);

        const string messageTemplate = "{paramName} must not have the expected count.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.HasExactCount(enumerable, count);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.Match, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not have at least the minimum count.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have at least the minimum count."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotHasMinCount<T>(this IMustClause _,
        IEnumerable<T>? value,
        int min,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooMany, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooMany, NonNegativeMinMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must not have at least the minimum count.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.HasMinCount(enumerable, min);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.TooMany, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value must not have at most the maximum count.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not have at most the maximum count."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotHasMaxCount<T>(this IMustClause _,
        IEnumerable<T>? value,
        int max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooFew, NullMessage, paramName, value);

        if (max < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.TooFew, NonNegativeMaxMessage, nameof(max), max);

        const string messageTemplate = "{paramName} must not have at most the maximum count.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.HasMaxCount(enumerable, max);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.TooFew, messageTemplate, paramName, enumerable, enumerable);
    }

    /// <summary>
    /// Validates that the specified value requires a valid count range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a valid count range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/collection">Collection Must Clauses documentation</seealso>
    public static MustResult<IEnumerable<T>> NotHasCountBetween<T>(this IMustClause _,
        IEnumerable<T>? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.InRange, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.InRange, NonNegativeMinMessage, nameof(min), min);

        if (max < 0)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.InRange, NonNegativeMaxMessage, nameof(max), max);

        if (min > max)
            return MustResult<IEnumerable<T>>.Fail(MustCodes.Collection.Count.InRange, "{paramName} requires a valid count range.", nameof(min), min);

        const string messageTemplate = "{paramName} must not have a count within the expected range.";

        var enumerable = value as T[] ?? [.. value];
        var ok = !CollectionRules.HasCountBetween(enumerable, min, max, inclusion);
        return MustResult<IEnumerable<T>>.FromBool(ok, MustCodes.Collection.Count.InRange, messageTemplate, paramName, enumerable, enumerable);
    }
}
