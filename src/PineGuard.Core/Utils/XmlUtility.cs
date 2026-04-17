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
