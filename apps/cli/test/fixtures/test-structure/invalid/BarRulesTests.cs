using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

// Deliberately invalid for the invalid/ fixture, two ways:
//  1. v11 §5.1 — "Test class `XxxTests` must be `sealed`"; this one is not.
//  2. v11 §5.1 — "Test methods must be **instance** methods declared
//     `public void`"; IsBar_BehavesAsExpected below is `public static void`,
//     which every one of the five layer addenda also forbids explicitly
//     ("Instance methods — `public void` (not `public static void`)").
public class BarRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(BarRulesTestData.IsBar.Cases), MemberType = typeof(BarRulesTestData.IsBar))]
    public static void IsBar_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = BarRules.IsBar(tc.Value);

        // Assert
        Assert.Equal(tc.Expected.IsValid, result);
    }
}
