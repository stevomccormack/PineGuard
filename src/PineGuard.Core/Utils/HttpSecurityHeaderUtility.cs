namespace PineGuard.Utils;

/// <summary>
/// Provides HTTP security header parsing utility methods (HSTS, CSP directives).
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/http-security-header">HTTP Security Header Utility documentation</seealso>
public static class HttpSecurityHeaderUtility
{
    public static bool TrySplitSemicolonSeparatedSegments(string? value, out IReadOnlyList<string>? segments)
    {
        segments = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var list = new List<string>();

        foreach (var raw in trimmed.Split(';'))
        {
            if (!StringUtility.TryGetTrimmed(raw, out var segment))
                continue;

            list.Add(segment);
        }

        if (list.Count == 0)
            return false;

        segments = list;
        return true;
    }

    internal readonly record struct HstsDirectives(long? MaxAgeSeconds, bool IncludeSubDomains, bool Preload);

    internal static HstsDirectives ParseHstsDirectives(IReadOnlyList<string> segments)
    {
        long? maxAgeSeconds = null;
        var hasIncludeSubDomains = false;
        var hasPreload = false;

        foreach (var segment in segments)
        {
            if (TryParseMaxAge(segment, out var maxAge))
            {
                maxAgeSeconds = maxAge;
                continue;
            }

            if (string.Equals(segment, "includeSubDomains", StringComparison.OrdinalIgnoreCase))
            {
                hasIncludeSubDomains = true;
                continue;
            }

            if (string.Equals(segment, "preload", StringComparison.OrdinalIgnoreCase))
                hasPreload = true;
        }

        return new HstsDirectives(maxAgeSeconds, hasIncludeSubDomains, hasPreload);
    }

    private static bool TryParseMaxAge(string segment, out long maxAge)
    {
        maxAge = 0;

        if (!segment.StartsWith("max-age", StringComparison.OrdinalIgnoreCase))
            return false;

        var eq = segment.IndexOf('=');
        if (eq < 0)
            return false;

        var raw = segment[(eq + 1)..].Trim();
        return long.TryParse(raw, out maxAge) && maxAge >= 0;
    }
}
