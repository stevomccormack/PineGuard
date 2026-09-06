using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class NullRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(NullRulesTestData.IsNull.Cases), MemberType = typeof(NullRulesTestData.IsNull))]
    public void IsNull_BehavesAsExpected(RuleCase<object?> tc)
    {
        // Act
        var result = NullRules.IsNull(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NullRulesTestData.IsNotNull.Cases), MemberType = typeof(NullRulesTestData.IsNotNull))]
    public void IsNotNull_BehavesAsExpected(RuleCase<object?> tc)
    {
        // Act
        var result = NullRules.IsNotNull(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
