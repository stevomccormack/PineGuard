using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for HTTP security header validation including
/// Content-Security-Policy, Strict-Transport-Security, X-Content-Type-Options, X-Frame-Options,
/// Referrer-Policy, and Permissions-Policy headers.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/http-security">Fluent HTTP Security Header Extensions documentation</seealso>
public static class FluentHttpSecurityHeaderExtensions
{
    /// <summary>Validates that the header collection contains a Content-Security-Policy header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.Headers).ContentSecurityPolicyHeader();</code></example>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ContentSecurityPolicyHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.ContentSecurityPolicyHeader(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Content-Security-Policy header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotContentSecurityPolicyHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotContentSecurityPolicyHeader(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Content-Security-Policy header with recommended default directives.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ContentSecurityPolicyWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.ContentSecurityPolicyWithDefaults(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Content-Security-Policy header with recommended default directives.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotContentSecurityPolicyWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotContentSecurityPolicyWithDefaults(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Content-Security-Policy header with the specified directive values.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="requiredDefaultSrcValue">The required default-src directive value.</param>
    /// <param name="requiredObjectSrcValue">The required object-src directive value.</param>
    /// <param name="requiredBaseUriValue">The required base-uri directive value.</param>
    /// <param name="requiredFrameAncestorsValue">The required frame-ancestors directive value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ContentSecurityPolicy<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? requiredDefaultSrcValue,
        string? requiredObjectSrcValue,
        string? requiredBaseUriValue,
        string? requiredFrameAncestorsValue,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.ContentSecurityPolicy(val, requiredDefaultSrcValue, requiredObjectSrcValue, requiredBaseUriValue, requiredFrameAncestorsValue, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection contains a Strict-Transport-Security header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> StrictTransportSecurityHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.StrictTransportSecurityHeader(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Strict-Transport-Security header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotStrictTransportSecurityHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotStrictTransportSecurityHeader(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Strict-Transport-Security header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> StrictTransportSecurityWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.StrictTransportSecurityWithDefaults(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Strict-Transport-Security header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotStrictTransportSecurityWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotStrictTransportSecurityWithDefaults(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Strict-Transport-Security header with the specified constraints.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="minMaxAgeSeconds">The minimum required max-age value in seconds.</param>
    /// <param name="requireIncludeSubDomains">Whether the includeSubDomains directive is required.</param>
    /// <param name="requirePreload">Whether the preload directive is required.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> StrictTransportSecurity<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        int minMaxAgeSeconds,
        bool requireIncludeSubDomains,
        bool requirePreload,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.StrictTransportSecurity(val, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection contains an X-Content-Type-Options header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> XContentTypeOptionsHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.XContentTypeOptionsHeader(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain an X-Content-Type-Options header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotXContentTypeOptionsHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotXContentTypeOptionsHeader(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains an X-Content-Type-Options header with the recommended "nosniff" value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> XContentTypeOptionsWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.XContentTypeOptionsWithDefaults(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain an X-Content-Type-Options header with the recommended "nosniff" value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotXContentTypeOptionsWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotXContentTypeOptionsWithDefaults(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains an X-Content-Type-Options header with the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="expectedValue">The expected header value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> XContentTypeOptions<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? expectedValue,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.XContentTypeOptions(val, expectedValue, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection contains an X-Frame-Options header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> XFrameOptionsHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.XFrameOptionsHeader(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain an X-Frame-Options header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotXFrameOptionsHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotXFrameOptionsHeader(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains an X-Frame-Options header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> XFrameOptionsWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.XFrameOptionsWithDefaults(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain an X-Frame-Options header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotXFrameOptionsWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotXFrameOptionsWithDefaults(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains an X-Frame-Options header with the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="expectedValue">The expected header value (e.g., "DENY" or "SAMEORIGIN").</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> XFrameOptions<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? expectedValue,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.XFrameOptions(val, expectedValue, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection contains a Referrer-Policy header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ReferrerPolicyHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.ReferrerPolicyHeader(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Referrer-Policy header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotReferrerPolicyHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotReferrerPolicyHeader(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Referrer-Policy header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ReferrerPolicyWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.ReferrerPolicyWithDefaults(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Referrer-Policy header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotReferrerPolicyWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotReferrerPolicyWithDefaults(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Referrer-Policy header with the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="expectedValue">The expected Referrer-Policy value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ReferrerPolicy<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? expectedValue,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.ReferrerPolicy(val, expectedValue, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection contains a Permissions-Policy header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicyHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.PermissionsPolicyHeader(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Permissions-Policy header.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotPermissionsPolicyHeader<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotPermissionsPolicyHeader(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Permissions-Policy header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicyWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.PermissionsPolicyWithDefaults(val, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection does not contain a Permissions-Policy header with recommended defaults.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> NotPermissionsPolicyWithDefaults<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotPermissionsPolicyWithDefaults(val, paramName: null),
            message);

    /// <summary>Validates that the header collection contains a Permissions-Policy header with the specified value.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="expectedValue">The expected Permissions-Policy value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicy<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? expectedValue,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.PermissionsPolicy(val, expectedValue, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);

    /// <summary>Validates that the header collection contains a Permissions-Policy header containing the specified policy fragments.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="requiredFragments">The required policy directive fragments.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> PermissionsPolicyContaining<TModel>(this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string[]? requiredFragments,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null
                ? Must.Be.PermissionsPolicyContaining(val, requiredFragments, paramName: null)
                : MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.Ok(null),
            message);
}
