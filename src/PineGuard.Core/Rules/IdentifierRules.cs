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
    /// Validates the conventional ASCII URL-slug grammar <c>[a-z0-9]+(-[a-z0-9]+)*</c>: lowercase
    /// ASCII letters and digits, grouped into words separated by single hyphens, with no leading
    /// or trailing hyphens and no consecutive hyphens. Unlike <see cref="StringRules.IsKebabCase"/>,
    /// non-ASCII letters and digits are rejected.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = IdentifierRules.IsSlug("my-page-slug"); // true
    /// bool invalid = IdentifierRules.IsSlug("My Page");    // false
    /// </code>
    /// </example>
    public static bool IsSlug(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value[0] == '-' || value[^1] == '-')
            return false;

        var previousWasHyphen = false;
        foreach (var ch in value)
        {
            if (ch == '-')
            {
                if (previousWasHyphen)
                    return false;

                previousWasHyphen = true;
                continue;
            }

            if (ch is < 'a' or > 'z' && ch is < '0' or > '9')
                return false;

            previousWasHyphen = false;
        }

        return true;
    }
}
