using PineGuard.Rules;
using System.Buffers;

namespace PineGuard.Utils;

public static class BufferUtility
{
    public static bool TryParseHexString(string? value)
    {
        if (!StringUtility.TryGetNonEmptyTrimmed(value, out var trimmed))
            return false;

        return IsHexString(trimmed);
    }

    public static bool TryParseBase64String(string? value)
    {
        if (!StringUtility.TryGetNonEmptyTrimmed(value, out var trimmed))
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
    private static bool IsHexString(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
            return false;

        foreach (var ch in value)
        {
            if (!CharRules.IsHexDigit(ch))
                return false;
        }

        return true;
    }
}
