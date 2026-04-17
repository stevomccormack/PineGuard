#if NET8_0_OR_GREATER
using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="System.DateOnly"/> values.
    /// </summary>
    public static class DateOnly
    {
        /// <summary>
        /// Attempts to parse the specified string as a nullable <see cref="System.DateOnly"/> using invariant culture.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="date">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="System.DateOnly"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <param name="styles">
        /// The <see cref="DateTimeStyles"/> to apply during parsing.
        /// Defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.DateOnly.TryParse("2024-01-15", out var date); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out System.DateOnly? date, DateTimeStyles styles = DateTimeStyles.AllowWhiteSpaces)
        {
            date = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (!System.DateOnly.TryParse(value, CultureInfo.InvariantCulture, styles,
                    out var parsed))
                return false;

            date = parsed;
            return true;

        }
    }
}
#endif
