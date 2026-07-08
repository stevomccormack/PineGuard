using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TimeOnlyRangeAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    private static TimeOnlyRange Range(int startHour, int endHour) =>
        new(new TimeOnly(startHour, 0), new TimeOnly(endHour, 0));

    // Chronological() — exclusive, start must be strictly before end
    public static class ChronologicalTimeOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("start before end", Range(10, 12), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("start equals end", Range(10, 10), false)];
    }

    // Overlapping("13:00", "17:00") — range2 = [13, 17], exclusive
    public static class OverlappingTimeOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("overlaps", Range(10, 14), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("no overlap", Range(6, 8), false)];
    }

    // NotOverlapping("13:00", "17:00") — range2 = [13, 17], exclusive
    public static class NotOverlappingTimeOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("no overlap", Range(6, 8), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("overlaps", Range(10, 14), false)];
    }

    // Contains("14:00") — inclusive
    public static class ContainsTimeOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("contains", Range(10, 16), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("does not contain", Range(6, 8), false)];
    }

    // NotContains("14:00") — inclusive
    public static class NotContainsTimeOnlyRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("does not contain", Range(6, 8), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("contains", Range(10, 16), false)];
    }
}
