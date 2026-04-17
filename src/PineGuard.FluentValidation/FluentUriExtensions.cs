using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for URI and URL property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/uri">Fluent URI Extensions documentation</seealso>
public static class FluentUriExtensions
{
    /// <summary>
    /// Validates that the string value is an absolute URI.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.AbsoluteUri"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Endpoint).AbsoluteUri();</code></example>
    /// <seealso cref="MustUriClauses.AbsoluteUri"/>
    public static IRuleBuilderOptions<TModel, string?> AbsoluteUri<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.AbsoluteUri(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a relative URI.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.RelativeUri"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Path).RelativeUri();</code></example>
    /// <seealso cref="MustUriClauses.RelativeUri"/>
    public static IRuleBuilderOptions<TModel, string?> RelativeUri<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.RelativeUri(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a valid web URL (HTTP or HTTPS).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.Url"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Website).WebUrl();</code></example>
    /// <seealso cref="MustUriClauses.Url"/>
    public static IRuleBuilderOptions<TModel, string?> WebUrl<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Url(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a valid web URL. Alias for <see cref="WebUrl{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="WebUrl{TModel}"/>.</remarks>
    /// <example><code>RuleFor(x => x.Website).Url();</code></example>
    public static IRuleBuilderOptions<TModel, string?> Url<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.WebUrl(message);

    /// <summary>
    /// Validates that the string value is a valid HTTPS URL.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.HttpsUrl"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.SecureEndpoint).HttpsUrl();</code></example>
    /// <seealso cref="MustUriClauses.HttpsUrl"/>
    public static IRuleBuilderOptions<TModel, string?> HttpsUrl<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HttpsUrl(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a valid HTTP URL.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.HttpUrl"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.LegacyEndpoint).HttpUrl();</code></example>
    /// <seealso cref="MustUriClauses.HttpUrl"/>
    public static IRuleBuilderOptions<TModel, string?> HttpUrl<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HttpUrl(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a valid file:// URI.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.FileUri"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.LocalResource).FileUri();</code></example>
    /// <seealso cref="MustUriClauses.FileUri"/>
    public static IRuleBuilderOptions<TModel, string?> FileUri<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FileUri(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a valid file path.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.FilePath"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.OutputPath).FilePath();</code></example>
    /// <seealso cref="MustUriClauses.FilePath"/>
    public static IRuleBuilderOptions<TModel, string?> FilePath<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.FilePath(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is not a valid file path.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.NotFilePath"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Identifier).NotFilePath();</code></example>
    /// <seealso cref="MustUriClauses.NotFilePath"/>
    public static IRuleBuilderOptions<TModel, string?> NotFilePath<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotFilePath(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the URI string has the specified scheme.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="scheme">The expected URI scheme (e.g., "https", "ftp").</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.HasScheme"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Endpoint).HasScheme("https");</code></example>
    /// <seealso cref="MustUriClauses.HasScheme"/>
    public static IRuleBuilderOptions<TModel, string?> HasScheme<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string scheme,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasScheme(val, scheme, paramName: null),
            message);

    /// <summary>
    /// Validates that the URI string does not have the specified scheme.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="scheme">The disallowed URI scheme.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustUriClauses.NotHasScheme"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Endpoint).NotHasScheme("http");</code></example>
    /// <seealso cref="MustUriClauses.NotHasScheme"/>
    public static IRuleBuilderOptions<TModel, string?> NotHasScheme<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string scheme,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasScheme(val, scheme, paramName: null),
            message);
}
