using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure binary-encoding validation predicates for hexadecimal and Base64 strings.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/buffer">Buffer Rules documentation</seealso>
public static class BufferRules
{
    /// <summary>
    /// The number of Base64-encoded characters per 3-byte quantum (4 characters encode 3 bytes).
    /// </summary>
    public const int Base64CharsPerQuantum = 4;

    /// <summary>
    /// The number of raw bytes per Base64 quantum (3 bytes are encoded as 4 characters).
    /// </summary>
    public const int Base64BytesPerQuantum = 3;

    /// <summary>
    /// The character that pads a Base64 value out to a whole quantum (<c>'='</c>).
    /// </summary>
    public const char Base64PaddingChar = '=';

    /// <summary>
    /// The greatest number of <see cref="Base64PaddingChar"/> characters a Base64 value can end with.
    /// </summary>
    public const int MaxBase64PaddingChars = 2;

    /// <summary>
    /// Determines whether the specified value is a valid hexadecimal string.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains only hex digits (0–9, a–f, A–F);
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Leading and trailing whitespace is trimmed before validation; the original, untrimmed
    /// <paramref name="value"/> should not be assumed to be directly decodable.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = BufferRules.IsHex("deadbeef"); // true
    /// bool invalid = BufferRules.IsHex("xyz");    // false
    /// </code>
    /// </example>
    public static bool IsHex(string? value) =>
        BufferUtility.IsHexString(value);

    /// <summary>
    /// Determines whether the specified value is a valid Base64-encoded string.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid Base64 string; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Leading and trailing whitespace is trimmed before validation, and any amount of whitespace embedded
    /// within the value is ignored; the original, untrimmed <paramref name="value"/> should not be assumed
    /// to be directly decodable.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = BufferRules.IsBase64("SGVsbG8="); // true
    /// bool invalid = BufferRules.IsBase64("not!b64"); // false
    /// </code>
    /// </example>
    public static bool IsBase64(string? value) =>
        BufferUtility.IsBase64String(value);

    /// <summary>
    /// Determines whether the specified value is a valid Base64Url-encoded string
    /// (RFC 4648 §5, the URL- and filename-safe alphabet).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid Base64Url string; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// The alphabet substitutes <c>-</c> and <c>_</c> for Base64's <c>+</c> and <c>/</c>, so a value carrying
    /// either of the latter is rejected. Trailing <see cref="Base64PaddingChar"/> padding is optional — the form
    /// used in JSON Web Tokens omits it — but when present the value must still be a whole number of quanta.
    /// Unlike <see cref="IsBase64(string?)"/>, embedded whitespace is not tolerated: a Base64Url value is meant
    /// to survive a URL or a token segment unaltered. Leading and trailing whitespace is trimmed before validation.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = BufferRules.IsBase64Url("SGVsbG8_d29ybGQ");  // true
    /// bool invalid = BufferRules.IsBase64Url("SGVsbG8/d29ybGQ"); // false ('/' is Base64, not Base64Url)
    /// </code>
    /// </example>
    public static bool IsBase64Url(string? value) =>
        BufferUtility.IsBase64UrlString(value);

    /// <summary>
    /// Determines whether the specified bytes are well-formed UTF-8 text.
    /// </summary>
    /// <param name="value">The bytes to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> decodes as UTF-8 without substitution;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Overlong encodings, unpaired surrogates, truncated sequences and code points above U+10FFFF are all
    /// rejected, so a value that passes can be decoded without silently becoming U+FFFD replacement characters.
    /// An empty buffer carries no text and is reported as invalid, consistent with how the other members of this
    /// class treat an empty value.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = BufferRules.IsUtf8([0xE2, 0x82, 0xAC]);   // true (U+20AC EURO SIGN)
    /// bool invalid = BufferRules.IsUtf8([0xC0, 0x80]);       // false (overlong encoding of U+0000)
    /// </code>
    /// </example>
    public static bool IsUtf8(byte[]? value)
    {
        if (value is null || value.Length == 0)
            return false;

#if NET8_0_OR_GREATER
        return System.Text.Unicode.Utf8.IsValid(value);
#else
        return BufferUtility.TryDecodeUtf8(value, out _);
#endif
    }
}
