using PineGuard.Common;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PineGuard.Rules;

public static class StringRules
{
    public static bool IsExactLength(string? value, int length)
    {
        if (value is null)
            return false;

        return value.Length == length;
    }

    public static bool IsLengthBetween(string? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Length, min, max, inclusion);
    }

    public static bool IsLongerThan(string? value, int length, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Length, length, inclusion);
    }

    public static bool IsShorterThan(string? value, int length, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsLessThan(value.Length, length, inclusion);
    }

    public static bool IsMatch(string? value, Regex pattern)
    {
        ArgumentNullException.ThrowIfNull(pattern);

        if (value is null)
            return false;

        return pattern.IsMatch(value);
    }

    public static bool IsAlphabetic(string? value, char[]? inclusions = null)
    {
        if (value is null)
            return false;

        return IsAllFromAllowedSet(value, char.IsLetter, inclusions);
    }

    public static bool IsNumeric(string? value, char[]? inclusions = null)
    {
        if (value is null)
            return false;

        return IsAllFromAllowedSet(value, IsDigit, inclusions);
    }

    public static bool IsAlphanumeric(string? value, char[]? inclusions = null)
    {
        if (value is null)
            return false;

        return IsAllFromAllowedSet(value, char.IsLetterOrDigit, inclusions);
    }

    public static bool IsDecimal(string? value, int decimalPlaces = 2)
    {
        if (value is null)
            return false;

        value = value.Trim();
        if (value.Length == 0)
            return false;

        if (decimalPlaces < 0)
            return false;

        var decimalPattern = decimalPlaces == 0
            ? "^[\\+\\-]?\\d+$"
            : $"^[\\+\\-]?\\d+(?:\\.\\d{{1,{decimalPlaces}}})?$";

        if (!Regex.IsMatch(value, decimalPattern, RegexOptions.CultureInvariant))
            return false;

        return decimal.TryParse(
            value,
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out _);
    }

    public static bool IsExactDecimal(string? value, int exactDecimalPlaces = 2)
    {
        if (value is null)
            return false;

        value = value.Trim();
        if (value.Length == 0)
            return false;

        if (exactDecimalPlaces < 0)
            return false;

        var decimalPattern = exactDecimalPlaces == 0
            ? "^[\\+\\-]?\\d+$"
            : $"^[\\+\\-]?\\d+\\.\\d{{{exactDecimalPlaces}}}$";

        if (!Regex.IsMatch(value, decimalPattern, RegexOptions.CultureInvariant))
            return false;

        return decimal.TryParse(
            value,
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out _);
    }

    private static bool IsAllFromAllowedSet(string value, Func<char, bool> basePredicate, char[]? inclusions)
    {
        if (inclusions is null || inclusions.Length == 0)
        {
            foreach (var ch in value)
            {
                if (!basePredicate(ch))
                    return false;
            }

            return true;
        }

        var inclusionSet = new HashSet<char>(inclusions);

        foreach (var ch in value)
        {
            if (basePredicate(ch))
                continue;

            if (inclusionSet.Contains(ch))
                continue;

            return false;
        }

        return true;
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';
}
