#if !NET8_0_OR_GREATER
using System.Buffers;
#endif
using PineGuard.Rules;

namespace PineGuard.Utils;

/// <summary>
/// Provides buffer encoding validation utility methods (hex, Base64).
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/buffer">Buffer Utility documentation</seealso>
public static class BufferUtility
{
    /// <summary>
    /// Determines whether the specified string contains only hexadecimal digit characters.
    /// </summary>
    /// <param name="value">The string to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains only hex digits; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Leading and trailing whitespace is trimmed before validation; the original, untrimmed
    /// <paramref name="value"/> should not be assumed to be directly decodable.
    /// </remarks>
    public static bool IsHexString(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && trimmed.All(ch => CharRules.IsHexDigit(ch));

    /// <summary>
    /// Determines whether the specified string is a valid Base64-encoded string.
    /// </summary>
    /// <param name="value">The string to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value is valid Base64; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Leading and trailing whitespace is trimmed before validation, and any amount of whitespace embedded
    /// within the value is ignored (matching <see cref="Convert.TryFromBase64String(string, Span{byte}, out int)"/>
    /// semantics); the original, untrimmed <paramref name="value"/> should not be assumed to be directly decodable.
    /// </remarks>
    public static bool IsBase64String(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var significantLength = 0;
        foreach (var ch in trimmed)
        {
            if (!char.IsWhiteSpace(ch))
                significantLength++;
        }

        if (significantLength % BufferRules.Base64CharsPerQuantum != 0)
            return false;

#if NET8_0_OR_GREATER
        return System.Buffers.Text.Base64.IsValid(trimmed);
#else
        var bufferLength = significantLength / BufferRules.Base64CharsPerQuantum * BufferRules.Base64BytesPerQuantum;
        byte[]? rented = null;

        try
        {
            rented = ArrayPool<byte>.Shared.Rent(bufferLength);
            return Convert.TryFromBase64String(trimmed, rented, out _);
        }
        finally
        {
            if (rented is not null)
                ArrayPool<byte>.Shared.Return(rented);
        }
#endif
    }

    /// <summary>
    /// Determines whether the specified string is a valid Base64Url-encoded string
    /// (RFC 4648 §5, the URL- and filename-safe alphabet).
    /// </summary>
    /// <param name="value">The string to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value is valid Base64Url; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Trailing padding is optional, but a padded value must still be a whole number of quanta. Embedded
    /// whitespace is rejected rather than ignored, unlike <see cref="IsBase64String(string?)"/>. Leading and
    /// trailing whitespace is trimmed before validation.
    /// </remarks>
    public static bool IsBase64UrlString(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var padding = 0;
        while (padding < trimmed.Length && trimmed[trimmed.Length - 1 - padding] == BufferRules.Base64PaddingChar)
            padding++;

        if (padding > BufferRules.MaxBase64PaddingChars)
            return false;

        var dataLength = trimmed.Length - padding;

        if (dataLength == 0)
            return false;

        // Padding exists to complete a quantum, so a padded value that is not quantum-aligned is malformed.
        if (padding > 0 && trimmed.Length % BufferRules.Base64CharsPerQuantum != 0)
            return false;

        // A lone trailing character encodes no whole byte, so that remainder can never occur.
        if (dataLength % BufferRules.Base64CharsPerQuantum == 1)
            return false;

        for (var index = 0; index < dataLength; index++)
        {
            if (!IsBase64UrlChar(trimmed[index]))
                return false;
        }

        return true;
    }

    private static bool IsBase64UrlChar(char value) =>
        value is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or (>= '0' and <= '9') or '-' or '_';
}
