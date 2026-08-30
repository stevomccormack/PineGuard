using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="Guid"/> property or field is not an empty GUID
/// (<see cref="Guid.Empty"/>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustGuidClauses.NotEmpty"/>. Supported on properties, fields, and parameters
/// of type <see cref="Guid"/>.
/// </para>
/// <para>
/// An empty GUID is <c>00000000-0000-0000-0000-000000000000</c>. This attribute ensures the value is a
/// non-empty GUID. For <see cref="string"/> GUID validation, use <see cref="StringGuidAttribute"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EntityModel
/// {
///     [NotEmptyGuid]
///     public Guid Id { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="StringGuidAttribute"/>
/// <seealso cref="MustGuidClauses.NotEmpty"/>
/// <seealso href="https://pineguard.ai/docs/annotations/guid">GUID Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEmptyGuidAttribute() : ValidationAttributeBase(typeof(Guid), MustCodes.Guid.Emptiness.Empty)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var guidValue = (Guid)value!;

        var result = Must.Be.NotEmpty(guidValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Guid"/> property or field carries the specified UUID version.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustGuidClauses.HasGuidVersion"/>. Supported on properties, fields, and
/// parameters of type <see cref="Guid"/>.
/// </para>
/// <para>
/// <see cref="Version"/> must be between <see cref="GuidRules.MinVersion"/> and
/// <see cref="GuidRules.MaxVersion"/>; any other value fails validation and attributes the failure to the
/// version rather than the value. <see cref="Guid.Empty"/> is versionless, so it never matches. For
/// <see cref="string"/> GUID properties, use <see cref="HasGuidVersionStringAttribute"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EntityModel
/// {
///     [HasGuidVersion(4)]
///     public Guid Id { get; set; }
/// }
/// </code>
/// </example>
/// <param name="version">The UUID version the value must carry.</param>
/// <seealso cref="HasGuidVersionStringAttribute"/>
/// <seealso cref="MustGuidClauses.HasGuidVersion"/>
/// <seealso href="https://pineguard.ai/docs/annotations/guid">GUID Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasGuidVersionAttribute(int version) : ValidationAttributeBase(typeof(Guid), MustCodes.Guid.Version.Mismatch)
{
    /// <summary>Gets the UUID version the value must carry.</summary>
    public int Version { get; } = version;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var guidValue = (Guid)value!;

        var result = Must.Be.HasGuidVersion(guidValue, Version, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
