using System.Text;
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
    /// The Unicode byte-order mark character (<c>U+FEFF</c>), also known as the zero-width no-break space.
    /// </summary>
    public const char ByteOrderMark = '\uFEFF';

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
    /// <param name="inclusion">Whether the threshold is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if the string length satisfies the condition; otherwise, <see langword="false"/>.</returns>
    public static bool IsLongerThan(string? value, int length, Inclusion inclusion = Inclusion.Exclusive) =>
        value is not null && RuleComparison.IsGreaterThan(value.Length, length, inclusion);

    /// <summary>
    /// Determines whether the specified string is shorter than <paramref name="length"/> characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="length">The length threshold.</param>
    /// <param name="inclusion">Whether the threshold is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if the string length satisfies the condition; otherwise, <see langword="false"/>.</returns>
    public static bool IsShorterThan(string? value, int length, Inclusion inclusion = Inclusion.Exclusive) =>
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
    /// Determines whether the specified string is itself a syntactically valid regular expression.
    /// </summary>
    /// <param name="value">The pattern to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> compiles as a regular expression; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This validates the pattern, not a value against a pattern — <see cref="IsMatch(string?, Regex)"/> does the
    /// latter. It is what a caller reaches for when the pattern is configuration or user input, so that a malformed
    /// one is reported rather than thrown from deep inside a validator. The value is not trimmed, since whitespace is
    /// significant in a pattern, and an empty pattern is rejected as naming no expression. Only the syntax is checked:
    /// a pattern that parses can still be catastrophically slow, which is why
    /// <see cref="Utils.StringUtility.RegexMatchTimeout"/> is applied to the expression that is built from it.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = StringRules.IsRegexPattern(@"^\d{3}-\d{4}$"); // true
    /// bool invalid = StringRules.IsRegexPattern("[unclosed");    // false
    /// </code>
    /// </example>
    public static bool IsRegexPattern(string? value) =>
        StringUtility.TryCreateRegex(value, out _);

    /// <summary>
    /// Determines whether the specified string contains only Unicode letters, with optional additional allowed characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="inclusions">Optional additional characters that are also permitted.</param>
    /// <returns><see langword="true"/> if every character is a letter or an allowed inclusion; otherwise, <see langword="false"/>.</returns>
    public static bool IsAlphabetic(string? value, char[]? inclusions = null) =>
        value is not null && AllCharsAreAllowed(value, char.IsLetter, inclusions);

    /// <summary>
    /// Determines whether the specified string contains only ASCII decimal digit characters (<c>0</c>–<c>9</c>), with optional additional allowed characters.
    /// </summary>
    /// <remarks>
    /// Digit detection is ASCII-only (<see cref="CharRules.IsDigit(char?)"/>), unlike <see cref="IsAlphanumeric(string, char[])"/>, which
    /// accepts any Unicode decimal-digit character (e.g., Arabic-Indic or fullwidth digits). A string of non-ASCII digits therefore fails
    /// <see cref="IsNumeric(string, char[])"/> while passing <see cref="IsAlphanumeric(string, char[])"/>.
    /// </remarks>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="inclusions">Optional additional characters that are also permitted.</param>
    /// <returns><see langword="true"/> if every character is an ASCII digit (<c>0</c>–<c>9</c>) or an allowed inclusion; otherwise, <see langword="false"/>.</returns>
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
    /// <param name="allowedNonDigitChars">
    /// Non-digit characters that are also permitted (e.g., <c>-</c>, <c>+</c>). <see langword="null"/> means no non-digit characters
    /// are permitted (digits only) — it does <em>not</em> fall back to a default separator set.
    /// </param>
    /// <returns><see langword="true"/> if every character is a decimal digit or an allowed non-digit character; otherwise, <see langword="false"/>.</returns>
    public static bool IsDigitsOnly(string? value, char[]? allowedNonDigitChars) =>
        StringUtility.TryParseDigits(value, out _, allowedNonDigitChars ?? []);

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
    public static bool IsAscii(string? value)
    {
        if (value is null)
            return false;

        foreach (var ch in value)
        {
            if (ch > CharRules.AsciiMaxValue)
                return false;
        }

        return true;
    }

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
    public static bool IsWhitespace(string? value)
    {
        if (value is null)
            return false;

        foreach (var ch in value)
        {
            if (!char.IsWhiteSpace(ch))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified string contains at least one whitespace character.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the string contains one or more whitespace characters; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsWhitespace(string? value)
    {
        if (value is null)
            return false;

        foreach (var ch in value)
        {
            if (char.IsWhiteSpace(ch))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified string contains at least one control character.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the string contains one or more control characters; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsControlChars(string? value)
    {
        if (value is null)
            return false;

        foreach (var ch in value)
        {
            if (char.IsControl(ch))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified string contains no control characters.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if no character in the string is a control character; otherwise, <see langword="false"/>.</returns>
    public static bool NotContainsControlChars(string? value)
    {
        if (value is null)
            return false;

        foreach (var ch in value)
        {
            if (char.IsControl(ch))
                return false;
        }

        return true;
    }

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

        foreach (var ch in value)
        {
            if (Array.IndexOf(allowedChars, ch) < 0)
                return false;
        }

        return true;
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

        foreach (var ch in value)
        {
            if (Array.IndexOf(disallowedChars, ch) >= 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified string contains the given substring.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="substring">The substring to search for. An empty substring is always contained (BCL semantics).</param>
    /// <param name="comparison">The comparison rule used to locate <paramref name="substring"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> contains <paramref name="substring"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="substring"/> is <see langword="null"/>.</exception>
    public static bool Contains(string? value, string substring, StringComparison comparison = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(substring);

        return value is not null && value.Contains(substring, comparison);
    }

    /// <summary>
    /// Determines whether the specified string starts with the given prefix.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="prefix">The prefix to test for. An empty prefix always matches (BCL semantics).</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="prefix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> starts with <paramref name="prefix"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="prefix"/> is <see langword="null"/>.</exception>
    public static bool StartsWith(string? value, string prefix, StringComparison comparison = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(prefix);

        return value is not null && value.StartsWith(prefix, comparison);
    }

    /// <summary>
    /// Determines whether the specified string ends with the given suffix.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="suffix">The suffix to test for. An empty suffix always matches (BCL semantics).</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="suffix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> ends with <paramref name="suffix"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="suffix"/> is <see langword="null"/>.</exception>
    public static bool EndsWith(string? value, string suffix, StringComparison comparison = StringComparison.Ordinal)
    {
        ThrowHelper.ThrowIfNull(suffix);

        return value is not null && value.EndsWith(suffix, comparison);
    }

    /// <summary>
    /// Determines whether the specified string begins with the Unicode byte-order mark (<see cref="ByteOrderMark"/>, <c>U+FEFF</c>).
    /// </summary>
    /// <remarks>
    /// A byte-order mark that survives decoding is a leading character of the string like any other: it breaks equality
    /// comparisons, prefix matching, and numeric parsing. This rule reports its presence at the start of the string only —
    /// a <c>U+FEFF</c> anywhere else is a zero-width no-break space, not a byte-order mark.
    /// </remarks>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the first character of <paramref name="value"/> is the byte-order mark; otherwise, <see langword="false"/>.</returns>
    public static bool HasByteOrderMark(string? value)
    {
        if (value is null || value.Length == 0)
            return false;

        return value[0] == ByteOrderMark;
    }

    /// <summary>
    /// Determines whether the specified string is well-formed UTF-16 — every surrogate code unit forms a complete
    /// high/low pair.
    /// </summary>
    /// <remarks>
    /// A <see cref="string"/> is a sequence of UTF-16 code units, and nothing stops one from holding a high surrogate
    /// with no low surrogate after it (or a low surrogate with no high surrogate before it). The usual cause is a string
    /// sliced through the middle of a surrogate pair. Such a string cannot be encoded to UTF-8, so it fails at the
    /// serialization boundary far from where it was created — this rule catches it at the input boundary instead.
    /// The empty string is well-formed; <see langword="null"/> is not a string and returns <see langword="false"/>.
    /// </remarks>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> contains no unpaired surrogate; otherwise, <see langword="false"/>.</returns>
    public static bool IsWellFormedUtf16(string? value)
    {
        if (value is null)
            return false;

        for (var i = 0; i < value.Length; i++)
        {
            var ch = value[i];

            if (char.IsLowSurrogate(ch))
                return false;

            if (!char.IsHighSurrogate(ch))
                continue;

            i++;

            if (i == value.Length || !char.IsLowSurrogate(value[i]))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified string is already in the given Unicode normalization form.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The same text can be spelled with different code points — <c>"é"</c> is either the precomposed
    /// <c>U+00E9</c> or an <c>"e"</c> followed by the combining acute accent <c>U+0301</c>. The two are visually
    /// identical but not ordinally equal, so unnormalized input silently breaks equality, uniqueness constraints
    /// and lookups. This rule reports whether the value is already in the requested form, so a caller can reject
    /// it rather than store two spellings of the same name.
    /// </para>
    /// <para>
    /// A value that is not well-formed UTF-16 returns <see langword="false"/> rather than propagating the
    /// <see cref="ArgumentException"/> the underlying framework raises: an unpaired surrogate has no normalized
    /// spelling, and rules do not throw on the validated value.
    /// </para>
    /// <para>
    /// Under globalization-invariant mode the framework treats every string as normalized, so this rule reports
    /// <see langword="true"/> for input it would reject on an ICU-backed runtime. The checks that do not depend
    /// on the mode — null, an undefined <paramref name="form"/>, and malformed UTF-16 — behave identically in
    /// both.
    /// </para>
    /// </remarks>
    /// <param name="value">The value to validate. If <see langword="null"/> or not well-formed UTF-16, returns <see langword="false"/>.</param>
    /// <param name="form">The <see cref="NormalizationForm"/> to test against. Defaults to <see cref="NormalizationForm.FormC"/>. An undefined form returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is already in <paramref name="form"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsNormalized(string? value, NormalizationForm form = NormalizationForm.FormC)
    {
        if (value is null)
            return false;

        if (!EnumRules.IsDefined<NormalizationForm>(form))
            return false;

        if (!IsWellFormedUtf16(value))
            return false;

        return value.IsNormalized(form);
    }

    private static bool AllCharsAreAllowed(string value, Func<char, bool> allowedCharPredicate,
        char[]? additionalAllowedChars)
    {
        if (value.Length == 0)
            return false;

        if (additionalAllowedChars is null || additionalAllowedChars.Length == 0)
        {
            foreach (var ch in value)
            {
                if (!allowedCharPredicate(ch))
                    return false;
            }

            return true;
        }

        foreach (var ch in value)
        {
            if (!allowedCharPredicate(ch) && Array.IndexOf(additionalAllowedChars, ch) < 0)
                return false;
        }

        return true;
    }
}
