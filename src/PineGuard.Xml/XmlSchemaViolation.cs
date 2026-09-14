namespace PineGuard.Xml;

/// <summary>
/// One problem found while validating an XML document against a schema set.
/// </summary>
/// <param name="Kind">The kind of problem found.</param>
/// <param name="Path">
/// The element path from the root, <c>/</c>-separated, local names only (e.g.
/// <c>Document/GrpHdr/MsgId</c>). An attribute violation carries its element's path, since the
/// reader is positioned on the element when the validation event fires.
/// </param>
/// <param name="Message">
/// The diagnostic message. For <see cref="XmlSchemaViolationKind.Error"/> and
/// <see cref="XmlSchemaViolationKind.Warning"/> this is the validation engine's own message; for
/// <see cref="XmlSchemaViolationKind.UnknownNamespace"/> it names the offending namespace.
/// </param>
/// <param name="LineNumber">
/// The one-based line number the problem was found at, or <c>0</c> when unknown. Relative to the
/// trimmed input passed to <see cref="XmlSchemaUtility.TryValidate"/>, not the original untrimmed value.
/// </param>
/// <param name="LinePosition">The one-based line position the problem was found at, or <c>0</c> when unknown.</param>
/// <seealso cref="XmlSchemaViolationKind"/>
/// <seealso cref="XmlSchemaUtility"/>
/// <seealso href="https://pineguard.ai/docs/xml/schema">XML Schema documentation</seealso>
public sealed record XmlSchemaViolation(XmlSchemaViolationKind Kind, string Path, string Message, int LineNumber, int LinePosition);
