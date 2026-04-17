using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesBoolTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesBoolTestData.IsTrue.Cases), MemberType = typeof(StringRulesBoolTestData.IsTrue))]
    public void IsTrue_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Bool.IsTrue(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesBoolTestData.IsFalse.Cases), MemberType = typeof(StringRulesBoolTestData.IsFalse))]
    public void IsFalse_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.Bool.IsFalse(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
