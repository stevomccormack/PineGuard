namespace PineGuard.Utils;

/// <summary>
/// Provides HTTP header access and query utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/http">HTTP Utility documentation</seealso>
public static class HttpUtility
{
    public static bool TryGetHeaderValues(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, out IEnumerable<string>? values)
    {
        values = null;

        if (headers is null)
            return false;

        if (!StringUtility.TryGetTrimmed(name, out var headerName))
            return false;

        if (headers.TryGetValue(headerName, out values))
            return true;

        foreach (var kvp in headers)
        {
            if (!string.Equals(kvp.Key, headerName, StringComparison.OrdinalIgnoreCase))
                continue;

            values = kvp.Value;
            return true;
        }

        return false;
    }

    public static bool TryGetSingleHeaderValue(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, out string? value)
    {
        value = null;

        if (!TryGetHeaderValues(headers, name, out var values) || values is null)
            return false;

        string? single = null;
        var seen = 0;

        foreach (var candidate in values)
        {
            if (!StringUtility.TryGetTrimmed(candidate, out var trimmed))
                continue;

            seen++;
            if (seen > 1)
                return false;

            single = trimmed;
        }

        if (seen != 1)
            return false;

        value = single;
        return true;
    }
}
