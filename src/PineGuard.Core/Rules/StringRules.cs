using PineGuard.Common;
using PineGuard.Utils;
using System.Text.RegularExpressions;

namespace PineGuard.Rules;

public static partial class StringRules
{

    public static bool IsExactLength(string? value, int length)
    {
        if (value is null)
            return false;

        return value.Length == length;
    }

    public static bool IsLengthBetween(string? value, int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsBetween(value.Length, min, max, inclusion);
    }

    public static bool IsLongerThan(string? value, int length, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (value is null)
            return false;

        return RuleComparison.IsGreaterThan(value.Length, length, inclusion);
    }

    public static bool IsShorterThan(string? value, int length, Inclusion inclusion = Inclusion.Inclusive)
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

        return IsAllFromAllowedSet(value, CharRules.IsDigit, inclusions);
    }

    public static bool IsAlphanumeric(string? value, char[]? inclusions = null)
    {
        if (value is null)
            return false;

        return IsAllFromAllowedSet(value, char.IsLetterOrDigit, inclusions);
    }

    public static bool IsDigitsOnly(string? value) =>
        StringUtility.TryParseDigitsOnly(value, out _);

    public static bool IsDigitsOnly(string? value, char[]? allowedNonDigitChars) =>
        StringUtility.TryParseDigits(value, out _, allowedNonDigitChars);

    public static bool IsUppercase(string? value, bool lettersOnly = false)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var hasLetter = false;

        if (lettersOnly)
        {
            foreach (var ch in trimmed)
            {
                if (!char.IsLetter(ch) || !char.IsUpper(ch))
                    return false;

                hasLetter = true;
            }

            return hasLetter;
        }

        foreach (var ch in trimmed)
        {
            if (char.IsUpper(ch))
            {
                hasLetter = true;
                continue;
            }

            if (char.IsLetter(ch))
                return false;
        }

        return hasLetter;
    }

    public static bool IsLowercase(string? value, bool lettersOnly = false)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var hasLetter = false;

        if (lettersOnly)
        {
            foreach (var ch in trimmed)
            {
                if (!char.IsLetter(ch) || !char.IsLower(ch))
                    return false;

                hasLetter = true;
            }

            return hasLetter;
        }

        foreach (var ch in trimmed)
        {
            if (char.IsLower(ch))
            {
                hasLetter = true;
                continue;
            }

            if (char.IsLetter(ch))
                return false;
        }

        return hasLetter;
    }

    public static bool IsAscii(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return trimmed.All(ch => ch <= CharRules.AsciiMaxValue);
    }

    public static bool IsPrintableAscii(string? value, bool allowCommonWhitespace = false)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        foreach (var ch in trimmed)
        {
            if (ch is >= CharRules.PrintableAsciiMinValue and <= CharRules.PrintableAsciiMaxValue)
                continue;

            if (allowCommonWhitespace && ch is '\r' or '\n' or '\t')
                continue;

            return false;
        }

        return true;
    }

    public static bool DoesNotContainWhitespace(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return trimmed.All(ch => !char.IsWhiteSpace(ch));
    }

    public static bool ContainsNoControlChars(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return trimmed.All(ch => !char.IsControl(ch));
    }

    public static bool ContainsOnlyAllowedChars(string? value, char[] allowedChars)
    {
        ArgumentNullException.ThrowIfNull(allowedChars);

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var allowed = new HashSet<char>(allowedChars);

        return trimmed.All(ch => allowed.Contains(ch));
    }

    public static bool ContainsAnyDisallowedChars(string? value, char[] allowedChars)
    {
        ArgumentNullException.ThrowIfNull(allowedChars);

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var allowed = new HashSet<char>(allowedChars);

        return trimmed.Any(ch => !allowed.Contains(ch));
    }

    private static bool IsAllFromAllowedSet(string value, Func<char, bool> basePredicate, char[]? inclusions)
    {
        if (inclusions is null || inclusions.Length == 0)
        {
            return value.All(basePredicate);
        }

        var inclusionSet = new HashSet<char>(inclusions);

        return value.Where(ch => !basePredicate(ch)).All(ch => inclusionSet.Contains(ch));
    }
}
