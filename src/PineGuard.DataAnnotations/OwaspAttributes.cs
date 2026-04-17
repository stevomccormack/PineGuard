using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain characters
/// considered unsafe according to OWASP input validation guidelines.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.OwaspSafe"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [OwaspSafe]
///     public string UserInput { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="XssSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.OwaspSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
/// <seealso href="https://owasp.org/www-community/attacks/xss/">OWASP XSS prevention</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OwaspSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OwaspSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain patterns
/// associated with Cross-Site Scripting (XSS) attacks.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.XssSafe"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CommentModel
/// {
///     [XssSafe]
///     public string Body { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.XssSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class XssSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.XssSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain patterns
/// associated with SQL injection attacks.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.SqlInjectionSafe"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SearchModel
/// {
///     [SqlInjectionSafe]
///     public string Query { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.SqlInjectionSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SqlInjectionSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.SqlInjectionSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain path traversal
/// sequences (e.g., <c>../</c>, <c>..\</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.PathTraversalSafe"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FileModel
/// {
///     [PathTraversalSafe]
///     public string FileName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.PathTraversalSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PathTraversalSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.PathTraversalSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain OS command
/// injection characters or patterns.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.CommandInjectionSafe"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ShellModel
/// {
///     [CommandInjectionSafe]
///     public string Argument { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.CommandInjectionSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CommandInjectionSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CommandInjectionSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain CR/LF
/// (carriage-return / line-feed) injection sequences.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.CrLfSafe"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class HeaderModel
/// {
///     [CrLfSafe]
///     public string HeaderValue { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.CrLfSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CrLfSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CrLfSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain characters
/// that are unsafe in LDAP filter expressions.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.LdapFilterSafe"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DirectoryModel
/// {
///     [LdapFilterSafe]
///     public string SearchFilter { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.LdapFilterSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LdapFilterSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.LdapFilterSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not represent a URL that
/// could be used in an open-redirect attack.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.OpenRedirectSafe"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RedirectModel
/// {
///     [OpenRedirectSafe]
///     public string ReturnUrl { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.OpenRedirectSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OpenRedirectSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OpenRedirectSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a URL with a scheme that is
/// safe from Server-Side Request Forgery (SSRF) attacks (e.g., not <c>file://</c>, <c>gopher://</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustOwaspClauses.SsrfSchemeSafe"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WebhookModel
/// {
///     [SsrfSchemeSafe]
///     public string TargetUrl { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OwaspSafeAttribute"/>
/// <seealso cref="MustOwaspClauses.SsrfSchemeSafe"/>
/// <seealso href="https://pineguard.ai/docs/annotations/owasp">OWASP Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SsrfSchemeSafeAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.SsrfSchemeSafe(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
