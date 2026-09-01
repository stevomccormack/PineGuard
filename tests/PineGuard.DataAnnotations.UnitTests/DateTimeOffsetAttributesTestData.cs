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

    public static class WeekdayDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsWeekday.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsWeekday.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a weekday.", Code: MustCodes.Date.Calendar.NotWeekday)
        });
    }

    public static class WeekendDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsWeekend.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsWeekend.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a weekend day.", Code: MustCodes.Date.Calendar.NotWeekend)
        });
    }

    public static class FirstDayOfMonthDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsFirstDayOfMonth.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsFirstDayOfMonth.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be the first day of the month.", Code: MustCodes.Date.Calendar.NotFirstDayOfMonth)
        });
    }

    public static class NotFirstDayOfMonthDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsFirstDayOfMonth.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsFirstDayOfMonth.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not be the first day of the month.", Code: MustCodes.Date.Calendar.FirstDayOfMonth),
            _ => new DataAnnotationExpected(true)
        });
    }

    public static class LastDayOfMonthDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLastDayOfMonth.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsLastDayOfMonth.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be the last day of the month.", Code: MustCodes.Date.Calendar.NotLastDayOfMonth)
        });
    }

    public static class NotLastDayOfMonthDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLastDayOfMonth.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsLastDayOfMonth.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not be the last day of the month.", Code: MustCodes.Date.Calendar.LastDayOfMonth),
            _ => new DataAnnotationExpected(true)
        });
    }
}
