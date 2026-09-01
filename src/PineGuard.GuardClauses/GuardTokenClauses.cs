using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for security tokens.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/token">Guard Token Clauses documentation</seealso>
public static class GuardTokenClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> does not have the shape of a JSON Web Token.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard as a JWT compact serialization.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustTokenClauses.Jwt"/>.
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
    /// Thrown when <paramref name="value"/> is not a structurally valid JWT and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustTokenClauses.Jwt"/>:
    /// <c>Guard.Against.NotJwt</c> passes when the value is three non-empty Base64Url segments whose
    /// header and payload decode to JSON objects. A passing guard proves only the token's shape —
    /// the signature is never verified and no claim is trusted.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotJwt(bearerToken);
    /// </code>
    /// </example>
    /// <seealso cref="MustTokenClauses.Jwt"/>
    public static string NotJwt(
        this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Jwt(value, paramName); // Guard.Against.NotJwt => Must.Be.Jwt (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
