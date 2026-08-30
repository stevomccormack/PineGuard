using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate the magic bytes a file leads with,
/// delegating to <see cref="FileSignatureRules"/> for core validation logic.
/// </summary>
/// <remarks>
/// These clauses take the leading bytes of a file and return a verdict; reading those bytes from disk,
/// a stream or an upload is the caller's job.
/// </remarks>
/// <seealso cref="FileSignatureRules"/>
/// <seealso href="https://pineguard.ai/docs/must/file-signature">File Signature Must Clauses documentation</seealso>
public static class MustFileSignatureClauses
{
    /// <summary>
    /// Validates that the specified header bytes match the file signature registered for the declared extension.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">
    /// The leading bytes of the file, of which at most
    /// <see cref="FileSignatureUtility.MaxSignatureLength"/> are read.
    /// </param>
    /// <param name="extension">
    /// The extension the file claims to have, with or without a leading dot and in any casing. It must be
    /// one of <see cref="FileSignatureUtility.KnownExtensions"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> starts with a signature registered for <paramref name="extension"/>, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result attributed to <paramref name="extension"/> when no signature is registered
    /// for it, and a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="FileSignatureRules.HasSignature"/>. The failure message follows the pattern
    /// <c>"{paramName} must match the file signature for the declared extension."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.FileSignature(uploadedHeader, ".png");
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="FileSignatureRules.HasSignature"/>
    /// <seealso href="https://pineguard.ai/docs/must/file-signature">File Signature Must Clauses documentation</seealso>
    public static MustResult<byte[]> FileSignature(this IMustClause _,
        byte[]? value,
        string extension,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!FileSignatureUtility.IsKnownExtension(extension))
            return MustResult<byte[]>.Fail(MustCodes.File.Signature.Unknown, "{paramName} must have a registered file signature.", nameof(extension), extension);

        if (value is null)
            return MustResult<byte[]>.Fail(MustCodes.File.Signature.Mismatch, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must match the file signature for the declared extension.";

        var ok = FileSignatureRules.HasSignature(value, extension);
        return MustResult<byte[]>.FromBool(ok, MustCodes.File.Signature.Mismatch, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified header bytes match one of the registered file signatures.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">
    /// The leading bytes of the file, of which at most
    /// <see cref="FileSignatureUtility.MaxSignatureLength"/> are read.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> starts with the signature of one of
    /// <see cref="FileSignatureUtility.KnownExtensions"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="FileSignatureRules.HasKnownSignature"/>. The failure message follows the
    /// pattern <c>"{paramName} must match a known file signature."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.KnownFileSignature(uploadedHeader);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="FileSignatureRules.HasKnownSignature"/>
    /// <seealso href="https://pineguard.ai/docs/must/file-signature">File Signature Must Clauses documentation</seealso>
    public static MustResult<byte[]> KnownFileSignature(this IMustClause _,
        byte[]? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<byte[]>.Fail(MustCodes.File.Signature.Unknown, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must match a known file signature.";

        var ok = FileSignatureRules.HasKnownSignature(value);
        return MustResult<byte[]>.FromBool(ok, MustCodes.File.Signature.Unknown, messageTemplate, paramName, value, value);
    }
}
