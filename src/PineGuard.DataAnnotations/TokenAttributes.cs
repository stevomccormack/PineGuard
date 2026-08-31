using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field has the shape of a JSON Web Token
/// (three non-empty Base64Url segments whose header and payload decode to JSON objects).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTokenClauses.Jwt"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The shape is checked only — the signature is not verified and the claims are not inspected, so a token
/// that passes may still be forged or expired. Use this to reject a malformed token at the boundary and
/// leave verification to a JOSE library. If the value is <see langword="null"/>, validation is skipped by
/// the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuthorizationModel
/// {
///     [Jwt]
///     public string AccessToken { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustTokenClauses.Jwt"/>
/// <seealso href="https://pineguard.ai/docs/annotations/token">Token Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class JwtAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Token.Jwt.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Jwt(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
