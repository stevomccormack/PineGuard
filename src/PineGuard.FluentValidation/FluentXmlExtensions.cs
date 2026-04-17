using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for XML content validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/xml">Fluent XML Extensions documentation</seealso>
public static class FluentXmlExtensions
{
    /// <summary>
    /// Validates that the property value is a well-formed XML string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustXmlClauses.Xml"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.XmlPayload).Xml();
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlClauses.Xml"/>
    public static IRuleBuilderOptions<TModel, string?> Xml<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Xml(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the HTTP headers dictionary contains an XML-compatible Content-Type header.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustXmlClauses.XmlContentType"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).XmlContentType();
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlClauses.XmlContentType"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> XmlContentType<TModel>(
        this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.XmlContentType(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is a valid, parseable XML document.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustXmlClauses.XmlDocument"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.XmlContent).XmlDocument();
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlClauses.XmlDocument"/>
    public static IRuleBuilderOptions<TModel, string?> XmlDocument<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.XmlDocument(val, paramName: null),
            message);
}
