using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
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

    [Theory]
    [MemberData(nameof(XmlRulesTestData.HasXmlRoot.Cases), MemberType = typeof(XmlRulesTestData.HasXmlRoot))]
    [MemberData(nameof(XmlRulesTestData.HasXmlRoot.AnyNamespaceCases), MemberType = typeof(XmlRulesTestData.HasXmlRoot))]
    public void HasXmlRoot_BehavesAsExpected(RuleCase<(string? value, string localName, string? namespaceUri)> tc)
    {
        // Arrange
        var (value, localName, namespaceUri) = tc.Value;

        // Act
        var result = XmlRules.HasXmlRoot(value, localName, namespaceUri);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(XmlRulesTestData.HasXmlRoot.InvalidCases), MemberType = typeof(XmlRulesTestData.HasXmlRoot))]
    public void HasXmlRoot_Throws_WhenLocalNameIsNullOrWhiteSpace(XmlRulesTestData.HasXmlRoot.InvalidCase tc)
    {
        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => XmlRules.HasXmlRoot(tc.Input.Value, tc.Input.LocalName));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
