using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Base class for <see cref="IDictionary{TKey,TValue}"/> validation attributes.
/// </summary>
/// <remarks>
/// <para>
/// Uses <see cref="GenericDictionaryAttributeBase"/> to resolve the generic key and value types at
/// runtime and invoke the corresponding <see cref="MustDictionaryClauses"/> method via reflection.
/// </para>
/// </remarks>
public abstract class DictionaryAttributeBase() : GenericDictionaryAttributeBase(typeof(IDictionary<,>), typeof(MustDictionaryClauses));

/// <summary>
/// Validates that the annotated <see cref="IDictionary{TKey,TValue}"/> property or field is empty
/// (contains no entries).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDictionaryClauses.Empty{TKey,TValue}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IDictionary{TKey,TValue}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class CacheModel
/// {
///     [EmptyDictionary]
///     public Dictionary<string, object> Cache { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="NotEmptyDictionaryAttribute"/>
/// <seealso cref="MustDictionaryClauses.Empty{TKey,TValue}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dictionary">Dictionary Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EmptyDictionaryAttribute : DictionaryAttributeBase
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeDictionaryMust(nameof(MustDictionaryClauses.Empty), value, validationContext);
}

/// <summary>
/// Validates that the annotated <see cref="IDictionary{TKey,TValue}"/> property or field is not empty
/// (contains at least one entry).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDictionaryClauses.NotEmpty{TKey,TValue}"/>. Supported on properties, fields, and
/// parameters of any type that implements <see cref="IDictionary{TKey,TValue}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class ConfigModel
/// {
///     [NotEmptyDictionary]
///     public Dictionary<string, string> Settings { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="EmptyDictionaryAttribute"/>
/// <seealso cref="MustDictionaryClauses.NotEmpty{TKey,TValue}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dictionary">Dictionary Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEmptyDictionaryAttribute : DictionaryAttributeBase
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeDictionaryMust(nameof(MustDictionaryClauses.NotEmpty), value, validationContext);
}

// Skipping HasKey as it requires passing Key value which might be complex type.
// If Key is string/int, we could support it, but generic TKey makes it hard to constructor injection.
// We can support String Key Dictionary specifically?
// For now, only generic Empty/NotEmpty for IDictionary.
