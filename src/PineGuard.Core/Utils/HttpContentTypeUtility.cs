using System.Diagnostics.CodeAnalysis;

namespace PineGuard.Utils;

/// <summary>
/// Provides HTTP Content-Type header parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/http-content-type">HTTP Content-Type Utility documentation</seealso>
public static class HttpContentTypeUtility
{
    /// <summary>
    /// Attempts to extract the media type(s) from the <c>Content-Type</c> header value(s) in the specified headers.
    /// </summary>
    /// <param name="headers">
    /// The HTTP headers to search, keyed by header name with one or more raw values per key. If
    /// <see langword="null"/> or no <c>Content-Type</c> header is present, returns <see langword="false"/>.
    /// </param>
    /// <param name="mediaTypes">
    /// When this method returns <see langword="true"/>, contains the media type extracted from each
    /// <c>Content-Type</c> header value that could be parsed. When <see langword="false"/>, contains
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if at least one <c>Content-Type</c> header value yielded a media type;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// HttpContentTypeUtility.TryGetContentTypeMediaTypes(headers, out var mediaTypes); // ["application/json"]
    /// </code>
    /// </example>
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
            if (!TryGetMediaType(raw, out var mediaType))
                continue;

            list.Add(mediaType);
        }

        if (list.Count == 0)
            return false;

        mediaTypes = list;
        return true;
    }

    /// <summary>
    /// Attempts to extract the media type from a single <c>Content-Type</c> header value, discarding any
    /// trailing parameters (e.g., <c>charset</c>).
    /// </summary>
    /// <param name="contentTypeHeaderValue">
    /// The raw <c>Content-Type</c> header value (e.g., <c>"application/json; charset=utf-8"</c>). If
    /// <see langword="null"/> or whitespace, returns <see langword="false"/>.
    /// </param>
    /// <param name="mediaType">
    /// When this method returns <see langword="true"/>, contains the media type portion of
    /// <paramref name="contentTypeHeaderValue"/> (e.g., <c>"application/json"</c>), with any trailing
    /// parameters removed. When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="contentTypeHeaderValue"/> yields a non-empty media type;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// HttpContentTypeUtility.TryGetMediaType("application/json; charset=utf-8", out var mediaType); // "application/json"
    /// </code>
    /// </example>
    public static bool TryGetMediaType(string? contentTypeHeaderValue, [NotNullWhen(true)] out string? mediaType)
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
