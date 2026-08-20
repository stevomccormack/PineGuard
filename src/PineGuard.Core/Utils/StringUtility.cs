using System.Globalization;
using PineGuard.Rules;

namespace PineGuard.Utils;

/// <summary>
/// Provides string parsing and transformation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/string">String Utility documentation</seealso>
public static partial class StringUtility
{
    /// <summary>
    /// Attempts to trim the specified string, returning <see langword="false"/> if the value is <see langword="null"/> or whitespace.
    /// </summary>
    /// <param name="value">The string to trim. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="trimmed">When this method returns, contains the trimmed string if successful; otherwise, <see cref="string.Empty"/>.</param>
    /// <returns><see langword="true"/> if the value was successfully trimmed; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetTrimmed(string? value, out string trimmed)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            trimmed = string.Empty;
            return false;
        }

        trimmed = value.Trim();
        return true;
    }

    /// <summary>
    /// Attempts to extract only digit characters from the specified string, rejecting any non-digit characters.
    /// </summary>
    /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="digitsOnly">When this method returns, contains the digit-only string if successful; otherwise, <see cref="string.Empty"/>.</param>
    /// <returns><see langword="true"/> if the value contains only digits; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseDigitsOnly(string? value, out string digitsOnly)
        => TryParseDigits(value, out digitsOnly, allowedNonDigitChars: []);

    /// <summary>
    /// Attempts to extract digit characters from the specified string, stripping allowed non-digit separator characters.
    /// </summary>
    /// <param name="value">The string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="digits">When this method returns, contains the extracted digits if successful; otherwise, <see cref="string.Empty"/>.</param>
    /// <param name="allowedNonDigitChars">The set of non-digit characters to allow and strip. Defaults to the standard digit separators.</param>
    /// <returns><see langword="true"/> if digits were successfully extracted; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseDigits(string? value, out string digits, char[]? allowedNonDigitChars = null)
    {
        digits = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        allowedNonDigitChars ??= StringRules.NumberTypes.DefaultAllowedDigitSeparators;

        var trimmed = value.Trim();

        if (allowedNonDigitChars.Length == 0)
        {
            if (!trimmed.All(ch => ch is >= '0' and <= '9'))
                return false;

            digits = trimmed;
            return true;
        }

        var allowed = new HashSet<char>(allowedNonDigitChars);

        const int maxStackAllocLength = 256;
        Span<char> buffer = trimmed.Length <= maxStackAllocLength
            ? stackalloc char[maxStackAllocLength]
            : new char[trimmed.Length];
        var written = 0;

        foreach (var ch in trimmed)
        {
            if (ch is >= '0' and <= '9')
            {
                buffer[written++] = ch;
                continue;
            }

            if (allowed.Contains(ch))
                continue;

            return false;
        }

        if (written == 0)
            return false;

        digits = new string(buffer[..written]);
        return true;
    }

    /// <summary>
    /// Converts the specified string to title case using invariant culture rules. Unlike a plain predicate,
    /// this overload performs the actual transformation and hands the result back through <paramref name="titleCased"/>.
    /// </summary>
    /// <param name="value">The string to convert. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="titleCased">When this method returns, contains the title-cased string if successful; otherwise, <see cref="string.Empty"/>.</param>
    /// <returns><see langword="true"/> if the conversion succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TitleCase(string? value, out string titleCased)
    {
        titleCased = string.Empty;

        if (value is null)
            return false;

        if (!TryGetTrimmed(value, out var trimmed))
            return false;

        var textInfo = CultureInfo.InvariantCulture.TextInfo;
        titleCased = textInfo.ToTitleCase(trimmed.ToLowerInvariant());

        return titleCased.Length != 0;
    }

    /// <summary>
    /// Determines whether the specified string can be converted to title case. This is a convertibility check, not a
    /// "is this string already title case" check — it returns <see langword="true"/> for any non-null, non-whitespace
    /// <paramref name="value"/>, regardless of its current casing. Use the <see cref="TitleCase(string?, out string)"/>
    /// overload to obtain the actual title-cased result.
    /// </summary>
    /// <param name="value">The string to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the conversion would succeed; otherwise, <see langword="false"/>.</returns>
    public static bool TitleCase(string? value)
        => TitleCase(value, out _);

    private delegate bool TryParseDelegate<T>(string value, out T parsed);

    private delegate bool TryCreateRangeDelegate<in T, TRange>(T start, T end, out TRange range);

    private static bool TryParseRange<T, TRange>(
        string? start,
        string? end,
        TryParseDelegate<T> tryParseValue,
        TryCreateRangeDelegate<T, TRange> tryCreateRange,
        out TRange? range)
        where TRange : struct
    {
        range = null;

        if (!TryGetTrimmed(start, out var startTrimmed))
            return false;

        if (!TryGetTrimmed(end, out var endTrimmed))
            return false;

        if (!tryParseValue(startTrimmed, out var s))
            return false;

        if (!tryParseValue(endTrimmed, out var e))
            return false;

        if (!tryCreateRange(s, e, out var created))
            return false;

        range = created;
        return true;
    }
}
