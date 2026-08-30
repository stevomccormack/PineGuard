using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides grapheme-cluster count predicates — length measured in the characters a reader sees rather than
    /// in UTF-16 code units.
    /// </summary>
    /// <remarks>
    /// <see cref="string.Length"/> counts UTF-16 code units, so a single family emoji reads as eleven characters
    /// and an accented letter written with a combining mark reads as two. Every rule here counts grapheme
    /// clusters instead, which is the count a length limit shown to a user is actually promising. The character
    /// counts follow the host runtime's Unicode tables — see
    /// <see cref="StringUtility.Graphemes.TryGetCount(string?, out int)"/>.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/rules/string/graphemes">String Graphemes Rules documentation</seealso>
    public static class Graphemes
    {
        /// <summary>
        /// Determines whether the specified string contains exactly <paramref name="count"/> grapheme clusters.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
        /// <param name="count">The required number of grapheme clusters. If negative, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> has exactly <paramref name="count"/> grapheme clusters; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// bool valid = StringRules.Graphemes.HasExactCount("a\r\nb", 3); // true — three characters, four code units
        /// </code>
        /// </example>
        public static bool HasExactCount(string? value, int count)
        {
            if (count < 0)
                return false;

            return StringUtility.Graphemes.TryGetCount(value, out var actual) && actual == count;
        }

        /// <summary>
        /// Determines whether the specified string contains at least <paramref name="min"/> grapheme clusters.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
        /// <param name="min">The minimum required number of grapheme clusters. If negative, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> has at least <paramref name="min"/> grapheme clusters; otherwise, <see langword="false"/>.</returns>
        public static bool HasMinCount(string? value, int min)
        {
            if (min < 0)
                return false;

            return StringUtility.Graphemes.TryGetCount(value, out var actual) && actual >= min;
        }

        /// <summary>
        /// Determines whether the specified string contains at most <paramref name="max"/> grapheme clusters.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
        /// <param name="max">The maximum allowed number of grapheme clusters. If negative, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> has at most <paramref name="max"/> grapheme clusters; otherwise, <see langword="false"/>.</returns>
        public static bool HasMaxCount(string? value, int max)
        {
            if (max < 0)
                return false;

            return StringUtility.Graphemes.TryGetCount(value, out var actual) && actual <= max;
        }

        /// <summary>
        /// Determines whether the grapheme-cluster count of the specified string falls within the given range.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the acceptable count range. If negative or greater than <paramref name="max"/>, returns <see langword="false"/>.</param>
        /// <param name="max">The upper bound of the acceptable count range. If negative, returns <see langword="false"/>.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <returns><see langword="true"/> if the grapheme-cluster count is within [<paramref name="min"/>, <paramref name="max"/>]; otherwise, <see langword="false"/>.</returns>
        public static bool HasCountBetween(string? value, int min, int max, Inclusion inclusion = Inclusion.Inclusive)
        {
            if (min < 0 || max < 0 || min > max)
                return false;

            return StringUtility.Graphemes.TryGetCount(value, out var actual) && RuleComparison.IsBetween(actual, min, max, inclusion);
        }
    }
}
