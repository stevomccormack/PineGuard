using System.Xml;

namespace PineGuard.Utils;

/// <summary>
/// Provides XML parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/xml">XML Utility documentation</seealso>
public static class XmlUtility
{
    /// <summary>
    /// Attempts to parse the specified string as well-formed XML with DTD processing prohibited.
    /// </summary>
    /// <param name="value">The XML string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="document">When this method returns, contains the parsed <see cref="XmlDocument"/> if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the XML was parsed successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <see cref="DtdProcessing.Prohibit"/> is used (together with a <see langword="null"/>
    /// <see cref="XmlReaderSettings.XmlResolver"/>) as a secure-by-default XXE/DoS hardening measure: a
    /// null resolver alone blocks external entity resolution but not internal entity expansion attacks
    /// (e.g., the "billion laughs" DoS), which prohibiting DTD processing entirely does prevent. A direct
    /// consequence is that any otherwise well-formed document containing a <c>&lt;!DOCTYPE ...&gt;</c>
    /// declaration (e.g., legacy XHTML, DOCTYPE-bearing SVG exports) is rejected, not just documents that
    /// attempt entity expansion.
    /// </remarks>
    public static bool TryParse(string? value, out XmlDocument? document)
    {
        document = null;

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

            var doc = new XmlDocument
            {
                XmlResolver = null
            };

            doc.Load(xmlReader);
            document = doc;
            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }
}
