using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="Common.DateTimeOffsetRange"/> values.
    /// </summary>
    public static class DateTimeOffsetRange
    {
        /// <summary>
        /// Attempts to parse the specified start and end strings as a <see cref="Common.DateTimeOffsetRange"/>.
        /// </summary>
        /// <param name="start">The start date-time-offset string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="end">The end date-time-offset string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="range">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="Common.DateTimeOffsetRange"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if both values were parsed and form a valid range; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Parses using <see cref="DateTimeStyles.RoundtripKind"/> | <see cref="DateTimeStyles.AssumeUniversal"/> |
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>, so offset-less <paramref name="start"/>/<paramref name="end"/>
        /// values are treated as UTC regardless of the host time zone.
        /// </remarks>
        /// <example>
        /// <code>
        /// StringUtility.DateTimeOffsetRange.TryParse("2024-01-01T00:00:00+00:00", "2024-12-31T23:59:59+00:00", out var range); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? start, string? end, out Common.DateTimeOffsetRange? range)
            => TryParseRange(
                start,
                end,
                static (value, out parsed)
                    => System.DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces, out parsed),
                static (System.DateTimeOffset s, System.DateTimeOffset e, out Common.DateTimeOffsetRange created)
                    => Common.DateTimeOffsetRange.TryCreate(s, e, out created),
                out range);
    }
}
