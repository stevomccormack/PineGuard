using System.Xml.Schema;
using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class XmlSchemaRulesTestData
{
    public static class IsValidXml
    {
        private static readonly XmlSchemaValidationOptions TreatWarningsAsViolationsOptions = new() { TreatWarningsAsViolations = true };
        private static readonly XmlSchemaValidationOptions DefaultOptions = new();

        public static TheoryData<RuleCase<(string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options)>> Cases =>
        [
            new(nameof(TestSchemas.ValidMessage), (TestSchemas.ValidMessage, TestSchemas.DocumentSet, null), new RuleExpected(true)),
            new($"{nameof(TestSchemas.ValidWithSupplementary)}-ExplicitDefaultOptions", (TestSchemas.ValidWithSupplementary, TestSchemas.DocumentSet, DefaultOptions), new RuleExpected(true)),
            new(nameof(TestSchemas.MsgIdTooLong), (TestSchemas.MsgIdTooLong, TestSchemas.DocumentSet, null), new RuleExpected(false)),
            new($"{nameof(TestSchemas.ValidWithSupplementary)}-TreatWarningsAsViolations", (TestSchemas.ValidWithSupplementary, TestSchemas.DocumentSet, TreatWarningsAsViolationsOptions), new RuleExpected(false)),
            new("Null", (null, TestSchemas.DocumentSet, null), new RuleExpected(false))
        ];
    }
}
