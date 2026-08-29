using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class ChecksumRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ChecksumRulesTestData.IsLuhn.Cases), MemberType = typeof(ChecksumRulesTestData.IsLuhn))]
    public void IsLuhn_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = ChecksumRules.IsLuhn(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
