using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate XML strings and HTTP headers.
/// </summary>
/// <seealso cref="XmlRules"/>
/// <seealso href="https://pineguard.ai/docs/must/xml">XML Must Clauses documentation</seealso>
public static class MustXmlClauses
{
    /// <summary>
    /// Validates that the specified string is well-formed XML.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as XML.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is valid XML, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="XmlRules.IsXml"/>. The failure message follows the pattern
    /// <c>"{paramName} must be XML."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Xml(responseBody);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="XmlRules.IsXml"/>
    /// <seealso href="https://pineguard.ai/docs/must/xml">XML Must Clauses documentation</seealso>
    public static MustResult<string> Xml(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be XML.";

        var ok = XmlRules.IsXml(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified HTTP headers dictionary contains an XML <c>Content-Type</c> header value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="headers"/> contains an XML content-type, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="XmlRules.IsXmlContentType"/>. The failure message follows the pattern
    /// <c>"{paramName} must contain an XML Content-Type."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.XmlContentType(response.Headers);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="XmlRules.IsXmlContentType"/>
    /// <seealso href="https://pineguard.ai/docs/must/xml">XML Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> XmlContentType(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain an XML Content-Type.";

        var ok = XmlRules.IsXmlContentType(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified string is a well-formed XML document.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as an XML document.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid XML document, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="XmlRules.IsXml"/>. The failure message follows the pattern
    /// <c>"{paramName} must be an XML document."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.XmlDocument(soapEnvelope);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="XmlRules.IsXml"/>
    /// <seealso href="https://pineguard.ai/docs/must/xml">XML Must Clauses documentation</seealso>
    public static MustResult<string> XmlDocument(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be an XML document.";

        var ok = XmlRules.IsXml(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }
}
