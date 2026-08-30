using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

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

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field parses as a GUID carrying the
/// specified UUID version.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGuidClauses.HasGuidVersion"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Every form <c>Guid.TryParse</c> accepts reads the same version, so the D, N, B, P and X formats are all
/// honoured. <see cref="Version"/> must be between <see cref="GuidRules.MinVersion"/> and
/// <see cref="GuidRules.MaxVersion"/>; any other value fails validation and attributes the failure to the
/// version rather than the value. For <see cref="Guid"/> value-type properties, use
/// <see cref="HasGuidVersionAttribute"/>. If the value is <see langword="null"/>, validation is skipped by
/// the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EntityModel
/// {
///     [HasGuidVersionString(4)]
///     public string CorrelationId { get; set; }
/// }
/// </code>
/// </example>
/// <param name="version">The UUID version the parsed value must carry.</param>
/// <seealso cref="HasGuidVersionAttribute"/>
/// <seealso cref="MustStringGuidClauses.HasGuidVersion"/>
/// <seealso href="https://pineguard.ai/docs/annotations/guid">GUID Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasGuidVersionStringAttribute(int version) : ValidationAttributeBase(typeof(string), MustCodes.Guid.Version.Mismatch)
{
    /// <summary>Gets the UUID version the parsed value must carry.</summary>
    public int Version { get; } = version;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.HasGuidVersion(strValue, Version, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
