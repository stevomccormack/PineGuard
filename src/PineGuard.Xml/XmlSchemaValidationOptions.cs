namespace PineGuard.Xml;

/// <summary>
/// Configures how <see cref="XmlSchemaUtility.TryValidate"/> treats the root namespace and
/// schema-validation warnings.
/// </summary>
/// <seealso cref="XmlSchemaUtility"/>
/// <seealso href="https://pineguard.ai/docs/xml/schema">XML Schema documentation</seealso>
public sealed class XmlSchemaValidationOptions
{
    /// <summary>
    /// Gets a value indicating whether the document's root element namespace must be covered by
    /// the schema set.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="true"/>. Without this check, a document in a namespace the
    /// schema set knows nothing about produces no schema errors at all — the validator silently
    /// treats every element as unconstrained. Turning this off falls through to whatever the
    /// schema-validation warnings report for the unrecognised content.
    /// </remarks>
    public bool RequireKnownNamespace { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether schema-validation warnings are treated as violations.
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="false"/>. ISO 20022 messages commonly carry a
    /// <c>SplmtryData</c> element declared with <c>xs:any processContents="lax"</c>, which the
    /// validation engine reports as a warning on perfectly valid messages. Set this to
    /// <see langword="true"/> to require every warning to be resolved as well.
    /// </remarks>
    public bool TreatWarningsAsViolations { get; init; }

    /// <summary>
    /// Gets the default <see cref="XmlSchemaValidationOptions"/>: <see cref="RequireKnownNamespace"/>
    /// <see langword="true"/>, <see cref="TreatWarningsAsViolations"/> <see langword="false"/>.
    /// </summary>
    public static XmlSchemaValidationOptions Default { get; } = new();
}
