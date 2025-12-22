using System.Globalization;
using System.Text.RegularExpressions;

namespace PineGuard.Rules;

public static class StringRules
{
    public static bool IsNullOrEmpty(string? value) => string.IsNullOrEmpty(value);

    public static bool IsNotNullOrEmpty(string? value) => !string.IsNullOrEmpty(value);

    public static bool IsNullOrWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value);

    public static bool IsNotNullOrWhiteSpace(string? value) => !string.IsNullOrWhiteSpace(value);

    public static bool IsExactLength(string? value, int length)
    {
        if (value is null)
        {
            return false;
        }

        return value.Length == length;
    }

    public static bool IsNotExactLength(string? value, int length) => !IsExactLength(value, length);

    public static bool IsLengthBetween(string? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
        {
            return false;
        }

        return RuleComparison.IsBetween(value.Length, min, max, inclusion);
    }

    public static bool IsNotLengthBetween(string? value, int min, int max, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        !IsLengthBetween(value, min, max, inclusion);

    public static bool IsLongerThan(string? value, int length, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
        {
            return false;
        }

        return RuleComparison.IsGreaterThan(value.Length, length, inclusion);
    }

    public static bool IsNotLongerThan(string? value, int length, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        !IsLongerThan(value, length, inclusion);

    public static bool IsShorterThan(string? value, int length, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (value is null)
        {
            return false;
        }

        return RuleComparison.IsLessThan(value.Length, length, inclusion);
    }

    public static bool IsNotShorterThan(string? value, int length, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        !IsShorterThan(value, length, inclusion);

    public static bool IsMatch(string? value, Regex pattern)
    {
        if (value is null)
        {
            return false;
        }

        if (pattern is null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        return pattern.IsMatch(value);
    }

    public static bool IsNotMatch(string? value, Regex pattern) => !IsMatch(value, pattern);

    public static bool IsDigitsOnly(string? value, char[]? inclusions = null) =>
        IsAllFromAllowedSet(value, IsDigit, inclusions);

    public static bool IsNotDigitsOnly(string? value, char[]? inclusions = null) =>
        !IsDigitsOnly(value, inclusions);

    public static bool IsAlphanumeric(string? value, char[]? inclusions = null) =>
        IsAllFromAllowedSet(value, IsLetterOrDigit, inclusions);

    public static bool IsNotAlphanumeric(string? value, char[]? inclusions = null) =>
        !IsAlphanumeric(value, inclusions);

    public static bool IsAlphabetic(string? value, char[]? inclusions = null) =>
        IsAllFromAllowedSet(value, IsLetter, inclusions);

    public static bool IsNotAlphabetic(string? value, char[]? inclusions = null) =>
        !IsAlphabetic(value, inclusions);

    public static bool IsDecimalString(string? value, int decimalPlaces = 2)
    {
        if (value is null)
        {
            return false;
        }

        value = value.Trim();
        if (value.Length == 0)
        {
            return false;
        }

        if (decimalPlaces < 0)
        {
            return false;
        }

        var decimalPattern = decimalPlaces == 0
            ? @"^[\+\-]?\d+$"
            : $@"^[\+\-]?\d+(?:\.\d{{1,{decimalPlaces}}})?$";

        if (!Regex.IsMatch(value, decimalPattern, RegexOptions.CultureInvariant))
        {
            return false;
        }

        return decimal.TryParse(
            value,
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out _);
    }

    public static bool IsNotDecimalString(string? value, int decimalPlaces = 2) =>
        !IsDecimalString(value, decimalPlaces);

    private static bool IsAllFromAllowedSet(string? value, Func<char, bool> basePredicate, char[]? inclusions)
    {
        if (value is null)
        {
            return false;
        }

        HashSet<char>? inclusionSet = inclusions is { Length: > 0 } ? new HashSet<char>(inclusions) : null;

        foreach (var ch in value)
        {
            if (basePredicate(ch))
            {
                continue;
            }

            if (inclusionSet is not null && inclusionSet.Contains(ch))
            {
                continue;
            }

            return false;
        }

        return true;
    }

    private static bool IsDigit(char c) => c >= '0' && c <= '9';

    private static bool IsLetter(char c) => char.IsLetter(c);

    private static bool IsLetterOrDigit(char c) => char.IsLetterOrDigit(c);
}
