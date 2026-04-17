#if NET8_0_OR_GREATER
namespace PineGuard.Rules;

/// <summary>
/// Provides pure geographic coordinate validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/geo">Geo Location Rules documentation</seealso>
public static class GeoLocationRules
{
    /// <summary>The minimum valid latitude value (-90.0°).</summary>
    public const double MinLatitude = -90.0;

    /// <summary>The maximum valid latitude value (+90.0°).</summary>
    public const double MaxLatitude = 90.0;

    /// <summary>The minimum valid longitude value (-180.0°).</summary>
    public const double MinLongitude = -180.0;

    /// <summary>The maximum valid longitude value (+180.0°).</summary>
    public const double MaxLongitude = 180.0;

    /// <summary>
    /// Determines whether the specified value is a valid geographic latitude (−90.0 to +90.0).
    /// </summary>
    /// <param name="latitude">The latitude to validate. If <see langword="null"/> or non-finite, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="latitude"/> is a finite value between
    /// <see cref="MinLatitude"/> and <see cref="MaxLatitude"/> inclusive; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = GeoLocationRules.IsLatitude(51.5074);  // true (London)
    /// bool invalid = GeoLocationRules.IsLatitude(91.0);   // false
    /// </code>
    /// </example>
    public static bool IsLatitude(double? latitude)
    {
        if (!NumberRules.IsFinite(latitude))
            return false;

        return latitude is >= MinLatitude and <= MaxLatitude;
    }

    /// <summary>
    /// Determines whether the specified value is a valid geographic longitude (−180.0 to +180.0).
    /// </summary>
    /// <param name="longitude">The longitude to validate. If <see langword="null"/> or non-finite, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="longitude"/> is a finite value between
    /// <see cref="MinLongitude"/> and <see cref="MaxLongitude"/> inclusive; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = GeoLocationRules.IsLongitude(-0.1278);  // true (London)
    /// bool invalid = GeoLocationRules.IsLongitude(181.0);  // false
    /// </code>
    /// </example>
    public static bool IsLongitude(double? longitude)
    {
        if (!NumberRules.IsFinite(longitude))
            return false;

        return longitude is >= MinLongitude and <= MaxLongitude;
    }

    /// <summary>
    /// Determines whether the specified latitude and longitude form a valid geographic coordinate pair.
    /// </summary>
    /// <param name="latitude">The latitude to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="longitude">The longitude to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if both <paramref name="latitude"/> and <paramref name="longitude"/> are valid;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = GeoLocationRules.IsGeoLocation(51.5074, -0.1278); // true
    /// </code>
    /// </example>
    public static bool IsGeoLocation(double? latitude, double? longitude) =>
        IsLatitude(latitude) && IsLongitude(longitude);
}
#endif
