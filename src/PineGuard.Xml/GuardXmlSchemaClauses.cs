using System.Runtime.CompilerServices;
using System.Xml.Schema;
using PineGuard.GuardClauses;
using PineGuard.MustClauses;

namespace PineGuard.Xml;

/// <summary>
/// Guard clauses for XSD schema conformance.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/xml-schema">Guard XML Schema Clauses documentation</seealso>
public static class GuardXmlSchemaClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not well-formed XML that conforms to <paramref name="schemas"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="schemas">The compiled schema set to validate against.</param>
    /// <param name="options">
    /// The options controlling namespace and warning handling. When <see langword="null"/>,
    /// <see cref="XmlSchemaValidationOptions.Default"/> is used.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustXmlSchemaClauses.ValidXml"/>.
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
    /// Thrown when <paramref name="value"/> does not conform and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustXmlSchemaClauses.ValidXml"/>:
    /// <c>Guard.Against.InvalidXml</c> passes when the value conforms to <paramref name="schemas"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.InvalidXml(payload, schemas);
    /// </code>
    /// </example>
    /// <seealso cref="MustXmlSchemaClauses.ValidXml"/>
    public static string InvalidXml(this IGuardClause _,
        string? value,
        XmlSchemaSet schemas,
        XmlSchemaValidationOptions? options = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.ValidXml(value, schemas, options, paramName); // Guard.Against.InvalidXml => Must.Be.ValidXml
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
