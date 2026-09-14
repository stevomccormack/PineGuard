using System.Xml;
using System.Xml.Schema;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Xml;

/// <summary>
/// Provides the streaming XSD conformance check behind every <c>PineGuard.Xml</c> entry point.
/// </summary>
/// <seealso cref="XmlSchemaRules"/>
/// <seealso cref="XmlSchemaSetBuilder"/>
/// <seealso href="https://pineguard.ai/docs/xml/schema">XML Schema documentation</seealso>
public static class XmlSchemaUtility
{
    /// <summary>
    /// Attempts to validate <paramref name="value"/> against <paramref name="schemas"/>, collecting
    /// every violation found rather than stopping at the first.
    /// </summary>
    /// <param name="value">The XML string to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="schemas">The compiled schema set to validate against.</param>
    /// <param name="options">
    /// The options controlling namespace and warning handling. When <see langword="null"/>,
    /// <see cref="XmlSchemaValidationOptions.Default"/> is used.
    /// </param>
    /// <param name="violations">
    /// When this method returns, contains every <see cref="XmlSchemaViolation"/> found, in
    /// document order; empty when validation succeeds, or when <paramref name="value"/> is
    /// <see langword="null"/> or whitespace.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is well-formed, its root namespace is
    /// covered by <paramref name="schemas"/> (when required), and no violation was collected;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Reads with a forward-only, schema-validating <see cref="XmlReader"/>
    /// (<see cref="ValidationType.Schema"/>) rather than building an <see cref="XmlDocument"/>, so a
    /// multi-megabyte ISO 20022 message is validated without ever materialising a DOM.
    /// <see cref="DtdProcessing.Prohibit"/> and a <see langword="null"/>
    /// <see cref="XmlReaderSettings.XmlResolver"/> apply the same secure-by-default policy as
    /// <see cref="PineGuard.Utils.XmlUtility.TryGetRootName"/>.
    /// </para>
    /// <para>
    /// <see cref="XmlSchemaValidationFlags.ProcessSchemaLocation"/> and
    /// <see cref="XmlSchemaValidationFlags.ProcessInlineSchema"/> are never enabled, so a document
    /// cannot smuggle in schema instructions of its own — every schema comes from
    /// <paramref name="schemas"/>, and nowhere else. <see cref="XmlSchemaValidationFlags.ReportValidationWarnings"/>
    /// is added only when <see cref="XmlSchemaValidationOptions.TreatWarningsAsViolations"/> is
    /// <see langword="true"/> — the engine's default <see cref="XmlReaderSettings.ValidationFlags"/>
    /// never raise a validation event for a warning-only condition (such as <c>xs:any</c> content in
    /// an unrecognised namespace). Because the flag is never set otherwise, the handler never needs to
    /// filter warnings itself: every warning it is handed was already gated on
    /// <see cref="XmlSchemaValidationOptions.TreatWarningsAsViolations"/> being <see langword="true"/>,
    /// so every delivered warning is recorded.
    /// </para>
    /// <para>
    /// When <see cref="XmlSchemaValidationOptions.RequireKnownNamespace"/> is <see langword="true"/>
    /// (the default) and the root element's namespace is not <see cref="XmlSchemaSet.Contains(string)"/>
    /// in <paramref name="schemas"/>, validation stops immediately with a single
    /// <see cref="XmlSchemaViolationKind.UnknownNamespace"/> violation. Any validation events already
    /// raised by the reader for the root element itself are discarded first, so the result is exactly
    /// this one violation. Without this check, an unrecognised namespace produces no schema errors at
    /// all, because the engine has nothing to validate it against.
    /// </para>
    /// <para>
    /// <see cref="XmlSchemaViolation.Path"/> is tracked with a stack that is only pushed once
    /// <see cref="XmlReader.Read()"/> has returned for an element-start node, yet validation events for
    /// that same element fire from inside the <see cref="XmlReader.Read()"/> call that produced it —
    /// before the push happens. The current node is captured in a local visible to the event handler
    /// so its <see cref="XmlReader.LocalName"/> can be appended to the path on the fly. An
    /// attribute-value violation is observed to fire while the reader is still positioned on the
    /// owning element (<see cref="XmlReader.NodeType"/> is <see cref="System.Xml.XmlNodeType.Element"/>),
    /// so it carries that element's path as a best-effort approximation — the reader gives no distinct
    /// node for "this attribute", only the element that hosts it.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="schemas"/> is <see langword="null"/>.</exception>
    public static bool TryValidate(string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options, out IReadOnlyList<XmlSchemaViolation> violations)
    {
        ThrowHelper.ThrowIfNull(schemas);

        var effectiveOptions = options ?? XmlSchemaValidationOptions.Default;
        var collected = new List<XmlSchemaViolation>();
        violations = collected;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var pathSegments = new Stack<string>();
        // Assigned before the first Read(); events cannot fire earlier.
        XmlReader current = null!;

        void OnValidationEvent(object? sender, ValidationEventArgs e)
        {
            var path = current.NodeType == XmlNodeType.Element
                ? BuildPath(pathSegments, current.LocalName)
                : BuildPath(pathSegments);

            var kind = e.Severity == XmlSeverityType.Warning ? XmlSchemaViolationKind.Warning : XmlSchemaViolationKind.Error;
            collected.Add(new XmlSchemaViolation(kind, path, e.Message, e.Exception.LineNumber, e.Exception.LinePosition));
        }

        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = schemas,
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null
        };

        if (effectiveOptions.TreatWarningsAsViolations)
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

        settings.ValidationEventHandler += OnValidationEvent;

        try
        {
            using var stringReader = new StringReader(trimmed);
            using var xmlReader = XmlReader.Create(stringReader, settings);
            current = xmlReader;

            var isFirstElement = true;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (isFirstElement)
                        {
                            isFirstElement = false;

                            if (effectiveOptions.RequireKnownNamespace && !schemas.Contains(xmlReader.NamespaceURI))
                            {
                                collected.Clear();
                                collected.Add(new XmlSchemaViolation(XmlSchemaViolationKind.UnknownNamespace, xmlReader.LocalName, xmlReader.NamespaceURI, 0, 0));
                                return false;
                            }
                        }

                        if (!xmlReader.IsEmptyElement)
                            pathSegments.Push(xmlReader.LocalName);
                        break;

                    case XmlNodeType.EndElement:
                        if (pathSegments.Count > 0)
                            pathSegments.Pop();
                        break;
                }
            }
        }
        catch (XmlException ex)
        {
            collected.Add(new XmlSchemaViolation(XmlSchemaViolationKind.NotWellFormed, BuildPath(pathSegments), "The document is not well-formed XML.", ex.LineNumber, ex.LinePosition));
            return false;
        }

        return collected.Count == 0;
    }

    private static string BuildPath(Stack<string> pathSegments) =>
        string.Join("/", pathSegments.Reverse());

    private static string BuildPath(Stack<string> pathSegments, string currentLocalName) =>
        string.Join("/", pathSegments.Reverse().Append(currentLocalName));
}
