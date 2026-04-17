using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesGuidTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesGuidTestData.IsGuid.Cases), MemberType = typeof(StringRulesGuidTestData.IsGuid))]
    public void IsGuid_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Guid.IsGuid(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesGuidTestData.IsNotEmpty.Cases), MemberType = typeof(StringRulesGuidTestData.IsNotEmpty))]
    public void IsNotEmpty_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Guid.IsNotEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
