#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for geographic coordinate validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/geolocation">Guard Geo Location Clauses documentation</seealso>
public static class GuardGeoLocationClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid latitude (−90 to +90 degrees).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The latitude value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustGeoLocationClauses.Latitude"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is outside the valid latitude range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustGeoLocationClauses.Latitude"/>:
    /// <c>Guard.Against.NotLatitude</c> passes when the value is a valid latitude (−90 to +90).
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotLatitude(lat);
    /// </code>
    /// </example>
    /// <seealso cref="MustGeoLocationClauses.Latitude"/>
    public static double NotLatitude(this IGuardClause _,
        double value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Latitude(value, paramName); // Guard.Against.NotLatitude => Must.Be.Latitude
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid longitude (−180 to +180 degrees).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The longitude value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustGeoLocationClauses.Longitude"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is outside the valid longitude range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustGeoLocationClauses.Longitude"/>:
    /// <c>Guard.Against.NotLongitude</c> passes when the value is a valid longitude (−180 to +180).
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotLongitude(lon);
    /// </code>
    /// </example>
    /// <seealso cref="MustGeoLocationClauses.Longitude"/>
    public static double NotLongitude(this IGuardClause _,
        double value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Longitude(value, paramName); // Guard.Against.NotLongitude => Must.Be.Longitude
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if the coordinate pair (<paramref name="latitude"/>, <paramref name="longitude"/>) is not a valid geographic location.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="latitude">The latitude component to validate (must be −90 to +90).</param>
    /// <param name="longitude">The longitude component to validate (must be −180 to +180).</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustGeoLocationClauses.GeoLocation"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>A <c>(Latitude, Longitude)</c> tuple with the validated coordinates if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when either coordinate is out of range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustGeoLocationClauses.GeoLocation"/>:
    /// <c>Guard.Against.NotGeoLocation</c> passes when both latitude and longitude are within valid ranges.
    /// </remarks>
    /// <example>
    /// <code>
    /// var (lat, lon) = Guard.Against.NotGeoLocation(latitude, longitude);
    /// </code>
    /// </example>
    /// <seealso cref="MustGeoLocationClauses.GeoLocation"/>
    public static (double Latitude, double Longitude) NotGeoLocation(this IGuardClause _,
        double latitude,
        double longitude,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(latitude))] string? paramName = null)
    {
        var result = Must.Be.GeoLocation(latitude, longitude, paramName); // Guard.Against.NotGeoLocation => Must.Be.GeoLocation
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
#endif
