using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate security tokens,
/// delegating to <see cref="TokenRules"/> for core validation logic.
/// </summary>
/// <seealso cref="TokenRules"/>
/// <seealso href="https://pineguard.ai/docs/must/token">Token Must Clauses documentation</seealso>
public static class MustTokenClauses
{
    /// <summary>
    /// Validates that the specified string has the shape of a JSON Web Token.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a JWT compact serialization.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a well-formed JWT compact serialization, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="TokenRules.IsJwt"/>, which checks the shape only — three non-empty Base64Url
    /// segments whose header and payload decode to JSON objects. The signature is not verified and the claims
    /// are not inspected, so a token that passes may still be forged or expired; this clause rejects a
    /// malformed token at the boundary and leaves verification to a JOSE library. The failure message follows
    /// the pattern <c>"{paramName} must be a valid JWT."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Jwt(authorizationHeader);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="TokenRules.IsJwt"/>
    /// <seealso href="https://pineguard.ai/docs/must/token">Token Must Clauses documentation</seealso>
    public static MustResult<string> Jwt(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Token.Jwt.Invalid, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a valid JWT.";

        var ok = TokenRules.IsJwt(value);
        return MustResult<string>.FromBool(ok, MustCodes.Token.Jwt.Invalid, messageTemplate, paramName, value, value);
    }
}
