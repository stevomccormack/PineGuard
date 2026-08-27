using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for OWASP input-security validation (injection, XSS, traversal, and related attacks).
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/owasp">Guard OWASP Clauses documentation</seealso>
public static class GuardOwaspClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> contains any OWASP unsafe input patterns.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.OwaspSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains OWASP unsafe patterns and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.OwaspSafe"/>:
    /// <c>Guard.Against.OwaspUnsafe</c> passes when the value contains no unsafe patterns.
    /// Checks for XSS, SQL injection, path traversal, command injection, CRLF, LDAP filter, open redirect, and SSRF schemes.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.OwaspUnsafe(userInput);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.OwaspSafe"/>
    public static string OwaspUnsafe(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.OwaspSafe(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains potential XSS payloads.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.XssSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains XSS patterns and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.XssSafe"/>:
    /// <c>Guard.Against.Xss</c> passes when the value contains no XSS indicators.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Xss(htmlInput);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.XssSafe"/>
    public static string Xss(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.XssSafe(value, paramName); // Guard.Against.Xss => Must.Be.XssSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains potential SQL injection payloads.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.SqlInjectionSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains SQL injection patterns and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.SqlInjectionSafe"/>:
    /// <c>Guard.Against.SqlInjection</c> passes when the value is free of SQL injection indicators.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.SqlInjection(searchQuery);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.SqlInjectionSafe"/>
    public static string SqlInjection(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.SqlInjectionSafe(value, paramName); // Guard.Against.SqlInjection => Must.Be.SqlInjectionSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains path traversal sequences (e.g., <c>../</c>).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.PathTraversalSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains traversal sequences and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.PathTraversalSafe"/>:
    /// <c>Guard.Against.PathTraversal</c> passes when no traversal sequences are present.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.PathTraversal(filePath);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.PathTraversalSafe"/>
    public static string PathTraversal(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.PathTraversalSafe(value, paramName); // Guard.Against.PathTraversal => Must.Be.PathTraversalSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains command injection sequences.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.CommandInjectionSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains command injection patterns and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.CommandInjectionSafe"/>:
    /// <c>Guard.Against.CommandInjection</c> passes when no command injection sequences are present.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.CommandInjection(shellInput);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.CommandInjectionSafe"/>
    public static string CommandInjection(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.CommandInjectionSafe(value, paramName); // Guard.Against.CommandInjection => Must.Be.CommandInjectionSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains carriage-return/line-feed (CRLF) injection sequences.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.CrLfSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains CRLF sequences and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.CrLfSafe"/>:
    /// <c>Guard.Against.CrLf</c> passes when the value contains no CRLF injection characters.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.CrLf(headerValue);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.CrLfSafe"/>
    public static string CrLf(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.CrLfSafe(value, paramName); // Guard.Against.CrLf => Must.Be.CrLfSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains LDAP filter injection sequences.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.LdapFilterSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> contains LDAP filter patterns and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.LdapFilterSafe"/>:
    /// <c>Guard.Against.LdapFilter</c> passes when the value is safe for LDAP filter use.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.LdapFilter(ldapInput);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.LdapFilterSafe"/>
    public static string LdapFilter(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.LdapFilterSafe(value, paramName); // Guard.Against.LdapFilter => Must.Be.LdapFilterSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is a potential open-redirect target URL.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The URL string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.OpenRedirectSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is an open-redirect risk and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.OpenRedirectSafe"/>:
    /// <c>Guard.Against.OpenRedirect</c> passes when the URL is not an open redirect vector.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.OpenRedirect(returnUrl);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.OpenRedirectSafe"/>
    public static string OpenRedirect(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.OpenRedirectSafe(value, paramName); // Guard.Against.OpenRedirect => Must.Be.OpenRedirectSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> uses a URI scheme associated with SSRF risk (e.g., <c>file://</c>, <c>gopher://</c>).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The URL or URI string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustOwaspClauses.SsrfSchemeSafe"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> uses a dangerous scheme and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustOwaspClauses.SsrfSchemeSafe"/>:
    /// <c>Guard.Against.SsrfScheme</c> passes when the scheme is not an SSRF risk.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.SsrfScheme(externalUrl);
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.SsrfSchemeSafe"/>
    public static string SsrfScheme(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.SsrfSchemeSafe(value, paramName); // Guard.Against.SsrfScheme => Must.Be.SsrfSchemeSafe (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    //------------------------------------------------------------------------------------------------
}
