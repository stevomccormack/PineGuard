using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringTimeSpanAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    // DurationBetween("00:05:00", "02:00:00") — inclusive
    public static class DurationBetweenTimeSpanString
    {
        public static TheoryData<ValidCase> ValidCases => [new("within", "01:00:00", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("too large", "03:00:00", false), new("unparseable", "abc", false)];
    }

    // GreaterThan("00:05:00") — exclusive
    public static class GreaterThanTimeSpanString
    {
        public static TheoryData<ValidCase> ValidCases => [new("greater", "01:00:00", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("smaller", "00:01:00", false), new("unparseable", "abc", false)];
    }

    // LessThan("02:00:00") — exclusive
    public static class LessThanTimeSpanString
    {
        public static TheoryData<ValidCase> ValidCases => [new("less", "01:00:00", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("greater", "03:00:00", false), new("unparseable", "abc", false)];
    }

    // NotDurationBetween("00:05:00", "02:00:00") — inclusive
    public static class NotDurationBetweenTimeSpanString
    {
        public static TheoryData<ValidCase> ValidCases => [new("outside", "03:00:00", true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("within", "01:00:00", false), new("unparseable", "abc", false)];
    }
}
