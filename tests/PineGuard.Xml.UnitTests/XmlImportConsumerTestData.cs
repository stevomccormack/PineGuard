using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Xml.UnitTests.Schemas;

namespace PineGuard.Xml.UnitTests;

public static class XmlImportConsumerTestData
{
    public static TheoryData<RuleCase<(string xml, bool wellFormed)>> Cases =>
    [
        new("SchemaConforming", (TestSchemas.ValidMessage, true), new RuleExpected(true)),
        new("WellFormedSchemaViolation", (TestSchemas.MissingMsgId, true), new RuleExpected(false)),
        new("Malformed", (TestSchemas.Malformed, false), new RuleExpected(false))
    ];
}
