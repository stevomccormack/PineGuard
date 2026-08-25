using PineGuard.Rules.Owasp;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides OWASP security validation predicates for common injection and attack vectors.
/// </summary>
/// <remarks>
/// Each method checks input against known attack patterns. The <see cref="IsOwaspSafe"/> method
/// is a composite check that validates against all supported categories.
/// <para>
/// These are homegrown deny-list heuristics inspired by the OWASP Top Ten attack categories; they are
/// <em>not</em> drawn from the OWASP Validation Regex Repository or any other OWASP-published detection
/// set. Because <see cref="IsOwaspSafe"/> AND-combines several deny-lists, it is tuned for identifier-like
/// fields (usernames, slugs, path segments) and will reject a great deal of legitimate free text (e.g.
/// apostrophes, parentheses, or SQL-keyword-shaped words). Treat these checks as a coarse, best-effort
/// defense-in-depth layer, not a substitute for parameterized queries, output encoding, or an
/// allow-list validator appropriate to the field.
/// </para>
/// <para>
/// <b>Null contract:</b> every method here treats <see langword="null"/> or whitespace-only input as
/// <b>unsafe</b> (returns <see langword="false"/>), since there is no value to validate. This is the
/// opposite convention from the risk-detection heuristics in <see cref="PineGuard.Utils.OwaspUtility"/>,
/// where <c>Contains*Risk(null)</c> returns <see langword="false"/> meaning "no risk pattern found". The
/// two are not interchangeable: <c>!OwaspUtility.ContainsSqlInjectionRisk(value)</c> is <b>not</b>
/// equivalent to <see cref="IsSqlInjectionSafe"/> for <see langword="null"/> or whitespace input.
/// </para>
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/owasp">OWASP Rules documentation</seealso>
/// <seealso href="https://owasp.org/www-project-top-ten/">OWASP Top Ten</seealso>
public static class OwaspRules
{
    /// <summary>
    /// Determines whether the specified value passes all OWASP security checks (XSS, SQL injection,
    /// path traversal, command injection, CRLF, LDAP, open redirect, and SSRF).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value passes all security checks; otherwise, <see langword="false"/>.</returns>
    public static bool IsOwaspSafe(string? value) =>
        IsXssSafe(value)
        && IsSqlInjectionSafe(value)
        && IsPathTraversalSafe(value)
        && IsCommandInjectionSafe(value)
        && IsCrLfSafe(value)
        && IsLdapFilterSafe(value)
        && IsOpenRedirectSafe(value)
        && IsSsrfSchemeSafe(value);

    //---------------------------------------------------------------------------------------

    /// <summary>
    /// Determines whether the specified value is safe from cross-site scripting (XSS) attacks.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no XSS attack patterns; otherwise, <see langword="false"/>.</returns>
    public static bool IsXssSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !(!OwaspRegex.Xss.NoAngleBracketsRegex().IsMatch(trimmed)
            || OwaspRegex.Xss.ScriptProtocolRegex().IsMatch(trimmed)
            || OwaspRegex.Xss.HtmlEventHandlerAttributeRegex().IsMatch(trimmed)
            || OwaspRegex.Xss.HtmlEntityEncodedAngleBracketRegex().IsMatch(trimmed)
            || OwaspRegex.Xss.PercentEncodedAngleBracketRegex().IsMatch(trimmed));
    }

    /// <summary>
    /// Determines whether the specified value is safe from SQL injection attacks.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no SQL injection patterns; otherwise, <see langword="false"/>.</returns>
    public static bool IsSqlInjectionSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !(OwaspRegex.SqlInjection.SqlKeywordRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlCommentRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlStatementTerminatorRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlBooleanRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlUnionSelectRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlQuoteRegex().IsMatch(trimmed));
    }

    /// <summary>
    /// Determines whether the specified value is safe from path traversal attacks.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no path traversal patterns; otherwise, <see langword="false"/>.</returns>
    public static bool IsPathTraversalSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !(OwaspRegex.PathTraversal.DotDotSegmentRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.AbsoluteUnixPathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.WindowsDriveAbsolutePathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.UncPathRegex().IsMatch(trimmed));
    }

    /// <summary>
    /// Determines whether the specified value is safe from command injection attacks.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no command injection patterns; otherwise, <see langword="false"/>.</returns>
    public static bool IsCommandInjectionSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !(OwaspRegex.CommandInjection.ShellMetacharactersRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.CommandChainingRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.NewlineRegex().IsMatch(trimmed));
    }

    /// <summary>
    /// Determines whether the specified value is safe from CRLF injection (HTTP header injection) attacks.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no CR/LF injection patterns; otherwise, <see langword="false"/>.</returns>
    public static bool IsCrLfSafe(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return !OwaspRegex.HeaderInjection.CrLfRegex().IsMatch(value);
    }

    /// <summary>
    /// Determines whether the specified value is safe from LDAP filter injection attacks.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no LDAP filter special characters; otherwise, <see langword="false"/>.</returns>
    public static bool IsLdapFilterSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.LdapInjection.LdapFilterSpecialCharsRegex().IsMatch(trimmed);
    }

    /// <summary>
    /// Determines whether the specified value is safe from open redirect attacks.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no absolute or protocol-relative URL patterns; otherwise, <see langword="false"/>.</returns>
    public static bool IsOpenRedirectSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.OpenRedirect.AbsoluteOrProtocolRelativeUrlRegex().IsMatch(trimmed);
    }

    /// <summary>
    /// Determines whether the specified value is safe from server-side request forgery (SSRF) via dangerous URI schemes.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the value contains no dangerous URI scheme patterns; otherwise, <see langword="false"/>.</returns>
    public static bool IsSsrfSchemeSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.Ssrf.DangerousSchemeRegex().IsMatch(trimmed);
    }
}
