using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class NumberAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static class Positive
    {
        public static TheoryData<ValidCase> ValidCases => [new("positive int", 1, true), new("positive double", 1.5, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", 0, false), new("negative", -1, false)];
    }

    public static class Negative
    {
        public static TheoryData<ValidCase> ValidCases => [new("negative int", -1, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", 0, false), new("positive", 1, false)];
    }

    public static class Zero
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", 0, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("one", 1, false)];
    }

    public static class NotZero
    {
        public static TheoryData<ValidCase> ValidCases => [new("one", 1, true), new("negative", -1, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", 0, false)];
    }

    public static class ZeroOrPositive
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", 0, true), new("positive", 1, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("negative", -1, false)];
    }

    public static class ZeroOrNegative
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", 0, true), new("negative", -1, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("positive", 1, false)];
    }

    public static class Even
    {
        public static TheoryData<ValidCase> ValidCases => [new("even int", 2, true), new("even long", 2L, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("odd int", 1, false), new("odd long", 1L, false)];
    }

    public static class Odd
    {
        public static TheoryData<ValidCase> ValidCases => [new("odd int", 1, true), new("odd long", 1L, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("even int", 2, false), new("even long", 2L, false)];
    }

    public static class Finite
    {
        public static TheoryData<ValidCase> ValidCases => [new("finite double", 1.0, true), new("finite float", 1.0f, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("infinity", double.PositiveInfinity, false)];
    }

    public static class NotFinite
    {
        public static TheoryData<ValidCase> ValidCases => [new("infinity double", double.PositiveInfinity, true), new("infinity float", float.PositiveInfinity, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("finite", 1.0, false)];
    }

    public static class NaN
    {
        public static TheoryData<ValidCase> ValidCases => [new("NaN", double.NaN, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("number", 1.0, false)];
    }

    public static class NotNaN
    {
        public static TheoryData<ValidCase> ValidCases => [new("number double", 1.0, true), new("number float", 1.0f, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("NaN", double.NaN, false)];
    }

    // Config: Min=10.
    public static class GreaterThanOrEqual
    {
        public static TheoryData<ValidCase> ValidCases => [new("equal", 10, true), new("greater", 11, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("less", 9, false)];
    }

    // Config: Max=10.
    public static class LessThanOrEqual
    {
        public static TheoryData<ValidCase> ValidCases => [new("equal", 10, true), new("less", 9, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("greater", 11, false)];
    }

    // Config: [10, 20] Inclusive
    public static class InRange
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("min", 10, true),
            new("max", 20, true),
            new("mid", 15, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("below", 9, false), new("above", 21, false)];
    }

    // Config: [10, 20] Inclusive
    public static class OutOfRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("below", 9, true), new("above", 21, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("min", 10, false), new("max", 20, false)];
    }

    // Config: Factor=5
    public static class MultipleOf
    {
        public static TheoryData<ValidCase> ValidCases => [new("multiple", 10, true), new("factor", 5, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("not multiple", 6, false)];
    }

    // Config: Factor=5
    public static class NotMultipleOf
    {
        public static TheoryData<ValidCase> ValidCases => [new("not multiple", 6, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("multiple", 10, false)];
    }

    // Config: Target=10. Tolerance=1. Range [9, 11].
    public static class Approximately
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("exact", 10.0, true),
            new("near max", 11.0, true),
            new("near min", 9.0, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("too low", 8.9, false), new("too high", 11.1, false)];
    }

    // Config: Target=10. Tolerance=1.
    public static class NotApproximately
    {
        public static TheoryData<ValidCase> ValidCases => [new("too low", 8.9, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("exact", 10.0, false)];
    }

    // Config: Target=10. No tolerance (null tolerance always fails in MustNumberClauses).
    public static class ApproximatelyNoTolerance
    {
        public static TheoryData<ValidCase> InvalidCases => [new("null tolerance exact", 10.0, false), new("null tolerance far", 100.0, false)];
    }

    // Config: Target=10. No tolerance (null tolerance always fails in MustNumberClauses).
    public static class NotApproximatelyNoTolerance
    {
        public static TheoryData<ValidCase> InvalidCases => [new("null tolerance exact", 10.0, false), new("null tolerance far", 100.0, false)];
    }
}
