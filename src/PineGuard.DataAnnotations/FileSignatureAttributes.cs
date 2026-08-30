using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <c>byte[]</c> property or field leads with the file signature registered
/// for the declared extension.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustFileSignatureClauses.FileSignature"/>. Supported on properties, fields, and
/// parameters of type <c>byte[]</c> holding the leading bytes of a file; reading those bytes from disk, a
/// stream or an upload is the caller's job.
/// </para>
/// <para>
/// The declared extension may be written with or without a leading dot and in any casing, but it must be
/// one of the extensions PineGuard registers a signature for; an unregistered extension fails validation
/// with a message naming the extension rather than the annotated member. If the value is
/// <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UploadModel
/// {
///     [FileSignature(".png")]
///     public byte[] Header { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="KnownFileSignatureAttribute"/>
/// <seealso cref="MustFileSignatureClauses.FileSignature"/>
/// <seealso href="https://pineguard.ai/docs/annotations/file-signature">File Signature Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FileSignatureAttribute(string extension)
    : ValidationAttributeBase(typeof(byte[]), MustCodes.File.Signature.Mismatch)
{
    /// <summary>Gets the extension the annotated bytes are declared to carry (e.g., <c>".png"</c>).</summary>
    public string Extension { get; } = extension;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var byteValue = (byte[])value!;

        var result = Must.Be.FileSignature(byteValue, Extension, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <c>byte[]</c> property or field leads with one of the registered file
/// signatures.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustFileSignatureClauses.KnownFileSignature"/>. Supported on properties, fields,
/// and parameters of type <c>byte[]</c> holding the leading bytes of a file; reading those bytes from disk,
/// a stream or an upload is the caller's job.
/// </para>
/// <para>
/// A passing value proves only that the bytes lead with a known signature — never that the rest of the file
/// is well-formed or safe. If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UploadModel
/// {
///     [KnownFileSignature]
///     public byte[] Header { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FileSignatureAttribute"/>
/// <seealso cref="MustFileSignatureClauses.KnownFileSignature"/>
/// <seealso href="https://pineguard.ai/docs/annotations/file-signature">File Signature Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class KnownFileSignatureAttribute()
    : ValidationAttributeBase(typeof(byte[]), MustCodes.File.Signature.Unknown)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var byteValue = (byte[])value!;

        var result = Must.Be.KnownFileSignature(byteValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
