#if NET8_0_OR_GREATER
using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="Common.DateOnlyRange"/> values.
    /// </summary>
    public static class DateOnlyRange
    {
        /// <summary>
        /// Attempts to parse the specified start and end strings as a <see cref="Common.DateOnlyRange"/>.
        /// </summary>
        /// <param name="start">The start date string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="end">The end date string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="range">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="Common.DateOnlyRange"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if both dates were parsed and form a valid range; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.DateOnlyRange.TryParse("2024-01-01", "2024-12-31", out var range); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? start, string? end, out Common.DateOnlyRange? range)
            => TryParseRange(
                start,
                end,
                static (value, out parsed)
                    => System.DateOnly.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed),
                static (System.DateOnly s, System.DateOnly e, out Common.DateOnlyRange created)
                    => Common.DateOnlyRange.TryCreate(s, e, out created),
                out range);
    }
}
#endif
