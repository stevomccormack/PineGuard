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
        /// The string to parse. If <see langword="null"/>, sets <paramref name="result"/> to <see langword="null"/>
        /// and returns <see langword="true"/> (null is treated as a valid absent value).
        /// If whitespace-only or not a valid boolean representation, returns <see langword="false"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="bool"/> value,
        /// or <see langword="null"/> if <paramref name="value"/> was <see langword="null"/>.
        /// When <see langword="false"/>, contains <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed or was <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.Bool.TryParse("true", out var result);  // true, result = true
        /// StringUtility.Bool.TryParse(null, out var result2);    // true, result2 = null
        /// </code>
        /// </example>
        public static bool TryParse(string? value, out bool? result)
        {
            result = null;

            if (value is null)
                return true;

            if (!TryGetTrimmed(value, out var trimmed))
                return false;

            if (!bool.TryParse(trimmed, out var parsed))
                return false;

            result = parsed;
            return true;

        }
    }
}
