using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for string-to-geolocation parsing guard clauses.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/string-geo-location">Guard StringGeoLocation documentation</seealso>
public static class GuardStringGeoLocationClauses
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotLatitude constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringGeoLocationClauses.Latitude"/>
    public static double NotLatitude(this IGuardClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Latitude(value, styles, paramName); // Guard.Against.NotLatitude => Must.Be.Latitude
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotLongitude constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringGeoLocationClauses.Longitude"/>
    public static double NotLongitude(this IGuardClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Longitude(value, styles, paramName); // Guard.Against.NotLongitude => Must.Be.Longitude
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if the value violates the NotGeoLocation constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="latitude">The latitude string to guard.</param>
    /// <param name="longitude">The longitude string to guard.</param>
    /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringGeoLocationClauses.GeoLocation"/>
    public static (double Latitude, double Longitude) NotGeoLocation(this IGuardClause _,
        string? latitude,
        string? longitude,
        NumberStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(latitude))] string? paramName = null)
    {
        var result = Must.Be.GeoLocation(latitude, longitude, styles, paramName); // Guard.Against.NotGeoLocation => Must.Be.GeoLocation
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
