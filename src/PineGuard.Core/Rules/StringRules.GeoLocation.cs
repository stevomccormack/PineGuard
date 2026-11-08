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
        private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

        /// <summary>
        /// Determines whether the specified string parses to a valid geographic latitude (-90.0 to +90.0).
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
        /// <returns><see langword="true"/> if the parsed value is a valid latitude; otherwise, <see langword="false"/>.</returns>
        public static bool IsLatitude(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.IsValidFloatingPointStyles(styles) && StringUtility.NumberTypes.TryParseDouble(value, out var parsed, styles, provider: CultureInfo.InvariantCulture) && GeoLocationRules.IsLatitude(parsed);

        /// <summary>
        /// Determines whether the specified string parses to a valid geographic longitude (-180.0 to +180.0).
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
        /// <returns><see langword="true"/> if the parsed value is a valid longitude; otherwise, <see langword="false"/>.</returns>
        public static bool IsLongitude(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.IsValidFloatingPointStyles(styles) && StringUtility.NumberTypes.TryParseDouble(value, out var parsed, styles, provider: CultureInfo.InvariantCulture) && GeoLocationRules.IsLongitude(parsed);

        /// <summary>
        /// Determines whether the specified latitude and longitude strings parse to a valid geographic coordinate pair.
        /// </summary>
        /// <param name="latitude">The latitude string. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="longitude">The longitude string. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
        /// <returns><see langword="true"/> if both parsed values form a valid geographic coordinate; otherwise, <see langword="false"/>.</returns>
        public static bool IsGeoLocation(string? latitude, string? longitude, NumberStyles styles = DefaultStyles)
        {
            if (!StringUtility.NumberTypes.IsValidFloatingPointStyles(styles) || !StringUtility.NumberTypes.TryParseDouble(latitude, out var lat, styles, provider: CultureInfo.InvariantCulture))
                return false;

            return StringUtility.NumberTypes.TryParseDouble(longitude, out var lon, styles, provider: CultureInfo.InvariantCulture) && GeoLocationRules.IsGeoLocation(lat, lon);
        }
    }
}
