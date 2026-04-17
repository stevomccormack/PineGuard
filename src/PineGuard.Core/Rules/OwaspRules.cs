using PineGuard.Rules.Owasp;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides OWASP security validation predicates for common injection and attack vectors.
/// </summary>
/// <remarks>
/// Each method checks input against known attack patterns. The <see cref="IsOwaspSafe"/> method
/// is a composite check that validates against all supported categories.
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
    public static bool IsXssSafe(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && OwaspRegex.Xss.NoAngleBracketsRegex().IsMatch(trimmed);

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
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.HeaderInjection.CrLfRegex().IsMatch(trimmed);
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
