using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringDateOnlyClausesTestData
{
    private const string LeapDayBirth = "2008-02-29";

    private static readonly DateOnly Ref = new(2020, 6, 15);
    private static readonly DateOnly RefMin = new(2020, 1, 1);
    private static readonly DateOnly RefMax = new(2020, 12, 31);

    public static class PastDateOnly
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("past", "2000-01-01", new MustExpected(true)),
            new("future", "2999-01-01", new MustExpected(false, "value must be a date in the past.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date in the past."))
        ];
    }

    public static class PastOrPresentDateOnly
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("past", "2000-01-01", new MustExpected(true)),
            new("future", "2999-01-01", new MustExpected(false, "value must be a date in the past or present.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date in the past or present."))
        ];
    }

    public static class FutureDateOnly
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("future", "2999-01-01", new MustExpected(true)),
            new("past", "2000-01-01", new MustExpected(false, "value must be a date in the future.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date in the future."))
        ];
    }

    public static class FutureOrPresentDateOnly
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("future", "2999-01-01", new MustExpected(true)),
            new("past", "2000-01-01", new MustExpected(false, "value must be a date in the future or present.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date in the future or present."))
        ];
    }

    public static class BetweenDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>> Cases =>
        [
            new("in-range", ("2020-06-15", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(true)),
            new("on-min-inclusive", ("2020-01-01", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(true)),
            new("on-min-exclusive", ("2020-01-01", RefMin, RefMax, Inclusion.Exclusive), new MustExpected(false, "value must be a date within the expected range.")),
            new("out-of-range", ("2019-01-01", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date within the expected range.")),
            new("null-value", (null, RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date within the expected range.")),
            new("min-gt-max", ("2020-06-15", RefMax, RefMin, Inclusion.Inclusive), new MustExpected(false, "min must be less than or equal to max.", "min"))
        ];
    }

    public static class NotBetweenDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>> Cases =>
        [
            new("out-of-range", ("2019-01-01", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(true)),
            new("in-range", ("2020-06-15", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date not within the expected range.")),
            new("null-value", (null, RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date not within the expected range.")),
            new("min-gt-max", ("2020-06-15", RefMax, RefMin, Inclusion.Inclusive), new MustExpected(false, "min must be less than or equal to max.", "min"))
        ];
    }

    public static class WithinDaysDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly? reference, int days)>> Cases =>
        [
            new("within", ("2020-06-16", Ref, 2), new MustExpected(true)),
            new("not-within", ("2020-07-15", Ref, 2), new MustExpected(false, "value must be a date within the expected number of days.")),
            new("null-value", (null, Ref, 2), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref, 2), new MustExpected(false, "value must be a date within the expected number of days.")),
            new("negative-days", ("2020-06-15", Ref, -1), new MustExpected(false, "days requires a non-negative number of days.", "days"))
        ];
    }

    public static class NotWithinDaysDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly? reference, int days)>> Cases =>
        [
            new("not-within", ("2020-07-15", Ref, 2), new MustExpected(true)),
            new("within", ("2020-06-16", Ref, 2), new MustExpected(false, "value must be a date not within the expected number of days.")),
            new("null-value", (null, Ref, 2), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref, 2), new MustExpected(false, "value must be a date not within the expected number of days.")),
            new("negative-days", ("2020-06-15", Ref, -1), new MustExpected(false, "days requires a non-negative number of days.", "days"))
        ];
    }

    public static class WithinCalendarMonthsDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly? reference, int months)>> Cases =>
        [
            new("within", ("2020-07-15", Ref, 2), new MustExpected(true)),
            new("not-within", ("2020-12-15", Ref, 2), new MustExpected(false, "value must be a date within the expected number of calendar months.")),
            new("null-value", (null, Ref, 2), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref, 2), new MustExpected(false, "value must be a date within the expected number of calendar months.")),
            new("negative-months", ("2020-06-15", Ref, -1), new MustExpected(false, "months requires a non-negative number of months.", "months"))
        ];
    }

    public static class NotWithinCalendarMonthsDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly? reference, int months)>> Cases =>
        [
            new("not-within", ("2020-12-15", Ref, 2), new MustExpected(true)),
            new("within", ("2020-07-15", Ref, 2), new MustExpected(false, "value must be a date not within the expected number of calendar months.")),
            new("null-value", (null, Ref, 2), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref, 2), new MustExpected(false, "value must be a date not within the expected number of calendar months.")),
            new("negative-months", ("2020-06-15", Ref, -1), new MustExpected(false, "months requires a non-negative number of months.", "months"))
        ];
    }

    public static class BeforeDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("before", ("2020-06-14", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(false, "value must be a date before the specified date.")),
            new("after", ("2020-06-16", Ref), new MustExpected(false, "value must be a date before the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must be a date before the specified date."))
        ];
    }

    public static class NotBeforeDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("after", ("2020-06-16", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(true)),
            new("before", ("2020-06-14", Ref), new MustExpected(false, "value must not be a date before the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must not be a date before the specified date."))
        ];
    }

    public static class OnOrBeforeDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("before", ("2020-06-14", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(true)),
            new("after", ("2020-06-16", Ref), new MustExpected(false, "value must be a date on or before the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must be a date on or before the specified date."))
        ];
    }

    public static class NotOnOrBeforeDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("after", ("2020-06-16", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(false, "value must not be a date on or before the specified date.")),
            new("before", ("2020-06-14", Ref), new MustExpected(false, "value must not be a date on or before the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must not be a date on or before the specified date."))
        ];
    }

    public static class AfterDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("after", ("2020-06-16", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(false, "value must be a date after the specified date.")),
            new("before", ("2020-06-14", Ref), new MustExpected(false, "value must be a date after the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must be a date after the specified date."))
        ];
    }

    public static class NotAfterDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("before", ("2020-06-14", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(true)),
            new("after", ("2020-06-16", Ref), new MustExpected(false, "value must not be a date after the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must not be a date after the specified date."))
        ];
    }

    public static class OnOrAfterDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("after", ("2020-06-16", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(true)),
            new("before", ("2020-06-14", Ref), new MustExpected(false, "value must be a date on or after the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must be a date on or after the specified date."))
        ];
    }

    public static class NotOnOrAfterDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("before", ("2020-06-14", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(false, "value must not be a date on or after the specified date.")),
            new("after", ("2020-06-16", Ref), new MustExpected(false, "value must not be a date on or after the specified date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must not be a date on or after the specified date."))
        ];
    }

    public static class SameDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("same", ("2020-06-15", Ref), new MustExpected(true)),
            new("different", ("2020-06-14", Ref), new MustExpected(false, "value must be the same date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must be the same date."))
        ];
    }

    public static class NotSameDateOnly
    {
        public static TheoryData<MustCase<(string? value, DateOnly other)>> Cases =>
        [
            new("different", ("2020-06-14", Ref), new MustExpected(true)),
            new("same", ("2020-06-15", Ref), new MustExpected(false, "value must not be the same date.")),
            new("null-value", (null, Ref), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", Ref), new MustExpected(false, "value must not be the same date."))
        ];
    }

    public static class ChronologicalDateOnly
    {
        public static TheoryData<MustCase<(string? start, string? end)>> Cases =>
        [
            new("chrono", ("2020-01-01", "2020-12-31"), new MustExpected(true)),
            new("not-chrono", ("2020-12-31", "2020-01-01"), new MustExpected(false, "start must be chronological.")),
            new("same", ("2020-06-15", "2020-06-15"), new MustExpected(false, "start must be chronological.")),
            new("null-start", (null, "2020-12-31"), new MustExpected(false, "start must not be null.", "start")),
            new("null-end", ("2020-01-01", null), new MustExpected(false, "end must not be null.", "end")),
            new("unparseable-start", ("not-a-date", "2020-12-31"), new MustExpected(false, "start must be chronological.")),
            new("unparseable-end", ("2020-01-01", "not-a-date"), new MustExpected(false, "end must be chronological.", "end"))
        ];
    }

    public static class NotChronologicalDateOnly
    {
        public static TheoryData<MustCase<(string? start, string? end)>> Cases =>
        [
            new("not-chrono", ("2020-12-31", "2020-01-01"), new MustExpected(true)),
            new("same", ("2020-06-15", "2020-06-15"), new MustExpected(true)),
            new("chrono", ("2020-01-01", "2020-12-31"), new MustExpected(false, "start must not be chronological.")),
            new("null-start", (null, "2020-12-31"), new MustExpected(false, "start must not be null.", "start")),
            new("null-end", ("2020-01-01", null), new MustExpected(false, "end must not be null.", "end")),
            new("unparseable-start", ("not-a-date", "2020-12-31"), new MustExpected(false, "start must not be chronological.")),
            new("unparseable-end", ("2020-01-01", "not-a-date"), new MustExpected(false, "end must not be chronological.", "end"))
        ];
    }

    public static class OverlappingDateOnly
    {
        public static TheoryData<MustCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("overlapping", ("2020-01-01", "2020-06-30", "2020-03-01", "2020-12-31"), new MustExpected(true)),
            new("disjoint", ("2020-01-01", "2020-03-31", "2020-06-01", "2020-12-31"), new MustExpected(false, "start1 must be overlapping.")),
            new("null-start1", (null, "2020-06-30", "2020-03-01", "2020-12-31"), new MustExpected(false, "start1 must not be null.", "start1")),
            new("null-end1", ("2020-01-01", null, "2020-03-01", "2020-12-31"), new MustExpected(false, "end1 must not be null.", "end1")),
            new("null-start2", ("2020-01-01", "2020-06-30", null, "2020-12-31"), new MustExpected(false, "start2 must not be null.", "start2")),
            new("null-end2", ("2020-01-01", "2020-06-30", "2020-03-01", null), new MustExpected(false, "end2 must not be null.", "end2")),
            new("unparseable-start1", ("not-a-date", "2020-06-30", "2020-03-01", "2020-12-31"), new MustExpected(false, "start1 must be overlapping.")),
            new("unparseable-end1", ("2020-01-01", "not-a-date", "2020-03-01", "2020-12-31"), new MustExpected(false, "end1 must be overlapping.", "end1")),
            new("unparseable-start2", ("2020-01-01", "2020-06-30", "not-a-date", "2020-12-31"), new MustExpected(false, "start2 must be overlapping.", "start2")),
            new("unparseable-end2", ("2020-01-01", "2020-06-30", "2020-03-01", "not-a-date"), new MustExpected(false, "end2 must be overlapping.", "end2"))
        ];
    }

    public static class NotOverlappingDateOnly
    {
        public static TheoryData<MustCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("disjoint", ("2020-01-01", "2020-03-31", "2020-06-01", "2020-12-31"), new MustExpected(true)),
            new("overlapping", ("2020-01-01", "2020-06-30", "2020-03-01", "2020-12-31"), new MustExpected(false, "start1 must not be overlapping.")),
            new("null-start1", (null, "2020-06-30", "2020-03-01", "2020-12-31"), new MustExpected(false, "start1 must not be null.", "start1")),
            new("null-end1", ("2020-01-01", null, "2020-03-01", "2020-12-31"), new MustExpected(false, "end1 must not be null.", "end1")),
            new("null-start2", ("2020-01-01", "2020-06-30", null, "2020-12-31"), new MustExpected(false, "start2 must not be null.", "start2")),
            new("null-end2", ("2020-01-01", "2020-06-30", "2020-03-01", null), new MustExpected(false, "end2 must not be null.", "end2")),
            new("unparseable-start1", ("not-a-date", "2020-06-30", "2020-03-01", "2020-12-31"), new MustExpected(false, "start1 must not be overlapping.")),
            new("unparseable-end1", ("2020-01-01", "not-a-date", "2020-03-01", "2020-12-31"), new MustExpected(false, "end1 must not be overlapping.", "end1")),
            new("unparseable-start2", ("2020-01-01", "2020-06-30", "not-a-date", "2020-12-31"), new MustExpected(false, "start2 must not be overlapping.", "start2")),
            new("unparseable-end2", ("2020-01-01", "2020-06-30", "2020-03-01", "not-a-date"), new MustExpected(false, "end2 must not be overlapping.", "end2"))
        ];
    }

    public static class MinimumAge
    {
        public static TheoryData<MustCase<(string? value, int years)>> ValidCases => F.DateOnlyHasMinimumAge.AllValid.ToMustCases();

        public static TheoryData<MustCase<(string? value, int years)>> InvalidCases => F.DateOnlyHasMinimumAge.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.DateOnlyHasMinimumAge.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Date.Age.BelowMinimum),
            nameof(F.DateOnlyHasMinimumAge.NotADate) => new MustExpected(false, "value must meet the expected minimum age.", "value", MustCodes.Date.Format.Invalid),
            nameof(F.DateOnlyHasMinimumAge.NegativeYears) => new MustExpected(false, "years requires a non-negative number of years.", "years", MustCodes.Date.Age.BelowMinimum),
            _ => new MustExpected(false, "value must meet the expected minimum age.", "value", MustCodes.Date.Age.BelowMinimum)
        });
    }

    public static class MinimumAgeOnLeapDay
    {
        // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
        // here the boundary moves and the birth date stays put, which the shared provider cannot express.
        public static TheoryData<MustCase<(string? value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new MustCase<(string? value, int years, DateTimeOffset utcNow)>("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new MustExpected(false, "value must meet the expected minimum age.", "value", MustCodes.Date.Age.BelowMinimum)),
            new MustCase<(string? value, int years, DateTimeOffset utcNow)>("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new MustExpected(true)),
            new MustCase<(string? value, int years, DateTimeOffset utcNow)>("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new MustExpected(true))
        ];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
