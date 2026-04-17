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
        /// <example>
        /// <code>
        /// StringUtility.DateTimeRange.TryParse("2024-01-01T00:00:00Z", "2024-12-31T23:59:59Z", out var range); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? start, string? end, out Common.DateTimeRange? range)
            => TryParseRange(
                start,
                end,
                static (value, out parsed)
                    => DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsed),
                static (DateTime s, DateTime e, out Common.DateTimeRange created)
                    => Common.DateTimeRange.TryCreate(s, e, out created),
                out range);
    }
}
