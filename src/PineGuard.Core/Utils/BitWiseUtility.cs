#if NET8_0_OR_GREATER
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PineGuard.Utils;

/// <summary>
/// Provides bitwise mask parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/bitwise">BitWise Utility documentation</seealso>
public static class BitWiseUtility
{
    /// <summary>
    /// Attempts to parse a non-negative bitmask from a string literal (decimal, <c>0x</c> hex, or <c>0b</c> binary).
    /// </summary>
    /// <typeparam name="T">The integer type. Must implement <see cref="System.Numerics.IBinaryInteger{T}"/>.</typeparam>
    /// <param name="mask">The mask string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="parsed">When this method returns, contains the parsed mask if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the mask was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseNonNegativeMask<T>(string? mask, out T parsed)
        where T : struct, IBinaryInteger<T>
    {
        parsed = default;

        if (string.IsNullOrWhiteSpace(mask))
            return false;

        var raw = mask.Trim();

        // Reject a digit separator at the very start/end of the literal, and any run of one or more separators
        // that splits the "0x"/"0b" prefix itself (e.g. "0_x1F", "0__xFF", "0___bFF"). A separator immediately
        // *after* the prefix (e.g. "0x_FF") is legal C# 7.2+ literal syntax and is intentionally still accepted.
        if (raw[0] == '_' || raw[^1] == '_')
            return false;

        if (raw[0] == '0')
        {
            var afterLeadingZero = 1;
            while (afterLeadingZero < raw.Length && raw[afterLeadingZero] == '_')
                afterLeadingZero++;

            // The trailing-underscore guard above already ensures raw's last character is never '_', so the loop
            // above cannot have advanced all the way to raw.Length: indexing raw[afterLeadingZero] here is safe
            // whenever the run of underscores was non-empty.
            if (afterLeadingZero > 1 && raw[afterLeadingZero] is 'x' or 'X' or 'b' or 'B')
                return false;
        }

        var trimmed = raw.Replace("_", string.Empty);

        if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return TryParseHexLiteral(trimmed[2..], out parsed);

        if (trimmed.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
            return TryParseBinaryLiteral(trimmed[2..], out parsed);

        return BigInteger.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var dec)
               && dec >= 0
               && TryCreateChecked(dec, out parsed);
    }

    private static bool TryParseHexLiteral<T>(string digits, out T parsed)
        where T : struct, IBinaryInteger<T>
    {
        parsed = default;

        if (digits.Length == 0)
            return false;

        // Prefix with 0 to force non-negative two's-complement interpretation.
        return BigInteger.TryParse("0" + digits, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var big)
               && TryCreateChecked(big, out parsed);
    }

    private static bool TryParseBinaryLiteral<T>(string bits, out T parsed)
        where T : struct, IBinaryInteger<T>
    {
        parsed = default;

        if (bits.Length == 0)
            return false;

        // Skip leading zeros before checking length: they contribute no bits to the result but a hostile or
        // accidental multi-megabyte "0...0" prefix should not be walked bit-by-bit through BigInteger below.
        var start = 0;
        while (start < bits.Length && bits[start] == '0')
            start++;

        // Beyond T's bit width, the value is guaranteed to overflow T.CreateChecked; fail fast instead of
        // performing an O(n^2) BigInteger shift-and-add over an unbounded number of significant bits.
        if (bits.Length - start > Unsafe.SizeOf<T>() * 8)
            return false;

        var big = BigInteger.Zero;
        for (var i = start; i < bits.Length; i++)
        {
            var c = bits[i];
            if (c is not ('0' or '1'))
                return false;

            big <<= 1;
            if (c == '1')
                big += BigInteger.One;
        }

        return TryCreateChecked(big, out parsed);
    }

    private static bool TryCreateChecked<T>(BigInteger value, out T result)
        where T : struct, IBinaryInteger<T>
    {
        result = default;

        try
        {
            result = T.CreateChecked(value);
            return true;
        }
        catch (OverflowException)
        {
            return false;
        }
    }
}
#endif
