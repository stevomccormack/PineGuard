using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for file path and file name validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/filepath">Guard File Path Clauses documentation</seealso>
public static class GuardFilePathClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a safe file name (i.e., contains invalid characters or path separators).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The file name string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustFilePathClauses.SafeFileName"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a safe file name and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustFilePathClauses.SafeFileName"/>:
    /// <c>Guard.Against.NotSafeFileName</c> passes when the file name contains no disallowed characters.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotSafeFileName(uploadedFileName);
    /// </code>
    /// </example>
    /// <seealso cref="MustFilePathClauses.SafeFileName"/>
    public static string NotSafeFileName(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.SafeFileName(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="path"/> does not have a file extension, or its extension is not in <paramref name="allowed"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="path">The file path string to guard.</param>
    /// <param name="allowed">
    /// An optional list of allowed extensions (e.g., <c>[".pdf", ".docx"]</c>).
    /// If <see langword="null"/>, any extension is accepted.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustFilePathClauses.HasFileExtension"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="path"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the path has no extension or an extension not in <paramref name="allowed"/>, and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustFilePathClauses.HasFileExtension"/>:
    /// <c>Guard.Against.NotHasFileExtension</c> passes when the extension is present and allowed.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasFileExtension(filePath, allowed: [".pdf", ".docx"]);
    /// </code>
    /// </example>
    /// <seealso cref="MustFilePathClauses.HasFileExtension"/>
    public static string NotHasFileExtension(this IGuardClause _,
        string? path,
        string[]? allowed = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(path))] string? paramName = null)
    {
        var result = Must.Be.HasFileExtension(path, allowed, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
