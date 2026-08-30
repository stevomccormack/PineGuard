using PineGuard.Codes;
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
            new(nameof(F.Satisfies.NotMatching), (F.Satisfies.NotMatching.value, F.Satisfies.NotMatching.predicate), new MustExpected(false, "value must satisfy the predicate.", Code: MustCodes.Predicate.Result.False)),
            new(nameof(F.Satisfies.NullValue),   (F.Satisfies.NullValue.value,   F.Satisfies.NullValue.predicate),   new MustExpected(false, "value must satisfy the predicate.")),
            new("null-predicate", ("hello", null), new MustExpected(false, "predicate must not be null.", "predicate", MustCodes.Predicate.Callback.Null))
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
            new(nameof(F.NotSatisfies.Matching), (F.NotSatisfies.Matching.value, F.NotSatisfies.Matching.predicate), new MustExpected(false, "value must not satisfy the predicate.", Code: MustCodes.Predicate.Result.True)),
            new("null-predicate", ("hi", null), new MustExpected(false, "predicate must not be null.", "predicate", MustCodes.Predicate.Callback.Null))
        ];
    }

    public static class SatisfiesAsync
    {
        public static TheoryData<MustCase<(string? value, Func<string, CancellationToken, ValueTask<bool>>? predicate)>> ValidCases =>
        [
            new(nameof(F.Satisfies.Matching), (F.Satisfies.Matching.value, Awaited(F.Satisfies.Matching.predicate)), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(string? value, Func<string, CancellationToken, ValueTask<bool>>? predicate)>> InvalidCases =>
        [
            new(nameof(F.Satisfies.NotMatching), (F.Satisfies.NotMatching.value, Awaited(F.Satisfies.NotMatching.predicate)), new MustExpected(false, "value must satisfy the predicate.", Code: MustCodes.Predicate.Result.False)),
            new(nameof(F.Satisfies.NullValue),   (F.Satisfies.NullValue.value,   Awaited(F.Satisfies.NullValue.predicate)),   new MustExpected(false, "value must satisfy the predicate.")),
            new("null-predicate", ("hello", null), new MustExpected(false, "predicate must not be null.", "predicate", MustCodes.Predicate.Callback.Null))
        ];
    }

    public static class NotSatisfiesAsync
    {
        public static TheoryData<MustCase<(string? value, Func<string, CancellationToken, ValueTask<bool>>? predicate)>> ValidCases =>
        [
            new(nameof(F.NotSatisfies.NotMatching), (F.NotSatisfies.NotMatching.value, Awaited(F.NotSatisfies.NotMatching.predicate)), new MustExpected(true)),
            new(nameof(F.NotSatisfies.NullValue),   (F.NotSatisfies.NullValue.value,   Awaited(F.NotSatisfies.NullValue.predicate)),   new MustExpected(true))
        ];

        public static TheoryData<MustCase<(string? value, Func<string, CancellationToken, ValueTask<bool>>? predicate)>> InvalidCases =>
        [
            new(nameof(F.NotSatisfies.Matching), (F.NotSatisfies.Matching.value, Awaited(F.NotSatisfies.Matching.predicate)), new MustExpected(false, "value must not satisfy the predicate.", Code: MustCodes.Predicate.Result.True)),
            new("null-predicate", ("hi", null), new MustExpected(false, "predicate must not be null.", "predicate", MustCodes.Predicate.Callback.Null))
        ];
    }

    public static class AsyncCancellation
    {
        public static TheoryData<bool> Cases => [true];
    }

    /// <summary>
    /// Lifts a synchronous fixture predicate into the asynchronous shape the <c>*Async</c> clauses take, so
    /// both pairs are driven by the same fixtures and prove identical semantics.
    /// </summary>
    private static Func<string, CancellationToken, ValueTask<bool>> Awaited(Func<string, bool> predicate) =>
        (value, _) => new ValueTask<bool>(predicate(value));
}
