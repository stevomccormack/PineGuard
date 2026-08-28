using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Base class for <see cref="IReadOnlyDictionary{TKey,TValue}"/> validation attributes.
/// </summary>
/// <remarks>
/// <para>
/// Uses <see cref="GenericDictionaryAttributeBase"/> to resolve the generic key and value types at
/// runtime and invoke the corresponding <see cref="MustReadOnlyDictionaryClauses"/> method via reflection.
/// </para>
/// </remarks>
/// <param name="code">
/// The <c>MustCodes</c> catalogue constant identifying the clause the derived attribute adapts.
/// </param>
public abstract class ReadOnlyDictionaryAttributeBase(string code) : GenericDictionaryAttributeBase(typeof(IReadOnlyDictionary<,>), typeof(MustReadOnlyDictionaryClauses), code);

/// <summary>
/// Validates that the annotated <see cref="IReadOnlyDictionary{TKey,TValue}"/> property or field is empty
/// (contains no entries).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustReadOnlyDictionaryClauses.Empty{TKey,TValue}"/>. Supported on properties,
/// fields, and parameters of any type that implements <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class CacheModel
/// {
///     [EmptyReadOnlyDictionary]
///     public IReadOnlyDictionary<string, object> Cache { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="NotEmptyReadOnlyDictionaryAttribute"/>
/// <seealso cref="MustReadOnlyDictionaryClauses.Empty{TKey,TValue}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dictionary">Dictionary Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EmptyReadOnlyDictionaryAttribute() : ReadOnlyDictionaryAttributeBase(MustCodes.Dictionary.Items.NotEmpty)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeDictionaryMust(nameof(MustReadOnlyDictionaryClauses.Empty), value, validationContext);
}

/// <summary>
/// Validates that the annotated <see cref="IReadOnlyDictionary{TKey,TValue}"/> property or field is not
/// empty (contains at least one entry).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustReadOnlyDictionaryClauses.NotEmpty{TKey,TValue}"/>. Supported on properties,
/// fields, and parameters of any type that implements <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class ConfigModel
/// {
///     [NotEmptyReadOnlyDictionary]
///     public IReadOnlyDictionary<string, string> Settings { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="EmptyReadOnlyDictionaryAttribute"/>
/// <seealso cref="MustReadOnlyDictionaryClauses.NotEmpty{TKey,TValue}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dictionary">Dictionary Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEmptyReadOnlyDictionaryAttribute() : ReadOnlyDictionaryAttributeBase(MustCodes.Dictionary.Items.Empty)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) => InvokeDictionaryMust(nameof(MustReadOnlyDictionaryClauses.NotEmpty), value, validationContext);
}
