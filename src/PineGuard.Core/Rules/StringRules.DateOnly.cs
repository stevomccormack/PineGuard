#if NET8_0_OR_GREATER
using System.Globalization;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides date string parsing and validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/date">String Date Rules documentation</seealso>
    public static class DateOnly
    {
        private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

        /// <summary>
        /// Determines whether the specified string parses to a date in the past.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid date string, returns <see langword="false"/>.</param>
        /// <param name="inclusion">Whether the current date is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="styles">
        /// The <see cref="DateTimeStyles"/> to apply when parsing. Defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
        /// <see cref="System.DateOnly.TryParse(string?, IFormatProvider?, DateTimeStyles, out System.DateOnly)"/> only
        /// accepts the whitespace-handling flags (<see cref="DateTimeStyles.AllowWhiteSpaces"/> and its constituents);
        /// passing any other flag causes parsing to fail for every input.
        /// </param>
        /// <returns><see langword="true"/> if the parsed date is in the past; otherwise, <see langword="false"/>.</returns>
        public static bool IsInPast(string? value, Inclusion inclusion = Inclusion.Exclusive, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.DateOnly.TryParse(value, out var parsed, styles) && DateOnlyRules.IsInPast(parsed, inclusion);

        /// <summary>
        /// Determines whether the specified string parses to a date in the future.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid date string, returns <see langword="false"/>.</param>
        /// <param name="inclusion">Whether the current date is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
        /// <param name="styles">
        /// The <see cref="DateTimeStyles"/> to apply when parsing. Defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
        /// <see cref="System.DateOnly.TryParse(string?, IFormatProvider?, DateTimeStyles, out System.DateOnly)"/> only
        /// accepts the whitespace-handling flags (<see cref="DateTimeStyles.AllowWhiteSpaces"/> and its constituents);
        /// passing any other flag causes parsing to fail for every input.
        /// </param>
        /// <returns><see langword="true"/> if the parsed date is in the future; otherwise, <see langword="false"/>.</returns>
        public static bool IsInFuture(string? value, Inclusion inclusion = Inclusion.Exclusive, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.DateOnly.TryParse(value, out var parsed, styles) && DateOnlyRules.IsInFuture(parsed, inclusion);

        /// <summary>
        /// Determines whether the specified string parses to a date within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid date string, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the date range.</param>
        /// <param name="max">The upper bound of the date range.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <param name="styles">
        /// The <see cref="DateTimeStyles"/> to apply when parsing. Defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
        /// <see cref="System.DateOnly.TryParse(string?, IFormatProvider?, DateTimeStyles, out System.DateOnly)"/> only
        /// accepts the whitespace-handling flags (<see cref="DateTimeStyles.AllowWhiteSpaces"/> and its constituents);
        /// passing any other flag causes parsing to fail for every input.
        /// </param>
        /// <returns><see langword="true"/> if the parsed date falls within the specified range; otherwise, <see langword="false"/>.</returns>
        public static bool IsBetween(string? value, System.DateOnly min, System.DateOnly max, Inclusion inclusion = Inclusion.Inclusive, DateTimeStyles styles = DefaultStyles) =>
            StringUtility.DateOnly.TryParse(value, out var parsed, styles) && DateOnlyRules.IsBetween(parsed, min, max, inclusion);
    }
}
#endif
