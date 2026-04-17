using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class BoolRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsTrue.Cases), MemberType = typeof(BoolRulesTestData.IsTrue))]
    public void IsTrue_BehavesAsExpected(RuleCase<bool?> tc)
    {
        // Act
        var result = BoolRules.IsTrue(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsFalse.Cases), MemberType = typeof(BoolRulesTestData.IsFalse))]
    public void IsFalse_BehavesAsExpected(RuleCase<bool?> tc)
    {
        // Act
        var result = BoolRules.IsFalse(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
