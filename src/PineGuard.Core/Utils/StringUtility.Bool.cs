namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of <see cref="bool"/> values.
    /// </summary>
    public static class Bool
    {
        /// <summary>
        /// Attempts to parse the specified string as a nullable <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">
        /// The string to parse. If <see langword="null"/>, whitespace-only, or not a valid boolean representation,
        /// returns <see langword="false"/>.
        /// </param>
        /// <param name="boolean">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="bool"/> value.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.Bool.TryParse("true", out var boolean);  // true, boolean = true
        /// StringUtility.Bool.TryParse(null, out var boolean2);   // false, boolean2 = null
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out bool? boolean)
        {
            boolean = null;

            if (!TryGetTrimmed(value, out var trimmed))
                return false;

            if (!bool.TryParse(trimmed, out var parsed))
                return false;

            boolean = parsed;
            return true;

        }
    }
}
