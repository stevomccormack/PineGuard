using FluentValidation;
using PineGuard.Codes;
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
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
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
            message, MustCodes.Xml.Document.Invalid);

    /// <summary>
    /// Validates that the HTTP headers dictionary contains an XML-compatible Content-Type header.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustXmlClauses.XmlContentType"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
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
            message, MustCodes.Xml.ContentType.Mismatch);

    /// <summary>
    /// Validates that the property value is well-formed XML whose root element matches the given local name
    /// and, optionally, namespace URI.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="localName">The required root element local name (ordinal comparison).</param>
    /// <param name="namespaceUri">
    /// The required root element namespace URI (ordinal comparison). When <see langword="null"/>, any namespace
    /// is accepted; pass <see cref="string.Empty"/> to require no namespace.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustXmlClauses.HasXmlRoot"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Payload).HasXmlRoot("Document", "urn:iso:std:iso:20022:tech:xsd:pacs.008.001.08");
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlClauses.HasXmlRoot"/>
    public static IRuleBuilderOptions<TModel, string?> HasXmlRoot<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        string localName,
        string? namespaceUri = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasXmlRoot(val, localName, namespaceUri, paramName: null),
            message, MustCodes.Xml.Root.Mismatch);
}
