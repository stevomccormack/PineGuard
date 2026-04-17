#if NET8_0_OR_GREATER
using System.Globalization;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides numeric string parsing and numeric comparison validation predicates.
    /// </summary>
    /// <remarks>
    /// All methods parse the string to a numeric type (typically <see cref="decimal"/>) before
    /// delegating to the corresponding <see cref="NumberRules"/> predicate.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/rules/string/numbers">String Number Rules documentation</seealso>
    public static class Numbers
    {
        private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

        /// <summary>
        /// Determines whether the specified string parses to a positive number (greater than zero).
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is positive; otherwise, <see langword="false"/>.</returns>
        public static bool IsPositive(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsPositive<decimal>(parsed);

        /// <summary>
        /// Determines whether the specified string parses to a negative number (less than zero).
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is negative; otherwise, <see langword="false"/>.</returns>
        public static bool IsNegative(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsNegative<decimal>(parsed);

        /// <summary>
        /// Determines whether the specified string parses to exactly zero.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is zero; otherwise, <see langword="false"/>.</returns>
        public static bool IsZero(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsZero<decimal>(parsed);

        /// <summary>
        /// Determines whether the specified string parses to a number that is not zero.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is not zero; otherwise, <see langword="false"/>.</returns>
        public static bool IsNotZero(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsNotZero<decimal>(parsed);

        /// <summary>
        /// Determines whether the specified string parses to zero or a positive number.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is &gt;= zero; otherwise, <see langword="false"/>.</returns>
        public static bool IsZeroOrPositive(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsZeroOrPositive<decimal>(parsed);

        /// <summary>
        /// Determines whether the specified string parses to zero or a negative number.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is &lt;= zero; otherwise, <see langword="false"/>.</returns>
        public static bool IsZeroOrNegative(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsZeroOrNegative<decimal>(parsed);

        /// <summary>
        /// Determines whether the specified string parses to a number greater than <paramref name="min"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="min">The exclusive lower bound.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value &gt; <paramref name="min"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsGreaterThan(string? value, decimal min, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsGreaterThan(parsed, min);

        /// <summary>
        /// Determines whether the specified string parses to a number greater than or equal to <paramref name="min"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value &gt;= <paramref name="min"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsGreaterThanOrEqual(string? value, decimal min, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsGreaterThanOrEqual(parsed, min);

        /// <summary>
        /// Determines whether the specified string parses to a number less than <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="max">The exclusive upper bound.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value &lt; <paramref name="max"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsLessThan(string? value, decimal max, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsLessThan(parsed, max);

        /// <summary>
        /// Determines whether the specified string parses to a number less than or equal to <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value &lt;= <paramref name="max"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsLessThanOrEqual(string? value, decimal max, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsLessThanOrEqual(parsed, max);

        /// <summary>
        /// Determines whether the specified string parses to a number within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the range.</param>
        /// <param name="max">The upper bound of the range.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is within the range; otherwise, <see langword="false"/>.</returns>
        public static bool IsInRange(string? value, decimal min, decimal max, Inclusion inclusion = Inclusion.Inclusive, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsInRange(parsed, min, max, inclusion);

        /// <summary>
        /// Determines whether the specified string parses to a number approximately equal to <paramref name="target"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="target">The target value to compare against.</param>
        /// <param name="tolerance">The maximum allowed absolute difference. If <see langword="null"/>, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if <c>|parsed - target| &lt;= tolerance</c>; otherwise, <see langword="false"/>.</returns>
        public static bool IsApproximately(string? value, decimal target, decimal? tolerance, NumberStyles styles = DefaultStyles)
        {
            if (tolerance is null)
                return false;

            return StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsApproximately(parsed, target, tolerance);
        }

        /// <summary>
        /// Determines whether the specified string parses to a number that is a multiple of <paramref name="factor"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="factor">The factor to test divisibility against.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is evenly divisible by <paramref name="factor"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsMultipleOf(string? value, decimal factor, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles) && NumberRules.IsMultipleOf(parsed, factor);

        /// <summary>
        /// Determines whether the specified string parses to an even integer.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid integer, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed integer is even; otherwise, <see langword="false"/>.</returns>
        public static bool IsEven(string? value, NumberStyles styles = NumberStyles.Integer) =>
            StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles) && NumberRules.IsEven(parsed);

        /// <summary>
        /// Determines whether the specified string parses to an odd integer.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid integer, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed integer is odd; otherwise, <see langword="false"/>.</returns>
        public static bool IsOdd(string? value, NumberStyles styles = NumberStyles.Integer) =>
            StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles) && NumberRules.IsOdd(parsed);

        /// <summary>
        /// Determines whether the specified string parses to a finite number (not NaN, not infinite).
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is finite; otherwise, <see langword="false"/>.</returns>
        public static bool IsFinite(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDouble(value, out var d, styles) && NumberRules.IsFinite(d);

        /// <summary>
        /// Determines whether the specified string parses to NaN (not a number).
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid number, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed value is NaN; otherwise, <see langword="false"/>.</returns>
        public static bool IsNaN(string? value, NumberStyles styles = DefaultStyles) =>
            StringUtility.NumberTypes.TryParseDouble(value, out var d, styles) && NumberRules.IsNaN(d);
    }
}
#endif
