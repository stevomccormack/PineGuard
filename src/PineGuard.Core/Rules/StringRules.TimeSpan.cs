using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides duration string parsing and validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/timespan">String TimeSpan Rules documentation</seealso>
    public static class TimeSpan
    {
        /// <summary>
        /// Determines whether the specified string parses to a duration within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid duration string, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the duration range.</param>
        /// <param name="max">The upper bound of the duration range.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <returns><see langword="true"/> if the parsed duration falls within the specified range; otherwise, <see langword="false"/>.</returns>
        public static bool IsInRange(string? value, System.TimeSpan min, System.TimeSpan max, Inclusion inclusion = Inclusion.Inclusive)
        {
            TemporalRangeComparison.ValidateInclusion(inclusion);
            return StringUtility.TimeSpan.TryParse(value, out var parsed) && TimeSpanRules.IsInRange(parsed, min, max, inclusion);
        }

        /// <summary>
        /// Determines whether the specified string parses to a duration longer than the given other.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid duration string, returns <see langword="false"/>.</param>
        /// <param name="other">The other duration to compare against.</param>
        /// <param name="inclusion">Whether the other is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <returns><see langword="true"/> if the parsed duration exceeds the other; otherwise, <see langword="false"/>.</returns>
        public static bool IsLongerThan(string? value, System.TimeSpan other, Inclusion inclusion = Inclusion.Exclusive)
        {
            TemporalRangeComparison.ValidateInclusion(inclusion);
            return StringUtility.TimeSpan.TryParse(value, out var parsed) && TimeSpanRules.IsLongerThan(parsed, other, inclusion);
        }

        /// <summary>
        /// Determines whether the specified string parses to a duration shorter than the given other.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid duration string, returns <see langword="false"/>.</param>
        /// <param name="other">The other duration to compare against.</param>
        /// <param name="inclusion">Whether the other is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <returns><see langword="true"/> if the parsed duration is below the other; otherwise, <see langword="false"/>.</returns>
        public static bool IsShorterThan(string? value, System.TimeSpan other, Inclusion inclusion = Inclusion.Exclusive)
        {
            TemporalRangeComparison.ValidateInclusion(inclusion);
            return StringUtility.TimeSpan.TryParse(value, out var parsed) && TimeSpanRules.IsShorterThan(parsed, other, inclusion);
        }
    }
}
