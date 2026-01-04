using PineGuard.Utils;

namespace PineGuard.Rules.Owasp;

public static class OwaspRules
{
    public static bool IsOwaspSafe(string? value)
    {
        return IsXssSafe(value)
            && IsSqlInjectionSafe(value)
            && IsPathTraversalSafe(value)
            && IsCommandInjectionSafe(value)
            && IsCrLfSafe(value)
            && IsLdapFilterSafe(value)
            && IsOpenRedirectSafe(value)
            && IsSsrfSchemeSafe(value);
    }

    public static bool IsOwaspDangerous(string? value) =>
        !IsOwaspSafe(value);

    //---------------------------------------------------------------------------------------

    public static bool IsXssSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return OwaspRegex.Xss.NoAngleBracketsRegex().IsMatch(trimmed);
    }

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

    public static bool IsPathTraversalSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !(OwaspRegex.PathTraversal.DotDotSegmentRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.AbsoluteUnixPathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.WindowsDriveAbsolutePathRegex().IsMatch(trimmed)
            || OwaspRegex.PathTraversal.UncPathRegex().IsMatch(trimmed));
    }

    public static bool IsCommandInjectionSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !(OwaspRegex.CommandInjection.ShellMetacharactersRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.CommandChainingRegex().IsMatch(trimmed)
            || OwaspRegex.CommandInjection.NewlineRegex().IsMatch(trimmed));
    }

    public static bool IsCrLfSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.HeaderInjection.CrLfRegex().IsMatch(trimmed);
    }

    public static bool IsLdapFilterSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.LdapInjection.LdapFilterSpecialCharsRegex().IsMatch(trimmed);
    }

    public static bool IsOpenRedirectSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.OpenRedirect.AbsoluteOrProtocolRelativeUrlRegex().IsMatch(trimmed);
    }

    public static bool IsSsrfSchemeSafe(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return !OwaspRegex.Ssrf.DangerousSchemeRegex().IsMatch(trimmed);
    }
}