using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class StringUtilityGuidTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringUtilityGuidTestData.TryParse.Cases), MemberType = typeof(StringUtilityGuidTestData.TryParse))]
    public void TryParse_BehavesAsExpected(RuleCase<(string? value, Guid parsed)> tc)
    {
        // Act
        var success = StringUtility.Guid.TryParse(tc.Value.value, out var parsed);

        // Assert
        AssertResult(tc, success);
        Assert.Equal(tc.Value.parsed, parsed);
    }
}
