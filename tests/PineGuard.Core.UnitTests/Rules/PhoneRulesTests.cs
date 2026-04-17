using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class PhoneRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(PhoneRulesTestData.IsPhoneNumber.Cases), MemberType = typeof(PhoneRulesTestData.IsPhoneNumber))]
    public void IsPhoneNumber_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PhoneRules.IsPhoneNumber(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
