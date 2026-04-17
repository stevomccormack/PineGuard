using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class EmailRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(EmailRulesTestData.IsEmail.Cases), MemberType = typeof(EmailRulesTestData.IsEmail))]
    public void IsEmail_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = EmailRules.IsEmail(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EmailRulesTestData.IsStrictEmail.Cases), MemberType = typeof(EmailRulesTestData.IsStrictEmail))]
    public void IsStrictEmail_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = EmailRules.IsStrictEmail(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EmailRulesTestData.HasAlias.Cases), MemberType = typeof(EmailRulesTestData.HasAlias))]
    public void HasAlias_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = EmailRules.HasAlias(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
