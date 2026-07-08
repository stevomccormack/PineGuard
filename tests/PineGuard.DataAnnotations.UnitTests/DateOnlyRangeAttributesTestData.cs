using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class DateOnlyRangeAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    private static DateOnlyRange Range(int startDay, int endDay) =>
        new(new DateOnly(2020, 1, startDay), new DateOnly(2020, 1, endDay));

    // Chronological() — exclusive, start must be strictly before end
    public static class ChronologicalDateOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("start before end", Range(10, 20), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("start equals end", Range(10, 10), false)];
    }

    // Overlapping("2020-01-15", "2020-01-25") — range2 = [15, 25], exclusive
    public static class OverlappingDateOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("overlaps", Range(10, 20), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("no overlap", Range(1, 5), false)];
    }

    // NotOverlapping("2020-01-15", "2020-01-25") — range2 = [15, 25], exclusive
    public static class NotOverlappingDateOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("no overlap", Range(1, 5), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("overlaps", Range(10, 20), false)];
    }

    // Contains("2020-01-15") — inclusive
    public static class ContainsDateOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("contains", Range(10, 20), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("does not contain", Range(1, 5), false)];
    }

    // NotContains("2020-01-15") — inclusive
    public static class NotContainsDateOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("does not contain", Range(1, 5), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("contains", Range(10, 20), false)];
    }
}
