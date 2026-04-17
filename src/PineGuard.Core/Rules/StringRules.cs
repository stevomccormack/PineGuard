using System.Text.RegularExpressions;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure string content validation predicates.
/// </summary>
/// <remarks>
/// <para>
/// Partial class — additional validation rules are defined in:
/// <list type="bullet">
///   <item><description><c>StringRules.Bool.cs</c> — boolean string parsing</description></item>
///   <item><description><c>StringRules.Casing.cs</c> — case style validation</description></item>
///   <item><description><c>StringRules.DateOnly.cs</c> — date string validation</description></item>
///   <item><description><c>StringRules.DateTimeOffset.cs</c> — date-time-offset string validation</description></item>
///   <item><description><c>StringRules.GeoLocation.cs</c> — coordinate string validation</description></item>
///   <item><description><c>StringRules.Guid.cs</c> — GUID string validation</description></item>
///   <item><description><c>StringRules.Numbers.cs</c> — numeric string validation</description></item>
///   <item><description><c>StringRules.NumberTypes.cs</c> — numeric type string parsing</description></item>
///   <item><description><c>StringRules.TimeOnly.cs</c> — time string validation</description></item>
///   <item><description><c>StringRules.TimeSpan.cs</c> — duration string validation</description></item>
/// </list>
/// </para>
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/string">String Rules documentation</seealso>
public static partial class StringRules
{
    /// <summary>
    /// Determines whether the specified string has exactly <paramref name="length"/> characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="length">The required character count.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> has exactly <paramref name="length"/> characters; otherwise, <see langword="false"/>.</returns>
    public static bool IsExactLength(string? value, int length)
    {
        if (value is null)
            return false;

        return value.Length == length;
    }

    /// <summary>
    /// Determines whether the length of the specified string falls within [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The minimum character count.</param>
    /// <param name="max">The maximum character count.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if the length is within the specified range; otherwise, <see langword="false"/>.</returns>
    public static bool IsLengthBetween(string? value, int min, int max, Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsBetween(value.Length, min, max, inclusion);

    /// <summary>
    /// Determines whether the specified string is longer than <paramref name="length"/> characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="length">The length threshold.</param>
    /// <param name="inclusion">Whether the threshold is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if the string length satisfies the condition; otherwise, <see langword="false"/>.</returns>
    public static bool IsLongerThan(string? value, int length, Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsGreaterThan(value.Length, length, inclusion);

    /// <summary>
    /// Determines whether the specified string is shorter than <paramref name="length"/> characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="length">The length threshold.</param>
    /// <param name="inclusion">Whether the threshold is inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if the string length satisfies the condition; otherwise, <see langword="false"/>.</returns>
    public static bool IsShorterThan(string? value, int length, Inclusion inclusion = Inclusion.Inclusive) =>
        value is not null && RuleComparison.IsLessThan(value.Length, length, inclusion);

    /// <summary>
    /// Determines whether the specified string matches the given regular expression pattern.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="pattern">The compiled regular expression to test against.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> matches <paramref name="pattern"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="pattern"/> is <see langword="null"/>.</exception>
    public static bool IsMatch(string? value, Regex pattern)
    {
        ThrowHelper.ThrowIfNull(pattern);

        return value is not null && pattern.IsMatch(value);
    }

    /// <summary>
    /// Determines whether the specified string contains only Unicode letters, with optional additional allowed characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="inclusions">Optional additional characters that are also permitted.</param>
    /// <returns><see langword="true"/> if every character is a letter or an allowed inclusion; otherwise, <see langword="false"/>.</returns>
    public static bool IsAlphabetic(string? value, char[]? inclusions = null) =>
        value is not null && AllCharsAreAllowed(value, char.IsLetter, inclusions);

    /// <summary>
    /// Determines whether the specified string contains only decimal digit characters, with optional additional allowed characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="inclusions">Optional additional characters that are also permitted.</param>
    /// <returns><see langword="true"/> if every character is a digit or an allowed inclusion; otherwise, <see langword="false"/>.</returns>
    public static bool IsNumeric(string? value, char[]? inclusions = null) =>
        value is not null && AllCharsAreAllowed(value, ch => CharRules.IsDigit(ch), inclusions);

    /// <summary>
    /// Determines whether the specified string contains only Unicode letters and digits, with optional additional allowed characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="inclusions">Optional additional characters that are also permitted.</param>
    /// <returns><see langword="true"/> if every character is alphanumeric or an allowed inclusion; otherwise, <see langword="false"/>.</returns>
    public static bool IsAlphanumeric(string? value, char[]? inclusions = null) =>
        value is not null && AllCharsAreAllowed(value, char.IsLetterOrDigit, inclusions);

    /// <summary>
    /// Determines whether the specified string contains only decimal digit characters (0–9).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if every character is a decimal digit; otherwise, <see langword="false"/>.</returns>
    public static bool IsDigitsOnly(string? value) =>
        StringUtility.TryParseDigitsOnly(value, out _);

    /// <summary>
    /// Determines whether the specified string contains only decimal digit characters, with optional additional allowed non-digit characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="allowedNonDigitChars">Optional non-digit characters that are also permitted (e.g., <c>-</c>, <c>+</c>).</param>
    /// <returns><see langword="true"/> if every character is a decimal digit or an allowed non-digit character; otherwise, <see langword="false"/>.</returns>
    public static bool IsDigitsOnly(string? value, char[]? allowedNonDigitChars) =>
        StringUtility.TryParseDigits(value, out _, allowedNonDigitChars);

    /// <summary>
    /// Determines whether the specified string is entirely uppercase.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="lettersOnly">
    /// When <see langword="true"/>, every character must be an uppercase letter (non-letter characters are disallowed).
    /// When <see langword="false"/> (default), non-letter characters are permitted as long as all letters are uppercase.
    /// </param>
    /// <returns><see langword="true"/> if the string satisfies the uppercase condition; otherwise, <see langword="false"/>.</returns>
    public static bool IsUppercase(string? value, bool lettersOnly = false)
    {
        if (value is null)
            return false;

        var hasLetter = false;

        if (lettersOnly)
        {
            foreach (var ch in value)
            {
                if (!char.IsLetter(ch) || !char.IsUpper(ch))
                    return false;

                hasLetter = true;
            }

            return hasLetter;
        }

        foreach (var ch in value)
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

    /// <summary>
    /// Determines whether the specified string is entirely lowercase.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="lettersOnly">
    /// When <see langword="true"/>, every character must be a lowercase letter (non-letter characters are disallowed).
    /// When <see langword="false"/> (default), non-letter characters are permitted as long as all letters are lowercase.
    /// </param>
    /// <returns><see langword="true"/> if the string satisfies the lowercase condition; otherwise, <see langword="false"/>.</returns>
    public static bool IsLowercase(string? value, bool lettersOnly = false)
    {
        if (value is null)
            return false;

        var hasLetter = false;

        if (lettersOnly)
        {
            foreach (var ch in value)
            {
                if (!char.IsLetter(ch) || !char.IsLower(ch))
                    return false;

                hasLetter = true;
            }

            return hasLetter;
        }

        foreach (var ch in value)
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

    /// <summary>
    /// Determines whether the specified string contains only ASCII characters (code points 0–127).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if every character has a code point within the ASCII range; otherwise, <see langword="false"/>.</returns>
    public static bool IsAscii(string? value) =>
        value is not null && value.All(ch => ch <= CharRules.AsciiMaxValue);

    /// <summary>
    /// Determines whether the specified string contains only printable ASCII characters (code points 32–126).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="allowCommonWhitespace">
    /// When <see langword="true"/>, carriage return (<c>\r</c>), line feed (<c>\n</c>), and tab (<c>\t</c>) are also permitted.
    /// When <see langword="false"/> (default), only visible printable ASCII characters are allowed.
    /// </param>
    /// <returns><see langword="true"/> if every character is a printable ASCII character (or an allowed whitespace character); otherwise, <see langword="false"/>.</returns>
    public static bool IsPrintableAscii(string? value, bool allowCommonWhitespace = false)
    {
        if (value is null)
            return false;

        foreach (var ch in value)
        {
            if (ch is >= CharRules.PrintableAsciiMinValue and <= CharRules.PrintableAsciiMaxValue)
                continue;

            if (allowCommonWhitespace && ch is '\r' or '\n' or '\t')
                continue;

            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified string consists entirely of whitespace characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if every character is a whitespace character; otherwise, <see langword="false"/>.</returns>
    public static bool IsWhitespace(string? value) =>
        value is not null && value.All(char.IsWhiteSpace);

    /// <summary>
    /// Determines whether the specified string contains at least one whitespace character.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the string contains one or more whitespace characters; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsWhitespace(string? value) =>
        value is not null && value.Any(char.IsWhiteSpace);

    /// <summary>
    /// Determines whether the specified string contains at least one control character.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the string contains one or more control characters; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsControlChars(string? value) =>
        value is not null && value.Any(char.IsControl);

    /// <summary>
    /// Determines whether the specified string contains no control characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if no character in the string is a control character; otherwise, <see langword="false"/>.</returns>
    public static bool NotContainsControlChars(string? value) =>
        value is not null && value.All(ch => !char.IsControl(ch));

    /// <summary>
    /// Determines whether the specified string contains only characters from the provided allowed set.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="allowedChars">The set of permitted characters.</param>
    /// <returns><see langword="true"/> if every character in <paramref name="value"/> is present in <paramref name="allowedChars"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedChars"/> is <see langword="null"/>.</exception>
    public static bool ContainsAllowedOnly(string? value, char[] allowedChars)
    {
        ThrowHelper.ThrowIfNull(allowedChars);

        if (value is null)
            return false;

        var allowed = new HashSet<char>(allowedChars);

        return value.All(ch => allowed.Contains(ch));
    }

    /// <summary>
    /// Determines whether the specified string contains at least one character from the provided disallowed set.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="disallowedChars">The set of forbidden characters.</param>
    /// <returns><see langword="true"/> if the string contains one or more characters from <paramref name="disallowedChars"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="disallowedChars"/> is <see langword="null"/>.</exception>
    public static bool ContainsDisallowed(string? value, char[] disallowedChars)
    {
        ThrowHelper.ThrowIfNull(disallowedChars);

        if (value is null)
            return false;

        var disallowed = new HashSet<char>(disallowedChars);

        return value.Any(ch => disallowed.Contains(ch));
    }

    private static bool AllCharsAreAllowed(string value, Func<char, bool> allowedCharPredicate,
        char[]? additionalAllowedChars)
    {
        if (value.Length == 0)
            return false;

        if (additionalAllowedChars is null || additionalAllowedChars.Length == 0)
            return value.All(allowedCharPredicate);

        var additionalAllowedSet = new HashSet<char>(additionalAllowedChars);

        return value.All(ch => allowedCharPredicate(ch) || additionalAllowedSet.Contains(ch));
    }
}
