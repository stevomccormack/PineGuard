using System.Xml.Schema;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Xml.UnitTests;

public sealed class XmlSchemaRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(XmlSchemaRulesTestData.IsValidXml.Cases), MemberType = typeof(XmlSchemaRulesTestData.IsValidXml))]
    public void IsValidXml_BehavesAsExpected(RuleCase<(string? value, XmlSchemaSet schemas, XmlSchemaValidationOptions? options)> tc)
    {
        // Arrange
        var (value, schemas, options) = tc.Value;

        // Act
        var result = XmlSchemaRules.IsValidXml(value, schemas, options);

        // Assert
        AssertResult(tc, result);
    }
}
