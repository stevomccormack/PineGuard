using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class PredicateRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(PredicateRulesTestData.Satisfies.Cases), MemberType = typeof(PredicateRulesTestData.Satisfies))]
    public void Satisfies_BehavesAsExpected(RuleCase<(string? value, Func<string, bool> predicate)> tc)
    {
        // Act
        var result = PredicateRules.Satisfies(tc.Value.value, tc.Value.predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(PredicateRulesTestData.Satisfies.InvalidCases), MemberType = typeof(PredicateRulesTestData.Satisfies))]
    public void Satisfies_Throws_WhenPredicateIsNull(PredicateRulesTestData.Satisfies.InvalidCase tc)
    {
        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, () => PredicateRules.Satisfies(tc.Value.Value, tc.Value.Predicate!));

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
