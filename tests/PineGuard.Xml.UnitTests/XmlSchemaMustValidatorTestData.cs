using System.Xml.Schema;
using PineGuard.Codes;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Xml;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class XmlSchemaMustValidatorTestData
{
    public static class Validate
    {
        private const string Whitespace = "   ";

        private static readonly XmlSchemaValidationOptions TreatWarningsAsViolationsOptions = new() { TreatWarningsAsViolations = true };

        public static TheoryData<MustValidationCase<(string? xml, XmlSchemaValidationOptions? options)>> Cases =>
        [
            new(nameof(TestSchemas.ValidMessage), (TestSchemas.ValidMessage, null), new MustValidationExpected(true)),
            new($"{nameof(TestSchemas.ValidWithSupplementary)}-Default", (TestSchemas.ValidWithSupplementary, null), new MustValidationExpected(true)),
            new($"{nameof(TestSchemas.ValidWithSupplementary)}-TreatWarningsAsViolations", (TestSchemas.ValidWithSupplementary, TreatWarningsAsViolationsOptions), new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Document/SplmtryData/Extra", Code: MustCodes.Xml.Schema.Mismatch)),
            new(nameof(TestSchemas.MsgIdTooLong), (TestSchemas.MsgIdTooLong, null), new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Document/GrpHdr/MsgId", Code: MustCodes.Xml.Schema.Mismatch)),
            new(nameof(TestSchemas.TwoViolations), (TestSchemas.TwoViolations, null), new MustValidationExpected(false, FailureCount: 2, PropertyPath: "Document/GrpHdr/MsgId", Code: MustCodes.Xml.Schema.Mismatch)),
            new(nameof(TestSchemas.UnknownNamespaceDocument), (TestSchemas.UnknownNamespaceDocument, null), new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Document", Code: MustCodes.Xml.Namespace.Unknown)),
            new(nameof(TestSchemas.NoNamespaceDocument), (TestSchemas.NoNamespaceDocument, null), new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Document", Code: MustCodes.Xml.Namespace.Unknown)),
            new(nameof(TestSchemas.Malformed), (TestSchemas.Malformed, null), new MustValidationExpected(false, FailureCount: 1, Code: MustCodes.Xml.Document.Invalid)),
            new("Null", (null, null), new MustValidationExpected(false, "Value must not be null.", FailureCount: 1, PropertyPath: "", Code: MustCodes.Xml.Document.Invalid)),
            new(nameof(Whitespace), (Whitespace, null), new MustValidationExpected(false, "Value must be XML.", FailureCount: 1, PropertyPath: "", Code: MustCodes.Xml.Document.Invalid))
        ];
    }

    public static class ValidateAsync
    {
        public static TheoryData<MustValidationCase<(string? xml, XmlSchemaValidationOptions? options)>> Cases =>
        [
            new(nameof(TestSchemas.ValidMessage), (TestSchemas.ValidMessage, null), new MustValidationExpected(true)),
            new(nameof(TestSchemas.MsgIdTooLong), (TestSchemas.MsgIdTooLong, null), new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Document/GrpHdr/MsgId", Code: MustCodes.Xml.Schema.Mismatch))
        ];
    }

    public static class ValidatedType
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("String", typeof(string))
        ];

        public sealed record Case(string Name, Type Expected)
            : BaseCase(Name);
    }

    public static class NonGenericValidate
    {
        public static TheoryData<MustValidationCase<object?>> Cases =>
        [
            new(nameof(TestSchemas.ValidMessage), TestSchemas.ValidMessage, new MustValidationExpected(true)),
            new(nameof(TestSchemas.MsgIdTooLong), TestSchemas.MsgIdTooLong, new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Document/GrpHdr/MsgId", Code: MustCodes.Xml.Schema.Mismatch))
        ];
    }

    public static class NonGenericValidateAsync
    {
        public static TheoryData<MustValidationCase<(object? value, MustValidationMode? mode)>> Cases =>
        [
            new($"{nameof(TestSchemas.ValidMessage)}-DefaultMode", (TestSchemas.ValidMessage, null), new MustValidationExpected(true)),
            new($"{nameof(TestSchemas.ValidMessage)}-Aggregate", (TestSchemas.ValidMessage, MustValidationMode.Aggregate), new MustValidationExpected(true)),
            new($"{nameof(TestSchemas.MsgIdTooLong)}-Aggregate", (TestSchemas.MsgIdTooLong, MustValidationMode.Aggregate), new MustValidationExpected(false, FailureCount: 1, PropertyPath: "Document/GrpHdr/MsgId", Code: MustCodes.Xml.Schema.Mismatch))
        ];
    }

    public static class NonGenericValidateCastThrows
    {
        public static TheoryData<ThrowCase> InvalidCases =>
        [
            new("NonStringValue", 123, new ExpectedException(typeof(ArgumentException), "value"))
        ];

        public sealed record ThrowCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class Constructor
    {
        public static TheoryData<MustValidationCase<XmlSchemaValidationOptions?>> ValidCases =>
        [
            new("NullOptionsUsesDefault", null, new MustValidationExpected(true))
        ];

        public static TheoryData<ThrowCase> InvalidCases =>
        [
            new("NullSchemas", null, new ExpectedException(typeof(ArgumentNullException), "schemas"))
        ];

        public sealed record ThrowCase(string Name, XmlSchemaSet? Value, ExpectedException ExpectedException)
            : ThrowsCase<XmlSchemaSet?>(Name, Value, ExpectedException);
    }
}
