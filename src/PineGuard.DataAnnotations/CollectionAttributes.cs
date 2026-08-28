using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

// Suffixed with 'Collection' as per collision strategy (most have generic names like Empty)

/// <summary>
/// Base class for collection validation attributes that use reflection to invoke generic
/// <see cref="MustCollectionClauses"/> methods against any <see cref="IEnumerable{T}"/> property.
/// </summary>
/// <remarks>
/// <para>
/// Resolves the element type from the <see cref="IEnumerable{T}"/> interface at runtime and invokes
/// the corresponding generic MustClause method via reflection.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// <para>
/// If the value is non-<see langword="null"/> but its runtime type does not implement
/// <see cref="IEnumerable{T}"/>, the attribute is misapplied and an <see cref="InvalidOperationException"/>
/// is thrown rather than silently reporting the value as valid.
/// </para>
/// </remarks>
public abstract class CollectionAttributeBase(string code) : ValidationAttributeBase(typeof(object), code, allowNull: true)
{
    private static readonly ConcurrentDictionary<Type, Type?> InterfaceCache = new();
    private static readonly ConcurrentDictionary<(string MethodName, Type ItemType), MethodInfo> MethodCache = new();

    /// <summary>
    /// Invokes the specified <see cref="MustCollectionClauses"/> method against the annotated collection value.
    /// </summary>
    /// <param name="methodName">The name of the static method on <see cref="MustCollectionClauses"/> to invoke.</param>
    /// <param name="value">The collection value to validate.</param>
    /// <param name="ctx">The current <see cref="ValidationContext"/>.</param>
    /// <param name="args">Optional extra arguments forwarded after the collection value (e.g., count, comparer).</param>
    /// <returns>
    /// <see langword="null"/> on success, or a <see cref="ValidationResult"/> describing the failure.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="value"/>'s runtime type does not implement <see cref="IEnumerable{T}"/>.
    /// </exception>
    protected ValidationResult? InvokeCollectionMust(string methodName, object? value, ValidationContext ctx, params object?[] args)
    {
        var type = value!.GetType();
        var iEnum = InterfaceCache.GetOrAdd(type, t =>
            t.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)));

        if (iEnum == null)
            throw new InvalidOperationException(
                $"[{GetType().Name}] can only be applied to properties implementing IEnumerable<T>. " +
                $"Property '{ctx.DisplayName}' is of type {type.Name}.");

        var itemType = iEnum.GetGenericArguments()[0];

        var genericMethod = MethodCache.GetOrAdd((methodName, itemType), key =>
            typeof(MustCollectionClauses).GetMethod(key.MethodName, BindingFlags.Public | BindingFlags.Static)!
                .MakeGenericMethod(key.ItemType));

        var invokeArgs = BuildInvokeArgs(genericMethod, value, args);
        return InvokeAndMapResult(genericMethod, invokeArgs, ctx);
    }
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field is an empty collection
/// (contains no elements).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.Empty{T}"/>. Supported on properties, fields, and parameters
/// of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class BatchModel
/// {
///     [EmptyCollection]
///     public List<string> Errors { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="NotEmptyCollectionAttribute"/>
/// <seealso cref="MustCollectionClauses.Empty{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EmptyCollectionAttribute() : CollectionAttributeBase(MustCodes.Collection.Items.NotEmpty)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.Empty), value, validationContext);
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field is not an empty collection
/// (contains at least one element).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.NotEmpty{T}"/>. Supported on properties, fields, and parameters
/// of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class OrderModel
/// {
///     [NotEmptyCollection]
///     public List<string> Items { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="EmptyCollectionAttribute"/>
/// <seealso cref="MustCollectionClauses.NotEmpty{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEmptyCollectionAttribute() : CollectionAttributeBase(MustCodes.Collection.Items.Empty)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.NotEmpty), value, validationContext);
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field has exactly the specified
/// number of elements.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.HasExactCount{T}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class PairModel
/// {
///     [HasExactCountCollection(2)]
///     public List<string> Values { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="MustCollectionClauses.HasExactCount{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasExactCountCollectionAttribute(int count) : CollectionAttributeBase(MustCodes.Collection.Count.Mismatch)
{
    /// <summary>Gets the exact number of elements the collection must contain.</summary>
    public int Count { get; } = count;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.HasExactCount), value, validationContext, Count);
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field has at least the specified
/// number of elements.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.HasMinCount{T}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class SearchModel
/// {
///     [HasMinCountCollection(1)]
///     public List<string> Keywords { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="HasMaxCountCollectionAttribute"/>
/// <seealso cref="MustCollectionClauses.HasMinCount{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasMinCountCollectionAttribute(int min) : CollectionAttributeBase(MustCodes.Collection.Count.TooFew)
{
    /// <summary>Gets the minimum number of elements the collection must contain.</summary>
    public int Min { get; } = min;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.HasMinCount), value, validationContext, Min);
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field has no more than the
/// specified number of elements.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.HasMaxCount{T}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class TagsModel
/// {
///     [HasMaxCountCollection(10)]
///     public List<string> Tags { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="HasMinCountCollectionAttribute"/>
/// <seealso cref="MustCollectionClauses.HasMaxCount{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasMaxCountCollectionAttribute(int max) : CollectionAttributeBase(MustCodes.Collection.Count.TooMany)
{
    /// <summary>Gets the maximum number of elements the collection may contain.</summary>
    public int Max { get; } = max;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.HasMaxCount), value, validationContext, Max);
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field has a count within the
/// specified inclusive (or exclusive) range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.HasCountBetween{T}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class CartModel
/// {
///     [HasCountBetweenCollection(1, 20)]
///     public List<string> Items { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="MustCollectionClauses.HasCountBetween{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasCountBetweenCollectionAttribute(
    int min,
    int max,
    PineGuard.Common.Inclusion inclusion = PineGuard.Common.Inclusion.Inclusive)
    : CollectionAttributeBase(MustCodes.Collection.Count.OutOfRange)
{
    /// <summary>Gets the minimum element count boundary.</summary>
    public int Min { get; } = min;

    /// <summary>Gets the maximum element count boundary.</summary>
    public int Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public PineGuard.Common.Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.HasCountBetween), value, validationContext, Min, Max, Inclusion);
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field contains only distinct
/// elements (no duplicates), using the default equality comparer.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.HasDistinctItems{T}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// <para>
/// Uses <see cref="System.Collections.Generic.EqualityComparer{T}.Default"/> for comparison. Custom comparers
/// cannot be specified via an attribute; use the MustClause directly if a custom comparer is needed.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class TagsModel
/// {
///     [HasDistinctItemsCollection]
///     public List<string> Tags { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="HasDuplicateItemsCollectionAttribute"/>
/// <seealso cref="MustCollectionClauses.HasDistinctItems{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasDistinctItemsCollectionAttribute() : CollectionAttributeBase(MustCodes.Collection.Items.Duplicate)
{
    // Comparer is optional in MustClauses (defaults to EqualityComparer<T>.Default).
    // Attributes can't pass IEqualityComparer. So we use default.
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.HasDistinctItems), value, validationContext);
}

/// <summary>
/// Validates that the annotated <see cref="IEnumerable{T}"/> property or field contains at least one
/// duplicate element, using the default equality comparer.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCollectionClauses.HasDuplicateItems{T}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IEnumerable{T}"/>.
/// </para>
/// <para>
/// Uses <see cref="System.Collections.Generic.EqualityComparer{T}.Default"/> for comparison.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class ListModel
/// {
///     [HasDuplicateItemsCollection]
///     public List<int> Ids { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="HasDistinctItemsCollectionAttribute"/>
/// <seealso cref="MustCollectionClauses.HasDuplicateItems{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/collection">Collection Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasDuplicateItemsCollectionAttribute() : CollectionAttributeBase(MustCodes.Collection.Items.Distinct)
{
    // Comparer is optional (defaults to null in MustClauses signature in Attributes? No, method sig: (..., comparer = null)).
    // We pass null for comparer.
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeCollectionMust(nameof(MustCollectionClauses.HasDuplicateItems), value, validationContext);
}

// Skipping predicates (Func<T, bool>) as they cannot be passed via Attributes easily (unless types).
