#if NET8_0_OR_GREATER
using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="System.TimeOnly"/> values.
    /// </summary>
    public static class TimeOnly
    {
        /// <summary>
        /// Attempts to parse the specified string as a nullable <see cref="System.TimeOnly"/> using invariant culture.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="time">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="System.TimeOnly"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <param name="styles">
        /// The <see cref="DateTimeStyles"/> to apply during parsing.
        /// Defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.TimeOnly.TryParse("14:30:00", out var time); // true, time = 14:30:00
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out System.TimeOnly? time, DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces)
        {
            time = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (!System.TimeOnly.TryParse(value, CultureInfo.InvariantCulture, styles,
                    out var parsed))
                return false;

            time = parsed;
            return true;

        }
    }
}
#endif
