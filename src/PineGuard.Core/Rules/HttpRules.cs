using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure HTTP validation predicates for headers, status codes, and content types.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/http">HTTP Rules documentation</seealso>
public static partial class HttpRules
{
    /// <summary>
    /// Determines whether the specified value is a valid HTTP header name (RFC 7230 token characters only).
    /// </summary>
    /// <param name="name">The header name to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="name"/> contains only valid token characters; otherwise, <see langword="false"/>.</returns>
    public static bool IsHeaderName(string? name) =>
        StringUtility.TryGetTrimmed(name, out var trimmed) && trimmed.All(IsTokenChar);

    /// <summary>
    /// Determines whether the specified value is a valid HTTP header value (no CR, LF, or control characters).
    /// </summary>
    /// <param name="value">The header value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> contains no control characters; otherwise, <see langword="false"/>.</returns>
    public static bool IsHeaderValue(string? value)
    {
        if (value is null)
            return false;

        foreach (var ch in value)
        {
            if (ch is '\r' or '\n')
                return false;

            if (char.IsControl(ch))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether the specified value is a valid HTTP status code (100–599).
    /// </summary>
    /// <param name="status">The status code to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="status"/> is between 100 and 599 inclusive; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpStatusCode(int? status) =>
        status is >= 100 and <= 599;

    /// <summary>
    /// Determines whether the specified status code is informational (1xx).
    /// </summary>
    /// <param name="status">The status code to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="status"/> is between 100 and 199; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpStatusInformational(int? status) =>
        status is >= 100 and <= 199;

    /// <summary>
    /// Determines whether the specified status code indicates success (2xx).
    /// </summary>
    /// <param name="status">The status code to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="status"/> is between 200 and 299; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpStatusSuccess(int? status) =>
        status is >= 200 and <= 299;

    /// <summary>
    /// Determines whether the specified status code indicates a redirect (3xx).
    /// </summary>
    /// <param name="status">The status code to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="status"/> is between 300 and 399; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpStatusRedirect(int? status) =>
        status is >= 300 and <= 399;

    /// <summary>
    /// Determines whether the specified status code indicates a client error (4xx).
    /// </summary>
    /// <param name="status">The status code to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="status"/> is between 400 and 499; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpStatusClientError(int? status) =>
        status is >= 400 and <= 499;

    /// <summary>
    /// Determines whether the specified status code indicates a server error (5xx).
    /// </summary>
    /// <param name="status">The status code to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="status"/> is between 500 and 599; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpStatusServerError(int? status) =>
        status is >= 500 and <= 599;

    /// <summary>
    /// Determines whether the specified headers collection contains a header with the given name.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="name">The header name to look for. Case-insensitive.</param>
    /// <returns><see langword="true"/> if the header is present; otherwise, <see langword="false"/>.</returns>
    public static bool HasHeader(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name) =>
        HttpUtility.TryGetHeaderValues(headers, name, out _);

    /// <summary>
    /// Determines whether the specified header has at least one non-whitespace value.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="name">The header name to look for.</param>
    /// <returns><see langword="true"/> if the header has a non-empty value; otherwise, <see langword="false"/>.</returns>
    public static bool HasHeaderValue(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)
    {
        if (!HttpUtility.TryGetHeaderValues(headers, name, out var values) || values is null)
            return false;

        return values.Any(candidate => StringUtility.TryGetTrimmed(candidate, out _));
    }

    /// <summary>
    /// Determines whether the specified header contains the expected value.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="name">The header name to look for.</param>
    /// <param name="expectedValue">The expected header value. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="comparison">The string comparison type. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <returns><see langword="true"/> if the header has the expected value; otherwise, <see langword="false"/>.</returns>
    public static bool HasHeaderValue(
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        string? expectedValue,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        if (!StringUtility.TryGetTrimmed(expectedValue, out var trimmedExpected))
            return false;

        if (!HttpUtility.TryGetHeaderValues(headers, name, out var values) || values is null)
            return false;

        foreach (var candidate in values)
        {
            if (!StringUtility.TryGetTrimmed(candidate, out var trimmedCandidate))
                continue;

            if (string.Equals(trimmedCandidate, trimmedExpected, comparison))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified header has exactly one value.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="name">The header name to look for.</param>
    /// <returns><see langword="true"/> if the header has exactly one value; otherwise, <see langword="false"/>.</returns>
    public static bool HasSingleHeaderValue(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name) =>
        HttpUtility.TryGetSingleHeaderValue(headers, name, out _);

    /// <summary>
    /// Determines whether the <c>Content-Type</c> header matches one of the specified allowed media types.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="allowed">The set of allowed media types. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the content type matches an allowed type; otherwise, <see langword="false"/>.</returns>
    public static bool HasContentType(IReadOnlyDictionary<string, IEnumerable<string>>? headers, params string[]? allowed)
    {
        if (allowed is null || allowed.Length == 0)
            return false;

        if (!HttpContentTypeUtility.TryGetContentTypeMediaTypes(headers, out var mediaTypes) || mediaTypes is null)
            return false;

        foreach (var mediaType in mediaTypes)
        {
            foreach (var candidateAllowed in allowed)
            {
                if (!StringUtility.TryGetTrimmed(candidateAllowed, out var normalizedAllowed))
                    continue;

                if (string.Equals(mediaType, normalizedAllowed, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }

        return false;
    }

    private static bool IsTokenChar(char ch) =>
        ch switch
        {
            // RFC7230 token tchar: "!#$%&'*+-.^_`|~" plus digits and letters
            >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' => true,
            _ => ch is '!' or '#' or '$' or '%' or '&' or '\'' or '*' or '+' or '-' or '.' or '^' or '_' or '`' or '|' or '~'
        };
}
