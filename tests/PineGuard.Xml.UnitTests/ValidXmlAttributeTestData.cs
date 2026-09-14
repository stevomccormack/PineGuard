using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class ValidXmlAttributeTestData
{
    private sealed class SchemaServiceProvider(XmlSchemaSet? schemas, XmlSchemaValidationOptions? options) : IServiceProvider
    {
        public object? GetService(Type serviceType) =>
            serviceType == typeof(XmlSchemaSet) ? schemas :
            serviceType == typeof(XmlSchemaValidationOptions) ? options :
            null;
    }

    public static readonly IServiceProvider DocumentSchemaProvider = new SchemaServiceProvider(TestSchemas.DocumentSet, null);
    public static readonly IServiceProvider DocumentSchemaWarningsAsViolationsProvider = new SchemaServiceProvider(TestSchemas.DocumentSet, new XmlSchemaValidationOptions { TreatWarningsAsViolations = true });

    public static class ValidXml
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(TestSchemas.ValidMessage), TestSchemas.ValidMessage, new DataAnnotationExpected(true)),
            new(nameof(TestSchemas.ValidWithSupplementary), TestSchemas.ValidWithSupplementary, new DataAnnotationExpected(true)),
            new(nameof(TestSchemas.MsgIdTooLong), TestSchemas.MsgIdTooLong, new DataAnnotationExpected(false, "Object must be valid against the XML schema (1 violation(s), first at 'Document/GrpHdr/MsgId').")),
            new("Null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class ValidXmlTreatWarningsAsViolations
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(TestSchemas.ValidWithSupplementary), TestSchemas.ValidWithSupplementary, new DataAnnotationExpected(false, "Object must be valid against the XML schema (1 violation(s), first at 'Document/SplmtryData/Extra')."))
        ];
    }

    public static class ValidXmlTypeMismatch
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase(
                "int-value",
                () => new ValidXmlAttribute().GetValidationResult(123, new ValidationContext(new object())),
                new ExpectedException(typeof(InvalidOperationException)))
        ];
    }

    public static class ValidXmlMissingSchemaService
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase(
                "no-schema-service",
                () => new ValidXmlAttribute().GetValidationResult(TestSchemas.ValidMessage, new ValidationContext(new object())),
                new ExpectedException(typeof(InvalidOperationException), null, "XmlSchemaSet"))
        ];
    }
}
