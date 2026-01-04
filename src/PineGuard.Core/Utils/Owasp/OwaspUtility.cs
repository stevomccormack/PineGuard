using PineGuard.Rules.Owasp;

namespace PineGuard.Utils.Owasp;

public static class OwaspUtility
{
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

    public static bool ContainsPathTraversalRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.PathTraversal.DotDotSegmentRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.AbsoluteUnixPathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.WindowsDriveAbsolutePathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.UncPathRegex().IsMatch(trimmed);
    }

    public static bool ContainsCommandInjectionRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.CommandInjection.ShellMetacharactersRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.CommandChainingRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.NewlineRegex().IsMatch(trimmed);
    }

    public static bool ContainsCrLfRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.HeaderInjection.CrLfRegex().IsMatch(trimmed);
    }

    public static bool ContainsLdapFilterRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.LdapInjection.LdapFilterSpecialCharsRegex().IsMatch(trimmed);
    }

    public static bool ContainsOpenRedirectRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.OpenRedirect.AbsoluteOrProtocolRelativeUrlRegex().IsMatch(trimmed);
    }

    public static bool ContainsSsrfSchemeRisk(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.Ssrf.DangerousSchemeRegex().IsMatch(trimmed);
    }
}
