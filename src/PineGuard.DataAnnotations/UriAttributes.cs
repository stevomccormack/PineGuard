using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid absolute URI.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.AbsoluteUri"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LinkModel
/// {
///     [AbsoluteUri]
///     public string Endpoint { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="RelativeUriAttribute"/>
/// <seealso cref="MustUriClauses.AbsoluteUri"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AbsoluteUriAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.Form.NotAbsolute)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.AbsoluteUri(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid relative URI.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.RelativeUri"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LinkModel
/// {
///     [RelativeUri]
///     public string Path { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AbsoluteUriAttribute"/>
/// <seealso cref="MustUriClauses.RelativeUri"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class RelativeUriAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.Form.NotRelative)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.RelativeUri(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

// Named WebUrl to avoid colliding with framework-native [Url].
/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid HTTP or HTTPS web URL.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.Url"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// Named <c>[WebUrl]</c> to avoid collision with the framework-native <c>[Url]</c> attribute.
/// For HTTPS-only URLs, use <see cref="HttpsUrlAttribute"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LinkModel
/// {
///     [WebUrl]
///     public string Website { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HttpsUrlAttribute"/>
/// <seealso cref="HttpUrlAttribute"/>
/// <seealso cref="MustUriClauses.Url"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WebUrlAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.Form.NotUrl)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Url(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid HTTPS URL.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.HttpsUrl"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SecureModel
/// {
///     [HttpsUrl]
///     public string CallbackUrl { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="WebUrlAttribute"/>
/// <seealso cref="HttpUrlAttribute"/>
/// <seealso cref="MustUriClauses.HttpsUrl"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HttpsUrlAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.Scheme.NotHttps)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.HttpsUrl(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid HTTP URL
/// (plain HTTP scheme only).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.HttpUrl"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InternalModel
/// {
///     [HttpUrl]
///     public string InternalEndpoint { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HttpsUrlAttribute"/>
/// <seealso cref="WebUrlAttribute"/>
/// <seealso cref="MustUriClauses.HttpUrl"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HttpUrlAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.Scheme.NotHttp)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.HttpUrl(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid <c>file://</c> URI.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.FileUri"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FileModel
/// {
///     [FileUri]
///     public string FileReference { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustUriClauses.FileUri"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FileUriAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.Scheme.NotFile)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.FileUri(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid file system path.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.FilePath"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FileModel
/// {
///     [FilePath]
///     public string OutputPath { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotFilePathAttribute"/>
/// <seealso cref="MustUriClauses.FilePath"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FilePathAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.FilePath.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.FilePath(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not a file system path.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.NotFilePath"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [NotFilePath]
///     public string UserInput { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FilePathAttribute"/>
/// <seealso cref="MustUriClauses.NotFilePath"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotFilePathAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Uri.FilePath.WellFormed)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotFilePath(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a URI with the specified scheme.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.HasScheme"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LinkModel
/// {
///     [HasScheme("ftp")]
///     public string FtpUrl { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHasSchemeAttribute"/>
/// <seealso cref="MustUriClauses.HasScheme"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasSchemeAttribute(string scheme) : ValidationAttributeBase(typeof(string), MustCodes.Uri.Scheme.Mismatch)
{
    /// <summary>Gets the URI scheme to match (e.g., <c>"https"</c>, <c>"ftp"</c>).</summary>
    public string Scheme { get; } = scheme;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.HasScheme(strValue, Scheme, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a URI that does not use the
/// specified scheme.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustUriClauses.NotHasScheme"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SecureModel
/// {
///     [NotHasScheme("http")]
///     public string Endpoint { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasSchemeAttribute"/>
/// <seealso cref="MustUriClauses.NotHasScheme"/>
/// <seealso href="https://pineguard.ai/docs/annotations/uri">URI Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHasSchemeAttribute(string scheme) : ValidationAttributeBase(typeof(string), MustCodes.Uri.Scheme.Match)
{
    /// <summary>Gets the URI scheme that the annotated value must not use.</summary>
    public string Scheme { get; } = scheme;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotHasScheme(strValue, Scheme, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
