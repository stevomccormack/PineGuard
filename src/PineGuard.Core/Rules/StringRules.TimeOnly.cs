#if NET8_0_OR_GREATER
using System.Globalization;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides time string parsing and validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/timeonly">String TimeOnly Rules documentation</seealso>
    public static class TimeOnly
    {
        private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

        /// <summary>
        /// Determines whether the specified string parses to a time within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid time string, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the time range.</param>
        /// <param name="max">The upper bound of the time range.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed time falls within the range; otherwise, <see langword="false"/>.</returns>
        public static bool IsBetween(string? value, System.TimeOnly min, System.TimeOnly max, Inclusion inclusion = Inclusion.Inclusive, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.TimeOnly.TryParse(value, out var parsed, styles) && TimeOnlyRules.IsBetween(parsed, min, max, inclusion);

        /// <summary>
        /// Determines whether the specified time string is within a given window of a reference time string.
        /// </summary>
        /// <param name="value">The time string to validate. If <see langword="null"/> or not a valid time, returns <see langword="false"/>.</param>
        /// <param name="reference">The reference time string. If <see langword="null"/> or not a valid time, returns <see langword="false"/>.</param>
        /// <param name="window">The maximum allowed time difference.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the time difference is within the window; otherwise, <see langword="false"/>.</returns>
        public static bool IsWithin(string? value, string? reference, System.TimeSpan window, DateTimeStyles styles = DefaultStyles)
        {
            if (!StringUtility.TimeOnly.TryParse(value, out var parsedValue, styles))
                return false;

            return StringUtility.TimeOnly.TryParse(reference, out var parsedReference, styles) && TimeOnlyRules.IsWithin(parsedValue, parsedReference, window);
        }

        /// <summary>
        /// Determines whether the specified time string parses to a time before the given reference.
        /// </summary>
        /// <param name="value">The time string to validate. If <see langword="null"/> or not a valid time, returns <see langword="false"/>.</param>
        /// <param name="other">The reference time to compare against.</param>
        /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="precision">Optional precision for time truncation before comparison.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed time is before <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsBefore(string? value, System.TimeOnly other, Inclusion inclusion = Inclusion.Exclusive, TimePrecision? precision = null, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.TimeOnly.TryParse(value, out var parsed, styles) && TimeOnlyRules.IsBefore(parsed, other, inclusion, precision);

        /// <summary>
        /// Determines whether the specified time string parses to a time after the given reference.
        /// </summary>
        /// <param name="value">The time string to validate. If <see langword="null"/> or not a valid time, returns <see langword="false"/>.</param>
        /// <param name="other">The reference time to compare against.</param>
        /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="precision">Optional precision for time truncation before comparison.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed time is after <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsAfter(string? value, System.TimeOnly other, Inclusion inclusion = Inclusion.Exclusive, TimePrecision? precision = null, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.TimeOnly.TryParse(value, out var parsed, styles) && TimeOnlyRules.IsAfter(parsed, other, inclusion, precision);

        /// <summary>
        /// Determines whether the specified time string parses to the same time as the given reference.
        /// </summary>
        /// <param name="value">The time string to validate. If <see langword="null"/> or not a valid time, returns <see langword="false"/>.</param>
        /// <param name="other">The reference time to compare against.</param>
        /// <param name="precision">Optional precision for time truncation before comparison.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed time equals <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsSame(string? value, System.TimeOnly other, TimePrecision? precision = null, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.TimeOnly.TryParse(value, out var parsed, styles) && TimeOnlyRules.IsSame(parsed, other, precision);

        /// <summary>
        /// Determines whether two time strings parse to chronologically ordered times (start before end).
        /// </summary>
        /// <param name="start">The start time string. If <see langword="null"/> or not a valid time, returns <see langword="false"/>.</param>
        /// <param name="end">The end time string. If <see langword="null"/> or not a valid time, returns <see langword="false"/>.</param>
        /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if start is before end; otherwise, <see langword="false"/>.</returns>
        public static bool IsChronological(string? start, string? end, Inclusion inclusion = Inclusion.Exclusive, DateTimeStyles styles = DefaultStyles)
        {
            if (!StringUtility.TimeOnly.TryParse(start, out var parsedStart, styles))
                return false;

            return StringUtility.TimeOnly.TryParse(end, out var parsedEnd, styles) && TimeOnlyRules.IsChronological(parsedStart, parsedEnd, inclusion);
        }

        /// <summary>
        /// Determines whether two time ranges (as strings) overlap.
        /// </summary>
        /// <param name="start1">The start of the first range. If <see langword="null"/> or invalid, returns <see langword="false"/>.</param>
        /// <param name="end1">The end of the first range. If <see langword="null"/> or invalid, returns <see langword="false"/>.</param>
        /// <param name="start2">The start of the second range. If <see langword="null"/> or invalid, returns <see langword="false"/>.</param>
        /// <param name="end2">The end of the second range. If <see langword="null"/> or invalid, returns <see langword="false"/>.</param>
        /// <param name="inclusion">Whether the boundaries are inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the two time ranges overlap; otherwise, <see langword="false"/>.</returns>
        public static bool IsOverlapping(
            string? start1,
            string? end1,
            string? start2,
            string? end2,
            Inclusion inclusion = Inclusion.Exclusive,
            DateTimeStyles styles = DefaultStyles)
        {
            if (!StringUtility.TimeOnly.TryParse(start1, out var s1, styles))
                return false;

            if (!StringUtility.TimeOnly.TryParse(end1, out var e1, styles))
                return false;

            if (!StringUtility.TimeOnly.TryParse(start2, out var s2, styles))
                return false;

            return StringUtility.TimeOnly.TryParse(end2, out var e2, styles) && TimeOnlyRules.IsOverlapping(s1, e1, s2, e2, inclusion);
        }
    }
}
#endif
