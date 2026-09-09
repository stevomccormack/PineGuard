using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class BoundaryRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(BoundaryRulesTestData.IsRollup.Cases), MemberType = typeof(BoundaryRulesTestData.IsRollup))]
    public void IsRollup_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = BoundaryRules.IsRollup(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BoundaryRulesTestData.IsSplit.ValidCases), MemberType = typeof(BoundaryRulesTestData.IsSplit))]
    [MemberData(nameof(BoundaryRulesTestData.IsSplit.EdgeCases), MemberType = typeof(BoundaryRulesTestData.IsSplit))]
    [MemberData(nameof(BoundaryRulesTestData.IsSplit.InvalidCases), MemberType = typeof(BoundaryRulesTestData.IsSplit))]
    public void IsSplit_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = BoundaryRules.IsSplit(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BoundaryRulesTestData.IsNullOnly.Cases), MemberType = typeof(BoundaryRulesTestData.IsNullOnly))]
    [MemberData(nameof(BoundaryRulesTestData.IsNullOnly.NullCases), MemberType = typeof(BoundaryRulesTestData.IsNullOnly))]
    public void IsNullOnly_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = BoundaryRules.IsNullOnly(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
