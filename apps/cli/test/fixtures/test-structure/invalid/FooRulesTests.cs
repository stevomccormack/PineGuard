using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class FooRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    // Deliberately invalid for the invalid/ fixture: v11 §5.1 requires the
    // Tests class to be flat. Operation Groups belong in TestData only.
    public static class NotAllowedHere
    {
        public const string Marker = "nested static group inside Tests.cs";
    }

    [Theory]
    [MemberData(nameof(FooRulesTestData.IsBar.ValidCases), MemberType = typeof(FooRulesTestData.IsBar))]
    [MemberData(nameof(FooRulesTestData.IsBar.InvalidCases), MemberType = typeof(FooRulesTestData.IsBar))]
    public void IsBar_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = FooRules.IsBar(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FooRulesTestData.IsBaz.ValidCases), MemberType = typeof(FooRulesTestData.IsBaz))]
    public void IsBaz_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = FooRules.IsBaz(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
