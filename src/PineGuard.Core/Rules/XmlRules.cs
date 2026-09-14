using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure XML content and HTTP content-type validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/xml">XML Rules documentation</seealso>
public static class XmlRules
{
    /// <summary>
    /// Determines whether the specified value is well-formed XML.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is parseable as well-formed XML; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// DTD processing is prohibited as a secure-by-default XXE/DoS hardening measure, so any otherwise
    /// well-formed document containing a <c>&lt;!DOCTYPE ...&gt;</c> declaration (e.g., legacy XHTML,
    /// DOCTYPE-bearing SVG exports) is rejected, not just documents that attempt entity expansion.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// bool valid = XmlRules.IsXml("<root/>");   // true
    /// bool invalid = XmlRules.IsXml("not xml");        // false
    /// ]]></code>
    /// </example>
    public static bool IsXml(string? value) =>
        XmlUtility.TryGetRootName(value, out _);

    /// <summary>
    /// Determines whether the specified value is well-formed XML whose root element matches the given
    /// local name and, optionally, namespace.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="localName">
    /// The expected root element local name, compared ordinally. Must not be <see langword="null"/> or whitespace.
    /// </param>
    /// <param name="namespaceUri">
    /// The expected root element namespace, compared ordinally. <see langword="null"/> (the default) matches
    /// any namespace; <see cref="string.Empty"/> matches only the no-namespace case reported by
    /// <see cref="System.Xml.XmlReader"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is well-formed XML and its root element's local name
    /// (and, when <paramref name="namespaceUri"/> is not <see langword="null"/>, its namespace) matches;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="localName"/> is <see langword="null"/> or whitespace.
    /// </exception>
    /// <example>
    /// <code><![CDATA[
    /// bool valid = XmlRules.HasXmlRoot("<Document xmlns=\"urn:test:doc\"/>", "Document", "urn:test:doc"); // true
    /// bool anyNs = XmlRules.HasXmlRoot("<Document/>", "Document");                                         // true
    /// bool wrong = XmlRules.HasXmlRoot("<Envelope/>", "Document");                                         // false
    /// ]]></code>
    /// </example>
    public static bool HasXmlRoot(string? value, string localName, string? namespaceUri = null)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(localName);

        if (!XmlUtility.TryGetRootName(value, out var rootName) || rootName is null)
            return false;

        if (!string.Equals(rootName.Name, localName, StringComparison.Ordinal))
            return false;

        return namespaceUri is null || string.Equals(rootName.Namespace, namespaceUri, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines whether the HTTP headers indicate an XML content type
    /// (<c>application/xml</c>, <c>text/xml</c>, or any <c>*+xml</c> media type).
    /// </summary>
    /// <param name="headers">
    /// The HTTP response/request headers to inspect. If <see langword="null"/>, returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <c>Content-Type</c> header indicates XML; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsXmlContentType(IReadOnlyDictionary<string, IEnumerable<string>>? headers)
    {
        if (!HttpContentTypeUtility.TryGetContentTypeMediaTypes(headers, out var mediaTypes) || mediaTypes is null)
            return false;

        foreach (var mediaType in mediaTypes)
        {
            if (string.Equals(mediaType, "application/xml", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(mediaType, "text/xml", StringComparison.OrdinalIgnoreCase))
                return true;

            if (mediaType.EndsWith("+xml", StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
