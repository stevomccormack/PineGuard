using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for version strings.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/version">Guard Version Clauses documentation</seealso>
public static class GuardVersionClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a Semantic Versioning 2.0.0 version.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard as a semantic version.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustVersionClauses.SemVer"/>.
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
    /// Thrown when <paramref name="value"/> is not a valid semantic version and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustVersionClauses.SemVer"/>:
    /// <c>Guard.Against.NotSemVer</c> passes when all three numeric components are present and any
    /// pre-release or build-metadata suffix is well formed. A leading <c>v</c> is a packaging
    /// convention rather than part of the specification, so it throws.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotSemVer(packageVersion);
    /// </code>
    /// </example>
    /// <seealso cref="MustVersionClauses.SemVer"/>
    public static string NotSemVer(
        this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.SemVer(value, paramName); // Guard.Against.NotSemVer => Must.Be.SemVer (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
