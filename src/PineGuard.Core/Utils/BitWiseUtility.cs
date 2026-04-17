#if NET8_0_OR_GREATER
using System.Globalization;
using System.Numerics;

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

        var trimmed = mask.Trim().Replace("_", string.Empty);

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

        var big = BigInteger.Zero;
        foreach (var c in bits)
        {
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
