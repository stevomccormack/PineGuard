using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure security-token validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/token">Token Rules documentation</seealso>
public static class TokenRules
{
    /// <summary>
    /// The character separating the segments of a JWT compact serialization (<c>'.'</c>).
    /// </summary>
    public const char JwtSegmentSeparator = '.';

    /// <summary>
    /// The number of segments in a JWT compact serialization (<c>3</c>: header, payload and signature).
    /// </summary>
    public const int JwtSegmentCount = 3;

    /// <summary>
    /// Determines whether the specified value has the shape of a JSON Web Token.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a well-formed JWT compact serialization;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This validates the shape only: <see cref="JwtSegmentCount"/> non-empty Base64Url segments separated
    /// by <see cref="JwtSegmentSeparator"/>, where the header and the payload each decode to a JSON object.
    /// Leading and trailing whitespace is trimmed before validation.
    /// </para>
    /// <para>
    /// The signature is <em>not</em> verified and the claims are <em>not</em> inspected, so a token that
    /// passes may still be forged, expired or issued by someone else. Verification needs a key and a clock,
    /// which are exactly the environment-dependent inputs a pure rule cannot have; use a JOSE library for
    /// that. What this rule buys is rejecting a malformed token at the boundary, before it reaches code that
    /// would otherwise fail deeper in with a less useful error.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = TokenRules.IsJwt("eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxIn0.c2ln"); // true
    /// bool invalid = TokenRules.IsJwt("eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxIn0");    // false (two segments)
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/token">Token Rules documentation</seealso>
    public static bool IsJwt(string? value) =>
        TokenUtility.TryParseJwt(value, out _, out _, out _);
}
