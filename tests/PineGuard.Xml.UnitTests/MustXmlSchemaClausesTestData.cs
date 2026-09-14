using System.Xml.Schema;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class MustXmlSchemaClausesTestData
{
    public static class ValidXml
    {
        public static TheoryData<MustCase<string?>> ValidCases =>
        [
            new(nameof(TestSchemas.ValidMessage), TestSchemas.ValidMessage, new MustExpected(true)),
            new(nameof(TestSchemas.ValidWithSupplementary), TestSchemas.ValidWithSupplementary, new MustExpected(true))
        ];

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(TestSchemas.MsgIdTooLong), TestSchemas.MsgIdTooLong, new MustExpected(false, "value must be valid against the XML schema (1 violation(s), first at 'Document/GrpHdr/MsgId').", Code: MustCodes.Xml.Schema.Mismatch)),
            new(nameof(TestSchemas.TwoViolations), TestSchemas.TwoViolations, new MustExpected(false, "value must be valid against the XML schema (2 violation(s), first at 'Document/GrpHdr/MsgId').", Code: MustCodes.Xml.Schema.Mismatch)),
            new(nameof(TestSchemas.UnknownNamespaceDocument), TestSchemas.UnknownNamespaceDocument, new MustExpected(false, "value must be XML in a namespace covered by the schema set (found 'urn:test:unknown').", Code: MustCodes.Xml.Namespace.Unknown)),
            new(nameof(TestSchemas.NoNamespaceDocument), TestSchemas.NoNamespaceDocument, new MustExpected(false, "value must be XML in a namespace covered by the schema set (found '').", Code: MustCodes.Xml.Namespace.Unknown)),
            new(nameof(TestSchemas.Malformed), TestSchemas.Malformed, new MustExpected(false, "value must be XML.", Code: MustCodes.Xml.Document.Invalid)),
            new(nameof(TestSchemas.NotXml), TestSchemas.NotXml, new MustExpected(false, "value must be XML.", Code: MustCodes.Xml.Document.Invalid)),
            new("Null", null, new MustExpected(false, "value must not be null.", "value", MustCodes.Xml.Document.Invalid)),
            new("Whitespace", "   ", new MustExpected(false, "value must be XML.", Code: MustCodes.Xml.Document.Invalid))
        ];
    }

    public static class ValidXmlWithOptions
    {
        public static TheoryData<MustCase<(string? value, XmlSchemaValidationOptions? options)>> InvalidCases =>
        [
            new("SupplementaryWarningsAsViolations", (TestSchemas.ValidWithSupplementary, new XmlSchemaValidationOptions { TreatWarningsAsViolations = true }), new MustExpected(false, "value must be valid against the XML schema (1 violation(s), first at 'Document/SplmtryData/Extra').", Code: MustCodes.Xml.Schema.Mismatch))
        ];
    }

    public static class ValidXmlNullSchemas
    {
        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(nameof(TestSchemas.ValidMessage), TestSchemas.ValidMessage, new ExpectedException(typeof(ArgumentNullException))),
            new InvalidCase("NullValueAndNullSchemas", null, new ExpectedException(typeof(ArgumentNullException)))
        ];
    }
}
