using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid GUID string
/// in any of the standard format specifiers (D, N, B, P, X).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGuidClauses.Guid"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// For <see cref="Guid"/> value-type properties, use <see cref="NotEmptyGuidAttribute"/> instead.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EntityModel
/// {
///     [StringGuid]
///     public string CorrelationId { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotEmptyGuidAttribute"/>
/// <seealso cref="MustStringGuidClauses.Guid"/>
/// <seealso href="https://pineguard.ai/docs/annotations/guid">GUID Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class StringGuidAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Guid.Format.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Guid(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
