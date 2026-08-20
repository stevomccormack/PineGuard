using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="Common.DateTimeRange"/> values.
    /// </summary>
    public static class DateTimeRange
    {
        /// <summary>
        /// Attempts to parse the specified start and end strings as a <see cref="Common.DateTimeRange"/>.
        /// </summary>
        /// <param name="start">The start date-time string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="end">The end date-time string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="range">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="Common.DateTimeRange"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if both date-times were parsed and form a valid range; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Inputs carrying an explicit offset (e.g. <c>"+05:00"</c> or a trailing <c>"Z"</c>) are normalized to
        /// <see cref="DateTimeKind.Utc"/> deterministically, based on the offset in the string itself — never on
        /// the host machine's local time zone. Offset-less input is parsed with <see cref="DateTimeKind.Unspecified"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// StringUtility.DateTimeRange.TryParse("2024-01-01T00:00:00Z", "2024-12-31T23:59:59Z", out var range); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? start, string? end, out Common.DateTimeRange? range)
            => TryParseRange(
                start,
                end,
                // AdjustToUniversal (not RoundtripKind) so inputs carrying an explicit offset (including "Z") are
                // normalized to Kind=Utc deterministically rather than converted to the host machine's local time;
                // offset-less input is left with Kind=Unspecified, unaffected by the host time zone.
                static (value, out parsed)
                    => DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out parsed),
                static (DateTime s, DateTime e, out Common.DateTimeRange created)
                    => Common.DateTimeRange.TryCreate(s, e, out created),
                out range);
    }
}
