using System.Runtime.CompilerServices;
using System.Xml.Schema;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.Xml;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate XML strings against a
/// compiled XSD schema set.
/// </summary>
/// <seealso cref="XmlSchemaRules"/>
/// <seealso href="https://pineguard.ai/docs/must/xml-schema">XML Schema Must Clauses documentation</seealso>
public static class MustXmlSchemaClauses
{
    /// <summary>
    /// Validates that the specified string is well-formed XML that conforms to <paramref name="schemas"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="schemas">The compiled schema set to validate against.</param>
    /// <param name="options">
    /// The options controlling namespace and warning handling. When <see langword="null"/>,
    /// <see cref="XmlSchemaValidationOptions.Default"/> is used.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> conforms, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="XmlSchemaUtility.TryValidate"/>. On failure, the code and message
    /// template are drawn from the <em>first</em> collected violation's
    /// <see cref="XmlSchemaViolationKind"/> — never from the validation engine's own message text,
    /// which differs between runtimes and would make the failure message brittle across target
    /// frameworks:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="XmlSchemaViolationKind.NotWellFormed"/> (<see cref="MustCodes.Xml.Document.Invalid"/>): <c>"{paramName} must be XML."</c></description></item>
    /// <item><description><see cref="XmlSchemaViolationKind.UnknownNamespace"/> (<see cref="MustCodes.Xml.Namespace.Unknown"/>): <c>"{paramName} must be XML in a namespace covered by the schema set (found '{ns}')."</c></description></item>
    /// <item><description><see cref="XmlSchemaViolationKind.Error"/> or <see cref="XmlSchemaViolationKind.Warning"/> (<see cref="MustCodes.Xml.Schema.Mismatch"/>): <c>"{paramName} must be valid against the XML schema ({n} violation(s), first at '{path}')."</c></description></item>
    /// </list>
    /// <para>
    /// Every violation the engine found is available via <see cref="XmlSchemaUtility.TryValidate"/>
    /// or <see cref="XmlSchemaMustValidator"/> when the full list is needed.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="schemas"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// var result = Must.Be.ValidXml(payload, schemas);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="XmlSchemaRules.IsValidXml"/>
    public static MustResult<string> ValidXml(this IMustClause _,
        string? value,
        XmlSchemaSet schemas,
        XmlSchemaValidationOptions? options = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        ThrowHelper.ThrowIfNull(schemas);

        if (value is null)
            return MustResult<string>.Fail(MustCodes.Xml.Document.Invalid, "{paramName} must not be null.", paramName, value);

        var ok = XmlSchemaUtility.TryValidate(value, schemas, options, out var violations);
        if (ok)
            return MustResult<string>.Ok(value, value, paramName);

        // Whitespace-only input yields no XmlSchemaViolation (TryValidate treats it like a null/blank
        // string), so it is reported the same way as a well-formedness failure.
        var (code, messageTemplate) = violations.Count == 0
            ? (MustCodes.Xml.Document.Invalid, "{paramName} must be XML.")
            : violations[0].Kind switch
            {
                XmlSchemaViolationKind.NotWellFormed => (MustCodes.Xml.Document.Invalid, "{paramName} must be XML."),
                XmlSchemaViolationKind.UnknownNamespace => (MustCodes.Xml.Namespace.Unknown, $"{{paramName}} must be XML in a namespace covered by the schema set (found '{violations[0].Message}')."),
                _ => (MustCodes.Xml.Schema.Mismatch, $"{{paramName}} must be valid against the XML schema ({violations.Count} violation(s), first at '{violations[0].Path}').")
            };

        return MustResult<string>.Fail(code, messageTemplate, paramName, value);
    }
}
