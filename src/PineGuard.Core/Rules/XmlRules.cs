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
    /// <example>
    /// <code><![CDATA[
    /// bool valid = XmlRules.IsXml("<root/>");   // true
    /// bool invalid = XmlRules.IsXml("not xml");        // false
    /// ]]></code>
    /// </example>
    public static bool IsXml(string? value) =>
        XmlUtility.TryParse(value, out _);

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
