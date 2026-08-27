using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate file path and file name strings.
/// </summary>
/// <seealso cref="FilePathRules"/>
/// <seealso href="https://pineguard.ai/docs/must/file-path">File Path Must Clauses documentation</seealso>
public static class MustFilePathClauses
{
    /// <summary>
    /// Validates that the specified string is a safe file name (contains no path traversal sequences or invalid characters).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a safe file name.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a safe file name, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="FilePathRules.IsSafeFileName"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a safe file name."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.SafeFileName(uploadedFileName);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="FilePathRules.IsSafeFileName"/>
    /// <seealso href="https://pineguard.ai/docs/must/file-path">File Path Must Clauses documentation</seealso>
    public static MustResult<string> SafeFileName(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.File.Name.Unsafe, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a safe file name.";

        var ok = FilePathRules.IsSafeFileName(value);
        return MustResult<string>.FromBool(ok, MustCodes.File.Name.Unsafe, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified file path has one of the allowed file extensions.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="path">The file path string to validate.</param>
    /// <param name="allowed">
    /// An array of allowed file extensions (e.g., <c>".jpg"</c>, <c>".png"</c>). Pass <see langword="null"/>
    /// to allow any extension.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if the extension of <paramref name="path"/> appears in <paramref name="allowed"/>, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="path"/> is <see langword="null"/>.
    /// Delegates to <see cref="FilePathRules.HasFileExtension"/>. The failure message follows the pattern
    /// <c>"{paramName} must have an allowed file extension."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HasFileExtension(filePath, [".jpg", ".png", ".gif"]);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="FilePathRules.HasFileExtension"/>
    /// <seealso href="https://pineguard.ai/docs/must/file-path">File Path Must Clauses documentation</seealso>
    public static MustResult<string> HasFileExtension(this IMustClause _,
        string? path,
        string[]? allowed,
        [CallerArgumentExpression(nameof(path))] string? paramName = null)
    {
        if (path is null)
            return MustResult<string>.Fail(MustCodes.File.Extension.Mismatch, "{paramName} must not be null.", paramName, path);

        const string messageTemplate = "{paramName} must have an allowed file extension.";

        var ok = FilePathRules.HasFileExtension(path, allowed);
        return MustResult<string>.FromBool(ok, MustCodes.File.Extension.Mismatch, messageTemplate, paramName, path, path);
    }
}
