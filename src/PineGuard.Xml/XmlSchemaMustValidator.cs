using System.Xml.Schema;
using PineGuard.Codes;
using PineGuard.MustClauses;

namespace PineGuard.Xml;

/// <summary>
/// An <see cref="IMustValidator{T}"/> that validates a string against a compiled XSD schema set,
/// carrying every violation found — not just the first — as a <see cref="MustFailure"/>.
/// </summary>
/// <param name="schemas">The compiled schema set to validate against.</param>
/// <param name="options">
/// The options controlling namespace and warning handling. When <see langword="null"/>,
/// <see cref="XmlSchemaValidationOptions.Default"/> is used.
/// </param>
/// <remarks>
/// <para>
/// Named <see cref="XmlSchemaMustValidator"/> after the <c>&lt;Qualifier&gt;MustValidator</c>
/// convention (see <see cref="PineGuard.MustClauses.InlineMustValidator{T}"/>) — the shorter
/// <c>XmlSchemaValidator</c> is already <see cref="System.Xml.Schema.XmlSchemaValidator"/> in the
/// framework.
/// </para>
/// <para>
/// Unlike <see cref="MustXmlSchemaClauses.ValidXml"/>, which reports only the first violation
/// through a deterministic message template, every <see cref="MustFailure"/> produced here carries
/// the validation engine's own message as-is — it is the diagnostic, not a template rendered for a
/// single parameter — and <see cref="MustFailure.PropertyPath"/> is the violating element's path
/// (e.g. <c>Document/GrpHdr/MsgId</c>). <see cref="MustFailure.Value"/> is always <see langword="null"/>:
/// the whole document is never echoed back through a failure.
/// </para>
/// </remarks>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="schemas"/> is <see langword="null"/>.</exception>
/// <seealso cref="MustXmlSchemaClauses.ValidXml"/>
/// <seealso cref="XmlSchemaUtility"/>
/// <seealso href="https://pineguard.ai/docs/xml/schema">XML Schema documentation</seealso>
public sealed class XmlSchemaMustValidator(XmlSchemaSet schemas, XmlSchemaValidationOptions? options = null) : IMustValidator<string>
{
    private readonly XmlSchemaSet _schemas = schemas ?? throw new ArgumentNullException(nameof(schemas));

    /// <summary>
    /// Validates <paramref name="value"/> against the configured schema set.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>
    /// <see cref="MustValidationResult.Ok"/> when <paramref name="value"/> conforms; otherwise a
    /// failed <see cref="MustValidationResult"/> with one <see cref="MustFailure"/> per violation found.
    /// </returns>
    /// <remarks>
    /// <see langword="null"/> is handled defensively even though <see cref="IMustValidator{T}"/>
    /// constrains its type parameter to <see langword="notnull"/>: the non-generic <see cref="IMustValidator"/>
    /// surface can still forward a <see langword="null"/> reference through
    /// <see cref="PineGuard.MustClauses.IMustValidator.Validate(object?)"/>. A <see langword="null"/>
    /// value yields one failure: <c>("", xml.document.invalid, "Value must not be null.", null)</c>.
    /// </remarks>
    public MustValidationResult Validate(string value)
    {
        if (value is null)
            return MustValidationResult.Fail(new MustFailure(string.Empty, MustCodes.Xml.Document.Invalid, "Value must not be null.", null));

        if (XmlSchemaUtility.TryValidate(value, _schemas, options, out var violations))
            return MustValidationResult.Ok();

        // Whitespace-only input yields no XmlSchemaViolation (TryValidate treats it like a null/blank
        // string), so it is reported as a well-formedness failure directly.
        if (violations.Count == 0)
            return MustValidationResult.Fail(new MustFailure(string.Empty, MustCodes.Xml.Document.Invalid, "Value must be XML.", null));

        var failures = violations.Select(v => new MustFailure(v.Path, CodeFor(v.Kind), v.Message, null)).ToList();
        return MustValidationResult.Fail(failures);
    }

    /// <summary>
    /// Validates <paramref name="value"/> against the configured schema set.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="cancellationToken">Unused: this validator's work is synchronous.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> wrapping the synchronous result of <see cref="Validate(string)"/>.</returns>
    public ValueTask<MustValidationResult> ValidateAsync(string value, CancellationToken cancellationToken = default) =>
        new(Validate(value));

    private static string CodeFor(XmlSchemaViolationKind kind) =>
        kind switch
        {
            XmlSchemaViolationKind.NotWellFormed => MustCodes.Xml.Document.Invalid,
            XmlSchemaViolationKind.UnknownNamespace => MustCodes.Xml.Namespace.Unknown,
            _ => MustCodes.Xml.Schema.Mismatch
        };
}
