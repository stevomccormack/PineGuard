using PineGuard.Common;
using PineGuard.Utils;
using System.Text.RegularExpressions;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static partial class NumberTypes
    {
        public const string SignedIntegerPattern = "^[\\+\\-]?\\d+$";

        [GeneratedRegex(SignedIntegerPattern, RegexOptions.CultureInvariant)]
        public static partial Regex SignedIntegerRegex();

        public static readonly char[] DefaultAllowedDigitSeparators = [' ', '-'];

        public static bool IsDecimal(string? value, int decimalPlaces = 2) =>
            StringUtility.NumberTypes.TryParseDecimal(value, decimalPlaces, out _);

        public static bool IsExactDecimal(string? value, int exactDecimalPlaces = 2) =>
            StringUtility.NumberTypes.TryParseExactDecimal(value, exactDecimalPlaces, out _);

        public static bool IsInt32(string? value) =>
            StringUtility.NumberTypes.TryParseInt32(value, out _);

        public static bool IsInt64(string? value) =>
            StringUtility.NumberTypes.TryParseInt64(value, out _);

        public static bool IsInt32InRange(string? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        {
            if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed))
                return false;

            return RuleComparison.IsBetween(parsed, min, max, inclusion);
        }

        public static bool IsInt64InRange(string? value, long min, long max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        {
            if (!StringUtility.NumberTypes.TryParseInt64(value, out var parsed))
                return false;

            return RuleComparison.IsBetween(parsed, min, max, inclusion);
        }
    }
}
