using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class GuidRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsEmpty.Cases), MemberType = typeof(GuidRulesTestData.IsEmpty))]
    public void IsEmpty_BehavesAsExpected(RuleCase<Guid?> tc)
    {
        // Act
        var result = GuidRules.IsEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsNotEmpty.Cases), MemberType = typeof(GuidRulesTestData.IsNotEmpty))]
    public void IsNotEmpty_BehavesAsExpected(RuleCase<Guid?> tc)
    {
        // Act
        var result = GuidRules.IsNotEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.HasVersion.Cases), MemberType = typeof(GuidRulesTestData.HasVersion))]
    public void HasVersion_BehavesAsExpected(RuleCase<(Guid? value, int version)> tc)
    {
        // Act
        var result = GuidRules.HasVersion(tc.Value.value, tc.Value.version);

        // Assert
        AssertResult(tc, result);
    }
}
