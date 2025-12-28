using PineGuard.Common;
using PineGuard.Utils;
using System.Text.RegularExpressions;

namespace PineGuard.Rules;

public static partial class StringNumberRules
{
    public const string SignedIntegerPattern = "^[\\+\\-]?\\d+$";

    [GeneratedRegex(SignedIntegerPattern, RegexOptions.CultureInvariant)]
    private static partial Regex SignedIntegerRegex();

    public static Regex SignedIntegerPatternRegex => SignedIntegerRegex();

    public static readonly char[] DefaultAllowedDigitSeparators = [' ', '-'];

    public static bool IsDigitsOnly(string? value) =>
        StringNumberUtility.TryParseDigitsOnly(value, out _);

    public static bool TrySanitizeDigits(string? value, out string digits, char[]? allowedNonDigitChars = null) =>
        StringNumberUtility.TrySanitizeDigits(value, out digits, allowedNonDigitChars);

    public static bool IsDecimal(string? value, int decimalPlaces = 2) =>
        StringNumberUtility.TryParseDecimalWithMaxPlaces(value, decimalPlaces, out _);

    public static bool IsExactDecimal(string? value, int exactDecimalPlaces = 2) =>
        StringNumberUtility.TryParseDecimalWithExactPlaces(value, exactDecimalPlaces, out _);

    public static bool IsInt32(string? value) =>
        StringNumberUtility.TryParseInt32Invariant(value, out _);

    public static bool IsInt64(string? value) =>
        StringNumberUtility.TryParseInt64Invariant(value, out _);

    public static bool IsInt32InRange(string? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        StringNumberUtility.TryParseInt32InRangeInvariant(value, min, max, out _, inclusion);

    public static bool IsInt64InRange(string? value, long min, long max, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        StringNumberUtility.TryParseInt64InRangeInvariant(value, min, max, out _, inclusion);

    public static bool IsDigitsOnlyFormatted(string? value, char[]? allowedNonDigitChars = null)
    {
        allowedNonDigitChars ??= DefaultAllowedDigitSeparators;
        return TrySanitizeDigits(value, out _, allowedNonDigitChars);
    }
}
