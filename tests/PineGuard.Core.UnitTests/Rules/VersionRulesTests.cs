using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class VersionRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(VersionRulesTestData.IsSemVer.Cases), MemberType = typeof(VersionRulesTestData.IsSemVer))]
    public void IsSemVer_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = VersionRules.IsSemVer(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
