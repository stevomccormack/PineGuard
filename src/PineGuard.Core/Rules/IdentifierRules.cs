namespace PineGuard.Rules;

/// <summary>
/// Provides validation predicates for identifier formats such as URL slugs.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/identifier">Identifier Rules documentation</seealso>
public static class IdentifierRules
{
    /// <summary>
    /// Determines whether the specified value is a valid URL slug (kebab-case identifier).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid kebab-case slug; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="StringRules.IsKebabCase"/>. A valid slug consists of lowercase
    /// words separated by hyphens with no leading or trailing hyphens.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = IdentifierRules.IsSlug("my-page-slug"); // true
    /// bool invalid = IdentifierRules.IsSlug("My Page");    // false
    /// </code>
    /// </example>
    /// <seealso cref="StringRules.IsKebabCase"/>
    public static bool IsSlug(string? value) =>
        StringRules.IsKebabCase(value);
}
