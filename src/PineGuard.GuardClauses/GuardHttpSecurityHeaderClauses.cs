using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for HTTP security header validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/http-security-header">Guard HttpSecurityHeader documentation</seealso>
public static class GuardHttpSecurityHeaderClauses
{
    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotContentSecurityPolicyHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.ContentSecurityPolicyHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotContentSecurityPolicyHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.ContentSecurityPolicyHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotContentSecurityPolicyWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.ContentSecurityPolicyWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotContentSecurityPolicyWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.ContentSecurityPolicyWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotContentSecurityPolicy constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="requiredDefaultSrcValue">The required default-src directive value.</param>
    /// <param name="requiredObjectSrcValue">The required object-src directive value.</param>
    /// <param name="requiredBaseUriValue">The required base-uri directive value.</param>
    /// <param name="requiredFrameAncestorsValue">The required frame-ancestors directive value.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.ContentSecurityPolicy"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotContentSecurityPolicy(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? requiredDefaultSrcValue,
        string? requiredObjectSrcValue,
        string? requiredBaseUriValue,
        string? requiredFrameAncestorsValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.ContentSecurityPolicy(
            headers,
            requiredDefaultSrcValue,
            requiredObjectSrcValue,
            requiredBaseUriValue,
            requiredFrameAncestorsValue,
            paramName);

        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotStrictTransportSecurityHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.StrictTransportSecurityHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotStrictTransportSecurityHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.StrictTransportSecurityHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotStrictTransportSecurityWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.StrictTransportSecurityWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotStrictTransportSecurityWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.StrictTransportSecurityWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotStrictTransportSecurity constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="minMaxAgeSeconds">The minimum max-age value in seconds.</param>
    /// <param name="requireIncludeSubDomains">Whether includeSubDomains is required.</param>
    /// <param name="requirePreload">Whether preload is required.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.StrictTransportSecurity"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotStrictTransportSecurity(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        int minMaxAgeSeconds,
        bool requireIncludeSubDomains,
        bool requirePreload,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.StrictTransportSecurity(headers, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotXContentTypeOptionsHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.XContentTypeOptionsHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotXContentTypeOptionsHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.XContentTypeOptionsHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotXContentTypeOptionsWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.XContentTypeOptionsWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotXContentTypeOptionsWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.XContentTypeOptionsWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotXContentTypeOptions constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="expectedValue">The expected header value.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.XContentTypeOptions"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotXContentTypeOptions(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.XContentTypeOptions(headers, expectedValue, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotXFrameOptionsHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.XFrameOptionsHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotXFrameOptionsHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.XFrameOptionsHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotXFrameOptionsWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.XFrameOptionsWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotXFrameOptionsWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.XFrameOptionsWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotXFrameOptions constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="expectedValue">The expected header value.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.XFrameOptions"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotXFrameOptions(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.XFrameOptions(headers, expectedValue, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotReferrerPolicyHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.ReferrerPolicyHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotReferrerPolicyHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.ReferrerPolicyHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotReferrerPolicyWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.ReferrerPolicyWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotReferrerPolicyWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.ReferrerPolicyWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotReferrerPolicy constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="expectedValue">The expected header value.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.ReferrerPolicy"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotReferrerPolicy(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.ReferrerPolicy(headers, expectedValue, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotPermissionsPolicyHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.PermissionsPolicyHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotPermissionsPolicyHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.PermissionsPolicyHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotPermissionsPolicyWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.PermissionsPolicyWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotPermissionsPolicyWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.PermissionsPolicyWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotPermissionsPolicy constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="expectedValue">The expected header value.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.PermissionsPolicy"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotPermissionsPolicy(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? expectedValue,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.PermissionsPolicy(headers, expectedValue, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the NotPermissionsPolicyContaining constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="requiredFragments">The required policy fragments.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.PermissionsPolicyContaining"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotPermissionsPolicyContaining(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string[]? requiredFragments = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.PermissionsPolicyContaining(headers, requiredFragments);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the ContentSecurityPolicyHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotContentSecurityPolicyHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? ContentSecurityPolicyHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotContentSecurityPolicyHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the ContentSecurityPolicyWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotContentSecurityPolicyWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? ContentSecurityPolicyWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotContentSecurityPolicyWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the StrictTransportSecurityHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotStrictTransportSecurityHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? StrictTransportSecurityHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotStrictTransportSecurityHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the StrictTransportSecurityWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotStrictTransportSecurityWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? StrictTransportSecurityWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotStrictTransportSecurityWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the XContentTypeOptionsHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotXContentTypeOptionsHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? XContentTypeOptionsHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotXContentTypeOptionsHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the XContentTypeOptionsWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotXContentTypeOptionsWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? XContentTypeOptionsWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotXContentTypeOptionsWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the XFrameOptionsHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotXFrameOptionsHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? XFrameOptionsHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotXFrameOptionsHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the XFrameOptionsWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotXFrameOptionsWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? XFrameOptionsWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotXFrameOptionsWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the ReferrerPolicyHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotReferrerPolicyHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? ReferrerPolicyHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotReferrerPolicyHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the ReferrerPolicyWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotReferrerPolicyWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? ReferrerPolicyWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotReferrerPolicyWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the PermissionsPolicyHeader constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotPermissionsPolicyHeader"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? PermissionsPolicyHeader(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotPermissionsPolicyHeader(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> violates the PermissionsPolicyWithDefaults constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to guard.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustHttpSecurityHeaderClauses.NotPermissionsPolicyWithDefaults"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? PermissionsPolicyWithDefaults(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.NotPermissionsPolicyWithDefaults(headers, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
