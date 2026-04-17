using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class XmlRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(XmlRulesTestData.IsXml.Cases), MemberType = typeof(XmlRulesTestData.IsXml))]
    public void IsXml_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = XmlRules.IsXml(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(XmlRulesTestData.IsXmlContentType.Cases), MemberType = typeof(XmlRulesTestData.IsXmlContentType))]
    public void IsXmlContentType_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = XmlRules.IsXmlContentType(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
