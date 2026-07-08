#if NET8_0_OR_GREATER
using System.Globalization;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides geographic coordinate string parsing and validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/geo">String GeoLocation Rules documentation</seealso>
    public static class GeoLocation
    {
        /// <summary>
        /// Determines whether the specified string parses to a valid geographic latitude (-90.0 to +90.0).
        /// </summary>
        /// <param name="latitude">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if the parsed value is a valid latitude; otherwise, <see langword="false"/>.</returns>
        public static bool IsLatitude(string? latitude) => StringUtility.NumberTypes.TryParseDouble(latitude, out var parsed, provider: CultureInfo.InvariantCulture) && GeoLocationRules.IsLatitude(parsed);

        /// <summary>
        /// Determines whether the specified string parses to a valid geographic longitude (-180.0 to +180.0).
        /// </summary>
        /// <param name="longitude">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if the parsed value is a valid longitude; otherwise, <see langword="false"/>.</returns>
        public static bool IsLongitude(string? longitude) =>
            StringUtility.NumberTypes.TryParseDouble(longitude, out var parsed, provider: CultureInfo.InvariantCulture) && GeoLocationRules.IsLongitude(parsed);

        /// <summary>
        /// Determines whether the specified latitude and longitude strings parse to a valid geographic coordinate pair.
        /// </summary>
        /// <param name="latitude">The latitude string. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="longitude">The longitude string. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if both parsed values form a valid geographic coordinate; otherwise, <see langword="false"/>.</returns>
        public static bool IsGeoLocation(string? latitude, string? longitude)
        {
            if (!StringUtility.NumberTypes.TryParseDouble(latitude, out var lat, provider: CultureInfo.InvariantCulture))
                return false;

            return StringUtility.NumberTypes.TryParseDouble(longitude, out var lon, provider: CultureInfo.InvariantCulture) && GeoLocationRules.IsGeoLocation(lat, lon);
        }
    }
}
#endif
