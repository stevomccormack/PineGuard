using System.Globalization;
using System.Text.RegularExpressions;
using PineGuard.Common;
using PineGuard.Utils;

#pragma warning disable CS8795 // Partial method must have an implementation part (source generator provides it)

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides numeric type string parsing and format validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/numbertypes">String Number Types Rules documentation</seealso>
    public static partial class NumberTypes
    {
        /// <summary>
        /// A regular expression pattern matching an optional sign followed by one or more digits.
        /// </summary>
        public const string SignedIntegerPattern = @"^[\+\-]?\d+$";

        /// <summary>
        /// Returns a compiled <see cref="Regex"/> for matching signed integer strings.
        /// </summary>
        /// <returns>A culture-invariant regex matching <see cref="SignedIntegerPattern"/>.</returns>
#if NET8_0_OR_GREATER
        [GeneratedRegex(SignedIntegerPattern, RegexOptions.CultureInvariant)]
        public static partial Regex SignedIntegerRegex();
#else
        public static Regex SignedIntegerRegex() => CompiledSignedIntegerRegex;
        private static readonly Regex CompiledSignedIntegerRegex = new(SignedIntegerPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, System.TimeSpan.FromMilliseconds(250));
#endif

        /// <summary>
        /// The default set of allowed digit separator characters: space and hyphen.
        /// </summary>
        public static readonly char[] DefaultAllowedDigitSeparators = [' ', '-'];

        /// <summary>
        /// Determines whether the specified string parses to a decimal with at most <paramref name="decimalPlaces"/> fractional digits.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid decimal, returns <see langword="false"/>.</param>
        /// <param name="decimalPlaces">The maximum number of fractional digits allowed. Defaults to 2.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is a valid decimal with at most <paramref name="decimalPlaces"/> fractional digits; otherwise, <see langword="false"/>.</returns>
        public static bool IsDecimal(
            string? value,
            int decimalPlaces = 2,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite) =>
            StringUtility.NumberTypes.TryParseDecimal(value, decimalPlaces, out _, styles);

        /// <summary>
        /// Determines whether the specified string parses to a decimal with exactly <paramref name="exactDecimalPlaces"/> fractional digits.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid decimal, returns <see langword="false"/>.</param>
        /// <param name="exactDecimalPlaces">The exact number of fractional digits required. Defaults to 2.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> has exactly <paramref name="exactDecimalPlaces"/> fractional digits; otherwise, <see langword="false"/>.</returns>
        public static bool IsExactDecimal(
            string? value,
            int exactDecimalPlaces = 2,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite) =>
            StringUtility.NumberTypes.TryParseExactDecimal(value, exactDecimalPlaces, out _, styles);

        /// <summary>
        /// Determines whether the specified string parses to a valid 32-bit signed integer.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid integer, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is a valid <see cref="int"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsInt32(string? value, NumberStyles styles = NumberStyles.Integer) =>
            StringUtility.NumberTypes.TryParseInt32(value, out _, styles);

        /// <summary>
        /// Determines whether the specified string parses to a valid 64-bit signed integer.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid integer, returns <see langword="false"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is a valid <see cref="long"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsInt64(string? value, NumberStyles styles = NumberStyles.Integer) =>
            StringUtility.NumberTypes.TryParseInt64(value, out _, styles);

        /// <summary>
        /// Determines whether the specified string parses to a 32-bit integer within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid integer, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the range.</param>
        /// <param name="max">The upper bound of the range.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed integer is within the range; otherwise, <see langword="false"/>.</returns>
        public static bool IsInt32InRange(string? value, int min, int max, Inclusion inclusion = Inclusion.Inclusive, NumberStyles styles = NumberStyles.Integer) =>
            StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles) && RuleComparison.IsBetween(parsed, min, max, inclusion);

        /// <summary>
        /// Determines whether the specified string parses to a 64-bit integer within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid integer, returns <see langword="false"/>.</param>
        /// <param name="min">The lower bound of the range.</param>
        /// <param name="max">The upper bound of the range.</param>
        /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
        /// <param name="styles">The <see cref="NumberStyles"/> to apply when parsing.</param>
        /// <returns><see langword="true"/> if the parsed integer is within the range; otherwise, <see langword="false"/>.</returns>
        public static bool IsInt64InRange(string? value, long min, long max, Inclusion inclusion = Inclusion.Inclusive, NumberStyles styles = NumberStyles.Integer) =>
            StringUtility.NumberTypes.TryParseInt64(value, out var parsed, styles) && RuleComparison.IsBetween(parsed, min, max, inclusion);
    }
}
