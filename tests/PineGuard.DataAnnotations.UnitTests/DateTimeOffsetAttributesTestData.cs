using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class DateTimeOffsetAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    // Past
    public static class PastDateTimeOffset
    {
        public static TheoryData<ValidCase> ValidCases => [new("past", F.IsPast.PastDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("future", F.IsPast.FutureDate!.Value, false)];
    }

    // PastOrPresent
    public static class PastOrPresentDateTimeOffset
    {
        public static TheoryData<ValidCase> ValidCases => [new("past", F.IsPast.PastDate!.Value, true), new("present", DateTimeOffset.Now, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("future", F.IsPast.FutureDate!.Value, false)];
    }

    // Future
    public static class FutureDateTimeOffset
    {
        public static TheoryData<ValidCase> ValidCases => [new("future", F.IsPast.FutureDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("past", F.IsPast.PastDate!.Value, false)];
    }

    // FutureOrPresent
    public static class FutureOrPresentDateTimeOffset
    {
        public static TheoryData<ValidCase> ValidCases => [new("future", F.IsPast.FutureDate!.Value, true), new("present", DateTimeOffset.Now.AddMilliseconds(100), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("past", F.IsPast.PastDate!.Value, false)];
    }
}
