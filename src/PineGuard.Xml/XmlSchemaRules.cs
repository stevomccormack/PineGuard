using System.Xml.Schema;

namespace PineGuard.Xml;

/// <summary>
/// Provides the pure XSD conformance predicate over a compiled <see cref="XmlSchemaSet"/>.
/// </summary>
/// <remarks>
/// Named <see cref="XmlSchemaRules"/> rather than <c>XmlRules</c> — <c>PineGuard.Rules.XmlRules</c>
/// already owns that simple name for well-formedness, and a file importing both namespaces would
/// collide.
/// </remarks>
/// <seealso cref="XmlSchemaUtility"/>
/// <seealso href="https://pineguard.ai/docs/xml/schema">XML Schema documentation</seealso>
public static class XmlSchemaRules
{
    /// <summary>
    /// Determines whether <paramref name="value"/> is well-formed XML that conforms to <paramref name="schemas"/>.
    /// </summary>
    /// <param name="value">The XML string to validate. <see langword="null"/> or whitespace returns <see langword="false"/>.</param>
    /// <param name="schemas">The compiled schema set to validate against.</param>
    /// <param name="options">
    /// The options controlling namespace and warning handling. When <see langword="null"/>,
    /// <see cref="XmlSchemaValidationOptions.Default"/> is used.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> conforms; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="schemas"/> is <see langword="null"/>.</exception>
    /// <seealso cref="XmlSchemaUtility.TryValidate"/>
    public static bool IsValidXml(string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options = null) =>
        XmlSchemaUtility.TryValidate(value, schemas, options, out _);
}
