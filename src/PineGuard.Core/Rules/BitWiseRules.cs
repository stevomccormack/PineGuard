#if NET8_0_OR_GREATER
using System.Numerics;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure bitwise validation predicates for any <see cref="IBinaryInteger{TSelf}"/> type.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/bitwise">Bitwise Rules documentation</seealso>
public static class BitWiseRules
{
    /// <summary>
    /// Determines whether the specified values are equal when masked with the given bitmask.
    /// </summary>
    /// <typeparam name="T">A binary integer type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="left">The first value to compare. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="right">The second value to compare. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="mask">The bitmask to apply before comparison. If zero, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <c>(left &amp; mask) == (right &amp; mask)</c>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool equal = BitWiseRules.IsBitwiseEqualTo(0b1100, 0b1110, 0b1100); // true (masked bits match)
    /// </code>
    /// </example>
    public static bool IsBitwiseEqualTo<T>(T? left, T? right, T mask)
        where T : struct, IBinaryInteger<T>
    {
        if (!left.HasValue || !right.HasValue)
            return false;

        if (mask == T.Zero)
            return false;

        return (left.Value & mask) == (right.Value & mask);
    }

    /// <summary>
    /// Determines whether the specified value has all bits set that are set in the given mask.
    /// </summary>
    /// <typeparam name="T">A binary integer type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="mask">The bitmask whose bits must all be set. If zero, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <c>(value &amp; mask) == mask</c>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool allSet = BitWiseRules.HasAllBits(0b1111, 0b1010); // true
    /// bool missing = BitWiseRules.HasAllBits(0b1000, 0b1010); // false
    /// </code>
    /// </example>
    public static bool HasAllBits<T>(T? value, T mask) where T : struct, IBinaryInteger<T>
    {
        if (!value.HasValue)
            return false;

        if (mask == T.Zero)
            return false;

        return (value.Value & mask) == mask;
    }

    /// <summary>
    /// Determines whether the specified value has any bits set that are set in the given mask.
    /// </summary>
    /// <typeparam name="T">A binary integer type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="mask">The bitmask to test against. If zero, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <c>(value &amp; mask) != 0</c>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool anySet = BitWiseRules.HasAnyBits(0b1000, 0b1010); // true
    /// bool noneSet = BitWiseRules.HasAnyBits(0b0100, 0b1010); // false
    /// </code>
    /// </example>
    public static bool HasAnyBits<T>(T? value, T mask) where T : struct, IBinaryInteger<T>
    {
        if (!value.HasValue)
            return false;

        if (mask == T.Zero)
            return false;

        return (value.Value & mask) != T.Zero;
    }

    /// <summary>
    /// Determines whether the specified value has none of the bits set in the given mask.
    /// </summary>
    /// <typeparam name="T">A binary integer type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="mask">The bitmask to test against. If zero, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <c>(value &amp; mask) == 0</c>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool noBits = BitWiseRules.HasNoBits(0b0100, 0b1010); // true
    /// bool hasBits = BitWiseRules.HasNoBits(0b1100, 0b1010); // false
    /// </code>
    /// </example>
    public static bool HasNoBits<T>(T? value, T mask) where T : struct, IBinaryInteger<T>
    {
        if (!value.HasValue)
            return false;

        if (mask == T.Zero)
            return false;

        return (value.Value & mask) == T.Zero;
    }

    /// <summary>
    /// Determines whether the specified value has no bits set outside the allowed mask.
    /// </summary>
    /// <typeparam name="T">A binary integer type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="allowedMask">The bitmask of allowed bit positions. If zero, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <c>(value &amp; ~allowedMask) == 0</c>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool onlyAllowed = BitWiseRules.HasOnlyBits(0b1010, 0b1110); // true
    /// bool extra = BitWiseRules.HasOnlyBits(0b1010, 0b0010);       // false
    /// </code>
    /// </example>
    public static bool HasOnlyBits<T>(T? value, T allowedMask) where T : struct, IBinaryInteger<T>
    {
        if (!value.HasValue)
            return false;

        if (allowedMask == T.Zero)
            return false;

        return (value.Value & ~allowedMask) == T.Zero;
    }

    /// <summary>
    /// Determines whether the specified value is a power of two.
    /// </summary>
    /// <typeparam name="T">A binary integer type that implements <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, zero, or negative, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a positive power of two (1, 2, 4, 8, ...);
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool isPow2 = BitWiseRules.IsPowerOfTwo(8);  // true
    /// bool notPow2 = BitWiseRules.IsPowerOfTwo(6); // false
    /// </code>
    /// </example>
    public static bool IsPowerOfTwo<T>(T? value)
        where T : struct, IBinaryInteger<T>
    {
        if (!value.HasValue)
            return false;

        var val = value.Value;

        if (val <= T.Zero)
            return false;

        return (val & (val - T.One)) == T.Zero;
    }
}
#endif
