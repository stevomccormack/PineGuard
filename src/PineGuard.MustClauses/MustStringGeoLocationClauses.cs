#if NET8_0_OR_GREATER
using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate geo-location string representations,
/// parsing the input string before delegating to geo-location rules.
/// </summary>
/// <seealso cref="GeoLocationRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-geo-location">String Geo Location Must Clauses documentation</seealso>
public static class MustStringGeoLocationClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified value must be a valid latitude.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid latitude."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-geo-location">String Geo Location Must Clauses documentation</seealso>
    public static MustResult<double> Latitude(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<double>.Fail(MustCodes.Geo.Latitude.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid latitude.";

        if (!StringUtility.NumberTypes.TryParseDouble(value, out var parsed, provider: CultureInfo.InvariantCulture))
            return MustResult<double>.FromBool(false, MustCodes.Geo.Latitude.Invalid, messageTemplate, paramName, value, result: default);

        var ok = GeoLocationRules.IsLatitude(parsed);
        return MustResult<double>.FromBool(ok, MustCodes.Geo.Latitude.Invalid, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must be a valid longitude.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid longitude."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-geo-location">String Geo Location Must Clauses documentation</seealso>
    public static MustResult<double> Longitude(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<double>.Fail(MustCodes.Geo.Longitude.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid longitude.";

        if (!StringUtility.NumberTypes.TryParseDouble(value, out var parsed, provider: CultureInfo.InvariantCulture))
            return MustResult<double>.FromBool(false, MustCodes.Geo.Longitude.Invalid, messageTemplate, paramName, value, result: default);

        var ok = GeoLocationRules.IsLongitude(parsed);
        return MustResult<double>.FromBool(ok, MustCodes.Geo.Longitude.Invalid, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must be a valid geo location.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid geo location."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-geo-location">String Geo Location Must Clauses documentation</seealso>
    public static MustResult<(double Latitude, double Longitude)> GeoLocation(this IMustClause _,
        string? latitude,
        string? longitude,
        [CallerArgumentExpression(nameof(latitude))] string? paramName = null)
    {
        if (latitude is null)
            return MustResult<(double Latitude, double Longitude)>.Fail(MustCodes.Geo.Coordinate.Invalid, NullMessage, paramName, latitude);

        if (longitude is null)
            return MustResult<(double Latitude, double Longitude)>.Fail(MustCodes.Geo.Coordinate.Invalid, NullMessage, nameof(longitude), longitude);

        const string messageTemplate = "{paramName} must be a valid geo location.";

        if (!StringUtility.NumberTypes.TryParseDouble(latitude, out var parsedLat, provider: CultureInfo.InvariantCulture))
            return MustResult<(double Latitude, double Longitude)>.FromBool(false, MustCodes.Geo.Coordinate.Invalid, messageTemplate, paramName, (latitude, longitude), result: default);

        if (!StringUtility.NumberTypes.TryParseDouble(longitude, out var parsedLon, provider: CultureInfo.InvariantCulture))
            return MustResult<(double Latitude, double Longitude)>.FromBool(false, MustCodes.Geo.Coordinate.Invalid, messageTemplate, nameof(longitude), (latitude, longitude), result: default);

        var ok = GeoLocationRules.IsGeoLocation(parsedLat, parsedLon);
        return MustResult<(double Latitude, double Longitude)>.FromBool(ok, MustCodes.Geo.Coordinate.Invalid, messageTemplate, paramName, (latitude, longitude), (parsedLat, parsedLon));
    }
}
#endif
