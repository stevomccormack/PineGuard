using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure collection validation predicates for <see cref="IEnumerable{T}"/> sequences.
/// </summary>
/// <remarks>
/// All methods return <see langword="false"/> when the collection is <see langword="null"/>,
/// rather than throwing. Count-based overloads use O(1) paths for <see cref="ICollection{T}"/>
/// and <see cref="IReadOnlyCollection{T}"/> before falling back to enumeration.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/collection">Collection Rules documentation</seealso>
public static class CollectionRules
{
    /// <summary>
    /// Determines whether the specified collection is empty (contains no elements).
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is non-null and contains no elements;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsEmpty<T>(IEnumerable<T>? value)
    {
        switch (value)
        {
            case null:
                return false;
            case ICollection<T> c:
                return c.Count == 0;
            case IReadOnlyCollection<T> rc:
                return rc.Count == 0;
            default:
                {
                    using var e = value.GetEnumerator();
                    return !e.MoveNext();
                }
        }
    }

    /// <summary>
    /// Determines whether the specified collection is not empty (contains at least one element).
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is non-null and contains at least one element;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsNotEmpty<T>(IEnumerable<T>? value)
    {
        switch (value)
        {
            case null:
                return false;
            case ICollection<T> c:
                return c.Count != 0;
            case IReadOnlyCollection<T> rc:
                return rc.Count != 0;
            default:
                {
                    using var e = value.GetEnumerator();
                    return e.MoveNext();
                }
        }
    }

    /// <summary>
    /// Determines whether the specified collection contains exactly <paramref name="count"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="count">The required number of elements. If negative, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains exactly <paramref name="count"/> elements;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasExactCount<T>(IEnumerable<T>? value, int count)
    {
        if (value is null || count < 0)
            return false;

        return value switch
        {
            ICollection<T> c => c.Count == count,
            IReadOnlyCollection<T> rc => rc.Count == count,
            _ => TryGetCountUpTo(value, maxInclusive: count, out var seen) && seen == count
        };
    }

    /// <summary>
    /// Determines whether the specified collection contains at least <paramref name="min"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The minimum required number of elements. If negative, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> has at least <paramref name="min"/> elements;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasMinCount<T>(IEnumerable<T>? value, int min)
    {
        if (value is null || min < 0)
            return false;

        return value switch
        {
            ICollection<T> c => c.Count >= min,
            IReadOnlyCollection<T> rc => rc.Count >= min,
            _ => min == 0 || HasIndex(value, min - 1)
        };
    }

    /// <summary>
    /// Determines whether the specified collection contains at most <paramref name="max"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="max">The maximum allowed number of elements. If negative, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> has at most <paramref name="max"/> elements;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasMaxCount<T>(IEnumerable<T>? value, int max)
    {
        if (value is null || max < 0)
            return false;

        return value switch
        {
            ICollection<T> c => c.Count <= max,
            IReadOnlyCollection<T> rc => rc.Count <= max,
            _ => !HasIndex(value, max)
        };
    }

    /// <summary>
    /// Determines whether the element count of the specified collection falls within the given range.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the acceptable count range.</param>
    /// <param name="max">The upper bound of the acceptable count range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the element count is within [<paramref name="min"/>, <paramref name="max"/>];
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasCountBetween<T>(IEnumerable<T>? value, int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        if (min < 0 || max < 0 || min > max)
            return false;

        switch (value)
        {
            case ICollection<T> c:
                return RuleComparison.IsBetween(c.Count, min, max, inclusion);
            case IReadOnlyCollection<T> rc:
                return RuleComparison.IsBetween(rc.Count, min, max, inclusion);
        }

        var upperBound = inclusion == Inclusion.Inclusive ? max : max - 1;
        if (upperBound < 0)
            return false;

        return TryGetCountUpTo(value, maxInclusive: upperBound, out var seen) && RuleComparison.IsBetween(seen, min, max, inclusion);
    }

    /// <summary>
    /// Determines whether any element in the specified collection satisfies the given predicate.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate function each element is tested against.</param>
    /// <returns>
    /// <see langword="true"/> if at least one element satisfies <paramref name="predicate"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static bool HasAny<T>(IEnumerable<T>? value, Func<T, bool> predicate)
    {
        ThrowHelper.ThrowIfNull(predicate);

        return value is not null && value.Any(predicate);
    }

    /// <summary>
    /// Determines whether all elements in the specified collection satisfy the given predicate.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">The predicate function each element is tested against.</param>
    /// <returns>
    /// <see langword="true"/> if all elements satisfy <paramref name="predicate"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    public static bool HasAll<T>(IEnumerable<T>? value, Func<T, bool> predicate)
    {
        ThrowHelper.ThrowIfNull(predicate);

        return value is not null && value.All(predicate);
    }

    /// <summary>
    /// Determines whether all elements in the specified collection are distinct (no duplicates).
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="comparer">
    /// An optional equality comparer. If <see langword="null"/>, uses the default comparer for <typeparamref name="T"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if no two elements are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasDistinctItems<T>(IEnumerable<T>? value, IEqualityComparer<T>? comparer = null)
    {
        if (value is null)
            return false;

        var set = new HashSet<T>(comparer);

        return value.All(item => set.Add(item));
    }

    /// <summary>
    /// Determines whether the specified collection contains duplicate elements.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="comparer">
    /// An optional equality comparer. If <see langword="null"/>, uses the default comparer for <typeparamref name="T"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if at least two elements are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasDuplicateItems<T>(IEnumerable<T>? value, IEqualityComparer<T>? comparer = null)
    {
        if (value is null)
            return false;

        return !HasDistinctItems(value, comparer);
    }

    /// <summary>
    /// Determines whether the specified collection of nullable reference types contains any <see langword="null"/> elements.
    /// </summary>
    /// <typeparam name="T">The element type (must be a reference type).</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if at least one element is <see langword="null"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ContainsNullItems<T>(IEnumerable<T?>? value) where T : class => value is not null && value.Any(item => item is null);

    /// <summary>
    /// Determines whether the specified collection contains the given <paramref name="item"/>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="item">The item to search for.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains <paramref name="item"/> according to the
    /// default equality comparer; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Contains<T>(IEnumerable<T>? value, T item) =>
        value switch
        {
            null => false,
            ICollection<T> c => c.Contains(item),
            _ => value.Any(element => EqualityComparer<T>.Default.Equals(element, item))
        };

    /// <summary>
    /// Determines whether all elements of the specified collection are contained in <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The superset collection. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if every element of <paramref name="value"/> exists in <paramref name="other"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsSubsetOf<T>(IEnumerable<T>? value, IEnumerable<T>? other)
    {
        if (value is null || other is null)
            return false;

        var otherSet = other as HashSet<T> ?? [.. other];
        return value.All(item => otherSet.Contains(item));
    }

    /// <summary>
    /// Determines whether the specified collection has an element at the given zero-based <paramref name="index"/>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="value">The collection to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="index">The zero-based index to check. If negative, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains at least <paramref name="index"/> + 1 elements;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasIndex<T>(IEnumerable<T>? value, int index)
    {
        if (value is null || index < 0)
            return false;

        return value switch
        {
            ICollection<T> c => index < c.Count,
            IReadOnlyCollection<T> rc => index < rc.Count,
            _ => CollectionUtility.TryGet(value, index, out _)
        };
    }

    private static bool TryGetCountUpTo<T>(IEnumerable<T> value, int maxInclusive, out int seen)
    {
        seen = 0;

        using var e = value.GetEnumerator();
        while (e.MoveNext())
        {
            seen++;

            if (seen > maxInclusive)
                return false;
        }

        return true;
    }
}
