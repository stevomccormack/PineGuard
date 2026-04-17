using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class JsonRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(JsonRulesTestData.IsJson.Cases), MemberType = typeof(JsonRulesTestData.IsJson))]
    public void IsJson_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = JsonRules.IsJson(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(JsonRulesTestData.IsJsonObject.Cases), MemberType = typeof(JsonRulesTestData.IsJsonObject))]
    public void IsJsonObject_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = JsonRules.IsJsonObject(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(JsonRulesTestData.IsJsonArray.Cases), MemberType = typeof(JsonRulesTestData.IsJsonArray))]
    public void IsJsonArray_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = JsonRules.IsJsonArray(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(JsonRulesTestData.IsJsonContentType.Cases), MemberType = typeof(JsonRulesTestData.IsJsonContentType))]
    public void IsJsonContentType_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = JsonRules.IsJsonContentType(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
