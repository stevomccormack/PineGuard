namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="System.Guid"/> values.
    /// </summary>
    public static class Guid
    {
        /// <summary>
        /// Attempts to parse the specified string as a nullable <see cref="System.Guid"/>.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="guid">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="System.Guid"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed as a <see cref="System.Guid"/>; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.Guid.TryParse("d3b07384-d9a0-4e9a-8f1a-0c1234567890", out System.Guid? guid); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out System.Guid? guid)
        {
            guid = null;

            if (!TryGetTrimmed(value, out var trimmed))
                return false;

            if (!System.Guid.TryParse(trimmed, out var parsed))
                return false;

            guid = parsed;
            return true;
        }

        /// <summary>
        /// Attempts to parse the specified string as a non-nullable <see cref="System.Guid"/>.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="guid">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="System.Guid"/>.
        /// When <see langword="false"/>, contains <see cref="System.Guid.Empty"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed as a <see cref="System.Guid"/>; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.Guid.TryParse("d3b07384-d9a0-4e9a-8f1a-0c1234567890", out System.Guid guid); // true
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out System.Guid guid)
        {
            guid = System.Guid.Empty;

            return TryGetTrimmed(value, out var trimmed) && System.Guid.TryParse(trimmed, out guid);
        }
    }
}
