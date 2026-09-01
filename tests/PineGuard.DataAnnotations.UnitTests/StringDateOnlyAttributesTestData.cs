using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringDateOnlyAttributesTestData
{
    public static class PastDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInPast.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInPast.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the past.", Code: MustCodes.Date.Relative.NotPast)
        });
    }

    public static class FutureDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInFuture.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInFuture.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the future.")
        });
    }

    public static class PastOrPresentDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInPast.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInPast.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the past or present.")
        });
    }

    public static class FutureOrPresentDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyIsInFuture.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.DateOnlyIsInFuture.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a date in the future or present.")
        });
    }

    public static class BeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must be a date before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotBeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "2999-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must not be a date before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrBeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must be a date on or before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date on or before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrBeforeDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "2999-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must not be a date on or before the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date on or before the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class AfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "3000-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must be a date after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotAfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("past", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must not be a date after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OnOrAfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("future", "2999-01-01", new DataAnnotationExpected(true)),
            new("past", "2000-01-01", new DataAnnotationExpected(false, "Value must be a date on or after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be a date on or after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOnOrAfterDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("past", "2000-01-01", new DataAnnotationExpected(true)),
            new("future", "2999-01-01", new DataAnnotationExpected(false, "Value must not be a date on or after the specified date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be a date on or after the specified date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class SameDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("same", "2000-01-01", new DataAnnotationExpected(true)),
            new("different", "2999-01-01", new DataAnnotationExpected(false, "Value must be the same date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be the same date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotSameDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("different", "2999-01-01", new DataAnnotationExpected(true)),
            new("same", "2000-01-01", new DataAnnotationExpected(false, "Value must not be the same date.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be the same date.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class ChronologicalDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("chronological", "2000-01-01", new DataAnnotationExpected(true)),
            new("non-chronological", "3000-01-01", new DataAnnotationExpected(false, "Value must be chronological.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be chronological.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotChronologicalDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("non-chronological", "3000-01-01", new DataAnnotationExpected(true)),
            new("chronological", "2000-01-01", new DataAnnotationExpected(false, "Value must not be chronological.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be chronological.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class OverlappingDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("overlapping", "2020-01-05", new DataAnnotationExpected(true)),
            new("non-overlapping", "2020-07-01", new DataAnnotationExpected(false, "Value must be overlapping.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must be overlapping.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    public static class NotOverlappingDateOnlyString
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("non-overlapping", "2020-07-01", new DataAnnotationExpected(true)),
            new("overlapping", "2020-01-05", new DataAnnotationExpected(false, "Value must not be overlapping.")),
            new("not-a-date", "not-a-date", new DataAnnotationExpected(false, "Value must not be overlapping.")),
            new("null", null, new DataAnnotationExpected(true))
        ];
    }

    // See DateOnlyAttributesTestData for why each row carries its own instant.
    private const string ClockSubject = "2100-01-01";
    private static readonly DateTimeOffset ClockAfterSubject = new(2200, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockBeforeSubject = new(2000, 01, 01, 12, 0, 0, TimeSpan.Zero);

    public static class PastDateOnlyStringOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be a date in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class FutureDateOnlyStringOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("ClockBeforeTheSubject", (ClockSubject, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("ClockAfterTheSubject", (ClockSubject, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be a date in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }

    // The whole fixture tuple travels in Value, because the minimum age varies per row and the attribute takes
    // it as a constructor argument; the test destructures it. Every birth date sits around the instant
    // FixedTimeProvider.Default reports, which is the clock the test registers on the validation context —
    // NotYetBorn is the row that proves the resolution happened, being future for the pinned clock and past
    // for the machine's. NotADate fails on the same message as an under-age value, so it needs no arm of its
    // own; NullValue never reaches the clause, the base class passing null through untouched.
    public static class MinimumAgeString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.DateOnlyHasMinimumAge.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.DateOnlyHasMinimumAge.NullValue) => new DataAnnotationExpected(true),
            nameof(F.DateOnlyHasMinimumAge.NegativeYears) => new DataAnnotationExpected(false, "years requires a non-negative number of years.", Code: MustCodes.Date.Age.BelowMinimum),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)
        });
    }

    // A 29-February birth date has no anniversary in a non-leap year, so each row pins its own clock: the
    // boundary moves while the birth date stays put, which the shared provider cannot express.
    private const string LeapDayBirth = "2008-02-29";

    public static class MinimumAgeStringOnLeapDay
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
