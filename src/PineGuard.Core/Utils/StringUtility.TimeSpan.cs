using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="System.TimeSpan"/> values.
    /// </summary>
    public static class TimeSpan
    {
        /// <summary>
        /// Attempts to parse the specified string as a nullable <see cref="System.TimeSpan"/> using invariant culture.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="timeSpan">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="System.TimeSpan"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.TimeSpan.TryParse("01:30:00", out var ts); // true, ts = 1h 30m
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out System.TimeSpan? timeSpan)
        {
            timeSpan = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (!System.TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out var parsed))
                return false;

            timeSpan = parsed;
            return true;

        }
    }
}
