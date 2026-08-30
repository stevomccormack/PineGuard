using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides validation predicates for identifier formats such as URL slugs.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/identifier">Identifier Rules documentation</seealso>
public static class IdentifierRules
{
    /// <summary>
    /// The number of characters in a ULID (26 Crockford base32 digits).
    /// </summary>
    public const int UlidLength = 26;

    /// <summary>
    /// The highest character a ULID can start with (<c>'7'</c>). The leading character carries the
    /// top three bits of a 48-bit timestamp, so a canonical ULID never begins above this.
    /// </summary>
    public const char MaxUlidFirstChar = '7';

    private const string UlidAlphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

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

    /// <summary>
    /// Determines whether the specified value is a canonical ULID (Universally Unique
    /// Lexicographically Sortable Identifier).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a canonical ULID; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// A canonical ULID is exactly <see cref="UlidLength"/> characters of Crockford base32 — the digits
    /// <c>0-9</c> and the letters <c>A-Z</c> excluding <c>I</c>, <c>L</c>, <c>O</c> and <c>U</c> — written
    /// in either case, and starting no higher than <see cref="MaxUlidFirstChar"/>. Leading and trailing
    /// whitespace is trimmed before validation. This checks the textual form only; it does not interpret
    /// the embedded timestamp.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = IdentifierRules.IsUlid("01ARZ3NDEKTSV4RRFFQ69G5FAV"); // true
    /// bool invalid = IdentifierRules.IsUlid("01ARZ3NDEKTSV4RRFFQ69G5FAI"); // false ('I' is not in the alphabet)
    /// </code>
    /// </example>
    public static bool IsUlid(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed) || trimmed.Length != UlidLength)
            return false;

        if (trimmed[0] is < '0' or > MaxUlidFirstChar)
            return false;

        foreach (var ch in trimmed)
        {
            if (!UlidAlphabet.Contains(char.ToUpperInvariant(ch), StringComparison.Ordinal))
                return false;
        }

        return true;
    }
}
