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

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid Base64Url-encoded string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBufferClauses.Base64Url"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The RFC 4648 §5 alphabet applies, so <c>-</c> and <c>_</c> take the place of <c>+</c> and <c>/</c> and a
/// value carrying either of the latter fails. Padding is optional, which is what makes this the right
/// attribute for a JWT segment or a URL-embedded identifier rather than <see cref="Base64Attribute"/>. If
/// the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TokenModel
/// {
///     [Base64Url]
///     public string Payload { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="Base64Attribute"/>
/// <seealso cref="MustBufferClauses.Base64Url"/>
/// <seealso href="https://pineguard.ai/docs/annotations/buffer">Buffer Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class Base64UrlAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Encoding.Base64url.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Base64Url(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <c>byte[]</c> property or field is well-formed UTF-8 text.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBufferClauses.Utf8"/>. Supported on properties, fields, and parameters of
/// type <c>byte[]</c> — unlike every other attribute in this file, which takes a <see cref="string"/>.
/// </para>
/// <para>
/// Overlong encodings, unpaired surrogates, truncated sequences and code points above U+10FFFF are all
/// rejected. An empty buffer is not the same thing as an absent one: it reaches the clause and fails,
/// because a buffer carrying no text is not well-formed UTF-8. If the value is <see langword="null"/>,
/// validation is skipped by the base class — express presence with <c>[Required]</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PayloadModel
/// {
///     [Utf8]
///     public byte[] RequestBody { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustBufferClauses.Utf8"/>
/// <seealso href="https://pineguard.ai/docs/annotations/buffer">Buffer Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class Utf8Attribute() : ValidationAttributeBase(typeof(byte[]), MustCodes.Encoding.Utf8.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var byteValue = (byte[])value!;

        var result = Must.Be.Utf8(byteValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
