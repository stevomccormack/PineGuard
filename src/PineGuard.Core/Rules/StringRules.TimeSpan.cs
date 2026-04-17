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
        public static bool IsDurationBetween(string? value, System.TimeSpan min, System.TimeSpan max, Inclusion inclusion = Inclusion.Inclusive) =>
            StringUtility.TimeSpan.TryParse(value, out var parsed) && TimeSpanRules.IsDurationBetween(parsed, min, max, inclusion);

        /// <summary>
        /// Determines whether the specified string parses to a duration greater than the given threshold.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid duration string, returns <see langword="false"/>.</param>
        /// <param name="threshold">The threshold duration to compare against.</param>
        /// <param name="inclusion">Whether the threshold is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <returns><see langword="true"/> if the parsed duration exceeds the threshold; otherwise, <see langword="false"/>.</returns>
        public static bool IsGreaterThan(string? value, System.TimeSpan threshold, Inclusion inclusion = Inclusion.Exclusive) =>
            StringUtility.TimeSpan.TryParse(value, out var parsed) && TimeSpanRules.IsGreaterThan(parsed, threshold, inclusion);

        /// <summary>
        /// Determines whether the specified string parses to a duration less than the given threshold.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid duration string, returns <see langword="false"/>.</param>
        /// <param name="threshold">The threshold duration to compare against.</param>
        /// <param name="inclusion">Whether the threshold is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <returns><see langword="true"/> if the parsed duration is below the threshold; otherwise, <see langword="false"/>.</returns>
        public static bool IsLessThan(string? value, System.TimeSpan threshold, Inclusion inclusion = Inclusion.Exclusive) =>
            StringUtility.TimeSpan.TryParse(value, out var parsed) && TimeSpanRules.IsLessThan(parsed, threshold, inclusion);
    }
}
