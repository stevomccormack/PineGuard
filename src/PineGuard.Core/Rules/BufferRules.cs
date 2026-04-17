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
    /// Determines whether the specified value is a valid hexadecimal string.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains only hex digits (0–9, a–f, A–F)
    /// with an even length; otherwise, <see langword="false"/>.
    /// </returns>
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
    /// <example>
    /// <code>
    /// bool valid = BufferRules.IsBase64("SGVsbG8="); // true
    /// bool invalid = BufferRules.IsBase64("not!b64"); // false
    /// </code>
    /// </example>
    public static bool IsBase64(string? value) =>
        BufferUtility.IsBase64String(value);
}
