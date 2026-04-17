#if NET8_0_OR_GREATER
using System.Numerics;
using System.Runtime.CompilerServices;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate bitwise operations on integer values,
/// delegating to <see cref="BitWiseRules"/> for core validation logic.
/// </summary>
/// <seealso cref="BitWiseRules"/>
/// <seealso href="https://pineguard.ai/docs/must/bitwise">Bitwise Must Clauses documentation</seealso>
public static class MustBitWiseClauses
{
    private const string InvalidMaskMessage = "{paramName} must be a valid bitwise mask.";

    /// <summary>
    /// Validates that the specified value must be bitwise equal to the expected value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be bitwise equal to the expected value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> BitwiseEqualTo<T>(this IMustClause _,
        T value,
        T other,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
        {
            const string equalTemplate = "{paramName} must be bitwise equal to the expected value.";
            return MustResult<T>.FromBool(value == other, equalTemplate, paramName, value, value);
        }

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must be bitwise equal to the expected value.";

        var ok = BitWiseRules.IsBitwiseEqualTo(value, other, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be bitwise equal to the expected value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be bitwise equal to the expected value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> NotBitwiseEqualTo<T>(this IMustClause _,
        T value,
        T other,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
        {
            const string notEqualTemplate = "{paramName} must not be bitwise equal to the expected value.";
            return MustResult<T>.FromBool(value != other, notEqualTemplate, paramName, value, value);
        }

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must not be bitwise equal to the expected value.";

        var ok = !BitWiseRules.IsBitwiseEqualTo(value, other, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must contain all required bits.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain all required bits."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> HasAllBits<T>(this IMustClause _,
        T value,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must contain all required bits.";

        var ok = BitWiseRules.HasAllBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must contain at least one required bit.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain at least one required bit."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> HasAnyBits<T>(this IMustClause _,
        T value,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must contain at least one required bit.";

        var ok = BitWiseRules.HasAnyBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must contain none of the forbidden bits.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain none of the forbidden bits."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> HasNoBits<T>(this IMustClause _,
        T value,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must contain none of the forbidden bits.";

        var ok = BitWiseRules.HasNoBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must contain only allowed bits.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="allowedMask">The allowed bitmask string.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain only allowed bits."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> HasOnlyBits<T>(this IMustClause _,
        T value,
        string? allowedMask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(allowedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(allowedMask), allowedMask);

        if (!BitWiseUtility.TryParseNonNegativeMask(allowedMask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(allowedMask), allowedMask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(allowedMask), allowedMask);

        const string messageTemplate = "{paramName} must contain only allowed bits.";

        var ok = BitWiseRules.HasOnlyBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be a power of two.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a power of two."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> PowerOfTwo<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        const string messageTemplate = "{paramName} must be a power of two.";

        var ok = BitWiseRules.IsPowerOfTwo<T>(value);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be a power of two.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a power of two."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> NotPowerOfTwo<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        const string messageTemplate = "{paramName} must not be a power of two.";

        var ok = !BitWiseRules.IsPowerOfTwo<T>(value);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not contain all required bits.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain all required bits."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> NotHasAllBits<T>(this IMustClause _,
        T value,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must not contain all required bits.";

        var ok = !BitWiseRules.HasAllBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not contain any of the specified bits.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain any of the specified bits."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> NotHasAnyBits<T>(this IMustClause _,
        T value,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must not contain any of the specified bits.";

        var ok = !BitWiseRules.HasAnyBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must contain at least one of the forbidden bits.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="mask">The bitmask string to apply.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain at least one of the forbidden bits."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> NotHasNoBits<T>(this IMustClause _,
        T value,
        string? mask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(mask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (!BitWiseUtility.TryParseNonNegativeMask(mask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(mask), mask);

        const string messageTemplate = "{paramName} must contain at least one of the forbidden bits.";

        var ok = !BitWiseRules.HasNoBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must contain bits not allowed by the mask.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="allowedMask">The allowed bitmask string.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain bits not allowed by the mask."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/bit-wise">Bit Wise Must Clauses documentation</seealso>
    public static MustResult<T> NotHasOnlyBits<T>(this IMustClause _,
        T value,
        string? allowedMask,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IBinaryInteger<T>
    {
        if (string.IsNullOrWhiteSpace(allowedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(allowedMask), allowedMask);

        if (!BitWiseUtility.TryParseNonNegativeMask(allowedMask, out T parsedMask))
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(allowedMask), allowedMask);

        if (parsedMask == T.Zero)
            return MustResult<T>.Fail(InvalidMaskMessage, nameof(allowedMask), allowedMask);

        const string messageTemplate = "{paramName} must contain bits not allowed by the mask.";

        var ok = !BitWiseRules.HasOnlyBits(value, parsedMask);
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }
}
#endif
