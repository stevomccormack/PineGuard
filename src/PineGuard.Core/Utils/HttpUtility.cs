namespace PineGuard.Utils;

/// <summary>
/// Provides HTTP header access and query utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/http">HTTP Utility documentation</seealso>
public static class HttpUtility
{
    /// <summary>
    /// Attempts to get the raw values of the header with the specified name, falling back to a
    /// case-insensitive key comparison when no exact key match is found.
    /// </summary>
    /// <param name="headers">
    /// The HTTP headers to search, keyed by header name with one or more raw values per key. If
    /// <see langword="null"/>, returns <see langword="false"/>.
    /// </param>
    /// <param name="name">
    /// The header name to look up. If <see langword="null"/> or whitespace, returns <see langword="false"/>.
    /// </param>
    /// <param name="values">
    /// When this method returns <see langword="true"/>, contains the raw values associated with
    /// <paramref name="name"/>. When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a header matching <paramref name="name"/> was found in
    /// <paramref name="headers"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// HttpUtility.TryGetHeaderValues(headers, "content-type", out var values);
    /// </code>
    /// </example>
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

    /// <summary>
    /// Attempts to get a single, unambiguous trimmed value for the header with the specified name.
    /// </summary>
    /// <param name="headers">
    /// The HTTP headers to search, keyed by header name with one or more raw values per key. If
    /// <see langword="null"/> or no header matching <paramref name="name"/> is found, returns
    /// <see langword="false"/>.
    /// </param>
    /// <param name="name">
    /// The header name to look up. If <see langword="null"/> or whitespace, returns <see langword="false"/>.
    /// </param>
    /// <param name="value">
    /// When this method returns <see langword="true"/>, contains the single trimmed header value.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if exactly one non-empty (after trimming) value was found for
    /// <paramref name="name"/>; otherwise, <see langword="false"/>, including when the header has zero or
    /// more than one non-empty value.
    /// </returns>
    /// <example>
    /// <code>
    /// HttpUtility.TryGetSingleHeaderValue(headers, "x-request-id", out var value);
    /// </code>
    /// </example>
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
