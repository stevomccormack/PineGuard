using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringNumbersAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static class PositiveString
    {
        public static TheoryData<ValidCase> ValidCases => [new("positive int", F.NumbersIsPositive.Positive, true), new("positive float", "1.5", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", F.NumbersIsPositive.Zero, false), new("negative", F.NumbersIsPositive.Negative, false), new("invalid", F.NumbersIsPositive.Letters, false)];
    }

    public static class NegativeString
    {
        public static TheoryData<ValidCase> ValidCases => [new("negative", F.NumbersIsNegative.Negative, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", F.NumbersIsNegative.Zero, false), new("positive", F.NumbersIsNegative.Positive, false)];
    }

    public static class ZeroString
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", F.NumbersIsZero.Zero, true), new("zero float", "0.0", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("one", F.NumbersIsZero.NonZero, false)];
    }

    public static class NotZeroString
    {
        public static TheoryData<ValidCase> ValidCases => [new("one", F.NumbersIsNotZero.Positive, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("zero", F.NumbersIsNotZero.Zero, false)];
    }

    public static class EvenString
    {
        public static TheoryData<ValidCase> ValidCases => [new("even", "2", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("odd", "1", false), new("decimal", "2.0", false)]; // Even(string) implies integer parsing usually?
        // Must.Be.Even(string) logic: TryParse int/long.
    }

    public static class OddString
    {
        public static TheoryData<ValidCase> ValidCases => [new("odd", "1", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("even", "2", false)];
    }

    public static class ZeroOrPositiveString
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", F.NumbersIsZeroOrPositive.Zero, true), new("positive", F.NumbersIsZeroOrPositive.Positive, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("negative", F.NumbersIsZeroOrPositive.Negative, false)];
    }

    public static class ZeroOrNegativeString
    {
        public static TheoryData<ValidCase> ValidCases => [new("zero", F.NumbersIsZeroOrNegative.Zero, true), new("negative", F.NumbersIsZeroOrNegative.Negative, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("positive", F.NumbersIsZeroOrNegative.Positive, false)];
    }

    // Min=10
    public static class GreaterThanOrEqualString
    {
        public static TheoryData<ValidCase> ValidCases => [new("equal", "10", true), new("greater", "11", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("less", "9", false)];
    }

    // Max=10
    public static class LessThanOrEqualString
    {
        public static TheoryData<ValidCase> ValidCases => [new("equal", "10", true), new("less", "9", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("greater", "11", false)];
    }

    // Range [10, 20]
    public static class InRangeString
    {
        public static TheoryData<ValidCase> ValidCases => [new("min", "10", true), new("max", "20", true), new("mid", "15", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("below", "9", false), new("above", "21", false)];
    }

    // Range [10, 20]
    public static class OutOfRangeString
    {
        public static TheoryData<ValidCase> ValidCases => [new("below", "9", true), new("above", "21", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("min", "10", false), new("max", "20", false)];
    }

    // Factor=5
    public static class MultipleOfString
    {
        public static TheoryData<ValidCase> ValidCases => [new("multiple", "10", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("not multiple", "6", false)];
    }

    // Factor=5
    public static class NotMultipleOfString
    {
        public static TheoryData<ValidCase> ValidCases => [new("not multiple", "6", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("multiple", "10", false)];
    }

    // Target=10, Tolerance=1. Range [9, 11]
    public static class ApproximatelyString
    {
        public static TheoryData<ValidCase> ValidCases => [new("exact", "10", true), new("near", "10.5", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("too low", "8.9", false), new("too high", "11.1", false)];
    }

    public static class NotApproximatelyString
    {
        public static TheoryData<ValidCase> ValidCases => [new("too low", "8.9", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("exact", "10", false)];
    }

    public static class FiniteString
    {
        public static TheoryData<ValidCase> ValidCases => [new("finite", "1", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("infinity", "Infinity", false), new("nan", F.NumbersIsNaN.NaN, false)];
        // Note: "Infinity" parsing depends on NumberStyles and Culture.
        // Expecting PineGuard default setup allows Infinity parsing if Float style used.
    }

    public static class NotFiniteString
    {
        public static TheoryData<ValidCase> ValidCases => [new("infinity", "Infinity", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("finite", "1", false)];
    }

    public static class NotNaNString
    {
        public static TheoryData<ValidCase> ValidCases => [new("finite", "1", true), new("infinity", "Infinity", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("nan", F.NumbersIsNaN.NaN, false)];
    }

    public static class PercentageString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.NumbersIsPercentage.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.NumbersIsPercentage.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a percentage between 0 and 100.", Code: MustCodes.Number.Range.NotPercentage)
        });
    }
}
