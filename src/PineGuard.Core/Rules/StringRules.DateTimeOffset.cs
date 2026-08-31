using System.Globalization;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides date-time-offset string parsing and validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/datetimeoffset">String DateTimeOffset Rules documentation</seealso>
    public static class DateTimeOffset
    {
        private const DateTimeStyles DefaultStyles = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;

        /// <summary>
        /// Determines whether the specified string parses to a date-time-offset in the past.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid date-time-offset string, returns <see langword="false"/>.</param>
        /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing. Defaults to roundtrip kind with assume-universal and whitespace, so offset-less input is treated as UTC regardless of the host time zone.</param>
        /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
        /// <returns><see langword="true"/> if the parsed date-time-offset is in the past; otherwise, <see langword="false"/>.</returns>
        public static bool IsInPast(string? value, Inclusion inclusion = Inclusion.Exclusive, DateTimeStyles styles = DefaultStyles, TimeProvider? timeProvider = null) =>
            StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles) && DateTimeOffsetRules.IsInPast(parsed, inclusion, timeProvider);

        /// <summary>
        /// Determines whether the specified string parses to a date-time-offset in the future.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid date-time-offset string, returns <see langword="false"/>.</param>
        /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing. Defaults to roundtrip kind with assume-universal and whitespace, so offset-less input is treated as UTC regardless of the host time zone.</param>
        /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
        /// <returns><see langword="true"/> if the parsed date-time-offset is in the future; otherwise, <see langword="false"/>.</returns>
        public static bool IsInFuture(string? value, Inclusion inclusion = Inclusion.Exclusive, DateTimeStyles styles = DefaultStyles, TimeProvider? timeProvider = null) =>
            StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles) && DateTimeOffsetRules.IsInFuture(parsed, inclusion, timeProvider);

        /// <summary>
        /// Determines whether the specified string parses to a date-time-offset within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid date-time-offset string, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the range.</param>
        /// <param name="max">The upper bound of the range.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <param name="styles">The <see cref="DateTimeStyles"/> to apply when parsing. Defaults to roundtrip kind with assume-universal and whitespace, so offset-less input is treated as UTC regardless of the host time zone.</param>
        /// <returns><see langword="true"/> if the parsed date-time-offset falls within the specified range; otherwise, <see langword="false"/>.</returns>
        public static bool IsBetween(string? value, System.DateTimeOffset min, System.DateTimeOffset max, Inclusion inclusion = Inclusion.Inclusive, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.DateTimeOffset.TryParse(value, out var parsed, styles) && DateTimeOffsetRules.IsBetween(parsed, min, max, inclusion);
    }
}
