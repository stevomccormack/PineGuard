using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="System.DateTimeOffset"/> values.
    /// </summary>
    public static class DateTimeOffset
    {
        /// <summary>
        /// Attempts to parse the specified string as a nullable <see cref="System.DateTimeOffset"/> using invariant culture.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="dateTimeOffset">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="System.DateTimeOffset"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <param name="styles">
        /// The <see cref="DateTimeStyles"/> to apply during parsing.
        /// Defaults to <see cref="DateTimeStyles.RoundtripKind"/> | <see cref="DateTimeStyles.AssumeUniversal"/> |
        /// <see cref="DateTimeStyles.AllowWhiteSpaces"/>. <see cref="DateTimeStyles.AssumeUniversal"/> makes
        /// offset-less input strings (e.g. <c>"2024-01-15T10:30:00"</c>) parse deterministically as UTC instead of
        /// being assigned the host machine's local UTC offset.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.DateTimeOffset.TryParse("2024-01-15T10:30:00+00:00", out var dto); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out System.DateTimeOffset? dateTimeOffset, DateTimeStyles styles = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces)
        {
            dateTimeOffset = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (!System.DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, styles, out var parsed))
                return false;

            dateTimeOffset = parsed;
            return true;

        }
    }
}
