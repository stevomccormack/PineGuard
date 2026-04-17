using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a well-formed XML fragment
/// or document.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustXmlClauses.Xml"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
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
/// <seealso cref="XmlDocumentStringAttribute"/>
/// <seealso cref="MustXmlClauses.Xml"/>
/// <seealso href="https://pineguard.ai/docs/annotations/xml">XML Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class XmlStringAttribute() : ValidationAttributeBase(typeof(string))
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
/// If the value is not an <see cref="IReadOnlyDictionary{TKey, TValue}"/>, validation passes silently.
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
public sealed class XmlContentTypeAttribute() : ValidationAttributeBase(typeof(object))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        if (value is not IReadOnlyDictionary<string, IEnumerable<string>> headers) return ValidationResult.Success;
        var result = Must.Be.XmlContentType(headers, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a well-formed, complete XML
/// document with a single root element.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustXmlClauses.XmlDocument"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// Unlike <see cref="XmlStringAttribute"/>, this attribute requires a complete document rather than a fragment.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DataModel
/// {
///     [XmlDocumentString]
///     public string XmlDocument { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="XmlStringAttribute"/>
/// <seealso cref="MustXmlClauses.XmlDocument"/>
/// <seealso href="https://pineguard.ai/docs/annotations/xml">XML Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class XmlDocumentStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.XmlDocument(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
