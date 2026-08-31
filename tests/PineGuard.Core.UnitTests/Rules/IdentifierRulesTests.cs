using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class IdentifierRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(IdentifierRulesTestData.IsSlug.Cases), MemberType = typeof(IdentifierRulesTestData.IsSlug))]
    public void IsSlug_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = IdentifierRules.IsSlug(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(IdentifierRulesTestData.IsSlug.AdHocCases), MemberType = typeof(IdentifierRulesTestData.IsSlug))]
    public void IsSlug_AdHoc_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = IdentifierRules.IsSlug(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(IdentifierRulesTestData.IsUlid.Cases), MemberType = typeof(IdentifierRulesTestData.IsUlid))]
    public void IsUlid_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = IdentifierRules.IsUlid(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
