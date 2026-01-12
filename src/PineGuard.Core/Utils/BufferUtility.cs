using PineGuard.Rules;
using System.Buffers;

namespace PineGuard.Utils;

public static class BufferUtility
{
    public static bool IsHexString(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return trimmed.All(CharRules.IsHexDigit);
    }

    public static bool IsBase64String(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (trimmed.Length % BufferRules.Base64CharsPerQuantum != 0)
            return false;

        var bufferLength = (trimmed.Length / BufferRules.Base64CharsPerQuantum) * BufferRules.Base64BytesPerQuantum;
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
