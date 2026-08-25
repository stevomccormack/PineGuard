using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides HTTP security header validation predicates for common security response headers.
/// </summary>
/// <remarks>
/// Validates presence and correctness of headers such as Content-Security-Policy, Strict-Transport-Security,
/// X-Content-Type-Options, X-Frame-Options, Referrer-Policy, and Permissions-Policy.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/http-security-headers">HTTP Security Header Rules documentation</seealso>
/// <seealso href="https://owasp.org/www-project-secure-headers/">OWASP Secure Headers Project</seealso>
public static class HttpSecurityHeaderRules
{
    /// <summary>
    /// The default expected value for the <c>X-Content-Type-Options</c> header.
    /// </summary>
    public const string DefaultXContentTypeOptionsValue = "nosniff";

    /// <summary>
    /// The default expected value for the <c>X-Frame-Options</c> header.
    /// </summary>
    public const string DefaultXFrameOptionsValue = "DENY";

    /// <summary>
    /// The default expected value for the <c>Referrer-Policy</c> header.
    /// </summary>
    public const string DefaultReferrerPolicyValue = "strict-origin-when-cross-origin";

    /// <summary>
    /// The default minimum <c>max-age</c> value in seconds for Strict-Transport-Security (365 days).
    /// </summary>
    public const int DefaultStrictTransportSecurityMinMaxAgeSeconds = 31_536_000; // 365 days

    /// <summary>
    /// The default required <c>default-src</c> directive value for Content-Security-Policy.
    /// </summary>
    public const string DefaultContentSecurityPolicyDefaultSrcValue = "'self'";

    /// <summary>
    /// The default required <c>object-src</c> directive value for Content-Security-Policy.
    /// </summary>
    public const string DefaultContentSecurityPolicyObjectSrcValue = "'none'";

    /// <summary>
    /// The default required <c>base-uri</c> directive value for Content-Security-Policy.
    /// </summary>
    public const string DefaultContentSecurityPolicyBaseUriValue = "'self'";

    /// <summary>
    /// The default required <c>frame-ancestors</c> directive value for Content-Security-Policy.
    /// </summary>
    public const string DefaultContentSecurityPolicyFrameAncestorsValue = "'none'";

    /// <summary>
    /// Checks whether the specified headers contain a <c>Content-Security-Policy</c> header.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header is present; otherwise, <see langword="false"/>.</returns>
    public static bool HasContentSecurityPolicyHeader(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HttpRules.HasHeader(headers, "Content-Security-Policy");

    /// <summary>
    /// Checks whether the specified headers contain a <c>Content-Security-Policy</c> header satisfying default directive values.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the CSP header satisfies default directives; otherwise, <see langword="false"/>.</returns>
    public static bool HasContentSecurityPolicyWithDefaults(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HasContentSecurityPolicy(
            headers,
            requiredDefaultSrcValue: DefaultContentSecurityPolicyDefaultSrcValue,
            requiredObjectSrcValue: DefaultContentSecurityPolicyObjectSrcValue,
            requiredBaseUriValue: DefaultContentSecurityPolicyBaseUriValue,
            requiredFrameAncestorsValue: DefaultContentSecurityPolicyFrameAncestorsValue);

    /// <summary>
    /// Checks whether the specified headers contain a <c>Content-Security-Policy</c> header satisfying the given directive values.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="requiredDefaultSrcValue">The required <c>default-src</c> directive value. If <see langword="null"/> or whitespace, the directive is not checked.</param>
    /// <param name="requiredObjectSrcValue">The required <c>object-src</c> directive value. If <see langword="null"/> or whitespace, the directive is not checked.</param>
    /// <param name="requiredBaseUriValue">The required <c>base-uri</c> directive value. If <see langword="null"/> or whitespace, the directive is not checked.</param>
    /// <param name="requiredFrameAncestorsValue">The required <c>frame-ancestors</c> directive value. If <see langword="null"/> or whitespace, the directive is not checked.</param>
    /// <returns><see langword="true"/> if the CSP header satisfies all specified directives; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Only the first occurrence of a given directive name in the header value is evaluated; per the CSP
    /// specification, browsers ignore any later duplicate of a directive that already appeared. When the
    /// required value is the keyword-source <c>'none'</c>, it is only treated as satisfied when it is the
    /// sole source in that directive's value, since a source list such as <c>'none' https://example.com</c>
    /// is enforced by browsers as if <c>'none'</c> were absent.
    /// </remarks>
    public static bool HasContentSecurityPolicy(
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? requiredDefaultSrcValue,
        string? requiredObjectSrcValue,
        string? requiredBaseUriValue,
        string? requiredFrameAncestorsValue)
    {
        if (!HttpUtility.TryGetHeaderValues(headers, "Content-Security-Policy", out var values) || values is null)
            return false;

        foreach (var candidate in values)
        {
            if (!StringUtility.TryGetTrimmed(candidate, out var trimmed)) continue;

            if (IsContentSecurityPolicySatisfied(
                    trimmed,
                    requiredDefaultSrcValue,
                    requiredObjectSrcValue,
                    requiredBaseUriValue,
                    requiredFrameAncestorsValue))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the specified headers contain a <c>Strict-Transport-Security</c> header.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header is present; otherwise, <see langword="false"/>.</returns>
    public static bool HasStrictTransportSecurityHeader(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HttpRules.HasHeader(headers, "Strict-Transport-Security");

    /// <summary>
    /// Checks whether the specified headers contain a <c>Strict-Transport-Security</c> header satisfying default requirements.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the HSTS header satisfies default requirements; otherwise, <see langword="false"/>.</returns>
    public static bool HasStrictTransportSecurityWithDefaults(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HasStrictTransportSecurity(
            headers,
            minMaxAgeSeconds: DefaultStrictTransportSecurityMinMaxAgeSeconds,
            requireIncludeSubDomains: true,
            requirePreload: false);

    /// <summary>
    /// Checks whether the specified headers contain a <c>Strict-Transport-Security</c> header satisfying the given requirements.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="minMaxAgeSeconds">The minimum required <c>max-age</c> value in seconds. If zero or negative, returns <see langword="false"/>.</param>
    /// <param name="requireIncludeSubDomains">Whether the <c>includeSubDomains</c> directive is required.</param>
    /// <param name="requirePreload">Whether the <c>preload</c> directive is required.</param>
    /// <returns><see langword="true"/> if the HSTS header satisfies all requirements; otherwise, <see langword="false"/>.</returns>
    public static bool HasStrictTransportSecurity(
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        int minMaxAgeSeconds,
        bool requireIncludeSubDomains,
        bool requirePreload)
    {
        if (minMaxAgeSeconds <= 0)
            return false;

        if (!HttpUtility.TryGetHeaderValues(headers, "Strict-Transport-Security", out var values) || values is null)
            return false;

        foreach (var candidate in values)
        {
            if (!StringUtility.TryGetTrimmed(candidate, out var trimmed)) continue;

            if (IsStrictTransportSecuritySatisfied(trimmed, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the specified headers contain an <c>X-Content-Type-Options</c> header.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header is present; otherwise, <see langword="false"/>.</returns>
    public static bool HasXContentTypeOptionsHeader(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HttpRules.HasHeader(headers, "X-Content-Type-Options");

    /// <summary>
    /// Checks whether the specified headers contain an <c>X-Content-Type-Options</c> header with the default value (<c>nosniff</c>).
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header has the default value; otherwise, <see langword="false"/>.</returns>
    public static bool HasXContentTypeOptionsWithDefaults(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HasXContentTypeOptions(headers, DefaultXContentTypeOptionsValue);

    /// <summary>
    /// Checks whether the specified headers contain an <c>X-Content-Type-Options</c> header with the expected value.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="expectedValue">The expected header value. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header matches the expected value; otherwise, <see langword="false"/>.</returns>
    public static bool HasXContentTypeOptions(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) =>
        HttpRules.HasHeaderValue(headers, "X-Content-Type-Options", expectedValue);

    /// <summary>
    /// Checks whether the specified headers contain an <c>X-Frame-Options</c> header.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header is present; otherwise, <see langword="false"/>.</returns>
    public static bool HasXFrameOptionsHeader(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HttpRules.HasHeader(headers, "X-Frame-Options");

    /// <summary>
    /// Checks whether the specified headers contain an <c>X-Frame-Options</c> header with the default value (<c>DENY</c>).
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header has the default value; otherwise, <see langword="false"/>.</returns>
    public static bool HasXFrameOptionsWithDefaults(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HasXFrameOptions(headers, DefaultXFrameOptionsValue);

    /// <summary>
    /// Checks whether the specified headers contain an <c>X-Frame-Options</c> header with the expected value.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="expectedValue">The expected header value. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header matches the expected value; otherwise, <see langword="false"/>.</returns>
    public static bool HasXFrameOptions(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) =>
        HttpRules.HasHeaderValue(headers, "X-Frame-Options", expectedValue);

    /// <summary>
    /// Checks whether the specified headers contain a <c>Referrer-Policy</c> header.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header is present; otherwise, <see langword="false"/>.</returns>
    public static bool HasReferrerPolicyHeader(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HttpRules.HasHeader(headers, "Referrer-Policy");

    /// <summary>
    /// Checks whether the specified headers contain a <c>Referrer-Policy</c> header with the default value (<c>strict-origin-when-cross-origin</c>).
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header has the default value; otherwise, <see langword="false"/>.</returns>
    public static bool HasReferrerPolicyWithDefaults(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HasReferrerPolicy(headers, DefaultReferrerPolicyValue);

    /// <summary>
    /// Checks whether the specified headers contain a <c>Referrer-Policy</c> header with the expected value.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="expectedValue">The expected header value. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header matches the expected value; otherwise, <see langword="false"/>.</returns>
    public static bool HasReferrerPolicy(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) =>
        HttpRules.HasHeaderValue(headers, "Referrer-Policy", expectedValue);

    /// <summary>
    /// Checks whether the specified headers contain a <c>Permissions-Policy</c> header.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header is present; otherwise, <see langword="false"/>.</returns>
    public static bool HasPermissionsPolicyHeader(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HttpRules.HasHeader(headers, "Permissions-Policy");

    /// <summary>
    /// Checks whether the specified headers contain a <c>Permissions-Policy</c> header containing default restriction fragments
    /// (<c>geolocation=()</c>, <c>microphone=()</c>, <c>camera=()</c>).
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header contains all default restriction fragments; otherwise, <see langword="false"/>.</returns>
    public static bool HasPermissionsPolicyWithDefaults(IReadOnlyDictionary<string, IEnumerable<string>>? headers) =>
        HasPermissionsPolicyContaining(headers, "geolocation=()", "microphone=()", "camera=()");

    /// <summary>
    /// Checks whether the specified headers contain a <c>Permissions-Policy</c> header with the expected value.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="expectedValue">The expected header value. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header matches the expected value; otherwise, <see langword="false"/>.</returns>
    public static bool HasPermissionsPolicy(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) =>
        HttpRules.HasHeaderValue(headers, "Permissions-Policy", expectedValue);

    /// <summary>
    /// Checks whether the specified headers contain a <c>Permissions-Policy</c> header that includes all required fragments.
    /// </summary>
    /// <param name="headers">The HTTP headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="requiredFragments">The policy fragments that must be present. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the header contains all required fragments; otherwise, <see langword="false"/>.</returns>
    public static bool HasPermissionsPolicyContaining(IReadOnlyDictionary<string, IEnumerable<string>>? headers, params string[]? requiredFragments)
    {
        if (requiredFragments is null) return false;

        if (!HttpUtility.TryGetHeaderValues(headers, "Permissions-Policy", out var values) || values is null)
            return false;

        foreach (var candidate in values)
        {
            if (!StringUtility.TryGetTrimmed(candidate, out var trimmed)) continue;

            var ok = true;

            foreach (var fragment in requiredFragments)
            {
                if (StringUtility.TryGetTrimmed(fragment, out var trimmedFragment) && trimmed.Contains(trimmedFragment, StringComparison.OrdinalIgnoreCase))
                    continue;

                ok = false;
                break;
            }

            if (ok)
                return true;
        }

        return false;
    }

    private static bool IsStrictTransportSecuritySatisfied(
        string headerValue,
        int minMaxAgeSeconds,
        bool requireIncludeSubDomains,
        bool requirePreload)
    {
        if (!HttpSecurityHeaderUtility.TrySplitSemicolonSeparatedSegments(headerValue, out var segments) || segments is null)
            return false;

        var directives = HttpSecurityHeaderUtility.ParseHstsDirectives(segments);

        if (directives.MaxAgeSeconds is null || directives.MaxAgeSeconds < minMaxAgeSeconds)
            return false;

        if (requireIncludeSubDomains && !directives.IncludeSubDomains)
            return false;

        return !requirePreload || directives.Preload;
    }

    private static bool IsContentSecurityPolicySatisfied(
        string headerValue,
        string? requiredDefaultSrcValue,
        string? requiredObjectSrcValue,
        string? requiredBaseUriValue,
        string? requiredFrameAncestorsValue)
    {
        if (!HttpSecurityHeaderUtility.TrySplitSemicolonSeparatedSegments(headerValue, out var segments) || segments is null)
            return false;

        if (!HasCspDirective(segments, "default-src", requiredDefaultSrcValue))
            return false;

        if (!HasCspDirective(segments, "object-src", requiredObjectSrcValue))
            return false;

        if (!HasCspDirective(segments, "base-uri", requiredBaseUriValue))
            return false;

#pragma warning disable IDE0046
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!HasCspDirective(segments, "frame-ancestors", requiredFrameAncestorsValue))
#pragma warning restore IDE0046
            return false;

        return true;
    }

    private const string NoneSourceKeyword = "'none'";

    private static bool HasCspDirective(IReadOnlyList<string> segments, string directiveName, string? requiredValue)
    {
        if (!StringUtility.TryGetTrimmed(requiredValue, out var trimmedRequired))
            return true;

        var requiresSoleNone = string.Equals(trimmedRequired, NoneSourceKeyword, StringComparison.OrdinalIgnoreCase);

        // CSP mandates that only the first occurrence of a directive name is enforced by the user agent;
        // any later duplicate of the same directive is ignored. Stop at the first matching segment instead
        // of scanning every segment, so a duplicate cannot be used to satisfy a requirement the first
        // (authoritative) occurrence does not actually meet.
        foreach (var segment in segments)
        {
            if (!segment.StartsWith(directiveName, StringComparison.OrdinalIgnoreCase))
                continue;

            if (segment.Length > directiveName.Length && !char.IsWhiteSpace(segment[directiveName.Length]))
                continue;

            var remainder = segment.Length == directiveName.Length ? string.Empty : segment[directiveName.Length..].Trim();
            if (remainder.Length == 0)
                return false;

            var tokens = remainder.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

            // 'none' only has meaning as the sole source in the list; combined with other sources it is
            // ignored by the user agent and the remaining sources are enforced instead, so a match requires
            // 'none' to be the only token present rather than merely one of several.
            if (requiresSoleNone)
                return tokens.Length == 1 && string.Equals(tokens[0], trimmedRequired, StringComparison.OrdinalIgnoreCase);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var token in tokens)
            {
                if (string.Equals(token, trimmedRequired, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        return false;
    }
}
