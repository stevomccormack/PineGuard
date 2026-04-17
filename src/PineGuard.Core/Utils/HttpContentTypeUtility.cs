namespace PineGuard.Utils;

/// <summary>
/// Provides HTTP Content-Type header parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/http-content-type">HTTP Content-Type Utility documentation</seealso>
public static class HttpContentTypeUtility
{
    public static bool TryGetContentTypeMediaTypes(
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        out IReadOnlyList<string>? mediaTypes)
    {
        mediaTypes = null;

        if (!HttpUtility.TryGetHeaderValues(headers, "Content-Type", out var values) || values is null)
            return false;

        var list = new List<string>();

        foreach (var raw in values)
        {
            if (!TryGetMediaType(raw, out var mediaType) || mediaType is null)
                continue;

            list.Add(mediaType);
        }

        if (list.Count == 0)
            return false;

        mediaTypes = list;
        return true;
    }

    public static bool TryGetMediaType(string? contentTypeHeaderValue, out string? mediaType)
    {
        mediaType = null;

        if (!StringUtility.TryGetTrimmed(contentTypeHeaderValue, out var trimmed))
            return false;

        var semi = trimmed.IndexOf(';');
        var candidate = semi < 0 ? trimmed : trimmed[..semi].Trim();

        if (!StringUtility.TryGetTrimmed(candidate, out var normalized))
            return false;

        mediaType = normalized;
        return true;
    }
}
