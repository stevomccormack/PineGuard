#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate geographic coordinate values.
/// </summary>
/// <seealso cref="GeoLocationRules"/>
/// <seealso href="https://pineguard.ai/docs/must/geo-location">Geo-Location Must Clauses documentation</seealso>
public static class MustGeoLocationClauses
{
    /// <summary>
    /// Validates that the specified value is a valid latitude coordinate (between −90 and +90 inclusive).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="double"/> latitude value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid latitude, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="GeoLocationRules.IsLatitude"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid latitude."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Latitude(lat);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="GeoLocationRules.IsLatitude"/>
    /// <seealso href="https://pineguard.ai/docs/must/geo-location">Geo-Location Must Clauses documentation</seealso>
    public static MustResult<double> Latitude(this IMustClause _,
        double value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a valid latitude.";

        var ok = GeoLocationRules.IsLatitude(value);
        return MustResult<double>.FromBool(ok, MustCodes.Geo.Latitude.Invalid, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value is a valid longitude coordinate (between −180 and +180 inclusive).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="double"/> longitude value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid longitude, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="GeoLocationRules.IsLongitude"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid longitude."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Longitude(lng);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="GeoLocationRules.IsLongitude"/>
    /// <seealso href="https://pineguard.ai/docs/must/geo-location">Geo-Location Must Clauses documentation</seealso>
    public static MustResult<double> Longitude(this IMustClause _,
        double value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a valid longitude.";

        var ok = GeoLocationRules.IsLongitude(value);
        return MustResult<double>.FromBool(ok, MustCodes.Geo.Longitude.Invalid, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified latitude and longitude pair form a valid geographic coordinate.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="latitude">The latitude component of the coordinate (−90 to +90).</param>
    /// <param name="longitude">The longitude component of the coordinate (−180 to +180).</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if both <paramref name="latitude"/> and <paramref name="longitude"/> form a valid coordinate pair, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="GeoLocationRules.IsGeoLocation"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid geo location."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.GeoLocation(lat, lng);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="GeoLocationRules.IsGeoLocation"/>
    /// <seealso href="https://pineguard.ai/docs/must/geo-location">Geo-Location Must Clauses documentation</seealso>
    public static MustResult<(double Latitude, double Longitude)> GeoLocation(this IMustClause _,
        double latitude,
        double longitude,
        [CallerArgumentExpression(nameof(latitude))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a valid geo location.";

        var ok = GeoLocationRules.IsGeoLocation(latitude, longitude);
        return MustResult<(double Latitude, double Longitude)>.FromBool(
            ok,
            MustCodes.Geo.Coordinate.Invalid,
            messageTemplate,
            paramName,
            value: (latitude, longitude),
            result: (latitude, longitude));
    }
}
#endif
