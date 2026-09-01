using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.DateTimeRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class DateTimeAttributesTestData
{
    public sealed record ValidCase(string Name, Func<object?> Value, bool Expected)
        : ReturnCase<Func<object?>, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", () => null, true)
    ];

    // Past
    public static class PastDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("past", () => F.IsPast.PastDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("future", () => F.IsPast.FutureDate!.Value, false)];
    }

    // PastOrPresent
    public static class PastOrPresentDateTime
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("past", () => F.IsPast.PastDate!.Value, true),
            new("present", () => DateTime.UtcNow, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("future", () => F.IsPast.FutureDate!.Value, false)];
    }

    // Future
    public static class FutureDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("future", () => F.IsPast.FutureDate!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("past", () => F.IsPast.PastDate!.Value, false)];
    }

    // FutureOrPresent
    public static class FutureOrPresentDateTime
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("future", () => F.IsPast.FutureDate!.Value, true),
            // True "present" cannot be tested reliably without a controllable clock.
            // We use a small buffer to ensure the generated value is still >= "now" when evaluated.
            new("present", () => DateTime.UtcNow.AddSeconds(1), true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("past", () => F.IsPast.PastDate!.Value, false)];
    }

    // Utc
    public static class UtcDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("utc", () => F.IsUtc.Utc!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("local", () => F.IsLocal.Local!.Value, false),
            new("unspecified", () => F.IsUnspecified.Unspecified!.Value, false)
        ];
    }

    // Local
    public static class LocalDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("local", () => F.IsLocal.Local!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("utc", () => F.IsLocal.Utc!.Value, false),
            new("unspecified", () => F.IsUnspecified.Unspecified!.Value, false)
        ];
    }

    // Unspecified
    public static class UnspecifiedDateTime
    {
        public static TheoryData<ValidCase> ValidCases => [new("unspecified", () => F.IsUnspecified.Unspecified!.Value, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("utc", () => F.IsUnspecified.Utc!.Value, false),
            new("local", () => F.IsLocal.Local!.Value, false)
        ];
    }

    // See DateOnlyAttributesTestData for why each row carries its own instant: a subject in 2100 read on a
    // 2200 clock is unambiguously past and on a 2000 clock unambiguously future, so neither verdict can be
    // reproduced by the system clock this decade.
    private static readonly DateTime ClockSubject = new(2100, 01, 01, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTimeOffset ClockAfterSubject = new(2200, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockBeforeSubject = new(2000, 01, 01, 12, 0, 0, TimeSpan.Zero);

    public static class PastDateTimeOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class FutureDateTimeOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }

    // The whole fixture tuple travels in Value, because the minimum age varies per row and the attribute takes
    // it as a constructor argument; the test destructures it. Every birth date sits around the instant
    // FixedTimeProvider.Default reports, which is the clock the test registers on the validation context —
    // NotYetBorn is the row that proves the resolution happened, being future for the pinned clock and past
    // for the machine's.
    public static class MinimumAgeDateTime
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasMinimumAge.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.HasMinimumAge.NullValue) => new DataAnnotationExpected(true),
            nameof(F.HasMinimumAge.NegativeYears) => new DataAnnotationExpected(false, "years requires a non-negative number of years.", Code: MustCodes.Date.Age.BelowMinimum),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)
        });
    }

    // A 29-February birth date has no anniversary in a non-leap year, so each row pins its own clock: the
    // boundary moves while the birth date stays put, which the shared provider cannot express.
    private static readonly DateTime LeapDayBirth = new(2008, 02, 29, 0, 0, 0, DateTimeKind.Utc);

    public static class MinimumAgeDateTimeOnLeapDay
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new DataAnnotationExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)),
            new("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new DataAnnotationExpected(true)),
            new("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new DataAnnotationExpected(true))
        ];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
