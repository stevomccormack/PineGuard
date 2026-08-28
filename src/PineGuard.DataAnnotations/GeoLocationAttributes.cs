#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="double"/> property or field is a valid geographic latitude
/// in the range −90.0 to +90.0 (inclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustGeoLocationClauses.Latitude"/>. Supported on properties, fields, and parameters
/// of type <see cref="double"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LocationModel
/// {
///     [Latitude]
///     public double Lat { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LongitudeAttribute"/>
/// <seealso cref="MustGeoLocationClauses.Latitude"/>
/// <seealso href="https://pineguard.ai/docs/annotations/geolocation">GeoLocation Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LatitudeAttribute() : ValidationAttributeBase(typeof(double), MustCodes.Geo.Latitude.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var doubleValue = (double)value!;

        var result = Must.Be.Latitude(doubleValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="double"/> property or field is a valid geographic longitude
/// in the range −180.0 to +180.0 (inclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustGeoLocationClauses.Longitude"/>. Supported on properties, fields, and parameters
/// of type <see cref="double"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LocationModel
/// {
///     [Longitude]
///     public double Lng { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LatitudeAttribute"/>
/// <seealso cref="MustGeoLocationClauses.Longitude"/>
/// <seealso href="https://pineguard.ai/docs/annotations/geolocation">GeoLocation Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LongitudeAttribute() : ValidationAttributeBase(typeof(double), MustCodes.Geo.Longitude.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var doubleValue = (double)value!;

        var result = Must.Be.Longitude(doubleValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
