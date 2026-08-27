using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for XML string and content-type validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/xml">Guard XML Clauses documentation</seealso>
public static class GuardXmlClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid XML string.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustXmlClauses.Xml"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not valid XML and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustXmlClauses.Xml"/>:
    /// <c>Guard.Against.NotXml</c> passes when the value is well-formed XML.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotXml(responseBody);
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlClauses.Xml"/>
    public static string NotXml(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Xml(value, paramName); // Guard.Against.NotXml => Must.Be.Xml
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> do not include an XML content-type header.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP response headers to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustXmlClauses.XmlContentType"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated headers if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the headers lack an XML content-type and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustXmlClauses.XmlContentType"/>:
    /// <c>Guard.Against.NotXmlContentType</c> passes when an XML content-type header is present.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotXmlContentType(response.Headers);
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlClauses.XmlContentType"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotXmlContentType(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.XmlContentType(headers, paramName); // Guard.Against.NotXmlContentType => Must.Be.XmlContentType
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid XML document (i.e., does not have a root element).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustXmlClauses.XmlDocument"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a valid XML document and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustXmlClauses.XmlDocument"/>:
    /// <c>Guard.Against.NotXmlDocument</c> passes when the value is a valid XML document with a root element.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotXmlDocument(xmlPayload);
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlClauses.XmlDocument"/>
    public static string NotXmlDocument(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.XmlDocument(value, paramName); // Guard.Against.NotXmlDocument => Must.Be.XmlDocument
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
