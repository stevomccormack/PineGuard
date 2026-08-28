using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate identifier strings such as URL slugs.
/// </summary>
/// <seealso cref="IdentifierRules"/>
/// <seealso href="https://pineguard.ai/docs/must/identifier">Identifier Must Clauses documentation</seealso>
public static class MustIdentifierClauses
{
    /// <summary>
    /// Validates that the specified string is a valid URL slug (lowercase letters, digits, and hyphens only).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a URL slug.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid slug, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="IdentifierRules.IsSlug"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid slug."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Slug(urlPath);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="IdentifierRules.IsSlug"/>
    /// <seealso href="https://pineguard.ai/docs/must/identifier">Identifier Must Clauses documentation</seealso>
    public static MustResult<string> Slug(
        this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Identifier.Slug.Invalid, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a valid slug.";

        var ok = IdentifierRules.IsSlug(value);
        return MustResult<string>.FromBool(ok, MustCodes.Identifier.Slug.Invalid, messageTemplate, paramName, value, value);
    }
}
