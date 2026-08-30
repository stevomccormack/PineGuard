using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid HTTP header name.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustHttpClauses.HeaderName"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RequestModel
/// {
///     [HttpHeaderName]
///     public string CustomHeader { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HttpHeaderValueAttribute"/>
/// <seealso cref="MustHttpClauses.HeaderName"/>
/// <seealso href="https://pineguard.ai/docs/annotations/http">HTTP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HttpHeaderNameAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Http.HeaderName.Malformed)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.HeaderName(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid HTTP header value.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustHttpClauses.HeaderValue"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RequestModel
/// {
///     [HttpHeaderValue]
///     public string AcceptValue { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HttpHeaderNameAttribute"/>
/// <seealso cref="MustHttpClauses.HeaderValue"/>
/// <seealso href="https://pineguard.ai/docs/annotations/http">HTTP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HttpHeaderValueAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Http.HeaderValue.Malformed)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.HeaderValue(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field is a valid HTTP status code
/// (100–599).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustHttpClauses.HttpStatusCode"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ResponseModel
/// {
///     [HttpStatusCode]
///     public int StatusCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HttpStatusSuccessAttribute"/>
/// <seealso cref="MustHttpClauses.HttpStatusCode"/>
/// <seealso href="https://pineguard.ai/docs/annotations/http">HTTP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HttpStatusCodeAttribute() : ValidationAttributeBase(typeof(int), MustCodes.Http.Status.OutOfRange)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;

        var result = Must.Be.HttpStatusCode(intValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field is a successful HTTP status code
/// (200–299).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustHttpClauses.HttpStatusSuccess"/>. Supported on properties, fields, and
/// parameters of type <see cref="int"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ResponseModel
/// {
///     [HttpStatusSuccess]
///     public int StatusCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HttpStatusCodeAttribute"/>
/// <seealso cref="MustHttpClauses.HttpStatusSuccess"/>
/// <seealso href="https://pineguard.ai/docs/annotations/http">HTTP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HttpStatusSuccessAttribute() : ValidationAttributeBase(typeof(int), MustCodes.Http.Status.NotSuccess)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;

        var result = Must.Be.HttpStatusSuccess(intValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid media type.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustHttpClauses.MediaType"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The RFC 6838 <c>type/subtype</c> shape is required, with an optional <c>+suffix</c> and an optional
/// trailing parameter list. The parameters are accepted and ignored: the verdict is about the media type
/// the value leads with. If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RequestModel
/// {
///     [MediaType]
///     public string RequestedFormat { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustHttpClauses.MediaType"/>
/// <seealso href="https://pineguard.ai/docs/annotations/http">HTTP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class MediaTypeAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Http.MediaType.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.MediaType(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
