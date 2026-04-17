using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for URL-safe identifier formats such as slugs.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/identifier">Guard Identifier Clauses documentation</seealso>
public static class GuardIdentifierClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid URL slug (lowercase letters, digits, and hyphens only).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustIdentifierClauses.Slug"/>.
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
    /// Thrown when <paramref name="value"/> is not a valid slug and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustIdentifierClauses.Slug"/>:
    /// <c>Guard.Against.NotSlug</c> passes when the value is a valid URL slug format.
    /// A valid slug contains only lowercase letters, digits, and hyphens, with no leading or trailing hyphens.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotSlug(urlSegment);
    /// </code>
    /// </example>
    /// <seealso cref="MustIdentifierClauses.Slug"/>
    public static string NotSlug(
        this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Slug(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }
}
