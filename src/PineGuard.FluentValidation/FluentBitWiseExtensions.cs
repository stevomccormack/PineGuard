#if NET8_0_OR_GREATER
using System.Numerics;
using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for bitwise integer validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/bitwise">Fluent Bitwise Extensions documentation</seealso>
public static class FluentBitWiseExtensions
{
    /// <summary>
    /// Validates that the property value is bitwise equal to the specified value, optionally masked.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="mask">An optional bitmask pattern to apply before comparison, or <see langword="null"/> to compare the full value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.BitwiseEqualTo"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Flags).BitwiseEqualTo(0b1010, "0b1111");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.BitwiseEqualTo"/>
    public static IRuleBuilderOptions<TModel, T?> BitwiseEqualTo<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T other,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.BitwiseEqualTo(val.Value, other, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value is not bitwise equal to the specified value, optionally masked.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="mask">An optional bitmask pattern to apply before comparison, or <see langword="null"/> to compare the full value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.NotBitwiseEqualTo"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Flags).NotBitwiseEqualTo(0b1010, "0b1111");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotBitwiseEqualTo"/>
    public static IRuleBuilderOptions<TModel, T?> NotBitwiseEqualTo<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T other,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotBitwiseEqualTo(val.Value, other, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value has all bits set in the specified mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="mask">A bitmask pattern specifying the bits that must all be set, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.HasAllBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).HasAllBits("0b0011");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasAllBits"/>
    public static IRuleBuilderOptions<TModel, T?> HasAllBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasAllBits(val.Value, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value does not have all bits set in the specified mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="mask">A bitmask pattern specifying the bits that must not all be set, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.NotHasAllBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).NotHasAllBits("0b1100");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasAllBits"/>
    public static IRuleBuilderOptions<TModel, T?> NotHasAllBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasAllBits(val.Value, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value has at least one bit set in the specified mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="mask">A bitmask pattern specifying the bits to check, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.HasAnyBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Flags).HasAnyBits("0b1000");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasAnyBits"/>
    public static IRuleBuilderOptions<TModel, T?> HasAnyBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasAnyBits(val.Value, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value has no bits set in the specified mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="mask">A bitmask pattern specifying the bits that must not be set, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.NotHasAnyBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Flags).NotHasAnyBits("0b0110");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasAnyBits"/>
    public static IRuleBuilderOptions<TModel, T?> NotHasAnyBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasAnyBits(val.Value, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value has none of the bits set in the specified mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="mask">A bitmask pattern specifying the bits that must all be clear, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.HasNoBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Flags).HasNoBits("0b1111");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasNoBits"/>
    public static IRuleBuilderOptions<TModel, T?> HasNoBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasNoBits(val.Value, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value has at least one bit set from the specified mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="mask">A bitmask pattern specifying the bits that must not all be clear, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.NotHasNoBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Flags).NotHasNoBits("0b0001");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasNoBits"/>
    public static IRuleBuilderOptions<TModel, T?> NotHasNoBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? mask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasNoBits(val.Value, mask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value has only bits that are in the specified allowed mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowedMask">A bitmask pattern defining the only bits that may be set, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.HasOnlyBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).HasOnlyBits("0b0111");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.HasOnlyBits"/>
    public static IRuleBuilderOptions<TModel, T?> HasOnlyBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? allowedMask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.HasOnlyBits(val.Value, allowedMask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value has bits set outside the specified allowed mask.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowedMask">A bitmask pattern defining the allowed bits, or <see langword="null"/> to skip validation.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.NotHasOnlyBits"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Permissions).NotHasOnlyBits("0b0001");
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotHasOnlyBits"/>
    public static IRuleBuilderOptions<TModel, T?> NotHasOnlyBits<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        string? allowedMask,
        string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotHasOnlyBits(val.Value, allowedMask, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value is a power of two.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.PowerOfTwo"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.BufferSize).PowerOfTwo();
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.PowerOfTwo"/>
    public static IRuleBuilderOptions<TModel, T?> PowerOfTwo<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.PowerOfTwo(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message);

    /// <summary>
    /// Validates that the property value is not a power of two.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The binary integer type, which must implement <see cref="IBinaryInteger{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBitWiseClauses.NotPowerOfTwo"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ChunkSize).NotPowerOfTwo();
    /// </code>
    /// </example>
    /// <seealso cref="MustBitWiseClauses.NotPowerOfTwo"/>
    public static IRuleBuilderOptions<TModel, T?> NotPowerOfTwo<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, IBinaryInteger<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotPowerOfTwo(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message);
}
#endif
