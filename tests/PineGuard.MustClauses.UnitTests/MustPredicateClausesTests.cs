using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustPredicateClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustPredicateClausesTestData.Satisfies.ValidCases), MemberType = typeof(MustPredicateClausesTestData.Satisfies))]
    [MemberData(nameof(MustPredicateClausesTestData.Satisfies.InvalidCases), MemberType = typeof(MustPredicateClausesTestData.Satisfies))]
    public void Satisfies_BehavesAsExpected(MustCase<(string? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.Satisfies(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustPredicateClausesTestData.NotSatisfies.ValidCases), MemberType = typeof(MustPredicateClausesTestData.NotSatisfies))]
    [MemberData(nameof(MustPredicateClausesTestData.NotSatisfies.InvalidCases), MemberType = typeof(MustPredicateClausesTestData.NotSatisfies))]
    public void NotSatisfies_BehavesAsExpected(MustCase<(string? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.NotSatisfies(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
