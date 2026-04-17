#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
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
            return MustResult<double>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid latitude.";

        if (!StringUtility.NumberTypes.TryParseDouble(value, out var parsed))
            return MustResult<double>.FromBool(false, messageTemplate, paramName, value);

        var ok = GeoLocationRules.IsLatitude(parsed);
        return MustResult<double>.FromBool(ok, messageTemplate, paramName, value, parsed);
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
            return MustResult<double>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid longitude.";

        if (!StringUtility.NumberTypes.TryParseDouble(value, out var parsed))
            return MustResult<double>.FromBool(false, messageTemplate, paramName, value);

        var ok = GeoLocationRules.IsLongitude(parsed);
        return MustResult<double>.FromBool(ok, messageTemplate, paramName, value, parsed);
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
            return MustResult<(double Latitude, double Longitude)>.Fail(NullMessage, paramName, latitude);

        if (longitude is null)
            return MustResult<(double Latitude, double Longitude)>.Fail(NullMessage, nameof(longitude), longitude);

        const string messageTemplate = "{paramName} must be a valid geo location.";

        if (!StringUtility.NumberTypes.TryParseDouble(latitude, out var parsedLat))
            return MustResult<(double Latitude, double Longitude)>.FromBool(false, messageTemplate, paramName, (latitude, longitude));

        if (!StringUtility.NumberTypes.TryParseDouble(longitude, out var parsedLon))
            return MustResult<(double Latitude, double Longitude)>.FromBool(false, messageTemplate, nameof(longitude), (latitude, longitude));

        var ok = GeoLocationRules.IsGeoLocation(parsedLat, parsedLon);
        return MustResult<(double Latitude, double Longitude)>.FromBool(ok, messageTemplate, paramName, (latitude, longitude), (parsedLat, parsedLon));
    }
}
#endif
