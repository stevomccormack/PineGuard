using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TokenRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TokenRulesTestData.IsJwt.Cases), MemberType = typeof(TokenRulesTestData.IsJwt))]
    public void IsJwt_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = TokenRules.IsJwt(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
