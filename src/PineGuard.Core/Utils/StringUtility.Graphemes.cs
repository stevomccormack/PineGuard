using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides grapheme-cluster helpers — the user-perceived characters a string is made of.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/utils/string/graphemes">String Graphemes Utility documentation</seealso>
    public static class Graphemes
    {
        /// <summary>
        /// Attempts to count the grapheme clusters in the specified string.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A grapheme cluster is what a reader calls a character, which is not what
        /// <see cref="string.Length"/> counts. <c>"e"</c> followed by a combining acute accent is two UTF-16
        /// code units and one character; a family emoji built from four people joined by zero-width joiners is
        /// eleven code units and one character; a carriage return followed by a line feed is two code units and
        /// one character. Counting code units instead is the defect behind every "your name is too long" message
        /// that fires on a name the user can see fits.
        /// </para>
        /// <para>
        /// Segmentation follows the host runtime's Unicode tables via <see cref="StringInfo"/>, so a string may
        /// segment differently on two runtimes built against different Unicode versions. Only
        /// <see langword="null"/> fails: the empty string succeeds with a count of zero, and a string containing
        /// an unpaired surrogate is counted rather than rejected, each unpaired surrogate forming its own cluster.
        /// </para>
        /// </remarks>
        /// <param name="value">The string to count. If <see langword="null"/>, returns <see langword="false"/>.</param>
        /// <param name="count">When this method returns <see langword="true"/>, contains the number of grapheme clusters; otherwise, zero.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was counted; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.Graphemes.TryGetCount("café", out var count); // true, count = 4
        /// </code>
        /// </example>
        public static bool TryGetCount(string? value, out int count)
        {
            count = 0;

            if (value is null)
                return false;

            count = new StringInfo(value).LengthInTextElements;
            return true;
        }
    }
}
