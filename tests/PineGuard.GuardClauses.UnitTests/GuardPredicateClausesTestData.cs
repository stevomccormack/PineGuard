using PineGuard.Testing.UnitTests.GuardClauses;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardPredicateClausesTestData
{
    // Guard.Against.NotSatisfies — throws when value does NOT satisfy predicate
    public static class NotSatisfies
    {
        public static TheoryData<GuardCase<(string? value, Func<string, bool> predicate)>> ValidCases =>
        [
            new("satisfies", ("hello", x => x.Length > 3), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, Func<string, bool> predicate)>> InvalidCases =>
        [
            new("not-satisfies", ("hi", x => x.Length > 3), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, x => x.Length > 3), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Satisfies — throws when value DOES satisfy predicate
    public static class Satisfies
    {
        public static TheoryData<GuardCase<(string? value, Func<string, bool> predicate)>> ValidCases =>
        [
            new("not-satisfies", ("hi", x => x.Length > 3), new GuardExpected(true)),
            new("null-value", (null, x => x.Length > 3), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? value, Func<string, bool> predicate)>> InvalidCases =>
        [
            new("satisfies", ("hello", x => x.Length > 3), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }
}
