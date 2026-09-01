using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.DateOnlyRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class DateOnlyAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.UtcNow);

    public static class PastDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("past", F.IsPast.PastDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("present", Today, false), new("future", F.IsPast.FutureDate!.Value, false)];
    }

    public static class PastOrPresentDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("past", F.IsPast.PastDate!.Value, true), new("present", Today, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("future", F.IsPast.FutureDate!.Value, false)];
    }

    public static class FutureDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("future", F.IsPast.FutureDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("present", Today, false), new("past", F.IsPast.PastDate!.Value, false)];
    }

    public static class FutureOrPresentDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("future", F.IsPast.FutureDate!.Value, true), new("present", Today, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("past", F.IsPast.PastDate!.Value, false)];
    }

    // Between("2020-01-01", "2020-01-31")
    public static class BetweenDateOnly
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("min", new DateOnly(2020, 1, 1), true),
            new("mid", new DateOnly(2020, 1, 15), true),
            new("max", new DateOnly(2020, 1, 31), true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("too small", new DateOnly(2019, 12, 31), false),
            new("too large", new DateOnly(2020, 2, 1), false)
        ];
    }

    // NotBetween("2020-01-01", "2020-01-31")
    public static class NotBetweenDateOnly
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("too small", new DateOnly(2019, 12, 31), true),
            new("too large", new DateOnly(2020, 2, 1), true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("min", new DateOnly(2020, 1, 1), false),
            new("mid", new DateOnly(2020, 1, 15), false),
            new("max", new DateOnly(2020, 1, 31), false)
        ];
    }

    // Before("2020-01-10")
    public static class BeforeDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("before", new DateOnly(2020, 1, 9), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("same", new DateOnly(2020, 1, 10), false), new("after", new DateOnly(2020, 1, 11), false)];
    }

    // OnOrBefore("2020-01-10")
    public static class OnOrBeforeDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("before", new DateOnly(2020, 1, 9), true), new("same", new DateOnly(2020, 1, 10), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("after", new DateOnly(2020, 1, 11), false)];
    }

    // After("2020-01-10")
    public static class AfterDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("after", new DateOnly(2020, 1, 11), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("same", new DateOnly(2020, 1, 10), false), new("before", new DateOnly(2020, 1, 9), false)];
    }

    // OnOrAfter("2020-01-10")
    public static class OnOrAfterDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("after", new DateOnly(2020, 1, 11), true), new("same", new DateOnly(2020, 1, 10), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("before", new DateOnly(2020, 1, 9), false)];
    }

    // Same("2020-01-10")
    public static class SameDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("same", new DateOnly(2020, 1, 10), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("diff", new DateOnly(2020, 1, 11), false)];
    }

    // NotSame("2020-01-10")
    public static class NotSameDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("diff", new DateOnly(2020, 1, 11), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("same", new DateOnly(2020, 1, 10), false)];
    }

    // Chronological("2020-01-10") => Value < End
    public static class ChronologicalDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("before", new DateOnly(2020, 1, 9), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("same", new DateOnly(2020, 1, 10), false), new("after", new DateOnly(2020, 1, 11), false)];
    }

    // Overlapping("2020-01-10", "2020-01-20", "2020-01-30")
    public static class OverlappingDateOnly
    {
        // End1 = 20th. Start2 = 15th. End2 = 25th.
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("overlap mid", new DateOnly(2020, 1, 10), true), // 10..20 overlaps 15..25 (15..20)
            new("inside", new DateOnly(2020, 1, 16), true) // 16..20 overlaps 15..25
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("no overlap left", new DateOnly(2020, 1, 1), true),
            new("no overlap right", new DateOnly(2020, 1, 25), false)
        ];
    }

    // NotChronological("2020-01-10") => Value >= End (NOT chronological means not before end)
    public static class NotChronologicalDateOnly
    {
        public static TheoryData<ValidCase> ValidCases => [new("same", new DateOnly(2020, 1, 10), true), new("after", new DateOnly(2020, 1, 11), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("before", new DateOnly(2020, 1, 9), false)];
    }

    // NotOverlapping("2020-01-30", "2020-01-10", "2020-01-20") — end1=Jan30, start2=Jan10, end2=Jan20
    public static class NotOverlappingDateOnly
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("no overlap", new DateOnly(2020, 1, 21), true) // range1=Jan21..Jan30, range2=Jan10..Jan20 — no overlap
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("overlap", new DateOnly(2020, 1, 10), false) // range1=Jan10..Jan30, range2=Jan10..Jan20 — overlap
        ];
    }

    // The clock-injection rows pin their own instant rather than share one provider. The subject sits in
    // 2100, so a clock in 2200 puts it in the past and a clock in 2000 puts it in the future — verdicts that
    // hold whatever the machine's clock reads, which is what makes them evidence the attribute resolved the
    // TimeProvider off the ValidationContext instead of falling through to the system clock.
    private static readonly DateOnly ClockSubject = new(2100, 01, 01);
    private static readonly DateTimeOffset ClockAfterSubject = new(2200, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockBeforeSubject = new(2000, 01, 01, 12, 0, 0, TimeSpan.Zero);

    public static class PastDateOnlyOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class FutureDateOnlyOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }
}
