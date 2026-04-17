using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    /// <summary>
    /// Provides methods for parsing string representations of numeric types.
    /// </summary>
    public static class NumberTypes
    {
        /// <summary>
        /// Attempts to parse the specified string as an <see cref="int"/> (Int32).
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="int"/>.
        /// When <see langword="false"/>, contains <c>0</c>.
        /// </param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply. Defaults to <see cref="NumberStyles.Integer"/>.</param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> for culture-specific formatting.
        /// If <see langword="null"/>, uses <see cref="CultureInfo.InvariantCulture"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.NumberTypes.TryParseInt32("42", out var result); // true, result = 42
        /// </code>
        /// </example>
        public static bool TryParseInt32(
            string? value,
            out int result,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return int.TryParse(value, styles, provider, out result);
        }

        /// <summary>
        /// Attempts to parse the specified string as a <see cref="long"/> (Int64).
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="long"/>.
        /// When <see langword="false"/>, contains <c>0</c>.
        /// </param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply. Defaults to <see cref="NumberStyles.Integer"/>.</param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> for culture-specific formatting.
        /// If <see langword="null"/>, uses <see cref="CultureInfo.InvariantCulture"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.NumberTypes.TryParseInt64("9999999999", out var result); // true
        /// </code>
        /// </example>
        public static bool TryParseInt64(
            string? value,
            out long result,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return long.TryParse(value, styles, provider, out result);
        }

        /// <summary>
        /// Attempts to parse the specified string as a <see cref="decimal"/>.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="decimal"/>.
        /// When <see langword="false"/>, contains <c>0</c>.
        /// </param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply. Defaults to sign, decimal point, and whitespace styles.</param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> for culture-specific formatting.
        /// If <see langword="null"/>, uses <see cref="CultureInfo.InvariantCulture"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.NumberTypes.TryParseDecimal("123.45", out var result); // true, result = 123.45m
        /// </code>
        /// </example>
        public static bool TryParseDecimal(
            string? value,
            out decimal result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return decimal.TryParse(value, styles, provider, out result);
        }

        /// <summary>
        /// Attempts to parse the specified string as a <see cref="decimal"/> with a maximum number of decimal places.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="decimalPlaces">
        /// The maximum number of decimal places allowed. Must be non-negative; if negative, returns <see langword="false"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="decimal"/> with at most
        /// <paramref name="decimalPlaces"/> fractional digits. When <see langword="false"/>, contains <c>0</c>.
        /// </param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply. Defaults to sign, decimal point, and whitespace styles.</param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> for culture-specific formatting.
        /// If <see langword="null"/>, uses <see cref="CultureInfo.InvariantCulture"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed and has at most <paramref name="decimalPlaces"/> fractional digits; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.NumberTypes.TryParseDecimal("12.34", 2, out var result); // true
        /// StringUtility.NumberTypes.TryParseDecimal("12.345", 2, out var result2); // false
        /// </code>
        /// </example>
        public static bool TryParseDecimal(
            string? value,
            int decimalPlaces,
            out decimal result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (decimalPlaces < 0)
                return false;

            provider ??= CultureInfo.InvariantCulture;

            if (!decimal.TryParse(value, styles, provider, out result))
                return false;

            var bits = decimal.GetBits(result);
            var scale = (bits[3] >> 16) & 0xFF;

            return scale <= decimalPlaces;
        }

        /// <summary>
        /// Attempts to parse the specified string as a <see cref="decimal"/> with an exact number of decimal places.
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="exactDecimalPlaces">
        /// The exact number of decimal places required. Must be non-negative; if negative, returns <see langword="false"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="decimal"/> with exactly
        /// <paramref name="exactDecimalPlaces"/> fractional digits. When <see langword="false"/>, contains <c>0</c>.
        /// </param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply. Defaults to sign, decimal point, and whitespace styles.</param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> for culture-specific formatting.
        /// If <see langword="null"/>, uses <see cref="CultureInfo.InvariantCulture"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed and has exactly <paramref name="exactDecimalPlaces"/> fractional digits; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.NumberTypes.TryParseExactDecimal("12.34", 2, out var result); // true
        /// StringUtility.NumberTypes.TryParseExactDecimal("12.3", 2, out var result2); // false
        /// </code>
        /// </example>
        public static bool TryParseExactDecimal(
            string? value,
            int exactDecimalPlaces,
            out decimal result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (exactDecimalPlaces < 0)
                return false;

            provider ??= CultureInfo.InvariantCulture;

            if (!decimal.TryParse(value, styles, provider, out result))
                return false;

            var bits = decimal.GetBits(result);
            var scale = (bits[3] >> 16) & 0xFF;

            return scale == exactDecimalPlaces;
        }

        /// <summary>
        /// Attempts to parse the specified string as a <see cref="float"/> (Single).
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="float"/>.
        /// When <see langword="false"/>, contains <c>0</c>.
        /// </param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply. Defaults to sign, decimal point, and whitespace styles.</param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> for culture-specific formatting.
        /// If <see langword="null"/>, uses <see cref="CultureInfo.InvariantCulture"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.NumberTypes.TryParseSingle("3.14", out var result); // true, result = 3.14f
        /// </code>
        /// </example>
        public static bool TryParseSingle(
            string? value,
            out float result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return float.TryParse(value, styles, provider, out result);
        }

        /// <summary>
        /// Attempts to parse the specified string as a <see cref="double"/> (Double).
        /// </summary>
        /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
        /// <param name="result">
        /// When this method returns <see langword="true"/>, contains the parsed <see cref="double"/>.
        /// When <see langword="false"/>, contains <c>0</c>.
        /// </param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply. Defaults to sign, decimal point, and whitespace styles.</param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> for culture-specific formatting.
        /// If <see langword="null"/>, uses <see cref="CultureInfo.InvariantCulture"/>.
        /// </param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// StringUtility.NumberTypes.TryParseDouble("3.14159", out var result); // true
        /// </code>
        /// </example>
        public static bool TryParseDouble(
            string? value,
            out double result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return double.TryParse(value, styles, provider, out result);
        }
    }
}
