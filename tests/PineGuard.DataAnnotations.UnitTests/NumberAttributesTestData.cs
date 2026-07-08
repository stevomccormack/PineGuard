using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.NumberRulesFixtures;

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
        public static TheoryData<ValidCase> ValidCases => [new("positive int", F.IsPositive.Positive, true), new("positive double", 1.5, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", F.IsPositive.Zero, false), new("negative", F.IsPositive.Negative, false)];
    }

    public static class Negative
    {
        public static TheoryData<ValidCase> ValidCases => [new("negative int", F.IsNegative.Negative, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", F.IsNegative.Zero, false), new("positive", F.IsNegative.Positive, false)];
    }

    public static class Zero
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", F.IsZero.Zero, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("one", F.IsZero.NonZero, false)];
    }

    public static class NotZero
    {
        public static TheoryData<ValidCase> ValidCases => [new("one", F.IsNotZero.Positive, true), new("negative", F.IsNotZero.Negative, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", F.IsNotZero.Zero, false)];
    }

    public static class ZeroOrPositive
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", F.IsZeroOrPositive.Zero, true), new("positive", F.IsZeroOrPositive.Positive, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("negative", F.IsZeroOrPositive.Negative, false)];
    }

    public static class ZeroOrNegative
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", F.IsZeroOrNegative.Zero, true), new("negative", F.IsZeroOrNegative.Negative, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("positive", F.IsZeroOrNegative.Positive, false)];
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
        public static TheoryData<ValidCase> ValidCases => [new("finite double", F.IsFinite.FiniteDouble, true), new("finite float", F.IsFinite.FiniteFloat, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("infinity", F.IsFinite.PositiveInfinityDouble, false)];
    }

    public static class NotFinite
    {
        public static TheoryData<ValidCase> ValidCases => [new("infinity double", F.IsFinite.PositiveInfinityDouble, true), new("infinity float", F.IsFinite.PositiveInfinityFloat, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("finite", F.IsFinite.FiniteDouble, false)];
    }

    public static class NaN
    {
        public static TheoryData<ValidCase> ValidCases => [new("NaN", F.IsNaN.NaNDouble, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("number", F.IsNaN.FiniteDouble, false)];
    }

    public static class NotNaN
    {
        public static TheoryData<ValidCase> ValidCases => [new("number double", F.IsNaN.FiniteDouble, true), new("number float", F.IsNaN.FiniteFloat, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("NaN", F.IsNaN.NaNDouble, false)];
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
