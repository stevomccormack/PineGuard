using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class GuardXmlSchemaClausesTestData
{
    public static class InvalidXml
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
        [
            new(nameof(TestSchemas.ValidMessage), TestSchemas.ValidMessage, new GuardExpected(true)),
            new(nameof(TestSchemas.ValidWithSupplementary), TestSchemas.ValidWithSupplementary, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(TestSchemas.MsgIdTooLong), TestSchemas.MsgIdTooLong, new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Xml.Schema.Mismatch)),
            new(nameof(TestSchemas.TwoViolations), TestSchemas.TwoViolations, new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Xml.Schema.Mismatch)),
            new(nameof(TestSchemas.UnknownNamespaceDocument), TestSchemas.UnknownNamespaceDocument, new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Xml.Namespace.Unknown)),
            new(nameof(TestSchemas.NoNamespaceDocument), TestSchemas.NoNamespaceDocument, new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Xml.Namespace.Unknown)),
            new(nameof(TestSchemas.Malformed), TestSchemas.Malformed, new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Xml.Document.Invalid)),
            new(nameof(TestSchemas.NotXml), TestSchemas.NotXml, new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Xml.Document.Invalid)),
            new("Null", null, new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Xml.Document.Invalid)),
            new("Whitespace", "   ", new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Xml.Document.Invalid))
        ];
    }

    public static class InvalidXmlExceptionCreator
    {
        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(TestSchemas.MsgIdTooLong), TestSchemas.MsgIdTooLong, new GuardExpected(false, typeof(InvalidOperationException), MessageContains: "custom"))
        ];
    }
}
