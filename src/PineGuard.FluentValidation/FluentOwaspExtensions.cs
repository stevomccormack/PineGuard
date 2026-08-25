using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for OWASP security input validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/owasp">Fluent OWASP Extensions documentation</seealso>
public static class FluentOwaspExtensions
{
    /// <summary>
    /// Validates that the property value is safe according to OWASP general input guidelines.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.OwaspSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UserInput).OwaspSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.OwaspSafe"/>
    public static IRuleBuilderOptions<TModel, string?> OwaspSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OwaspSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain cross-site scripting (XSS) patterns.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.XssSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Comment).XssSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.XssSafe"/>
    public static IRuleBuilderOptions<TModel, string?> XssSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.XssSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain SQL injection patterns.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.SqlInjectionSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SearchQuery).SqlInjectionSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.SqlInjectionSafe"/>
    public static IRuleBuilderOptions<TModel, string?> SqlInjectionSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.SqlInjectionSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain path traversal sequences.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.PathTraversalSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FilePath).PathTraversalSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.PathTraversalSafe"/>
    public static IRuleBuilderOptions<TModel, string?> PathTraversalSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PathTraversalSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain OS command injection patterns.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.CommandInjectionSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Command).CommandInjectionSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.CommandInjectionSafe"/>
    public static IRuleBuilderOptions<TModel, string?> CommandInjectionSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.CommandInjectionSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain CRLF injection sequences (<c>\r\n</c>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.CrLfSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.HeaderValue).CrLfSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.CrLfSafe"/>
    public static IRuleBuilderOptions<TModel, string?> CrLfSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.CrLfSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain LDAP filter injection characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.LdapFilterSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LdapQuery).LdapFilterSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.LdapFilterSafe"/>
    public static IRuleBuilderOptions<TModel, string?> LdapFilterSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.LdapFilterSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value does not contain open redirect patterns.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.OpenRedirectSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.RedirectUrl).OpenRedirectSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.OpenRedirectSafe"/>
    public static IRuleBuilderOptions<TModel, string?> OpenRedirectSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OpenRedirectSafe(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value uses a scheme that is safe against Server-Side Request Forgery (SSRF) attacks.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustOwaspClauses.SsrfSchemeSafe"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.WebhookUrl).SsrfSchemeSafe();
    /// </code>
    /// </example>
    /// <seealso cref="MustOwaspClauses.SsrfSchemeSafe"/>
    public static IRuleBuilderOptions<TModel, string?> SsrfSchemeSafe<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.SsrfSchemeSafe(val, paramName: null),
            message);
}
