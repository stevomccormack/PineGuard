using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a well-formed, complete XML
/// document with a single root element.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustXmlClauses.Xml"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// There is no fragment mode: a value such as <c>&lt;a/&gt;&lt;b/&gt;</c>, which has more than one root
/// element, fails validation.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DataModel
/// {
///     [XmlString]
///     public string XmlPayload { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustXmlClauses.Xml"/>
/// <seealso href="https://pineguard.ai/docs/annotations/xml">XML Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class XmlStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Xml.Document.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Xml(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated property or field is an HTTP headers dictionary where the
/// <c>Content-Type</c> header indicates XML content (e.g., <c>application/xml</c> or <c>text/xml</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustXmlClauses.XmlContentType"/>. Supported on properties, fields, and parameters
/// of type <see cref="IReadOnlyDictionary{TKey, TValue}"/> where TKey is <see cref="string"/> and TValue
/// is <see cref="IEnumerable{T}"/> of <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// <para>
/// If the value is non-<see langword="null"/> but is not an
/// <see cref="IReadOnlyDictionary{TKey, TValue}"/> of <see cref="string"/> to
/// <see cref="IEnumerable{T}"/> of <see cref="string"/>, the attribute is misapplied and an
/// <see cref="InvalidOperationException"/> is thrown rather than silently reporting the value as valid.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class RequestModel
/// {
///     [XmlContentType]
///     public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="MustXmlClauses.XmlContentType"/>
/// <seealso href="https://pineguard.ai/docs/annotations/xml">XML Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class XmlContentTypeAttribute() : ValidationAttributeBase(typeof(object), MustCodes.Xml.ContentType.Mismatch)
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="value"/>'s runtime type is not <see cref="IReadOnlyDictionary{TKey, TValue}"/>
    /// of <see cref="string"/> to <see cref="IEnumerable{T}"/> of <see cref="string"/>.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        if (value is not IReadOnlyDictionary<string, IEnumerable<string>> headers)
            throw new InvalidOperationException(
                $"[{nameof(XmlContentTypeAttribute)}] can only be applied to properties implementing " +
                $"IReadOnlyDictionary<string, IEnumerable<string>>. Property '{validationContext.DisplayName}' " +
                $"is of type {value!.GetType().Name}.");

        var result = Must.Be.XmlContentType(headers, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is well-formed XML whose root
/// element matches the given local name and, optionally, namespace.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustXmlClauses.HasXmlRoot"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// <see cref="NamespaceUri"/> defaults to <see langword="null"/>, which matches any namespace. Pass
/// <see cref="string.Empty"/> to require the no-namespace case reported by <see cref="System.Xml.XmlReader"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DataModel
/// {
///     [HasXmlRoot("Document", "urn:iso:std:iso:20022:tech:xsd:pacs.008.001.08")]
///     public string XmlPayload { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustXmlClauses.HasXmlRoot"/>
/// <seealso href="https://pineguard.ai/docs/annotations/xml">XML Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasXmlRootAttribute(string localName, string? namespaceUri = null) : ValidationAttributeBase(typeof(string), MustCodes.Xml.Root.Mismatch)
{
    /// <summary>Gets the expected root element local name, compared ordinally.</summary>
    public string LocalName { get; } = localName;

    /// <summary>
    /// Gets the expected root element namespace, compared ordinally. <see langword="null"/> matches any
    /// namespace; <see cref="string.Empty"/> matches only the no-namespace case.
    /// </summary>
    public string? NamespaceUri { get; } = namespaceUri;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.HasXmlRoot(strValue, LocalName, NamespaceUri, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
