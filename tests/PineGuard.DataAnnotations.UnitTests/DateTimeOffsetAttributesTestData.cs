using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
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

    // See DateOnlyAttributesTestData for why each row carries its own instant.
    private static readonly DateTimeOffset ClockSubject = new(2100, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockAfterSubject = new(2200, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockBeforeSubject = new(2000, 01, 01, 12, 0, 0, TimeSpan.Zero);

    public static class PastDateTimeOffsetOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class FutureDateTimeOffsetOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }
}
