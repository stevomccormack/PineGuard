namespace PineGuard.Xml;

/// <summary>
/// The kind of problem a single <see cref="XmlSchemaViolation"/> describes.
/// </summary>
/// <seealso cref="XmlSchemaViolation"/>
/// <seealso cref="XmlSchemaUtility"/>
/// <seealso href="https://pineguard.ai/docs/xml/schema">XML Schema documentation</seealso>
public enum XmlSchemaViolationKind
{
    /// <summary>The document is not well-formed XML. Maps to <c>xml.document.invalid</c>.</summary>
    NotWellFormed,

    /// <summary>The document's root namespace is not covered by the schema set. Maps to <c>xml.namespace.unknown</c>.</summary>
    UnknownNamespace,

    /// <summary>The document mismatches the schema and the validation engine reported an error. Maps to <c>xml.schema.mismatch</c>.</summary>
    Error,

    /// <summary>The validation engine reported a warning, recorded only when configured to. Maps to <c>xml.schema.mismatch</c>.</summary>
    Warning
}
