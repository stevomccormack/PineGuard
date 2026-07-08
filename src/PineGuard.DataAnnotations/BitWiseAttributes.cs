#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field is bitwise equal to a specified value.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.BitwiseEqualTo"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// The comparison is performed against <see cref="Value"/> with no mask applied.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FlagsModel
/// {
///     [BitwiseEqualTo(0b0011)]
///     public int Permissions { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotBitwiseEqualToAttribute"/>
/// <seealso cref="MustBitWiseClauses.BitwiseEqualTo"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BitwiseEqualToAttribute(int value) : ValidationAttributeBase(typeof(int))
{
    /// <summary>Gets the expected bitwise value to compare against.</summary>
    public int Value { get; } = value;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.BitwiseEqualTo(intValue, Value, mask: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field is not bitwise equal to a specified value.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.NotBitwiseEqualTo"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// The comparison is performed against <see cref="Value"/> with no mask applied.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FlagsModel
/// {
///     [NotBitwiseEqualTo(0b1111)]
///     public int Permissions { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BitwiseEqualToAttribute"/>
/// <seealso cref="MustBitWiseClauses.NotBitwiseEqualTo"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBitwiseEqualToAttribute(int value) : ValidationAttributeBase(typeof(int))
{
    /// <summary>Gets the bitwise value that the annotated field must not equal.</summary>
    public int Value { get; } = value;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.NotBitwiseEqualTo(intValue, Value, mask: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field has all bits set in the specified mask.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.HasAllBits"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// The check passes when <c>(value &amp; Mask) == Mask</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FlagsModel
/// {
///     [HasAllBits(0b0011)]
///     public int Permissions { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasAnyBitsAttribute"/>
/// <seealso cref="HasNoBitsAttribute"/>
/// <seealso cref="MustBitWiseClauses.HasAllBits"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasAllBitsAttribute(int mask) : ValidationAttributeBase(typeof(int))
{
    /// <summary>Gets the bitmask that all bits must be set in the annotated value.</summary>
    public int Mask { get; } = mask;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.HasAllBits(intValue, Mask.ToString(CultureInfo.InvariantCulture), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field has at least one bit set in the specified mask.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.HasAnyBits"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// The check passes when <c>(value &amp; Mask) != 0</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FlagsModel
/// {
///     [HasAnyBits(0b1100)]
///     public int Permissions { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasAllBitsAttribute"/>
/// <seealso cref="HasNoBitsAttribute"/>
/// <seealso cref="MustBitWiseClauses.HasAnyBits"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasAnyBitsAttribute(int mask) : ValidationAttributeBase(typeof(int))
{
    /// <summary>Gets the bitmask of which at least one bit must be set in the annotated value.</summary>
    public int Mask { get; } = mask;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.HasAnyBits(intValue, Mask.ToString(CultureInfo.InvariantCulture), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field has none of the bits set in the specified mask.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.HasNoBits"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// The check passes when <c>(value &amp; Mask) == 0</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FlagsModel
/// {
///     [HasNoBits(0b1100)]
///     public int Permissions { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasAllBitsAttribute"/>
/// <seealso cref="HasAnyBitsAttribute"/>
/// <seealso cref="MustBitWiseClauses.HasNoBits"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasNoBitsAttribute(int mask) : ValidationAttributeBase(typeof(int))
{
    /// <summary>Gets the bitmask of which no bits must be set in the annotated value.</summary>
    public int Mask { get; } = mask;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.HasNoBits(intValue, Mask.ToString(CultureInfo.InvariantCulture), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field has only the bits specified in the mask set,
/// and no others.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.HasOnlyBits"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// The check passes when <c>(value &amp; ~Mask) == 0</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FlagsModel
/// {
///     [HasOnlyBits(0b0011)]
///     public int Permissions { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasAllBitsAttribute"/>
/// <seealso cref="HasNoBitsAttribute"/>
/// <seealso cref="MustBitWiseClauses.HasOnlyBits"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasOnlyBitsAttribute(int mask) : ValidationAttributeBase(typeof(int))
{
    /// <summary>Gets the bitmask that represents all permitted bits in the annotated value.</summary>
    public int Mask { get; } = mask;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.HasOnlyBits(intValue, Mask.ToString(CultureInfo.InvariantCulture), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field is a power of two.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.PowerOfTwo"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// Values such as 1, 2, 4, 8, 16, etc. are valid. Zero and negative values are not valid powers of two.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BufferModel
/// {
///     [PowerOfTwo]
///     public int BlockSize { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotPowerOfTwoAttribute"/>
/// <seealso cref="MustBitWiseClauses.PowerOfTwo"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PowerOfTwoAttribute() : ValidationAttributeBase(typeof(int))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.PowerOfTwo(intValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field is not a power of two.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBitWiseClauses.NotPowerOfTwo"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// <para>
/// Zero and negative values are considered not-power-of-two.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ConfigModel
/// {
///     [NotPowerOfTwo]
///     public int ChunkSize { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PowerOfTwoAttribute"/>
/// <seealso cref="MustBitWiseClauses.NotPowerOfTwo"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bitwise">Bitwise Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotPowerOfTwoAttribute() : ValidationAttributeBase(typeof(int))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.NotPowerOfTwo(intValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
