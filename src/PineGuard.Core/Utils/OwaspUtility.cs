using PineGuard.Rules.Owasp;

namespace PineGuard.Utils;

/// <summary>
/// Provides OWASP security risk detection utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/owasp">OWASP Utility documentation</seealso>
public static class OwaspUtility
{
    /// <summary>
    /// Performs a heuristic pattern check for input that resembles SQL injection (keywords, comment markers,
    /// statement terminators, boolean tautologies, UNION SELECT, or unescaped quotes). This is a best-effort
    /// heuristic, not a guarantee that the input is safe or unsafe.
    /// </summary>
    /// <param name="value">The value to check. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> matches a known SQL injection pattern; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsSqlInjectionRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.SqlInjection.SqlKeywordRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlCommentRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlStatementTerminatorRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlBooleanRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlUnionSelectRegex().IsMatch(trimmed)
            || OwaspRegex.SqlInjection.SqlQuoteRegex().IsMatch(trimmed);
    }

    /// <summary>
    /// Performs a heuristic pattern check for input that resembles path traversal (<c>..</c> segments, absolute
    /// Unix paths, absolute Windows drive paths, or UNC paths). This is a best-effort heuristic, not a guarantee
    /// that the input is safe or unsafe.
    /// </summary>
    /// <param name="value">The value to check. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> matches a known path traversal pattern; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsPathTraversalRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.PathTraversal.DotDotSegmentRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.AbsoluteUnixPathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.WindowsDriveAbsolutePathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.UncPathRegex().IsMatch(trimmed);
    }

    /// <summary>
    /// Performs a heuristic pattern check for input that resembles OS command injection (shell metacharacters,
    /// command chaining operators, or embedded newlines). This is a best-effort heuristic, not a guarantee
    /// that the input is safe or unsafe.
    /// </summary>
    /// <param name="value">The value to check. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> matches a known command injection pattern; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsCommandInjectionRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.CommandInjection.ShellMetacharactersRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.CommandChainingRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.NewlineRegex().IsMatch(trimmed);
    }

    /// <summary>
    /// Performs a heuristic pattern check for input that resembles CRLF/HTTP header injection (embedded carriage
    /// return or line feed characters). This is a best-effort heuristic, not a guarantee that the input is safe or unsafe.
    /// </summary>
    /// <param name="value">The value to check. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> contains a CR or LF character; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsCrLfRisk(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && OwaspRegex.HeaderInjection.CrLfRegex().IsMatch(trimmed);

    /// <summary>
    /// Performs a heuristic pattern check for input that resembles LDAP filter injection (special filter
    /// characters such as <c>*</c>, <c>(</c>, <c>)</c>, <c>\</c>, or NUL). This is a best-effort heuristic,
    /// not a guarantee that the input is safe or unsafe.
    /// </summary>
    /// <param name="value">The value to check. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> matches a known LDAP filter injection pattern; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsLdapFilterRisk(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && OwaspRegex.LdapInjection.LdapFilterSpecialCharsRegex().IsMatch(trimmed);

    /// <summary>
    /// Performs a heuristic pattern check for input that resembles an open redirect target (an absolute URL or
    /// a protocol-relative URL). This is a best-effort heuristic, not a guarantee that the input is safe or unsafe.
    /// </summary>
    /// <param name="value">The value to check. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> matches a known open redirect pattern; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsOpenRedirectRisk(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && OwaspRegex.OpenRedirect.AbsoluteOrProtocolRelativeUrlRegex().IsMatch(trimmed);

    /// <summary>
    /// Performs a heuristic pattern check for input that resembles a server-side request forgery (SSRF) target
    /// (a dangerous URI scheme such as <c>file</c>, <c>gopher</c>, or <c>dict</c>). This is a best-effort
    /// heuristic, not a guarantee that the input is safe or unsafe.
    /// </summary>
    /// <param name="value">The value to check. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> starts with a known dangerous URI scheme; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsSsrfSchemeRisk(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && OwaspRegex.Ssrf.DangerousSchemeRegex().IsMatch(trimmed);
}
