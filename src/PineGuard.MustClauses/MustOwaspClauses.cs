using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate strings against OWASP security guidelines.
/// </summary>
/// <seealso cref="OwaspRules"/>
/// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
public static class MustOwaspClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified string is safe according to all OWASP checks (XSS, SQL injection, path traversal,
    /// command injection, CRLF, LDAP filter, open redirect, and SSRF scheme).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate for OWASP safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> passes all OWASP safety checks, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsOwaspSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be OWASP safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.OwaspSafe(userInput);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsOwaspSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> OwaspSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be OWASP safe.";

        var ok = OwaspRules.IsOwaspSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain XSS (Cross-Site Scripting) attack patterns.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate for XSS safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is free of XSS patterns, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsXssSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be XSS safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.XssSafe(htmlContent);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsXssSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> XssSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be XSS safe.";

        var ok = OwaspRules.IsXssSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain SQL injection patterns.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate for SQL injection safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is free of SQL injection patterns, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsSqlInjectionSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be SQL-injection safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.SqlInjectionSafe(searchQuery);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsSqlInjectionSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> SqlInjectionSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be SQL-injection safe.";

        var ok = OwaspRules.IsSqlInjectionSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain path traversal sequences (e.g., <c>../</c>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate for path traversal safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains no path traversal patterns, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsPathTraversalSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be path-traversal safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.PathTraversalSafe(filePath);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsPathTraversalSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> PathTraversalSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be path-traversal safe.";

        var ok = OwaspRules.IsPathTraversalSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain shell command injection patterns.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate for command injection safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is free of command injection patterns, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsCommandInjectionSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be command-injection safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.CommandInjectionSafe(shellArgument);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsCommandInjectionSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> CommandInjectionSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be command-injection safe.";

        var ok = OwaspRules.IsCommandInjectionSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain CRLF injection sequences (<c>\r\n</c>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate for CRLF injection safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains no CRLF sequences, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsCrLfSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be CRLF safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.CrLfSafe(headerValue);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsCrLfSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> CrLfSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be CRLF safe.";

        var ok = OwaspRules.IsCrLfSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain LDAP filter injection characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate for LDAP filter injection safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is free of LDAP filter injection characters, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsLdapFilterSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be LDAP-filter safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.LdapFilterSafe(searchFilter);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsLdapFilterSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> LdapFilterSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be LDAP-filter safe.";

        var ok = OwaspRules.IsLdapFilterSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified URL string does not facilitate an open-redirect attack.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The URL string to validate for open-redirect safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is safe from open-redirect exploitation, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsOpenRedirectSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be open-redirect safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.OpenRedirectSafe(redirectUrl);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsOpenRedirectSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> OpenRedirectSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be open-redirect safe.";

        var ok = OwaspRules.IsOpenRedirectSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified URL string uses only permitted schemes, guarding against SSRF (Server-Side Request Forgery) attacks.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The URL string to validate for SSRF scheme safety.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> uses a safe scheme, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="OwaspRules.IsSsrfSchemeSafe"/>. The failure message follows the pattern
    /// <c>"{paramName} must be SSRF-scheme safe."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.SsrfSchemeSafe(webhookUrl);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="OwaspRules.IsSsrfSchemeSafe"/>
    /// <seealso href="https://pineguard.ai/docs/must/owasp">OWASP Must Clauses documentation</seealso>
    public static MustResult<string> SsrfSchemeSafe(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        // ReSharper disable once StringLiteralTypo
        const string messageTemplate = "{paramName} must be SSRF-scheme safe.";

        var ok = OwaspRules.IsSsrfSchemeSafe(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    //--------------------------------------------------------------------------------------------
}
