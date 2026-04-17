using System.ComponentModel.DataAnnotations;
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
public sealed class SlugAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Slug(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
