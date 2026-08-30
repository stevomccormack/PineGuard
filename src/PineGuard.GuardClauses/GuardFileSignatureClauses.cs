using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for the magic bytes a file leads with.
/// </summary>
/// <remarks>
/// These clauses take the leading bytes of a file and throw on a mismatch; reading those bytes from disk,
/// a stream or an upload is the caller's job.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/guard/file-signature">Guard File Signature Clauses documentation</seealso>
public static class GuardFileSignatureClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> does not match the file signature registered for <paramref name="extension"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The leading bytes of the file to guard.</param>
    /// <param name="extension">
    /// The extension the file claims to have, with or without a leading dot and in any casing.
    /// It must be one of the extensions PineGuard registers a signature for.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustFileSignatureClauses.FileSignature"/>.
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
    /// Thrown when <paramref name="value"/> does not match the signature registered for
    /// <paramref name="extension"/>, or when no signature is registered for <paramref name="extension"/>
    /// at all, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustFileSignatureClauses.FileSignature"/>:
    /// <c>Guard.Against.NotFileSignature</c> passes when the header matches the declared extension.
    /// An unregistered <paramref name="extension"/> throws attributed to <paramref name="extension"/>,
    /// not to <paramref name="value"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotFileSignature(uploadedHeader, ".png");
    /// </code>
    /// </example>
    /// <seealso cref="MustFileSignatureClauses.FileSignature"/>
    public static byte[] NotFileSignature(this IGuardClause _,
        byte[]? value,
        string extension,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.FileSignature(value, extension, paramName); // Guard.Against.NotFileSignature => Must.Be.FileSignature (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not match any registered file signature.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The leading bytes of the file to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustFileSignatureClauses.KnownFileSignature"/>.
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
    /// Thrown when <paramref name="value"/> matches no registered file signature and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustFileSignatureClauses.KnownFileSignature"/>:
    /// <c>Guard.Against.NotKnownFileSignature</c> passes when the header matches one of the registered
    /// signatures. A passing guard proves only that the bytes lead with a known signature — never that
    /// the rest of the file is well-formed or safe.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotKnownFileSignature(uploadedHeader);
    /// </code>
    /// </example>
    /// <seealso cref="MustFileSignatureClauses.KnownFileSignature"/>
    public static byte[] NotKnownFileSignature(this IGuardClause _,
        byte[]? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.KnownFileSignature(value, paramName); // Guard.Against.NotKnownFileSignature => Must.Be.KnownFileSignature (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
