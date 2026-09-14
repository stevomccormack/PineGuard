using System.Xml;

namespace PineGuard.Utils;

/// <summary>
/// Provides XML parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/xml">XML Utility documentation</seealso>
public static class XmlUtility
{
    /// <summary>
    /// Attempts to parse the specified string as well-formed XML and determine the root element's qualified name.
    /// </summary>
    /// <param name="value">The XML string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="rootName">When this method returns, contains the root element's <see cref="XmlQualifiedName"/> if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the XML was parsed successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Reads with a forward-only <see cref="XmlReader"/> rather than building an <see cref="XmlDocument"/>: the
    /// whole document still has to be read to confirm well-formedness (a second root element, for instance, is
    /// only discovered after the first has already closed), but nothing beyond the first element's local name and
    /// namespace is retained. <see cref="DtdProcessing.Prohibit"/> is used (together with a <see langword="null"/>
    /// <see cref="XmlReaderSettings.XmlResolver"/>) as a deliberate secure-by-default XXE/DoS hardening policy for
    /// untrusted input: a null resolver alone blocks external entity resolution but not internal entity expansion
    /// attacks (e.g., the "billion laughs" DoS), which prohibiting DTD processing entirely does prevent. A direct
    /// consequence of that policy is that any otherwise well-formed document containing a
    /// <c>&lt;!DOCTYPE ...&gt;</c> declaration (e.g., legacy XHTML, DOCTYPE-bearing SVG exports) is rejected, not
    /// just documents that attempt entity expansion.
    /// </remarks>
    public static bool TryGetRootName(string? value, out XmlQualifiedName? rootName)
    {
        rootName = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        try
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            using var stringReader = new StringReader(trimmed);
            using var xmlReader = XmlReader.Create(stringReader, settings);

            XmlQualifiedName? found = null;

            while (xmlReader.Read())
            {
                if (found is null && xmlReader.NodeType == XmlNodeType.Element)
                    found = new XmlQualifiedName(xmlReader.LocalName, xmlReader.NamespaceURI);
            }

            rootName = found;
            return found is not null;
        }
        catch (XmlException)
        {
            rootName = null;
            return false;
        }
    }
}
