using System.Xml;
using System.Xml.Schema;
using PineGuard.Common;

namespace PineGuard.Xml;

/// <summary>
/// Compiles one or more XSD sources into a single <see cref="XmlSchemaSet"/>, hardened against
/// network and entity-expansion attacks by construction.
/// </summary>
/// <remarks>
/// <para>
/// Every source — file, stream, reader or inline text — is read through an
/// <see cref="XmlReader"/> configured with <see cref="DtdProcessing.Prohibit"/> and a
/// <see langword="null"/> <see cref="XmlReaderSettings.XmlResolver"/>, and the underlying
/// <see cref="XmlSchemaSet"/> is itself constructed with a <see langword="null"/>
/// <see cref="XmlSchemaSet.XmlResolver"/>, so a malicious <c>xs:import</c> or <c>xs:include</c>
/// inside a schema can never reach the network. <see cref="AddFile"/> opens the path with
/// <see cref="File.OpenRead(string)"/> and reads it through the same stream-based reader as
/// <see cref="AddStream"/>, rather than passing the path straight to
/// <see cref="XmlReader.Create(string, XmlReaderSettings)"/> — that overload substitutes its own
/// internal <see cref="XmlUrlResolver"/> whenever <see cref="XmlReaderSettings.XmlResolver"/> is
/// <see langword="null"/>, which would let a URL-shaped path reach the network regardless of the
/// <see langword="null"/> resolver configured here.
/// </para>
/// <para>
/// A builder is single-use: <see cref="Build()"/> compiles and returns the set, and a second
/// call throws <see cref="InvalidOperationException"/>. <see cref="Build()"/> also throws
/// <see cref="InvalidOperationException"/> when no schema was added first — an empty
/// <see cref="XmlSchemaSet"/> validates every document as vacuously schema-valid, which is never
/// the caller's intent.
/// </para>
/// <para>
/// The <see cref="XmlSchemaSet"/> returned by <see cref="Build()"/> is safe to share across
/// concurrent calls to <see cref="XmlSchemaUtility.TryValidate"/>, provided nothing subsequently
/// calls <see cref="XmlSchemaSet.Add(XmlSchema)"/> or <see cref="XmlSchemaSet.Compile()"/> on it —
/// it is the mutable BCL type, not a value returned by this builder, so that guarantee holds only
/// as long as callers treat it as read-only after <see cref="Build()"/> returns.
/// </para>
/// </remarks>
/// <seealso cref="XmlSchemaUtility"/>
/// <seealso href="https://pineguard.ai/docs/xml/schema">XML Schema documentation</seealso>
public sealed class XmlSchemaSetBuilder
{
    private readonly XmlSchemaSet _set = new() { XmlResolver = null };
    private bool _built;

    /// <summary>
    /// Adds a schema read from the file at <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The path of the XSD file to add. Must not be <see langword="null"/> or whitespace.</param>
    /// <returns><see langword="this"/>, so calls can be chained.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is <see langword="null"/> or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="Build()"/> has already been called.</exception>
    public XmlSchemaSetBuilder AddFile(string path)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(path);
        EnsureNotBuilt();

        using var stream = File.OpenRead(path);
        using var reader = CreateReader(stream);
        _set.Add(null, reader);
        return this;
    }

    /// <summary>
    /// Adds a schema read from <paramref name="stream"/>. The stream is not disposed — it was
    /// opened by the caller, and remains theirs to dispose.
    /// </summary>
    /// <param name="stream">The stream containing the XSD to add.</param>
    /// <returns><see langword="this"/>, so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="Build()"/> has already been called.</exception>
    public XmlSchemaSetBuilder AddStream(Stream stream)
    {
        ThrowHelper.ThrowIfNull(stream);
        EnsureNotBuilt();

        using var reader = CreateReader(stream);
        _set.Add(null, reader);
        return this;
    }

    /// <summary>
    /// Adds a schema read from <paramref name="reader"/>. The reader is not disposed — it was
    /// opened by the caller, and remains theirs to dispose.
    /// </summary>
    /// <param name="reader">The reader containing the XSD to add.</param>
    /// <returns><see langword="this"/>, so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="reader"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="Build()"/> has already been called.</exception>
    public XmlSchemaSetBuilder AddReader(TextReader reader)
    {
        ThrowHelper.ThrowIfNull(reader);
        EnsureNotBuilt();

        using var xmlReader = CreateReader(reader);
        _set.Add(null, xmlReader);
        return this;
    }

    /// <summary>
    /// Adds a schema read from the inline XSD text <paramref name="schemaXml"/>.
    /// </summary>
    /// <param name="schemaXml">The XSD document text to add. Must not be <see langword="null"/> or whitespace.</param>
    /// <returns><see langword="this"/>, so calls can be chained.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="schemaXml"/> is <see langword="null"/> or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="Build()"/> has already been called.</exception>
    public XmlSchemaSetBuilder AddText(string schemaXml)
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(schemaXml);
        EnsureNotBuilt();

        using var stringReader = new StringReader(schemaXml);
        using var xmlReader = CreateReader(stringReader);
        _set.Add(null, xmlReader);
        return this;
    }

    /// <summary>
    /// Compiles every added schema and returns the resulting <see cref="XmlSchemaSet"/>.
    /// </summary>
    /// <returns>The compiled <see cref="XmlSchemaSet"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when this method has already been called, or when no schema was added via
    /// <see cref="AddFile"/>, <see cref="AddStream"/>, <see cref="AddReader"/> or <see cref="AddText"/>
    /// before calling this method.
    /// </exception>
    /// <exception cref="XmlSchemaException">Thrown when an added schema is invalid.</exception>
    public XmlSchemaSet Build()
    {
        EnsureNotBuilt();

        if (_set.Count == 0)
            throw new InvalidOperationException("At least one schema must be added before Build().");

        _built = true;

        _set.Compile();
        return _set;
    }

    private void EnsureNotBuilt()
    {
        if (_built)
            throw new InvalidOperationException($"{nameof(XmlSchemaSetBuilder)}.{nameof(Build)} has already been called; a builder is single-use.");
    }

    private static XmlReader CreateReader(Stream stream) =>
        XmlReader.Create(stream, CreateReaderSettings());

    private static XmlReader CreateReader(TextReader reader) =>
        XmlReader.Create(reader, CreateReaderSettings());

    private static XmlReaderSettings CreateReaderSettings() =>
        new()
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null
        };
}
