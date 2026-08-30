using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentPredicateExtensionsTestData
{
    // Predicate: x => x > 0
    public static class Satisfies
    {
        public static TheoryData<FluentCase<int>> Cases =>
        [
            new("satisfies", 1, new FluentExpected(true)),
            new("not-satisfies", -1, new FluentExpected(false, "Value must satisfy the predicate.", Code: MustCodes.Predicate.Result.False))
        ];
    }

    // Predicate: x => x > 0  (NotSatisfies passes when predicate is false)
    public static class NotSatisfies
    {
        public static TheoryData<FluentCase<int>> Cases =>
        [
            new("not-satisfies", -1, new FluentExpected(true)),
            new("satisfies", 1, new FluentExpected(false, "Value must not satisfy the predicate."))
        ];
    }

    // Predicate: (x, ct) => x > 0
    public static class SatisfiesAsync
    {
        public static TheoryData<FluentCase<int>> Cases =>
        [
            new("satisfies", 1, new FluentExpected(true)),
            new("not-satisfies", -1, new FluentExpected(false, "Value must satisfy the predicate.", Code: MustCodes.Predicate.Result.False))
        ];
    }

    // Predicate: (x, ct) => x > 0  (NotSatisfiesAsync passes when predicate is false)
    public static class NotSatisfiesAsync
    {
        public static TheoryData<FluentCase<int>> Cases =>
        [
            new("not-satisfies", -1, new FluentExpected(true)),
            new("satisfies", 1, new FluentExpected(false, "Value must not satisfy the predicate.", Code: MustCodes.Predicate.Result.True))
        ];
    }
}
