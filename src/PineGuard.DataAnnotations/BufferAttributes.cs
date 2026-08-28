using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid hexadecimal string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBufferClauses.Hex"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The value must contain only characters in the range <c>0–9</c> and <c>A–F</c> (case-insensitive).
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DataModel
/// {
///     [Hex]
///     public string Hash { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHexAttribute"/>
/// <seealso cref="MustBufferClauses.Hex"/>
/// <seealso href="https://pineguard.ai/docs/annotations/buffer">Buffer Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HexAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Encoding.Hex.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Hex(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid Base64-encoded string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBufferClauses.Base64"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DataModel
/// {
///     [Base64]
///     public string EncodedPayload { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotBase64Attribute"/>
/// <seealso cref="MustBufferClauses.Base64"/>
/// <seealso href="https://pineguard.ai/docs/annotations/buffer">Buffer Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class Base64Attribute() : ValidationAttributeBase(typeof(string), MustCodes.Encoding.Base64.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Base64(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not a valid hexadecimal string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBufferClauses.NotHex"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DataModel
/// {
///     [NotHex]
///     public string DisplayLabel { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HexAttribute"/>
/// <seealso cref="MustBufferClauses.NotHex"/>
/// <seealso href="https://pineguard.ai/docs/annotations/buffer">Buffer Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHexAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Encoding.Hex.WellFormed)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.NotHex(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not a valid Base64-encoded string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBufferClauses.NotBase64"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DataModel
/// {
///     [NotBase64]
///     public string PlainPayload { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="Base64Attribute"/>
/// <seealso cref="MustBufferClauses.NotBase64"/>
/// <seealso href="https://pineguard.ai/docs/annotations/buffer">Buffer Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBase64Attribute() : ValidationAttributeBase(typeof(string), MustCodes.Encoding.Base64.WellFormed)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.NotBase64(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
