using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a safe file name
/// (contains no path separators or reserved OS characters).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustFilePathClauses.SafeFileName"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UploadModel
/// {
///     [SafeFileName]
///     public string FileName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasFileExtensionAttribute"/>
/// <seealso cref="MustFilePathClauses.SafeFileName"/>
/// <seealso href="https://pineguard.ai/docs/annotations/filepath">FilePath Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SafeFileNameAttribute() : ValidationAttributeBase(typeof(string), MustCodes.File.Name.Unsafe)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.SafeFileName(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a file name with one of the
/// specified allowed extensions.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustFilePathClauses.HasFileExtension"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Extensions should include the leading dot (e.g., <c>".pdf"</c>, <c>".png"</c>). Comparison is
/// case-insensitive. If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UploadModel
/// {
///     [HasFileExtension(".pdf", ".docx")]
///     public string Document { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="SafeFileNameAttribute"/>
/// <seealso cref="MustFilePathClauses.HasFileExtension"/>
/// <seealso href="https://pineguard.ai/docs/annotations/filepath">FilePath Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasFileExtensionAttribute(params string[] allowedExtensions)
    : ValidationAttributeBase(typeof(string), MustCodes.File.Extension.Mismatch)
{
    /// <summary>Gets the list of allowed file extensions (each with a leading dot, e.g., <c>".pdf"</c>).</summary>
    public string[] AllowedExtensions { get; } = allowedExtensions;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.HasFileExtension(strValue, AllowedExtensions, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
