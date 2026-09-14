using System.Xml.Schema;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class XmlSchemaUtilityTestData
{
    private const string Whitespace = "   ";

    private static readonly XmlSchemaValidationOptions TreatWarningsAsViolationsOptions = new() { TreatWarningsAsViolations = true };
    private static readonly XmlSchemaValidationOptions RequireKnownNamespaceFalseOptions = new() { RequireKnownNamespace = false };

    public static class TryValidate
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(TestSchemas.ValidMessage), (TestSchemas.ValidMessage, TestSchemas.DocumentSet, null), new ExpectedResult(true, [])),
            new($"{nameof(TestSchemas.ValidWithSupplementary)}-Default", (TestSchemas.ValidWithSupplementary, TestSchemas.DocumentSet, null), new ExpectedResult(true, [])),
            new($"{nameof(TestSchemas.ValidWithSupplementary)}-TreatWarningsAsViolations", (TestSchemas.ValidWithSupplementary, TestSchemas.DocumentSet, TreatWarningsAsViolationsOptions), new ExpectedResult(false, [(XmlSchemaViolationKind.Warning, "Document/SplmtryData/Extra")])),
            new(nameof(TestSchemas.MsgIdTooLong), (TestSchemas.MsgIdTooLong, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.Error, "Document/GrpHdr/MsgId")])),
            new(nameof(TestSchemas.BadDateTime), (TestSchemas.BadDateTime, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.Error, "Document/GrpHdr/CreDtTm")])),
            new(nameof(TestSchemas.MissingMsgId), (TestSchemas.MissingMsgId, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.Error, "Document/GrpHdr/CreDtTm")])),
            new(nameof(TestSchemas.TwoViolations), (TestSchemas.TwoViolations, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.Error, "Document/GrpHdr/MsgId"), (XmlSchemaViolationKind.Error, "Document/GrpHdr/CreDtTm")])),
            new($"{nameof(TestSchemas.UnknownNamespaceDocument)}-Default", (TestSchemas.UnknownNamespaceDocument, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.UnknownNamespace, "Document")])),
            new($"{nameof(TestSchemas.UnknownNamespaceDocument)}-RequireKnownNamespaceFalse", (TestSchemas.UnknownNamespaceDocument, TestSchemas.DocumentSet, RequireKnownNamespaceFalseOptions), new ExpectedResult(true, [])),
            new(nameof(TestSchemas.NoNamespaceDocument), (TestSchemas.NoNamespaceDocument, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.UnknownNamespace, "Document")])),
            new(nameof(TestSchemas.Malformed), (TestSchemas.Malformed, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.NotWellFormed, "Document/GrpHdr")])),
            new(nameof(TestSchemas.NotXml), (TestSchemas.NotXml, TestSchemas.DocumentSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.NotWellFormed, "")])),
            new("Null", (null, TestSchemas.DocumentSet, null), new ExpectedResult(false, [])),
            new(nameof(Whitespace), (Whitespace, TestSchemas.DocumentSet, null), new ExpectedResult(false, [])),
            new(nameof(TestSchemas.EmptyElementValid), (TestSchemas.EmptyElementValid, TestSchemas.AttributeSet, null), new ExpectedResult(true, [])),
            new(nameof(TestSchemas.MissingRequiredAttribute), (TestSchemas.MissingRequiredAttribute, TestSchemas.AttributeSet, null), new ExpectedResult(false, [(XmlSchemaViolationKind.Error, "Document/MsgId")]))
        ];

        public sealed record Case(string Name, (string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options) Value, ExpectedResult Expected)
            : ReturnCase<(string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options), ExpectedResult>(Name, Value, Expected);

        public sealed record ExpectedResult(bool Ok, (XmlSchemaViolationKind kind, string path)[] Violations);
    }

    public static class TryValidateNullSchemas
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("NullSchemas", null, new ExpectedException(typeof(ArgumentNullException), "schemas"))
        ];

        public sealed record InvalidCase(string Name, XmlSchemaSet? Value, ExpectedException ExpectedException)
            : ThrowsCase<XmlSchemaSet?>(Name, Value, ExpectedException);
    }
}
