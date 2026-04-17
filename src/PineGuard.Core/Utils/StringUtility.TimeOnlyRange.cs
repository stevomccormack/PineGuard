#if NET8_0_OR_GREATER
using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="Common.TimeOnlyRange"/> values.
    /// </summary>
    public static class TimeOnlyRange
    {
        /// <summary>
        /// Attempts to parse the specified start and end strings as a <see cref="Common.TimeOnlyRange"/>.
        /// </summary>
        /// <param name="start">The start time string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="end">The end time string. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="range">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="Common.TimeOnlyRange"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if both times were parsed and form a valid range; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.TimeOnlyRange.TryParse("09:00", "17:00", out var range); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? start, string? end, out Common.TimeOnlyRange? range)
            => TryParseRange(
                start,
                end,
                static (value, out parsed)
                    => System.TimeOnly.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed),
                static (System.TimeOnly s, System.TimeOnly e, out Common.TimeOnlyRange created)
                    => Common.TimeOnlyRange.TryCreate(s, e, out created),
                out range);
    }
}
#endif
