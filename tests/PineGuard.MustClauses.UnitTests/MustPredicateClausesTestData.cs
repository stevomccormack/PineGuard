using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.PredicateRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustPredicateClausesTestData
{
    public static class Satisfies
    {
        public static TheoryData<MustCase<(string? value, Func<string, bool>? predicate)>> ValidCases =>
        [
            new(nameof(F.Satisfies.Matching), (F.Satisfies.Matching.value, F.Satisfies.Matching.predicate), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(string? value, Func<string, bool>? predicate)>> InvalidCases =>
        [
            new(nameof(F.Satisfies.NotMatching), (F.Satisfies.NotMatching.value, F.Satisfies.NotMatching.predicate), new MustExpected(false, "value must satisfy the predicate.")),
            new(nameof(F.Satisfies.NullValue),   (F.Satisfies.NullValue.value,   F.Satisfies.NullValue.predicate),   new MustExpected(false, "value must satisfy the predicate.")),
            new("null-predicate", ("hello", null), new MustExpected(false, "predicate must not be null.", "predicate"))
        ];
    }

    public static class NotSatisfies
    {
        public static TheoryData<MustCase<(string? value, Func<string, bool>? predicate)>> ValidCases =>
        [
            new(nameof(F.NotSatisfies.NotMatching), (F.NotSatisfies.NotMatching.value, F.NotSatisfies.NotMatching.predicate), new MustExpected(true)),
            new(nameof(F.NotSatisfies.NullValue),   (F.NotSatisfies.NullValue.value,   F.NotSatisfies.NullValue.predicate),   new MustExpected(true))
        ];

        public static TheoryData<MustCase<(string? value, Func<string, bool>? predicate)>> InvalidCases =>
        [
            new(nameof(F.NotSatisfies.Matching), (F.NotSatisfies.Matching.value, F.NotSatisfies.Matching.predicate), new MustExpected(false, "value must not satisfy the predicate.")),
            new("null-predicate", ("hi", null), new MustExpected(false, "predicate must not be null.", "predicate"))
        ];
    }
}
