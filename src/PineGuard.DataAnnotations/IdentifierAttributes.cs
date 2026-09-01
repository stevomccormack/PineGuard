using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid URL-safe slug
/// (lowercase letters, digits, and hyphens only).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustIdentifierClauses.Slug"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ArticleModel
/// {
///     [Slug]
///     public string UrlSlug { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustIdentifierClauses.Slug"/>
/// <seealso href="https://pineguard.ai/docs/annotations/identifier">Identifier Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SlugAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Identifier.Slug.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Slug(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a canonical ULID
/// (26 Crockford base32 characters).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustIdentifierClauses.Ulid"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The textual form is checked only — the embedded timestamp is not interpreted. If the value is
/// <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EventModel
/// {
///     [Ulid]
///     public string EventId { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustIdentifierClauses.Ulid"/>
/// <seealso href="https://pineguard.ai/docs/annotations/identifier">Identifier Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UlidAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Identifier.Ulid.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Ulid(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
