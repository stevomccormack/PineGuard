#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

// NOTE: MustStringGeoLocationClauses.GeoLocation(latitude, longitude) is intentionally NOT wrapped here.
// It validates a coordinate pair spanning two separate string inputs, which does not fit the single-property
// ValidationAttributeBase model (one annotated member, one value). Only the single-input Latitude and
// Longitude clauses are surfaced as attributes. This mirrors how cross-property clauses such as
// MustPredicateClauses are excluded from the DataAnnotations layer.

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid geographic
/// latitude in the range −90.0 to +90.0 (inclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGeoLocationClauses.Latitude"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>. The value is parsed as a <see cref="double"/> using invariant
/// culture before evaluation. If the value is <see langword="null"/>, validation is skipped by the base
/// class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LocationModel
/// {
///     [LatitudeString]
///     public string Lat { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LongitudeStringAttribute"/>
/// <seealso cref="MustStringGeoLocationClauses.Latitude"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LatitudeStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Latitude(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid geographic
/// longitude in the range −180.0 to +180.0 (inclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGeoLocationClauses.Longitude"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>. The value is parsed as a <see cref="double"/> using invariant
/// culture before evaluation. If the value is <see langword="null"/>, validation is skipped by the base
/// class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LocationModel
/// {
///     [LongitudeString]
///     public string Lng { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LatitudeStringAttribute"/>
/// <seealso cref="MustStringGeoLocationClauses.Longitude"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LongitudeStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Longitude(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
