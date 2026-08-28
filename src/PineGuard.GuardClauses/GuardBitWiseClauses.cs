#if NET8_0_OR_GREATER
using System.Numerics;
using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for bitwise integer operations.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/bitwise">Guard Bitwise Clauses documentation</seealso>
public static class GuardBitWiseClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is bitwise equal to <paramref name="other"/> under the specified <paramref name="mask"/>.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="mask">An optional bitmask to apply before comparison.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.NotBitwiseEqualTo{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is bitwise equal to <paramref name="other"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.NotBitwiseEqualTo{T}"/>:
    /// <c>Guard.Against.BitwiseEqualTo</c> passes when the masked values are not equal.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.BitwiseEqualTo(flags, expected, mask: "0xFF");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotBitwiseEqualTo{T}"/>
    public static T BitwiseEqualTo<T>(this IGuardClause _,
        T value,
        T other,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.NotBitwiseEqualTo(value, other, mask, paramName); // Guard.Against.BitwiseEqualTo => Must.Be.NotBitwiseEqualTo (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not bitwise equal to <paramref name="other"/> under the specified <paramref name="mask"/>.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="mask">An optional bitmask to apply before comparison.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.BitwiseEqualTo{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not bitwise equal to <paramref name="other"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.BitwiseEqualTo{T}"/>:
    /// <c>Guard.Against.NotBitwiseEqualTo</c> passes when the masked values are equal.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotBitwiseEqualTo(flags, expected, mask: "0xFF");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.BitwiseEqualTo{T}"/>
    public static T NotBitwiseEqualTo<T>(this IGuardClause _,
        T value,
        T other,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.BitwiseEqualTo(value, other, mask, paramName); // Guard.Against.NotBitwiseEqualTo => Must.Be.BitwiseEqualTo (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have all bits specified by <paramref name="mask"/> set.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="mask">The bitmask that all bits must be set in <paramref name="value"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.HasAllBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> does not have all mask bits set and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.HasAllBits{T}"/>:
    /// <c>Guard.Against.NotHasAllBits</c> passes when all mask bits are set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasAllBits(permissions, mask: "0x03");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasAllBits{T}"/>
    public static T NotHasAllBits<T>(this IGuardClause _,
        T value,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.HasAllBits(value, mask, paramName); // Guard.Against.NotHasAllBits => Must.Be.HasAllBits (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not have any bits specified by <paramref name="mask"/> set.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="mask">The bitmask of which at least one bit must be set in <paramref name="value"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.HasAnyBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> has none of the mask bits set and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.HasAnyBits{T}"/>:
    /// <c>Guard.Against.NotHasAnyBits</c> passes when at least one mask bit is set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasAnyBits(flags, mask: "0x0F");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasAnyBits{T}"/>
    public static T NotHasAnyBits<T>(this IGuardClause _,
        T value,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.HasAnyBits(value, mask, paramName); // Guard.Against.NotHasAnyBits => Must.Be.HasAnyBits (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has none of the bits specified by <paramref name="mask"/> set.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="mask">The bitmask that must have no bits set in <paramref name="value"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.HasNoBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> has no mask bits set and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.HasNoBits{T}"/>:
    /// <c>Guard.Against.NotHasNoBits</c> passes when all mask bits are clear (none are set).
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasNoBits(value, mask: "0xF0");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasNoBits{T}"/>
    public static T NotHasNoBits<T>(this IGuardClause _,
        T value,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.HasNoBits(value, mask, paramName); // Guard.Against.NotHasNoBits => Must.Be.HasNoBits (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains bits outside of those permitted by <paramref name="allowedMask"/>.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="allowedMask">The bitmask of allowed bits; any bit set outside this mask causes a throw.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.HasOnlyBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains bits outside the allowed mask and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.HasOnlyBits{T}"/>:
    /// <c>Guard.Against.NotHasOnlyBits</c> passes when only allowed bits are set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasOnlyBits(flags, allowedMask: "0x07");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasOnlyBits{T}"/>
    public static T NotHasOnlyBits<T>(this IGuardClause _,
        T value,
        string? allowedMask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.HasOnlyBits(value, allowedMask, paramName); // Guard.Against.NotHasOnlyBits => Must.Be.HasOnlyBits (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a power of two.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.PowerOfTwo{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a power of two and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.PowerOfTwo{T}"/>:
    /// <c>Guard.Against.NotPowerOfTwo</c> passes when the value is a power of two.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotPowerOfTwo(bufferSize);
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.PowerOfTwo{T}"/>
    public static T NotPowerOfTwo<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.PowerOfTwo(value, paramName); // Guard.Against.NotPowerOfTwo => Must.Be.PowerOfTwo (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is a power of two.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.NotPowerOfTwo{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is a power of two and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.NotPowerOfTwo{T}"/>:
    /// <c>Guard.Against.PowerOfTwo</c> passes when the value is not a power of two.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.PowerOfTwo(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotPowerOfTwo{T}"/>
    public static T PowerOfTwo<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.NotPowerOfTwo(value, paramName); // Guard.Against.PowerOfTwo => Must.Be.NotPowerOfTwo (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has any bits specified by <paramref name="mask"/> set.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="mask">The bitmask that must have all bits set in <paramref name="value"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.NotHasAllBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> has all mask bits set and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.NotHasAllBits{T}"/>:
    /// <c>Guard.Against.HasAllBits</c> passes when not all mask bits are set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasAllBits(flags, mask: "0xFF");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasAllBits{T}"/>
    public static T HasAllBits<T>(this IGuardClause _,
        T value,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.NotHasAllBits(value, mask, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has any bits specified by <paramref name="mask"/> set.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="mask">The bitmask of which no bit may be set in <paramref name="value"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.NotHasAnyBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> has at least one mask bit set and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.NotHasAnyBits{T}"/>:
    /// <c>Guard.Against.HasAnyBits</c> passes when no mask bits are set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasAnyBits(flags, mask: "0x80");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasAnyBits{T}"/>
    public static T HasAnyBits<T>(this IGuardClause _,
        T value,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.NotHasAnyBits(value, mask, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has all bits specified by <paramref name="mask"/> clear.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="mask">The bitmask whose bits must all be clear in <paramref name="value"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.NotHasNoBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> has no mask bits set and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.NotHasNoBits{T}"/>:
    /// <c>Guard.Against.HasNoBits</c> passes when at least one mask bit is set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasNoBits(permissions, mask: "0x01");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasNoBits{T}"/>
    public static T HasNoBits<T>(this IGuardClause _,
        T value,
        string? mask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.NotHasNoBits(value, mask, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains only bits within the <paramref name="allowedMask"/>.
    /// </summary>
    /// <typeparam name="T">A binary integer type implementing <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The integer value to guard.</param>
    /// <param name="allowedMask">The bitmask of permitted bits.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBitWiseClauses.NotHasOnlyBits{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains only bits within the allowed mask and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBitWiseClauses.NotHasOnlyBits{T}"/>:
    /// <c>Guard.Against.HasOnlyBits</c> passes when bits outside the mask are set.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasOnlyBits(flags, allowedMask: "0x0F");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasOnlyBits{T}"/>
    public static T HasOnlyBits<T>(this IGuardClause _,
        T value,
        string? allowedMask,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        var result = Must.Be.NotHasOnlyBits(value, allowedMask, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
#endif
