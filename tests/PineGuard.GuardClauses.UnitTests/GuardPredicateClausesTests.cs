using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardPredicateClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardPredicateClausesTestData.NotSatisfies.ValidCases), MemberType = typeof(GuardPredicateClausesTestData.NotSatisfies))]
    [MemberData(nameof(GuardPredicateClausesTestData.NotSatisfies.InvalidCases), MemberType = typeof(GuardPredicateClausesTestData.NotSatisfies))]
    public void NotSatisfies_BehavesAsExpected(GuardCase<(string? value, Func<string, bool> predicate)> tc)
    {
        var value = tc.Value.value;
        var predicate = tc.Value.predicate;
        var result = AssertResult(tc, () => Guard.Against.NotSatisfies(value, predicate));
        AssertCustomMessage(tc, () => Guard.Against.NotSatisfies(value, predicate, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardPredicateClausesTestData.Satisfies.ValidCases), MemberType = typeof(GuardPredicateClausesTestData.Satisfies))]
    [MemberData(nameof(GuardPredicateClausesTestData.Satisfies.InvalidCases), MemberType = typeof(GuardPredicateClausesTestData.Satisfies))]
    public void Satisfies_BehavesAsExpected(GuardCase<(string? value, Func<string, bool> predicate)> tc)
    {
        var value = tc.Value.value;
        var predicate = tc.Value.predicate;
        var result = AssertResult(tc, () => Guard.Against.Satisfies(value, predicate));
        AssertCustomMessage(tc, () => Guard.Against.Satisfies(value, predicate, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
