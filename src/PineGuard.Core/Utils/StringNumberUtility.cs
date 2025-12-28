using PineGuard.Common;
using PineGuard.Rules;
using System.Globalization;

namespace PineGuard.Utils;

public static class StringNumberUtility
{
    public static bool TryParseInt32Invariant(string? value, out int result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return int.TryParse(value.Trim(), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParseInt64Invariant(string? value, out long result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return long.TryParse(value.Trim(), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParseDecimalInvariant(string? value, out decimal result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return decimal.TryParse(
            value.Trim(),
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out result);
    }

    public static bool TryParseInt32InRangeInvariant(string? value, int min, int max, out int result, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        result = default;

        if (min > max)
            return false;

        if (!TryParseInt32Invariant(value, out result))
            return false;

        return RuleComparison.IsBetween(result, min, max, inclusion);
    }

    public static bool TryParseInt64InRangeInvariant(string? value, long min, long max, out long result, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        result = default;

        if (min > max)
            return false;

        if (!TryParseInt64Invariant(value, out result))
            return false;

        return RuleComparison.IsBetween(result, min, max, inclusion);
    }

    public static bool TryParseDigitsOnly(string? value, out string digitsOnly)
    {
        digitsOnly = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (trimmed.Length == 0)
            return false;

        foreach (var ch in trimmed)
        {
            if (ch is < '0' or > '9')
                return false;
        }

        digitsOnly = trimmed;
        return true;
    }

    public static bool TrySanitizeDigits(string? value, out string digits, char[]? allowedNonDigitChars = null)
    {
        allowedNonDigitChars ??= StringNumberRules.DefaultAllowedDigitSeparators;
        return StringNumberRules.TrySanitizeDigits(value, out digits, allowedNonDigitChars);
    }

    public static bool TryParseDecimalWithMaxPlaces(string? value, int decimalPlaces, out decimal result)
    {
        result = default;

        if (value is null)
            return false;

        value = value.Trim();
        if (value.Length == 0)
            return false;

        if (decimalPlaces < 0)
            return false;

        if (decimalPlaces == 0)
        {
            if (!StringNumberRules.SignedIntegerPatternRegex.IsMatch(value))
                return false;

            return decimal.TryParse(
                value,
                NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out result);
        }

        if (!decimal.TryParse(
                value,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out result))
            return false;

        var bits = decimal.GetBits(result);
        var scale = (bits[3] >> 16) & 0xFF;

        return scale <= decimalPlaces;
    }

    public static bool TryParseDecimalWithExactPlaces(string? value, int exactDecimalPlaces, out decimal result)
    {
        result = default;

        if (value is null)
            return false;

        value = value.Trim();
        if (value.Length == 0)
            return false;

        if (exactDecimalPlaces < 0)
            return false;

        if (exactDecimalPlaces == 0)
        {
            if (!StringNumberRules.SignedIntegerPatternRegex.IsMatch(value))
                return false;

            return decimal.TryParse(
                value,
                NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out result);
        }

        if (!decimal.TryParse(
                value,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out result))
            return false;

        var bits = decimal.GetBits(result);
        var scale = (bits[3] >> 16) & 0xFF;

        return scale == exactDecimalPlaces;
    }
}
