using System.Buffers;
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
    public static bool IsHexString(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && trimmed.All(ch => CharRules.IsHexDigit(ch));

    /// <summary>
    /// Determines whether the specified string is a valid Base64-encoded string.
    /// </summary>
    /// <param name="value">The string to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value is valid Base64; otherwise, <see langword="false"/>.</returns>
    public static bool IsBase64String(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (trimmed.Length % BufferRules.Base64CharsPerQuantum != 0)
            return false;

        var bufferLength = trimmed.Length / BufferRules.Base64CharsPerQuantum * BufferRules.Base64BytesPerQuantum;
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
    }
}
