using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate HTTP security headers,
/// delegating to <see cref="HttpSecurityHeaderRules"/> for core validation logic.
/// </summary>
/// <seealso cref="HttpSecurityHeaderRules"/>
/// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
public static class MustHttpSecurityHeaderClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified value must contain a Content-Security-Policy header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Content-Security-Policy header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> ContentSecurityPolicyHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ContentSecurityPolicy.Missing, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Content-Security-Policy header.";

        var ok = HttpSecurityHeaderRules.HasContentSecurityPolicyHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentSecurityPolicy.Missing, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Content-Security-Policy header with secure default values.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Content-Security-Policy header with secure default values."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> ContentSecurityPolicyWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ContentSecurityPolicy.Weak, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Content-Security-Policy header with secure default values.";

        var ok = HttpSecurityHeaderRules.HasContentSecurityPolicyWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentSecurityPolicy.Weak, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Content-Security-Policy header that meets requirements.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Content-Security-Policy header that meets requirements."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> ContentSecurityPolicy(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? requiredDefaultSrcValue,
        string? requiredObjectSrcValue,
        string? requiredBaseUriValue,
        string? requiredFrameAncestorsValue,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ContentSecurityPolicy.Weak, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Content-Security-Policy header that meets requirements.";

        var ok = HttpSecurityHeaderRules.HasContentSecurityPolicy(
            headers,
            requiredDefaultSrcValue,
            requiredObjectSrcValue,
            requiredBaseUriValue,
            requiredFrameAncestorsValue);

        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentSecurityPolicy.Weak, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Strict-Transport-Security header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Strict-Transport-Security header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> StrictTransportSecurityHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.StrictTransportSecurity.Missing, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Strict-Transport-Security header.";

        var ok = HttpSecurityHeaderRules.HasStrictTransportSecurityHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.StrictTransportSecurity.Missing, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Strict-Transport-Security header with secure default values.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Strict-Transport-Security header with secure default values."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> StrictTransportSecurityWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.StrictTransportSecurity.Weak, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Strict-Transport-Security header with secure default values.";

        var ok = HttpSecurityHeaderRules.HasStrictTransportSecurityWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.StrictTransportSecurity.Weak, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Strict-Transport-Security header that meets requirements.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Strict-Transport-Security header that meets requirements."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> StrictTransportSecurity(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        int minMaxAgeSeconds,
        bool requireIncludeSubDomains,
        bool requirePreload,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.StrictTransportSecurity.Weak, NullMessage, paramName, headers);

        if (minMaxAgeSeconds <= 0)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.StrictTransportSecurity.Weak, "{paramName} must be positive.", nameof(minMaxAgeSeconds), minMaxAgeSeconds);

        const string messageTemplate = "{paramName} must contain a Strict-Transport-Security header that meets requirements.";

        var ok = HttpSecurityHeaderRules.HasStrictTransportSecurity(headers, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.StrictTransportSecurity.Weak, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain an X-Content-Type-Options header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an X-Content-Type-Options header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> XContentTypeOptionsHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ContentTypeOptions.Missing, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain an X-Content-Type-Options header.";

        var ok = HttpSecurityHeaderRules.HasXContentTypeOptionsHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentTypeOptions.Missing, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain an X-Content-Type-Options header with secure default values.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an X-Content-Type-Options header with secure default values."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> XContentTypeOptionsWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ContentTypeOptions.Mismatch, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain an X-Content-Type-Options header with secure default values.";

        var ok = HttpSecurityHeaderRules.HasXContentTypeOptionsWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentTypeOptions.Mismatch, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain an X-Content-Type-Options header with the expected value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an X-Content-Type-Options header with the expected value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> XContentTypeOptions(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ContentTypeOptions.Mismatch, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain an X-Content-Type-Options header with the expected value.";

        var ok = HttpSecurityHeaderRules.HasXContentTypeOptions(headers, expectedValue);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentTypeOptions.Mismatch, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain an X-Frame-Options header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an X-Frame-Options header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> XFrameOptionsHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.FrameOptions.Missing, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain an X-Frame-Options header.";

        var ok = HttpSecurityHeaderRules.HasXFrameOptionsHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.FrameOptions.Missing, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain an X-Frame-Options header with secure default values.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an X-Frame-Options header with secure default values."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> XFrameOptionsWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.FrameOptions.Mismatch, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain an X-Frame-Options header with secure default values.";

        var ok = HttpSecurityHeaderRules.HasXFrameOptionsWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.FrameOptions.Mismatch, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain an X-Frame-Options header with the expected value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an X-Frame-Options header with the expected value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> XFrameOptions(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.FrameOptions.Mismatch, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain an X-Frame-Options header with the expected value.";

        var ok = HttpSecurityHeaderRules.HasXFrameOptions(headers, expectedValue);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.FrameOptions.Mismatch, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Referrer-Policy header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Referrer-Policy header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> ReferrerPolicyHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ReferrerPolicy.Missing, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Referrer-Policy header.";

        var ok = HttpSecurityHeaderRules.HasReferrerPolicyHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ReferrerPolicy.Missing, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Referrer-Policy header with secure default values.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Referrer-Policy header with secure default values."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> ReferrerPolicyWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ReferrerPolicy.Mismatch, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Referrer-Policy header with secure default values.";

        var ok = HttpSecurityHeaderRules.HasReferrerPolicyWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ReferrerPolicy.Mismatch, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Referrer-Policy header with the expected value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Referrer-Policy header with the expected value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> ReferrerPolicy(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.ReferrerPolicy.Mismatch, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Referrer-Policy header with the expected value.";

        var ok = HttpSecurityHeaderRules.HasReferrerPolicy(headers, expectedValue);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ReferrerPolicy.Mismatch, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Permissions-Policy header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Permissions-Policy header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicyHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.PermissionsPolicy.Missing, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Permissions-Policy header.";

        var ok = HttpSecurityHeaderRules.HasPermissionsPolicyHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.PermissionsPolicy.Missing, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Permissions-Policy header with secure default values.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Permissions-Policy header with secure default values."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicyWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.PermissionsPolicy.NotContains, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Permissions-Policy header with secure default values.";

        var ok = HttpSecurityHeaderRules.HasPermissionsPolicyWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.PermissionsPolicy.NotContains, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Permissions-Policy header with the expected value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Permissions-Policy header with the expected value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicy(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.PermissionsPolicy.Mismatch, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Permissions-Policy header with the expected value.";

        var ok = HttpSecurityHeaderRules.HasPermissionsPolicy(headers, expectedValue);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.PermissionsPolicy.Mismatch, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a Permissions-Policy header containing required fragments.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a Permissions-Policy header containing required fragments."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicyContaining(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string[]? requiredFragments,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        if (headers is null)
            return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Fail(
                MustCodes.Http.PermissionsPolicy.NotContains, NullMessage, paramName, headers);

        const string messageTemplate = "{paramName} must contain a Permissions-Policy header containing required fragments.";

        var ok = HttpSecurityHeaderRules.HasPermissionsPolicyContaining(headers, requiredFragments);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.PermissionsPolicy.NotContains, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Content-Security-Policy header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Content-Security-Policy header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotContentSecurityPolicyHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Content-Security-Policy header.";
        var ok = !HttpSecurityHeaderRules.HasContentSecurityPolicyHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentSecurityPolicy.Present, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Content-Security-Policy header with defaults.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Content-Security-Policy header with defaults."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotContentSecurityPolicyWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Content-Security-Policy header with defaults.";
        var ok = !HttpSecurityHeaderRules.HasContentSecurityPolicyWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentSecurityPolicy.Strong, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Strict-Transport-Security header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Strict-Transport-Security header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotStrictTransportSecurityHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Strict-Transport-Security header.";
        var ok = !HttpSecurityHeaderRules.HasStrictTransportSecurityHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.StrictTransportSecurity.Present, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Strict-Transport-Security header with defaults.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Strict-Transport-Security header with defaults."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotStrictTransportSecurityWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Strict-Transport-Security header with defaults.";
        var ok = !HttpSecurityHeaderRules.HasStrictTransportSecurityWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.StrictTransportSecurity.Strong, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain an X-Content-Type-Options header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain an X-Content-Type-Options header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotXContentTypeOptionsHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain an X-Content-Type-Options header.";
        var ok = !HttpSecurityHeaderRules.HasXContentTypeOptionsHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentTypeOptions.Present, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain an X-Content-Type-Options header with defaults.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain an X-Content-Type-Options header with defaults."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotXContentTypeOptionsWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain an X-Content-Type-Options header with defaults.";
        var ok = !HttpSecurityHeaderRules.HasXContentTypeOptionsWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ContentTypeOptions.Match, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain an X-Frame-Options header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain an X-Frame-Options header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotXFrameOptionsHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain an X-Frame-Options header.";
        var ok = !HttpSecurityHeaderRules.HasXFrameOptionsHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.FrameOptions.Present, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain an X-Frame-Options header with defaults.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain an X-Frame-Options header with defaults."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotXFrameOptionsWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain an X-Frame-Options header with defaults.";
        var ok = !HttpSecurityHeaderRules.HasXFrameOptionsWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.FrameOptions.Match, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Referrer-Policy header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Referrer-Policy header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotReferrerPolicyHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Referrer-Policy header.";
        var ok = !HttpSecurityHeaderRules.HasReferrerPolicyHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ReferrerPolicy.Present, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Referrer-Policy header with defaults.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Referrer-Policy header with defaults."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotReferrerPolicyWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Referrer-Policy header with defaults.";
        var ok = !HttpSecurityHeaderRules.HasReferrerPolicyWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.ReferrerPolicy.Match, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Permissions-Policy header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Permissions-Policy header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotPermissionsPolicyHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Permissions-Policy header.";
        var ok = !HttpSecurityHeaderRules.HasPermissionsPolicyHeader(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.PermissionsPolicy.Present, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a Permissions-Policy header with defaults.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a Permissions-Policy header with defaults."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http-security-header">Http Security Header Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotPermissionsPolicyWithDefaults(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a Permissions-Policy header with defaults.";
        var ok = !HttpSecurityHeaderRules.HasPermissionsPolicyWithDefaults(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(
            ok, MustCodes.Http.PermissionsPolicy.Contains, messageTemplate, paramName, headers, headers);
    }
}
