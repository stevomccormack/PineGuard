using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for security token property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/token">Fluent Token Extensions documentation</seealso>
public static class FluentTokenExtensions
{
    /// <summary>
    /// Validates that the property value has the shape of a JSON Web Token.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTokenClauses.Jwt"/>, which checks the compact serialization only — three
    /// non-empty Base64Url segments whose header and payload decode to JSON objects. The signature is never
    /// verified and the claims are never inspected, so a passing rule rejects a malformed token at the model
    /// boundary and leaves authentication to a JOSE library. If the value is <see langword="null"/>, validation
    /// passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.AccessToken).Jwt();
    /// </code>
    /// </example>
    /// <seealso cref="MustTokenClauses.Jwt"/>
    public static IRuleBuilderOptions<TModel, string?> Jwt<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Jwt(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Token.Jwt.Invalid);
}
