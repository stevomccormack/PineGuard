using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.Xml;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is well-formed XML that
/// conforms to an <see cref="XmlSchemaSet"/> resolved from the validation context's services.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustXmlSchemaClauses.ValidXml"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="XmlSchemaSet"/> to validate against is resolved via
/// <see cref="ValidationContext.GetService(Type)"/> — there is no other way to give an attribute a
/// compiled schema set, since attribute arguments must be compile-time constants. Register one with
/// <c>services.AddSingleton(new XmlSchemaSetBuilder()....Build())</c> (or pass a service provider on
/// the <see cref="ValidationContext"/> directly) before validating. When no <see cref="XmlSchemaSet"/>
/// is registered, validation throws <see cref="InvalidOperationException"/> rather than silently
/// reporting the value as valid. An <see cref="XmlSchemaValidationOptions"/> is resolved the same
/// way and falls back to <see cref="XmlSchemaValidationOptions.Default"/> when none is registered.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MessageModel
/// {
///     [ValidXml]
///     public string Payload { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustXmlSchemaClauses.ValidXml"/>
/// <seealso href="https://pineguard.ai/docs/annotations/xml-schema">XML Schema Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ValidXmlAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Xml.Schema.Mismatch)
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// No <see cref="XmlSchemaSet"/> is registered as a service on the <see cref="ValidationContext"/>.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var schemas = validationContext.GetService(typeof(XmlSchemaSet)) as XmlSchemaSet
            ?? throw new InvalidOperationException(
                "[ValidXml] requires an XmlSchemaSet registered as a service (services.AddSingleton(new XmlSchemaSetBuilder()....Build())).");

        var options = validationContext.GetService(typeof(XmlSchemaValidationOptions)) as XmlSchemaValidationOptions
            ?? XmlSchemaValidationOptions.Default;

        var strValue = (string)value!;
        var result = Must.Be.ValidXml(strValue, schemas, options, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
