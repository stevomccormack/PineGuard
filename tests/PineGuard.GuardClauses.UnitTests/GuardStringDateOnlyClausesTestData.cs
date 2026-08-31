using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringDateOnlyClausesTestData
{
    private static readonly DateOnly D20200101 = new(2020, 1, 1);
    private static readonly DateOnly D20200115 = new(2020, 1, 15);
    private const string LeapDayBirth = "2008-02-29";

    // Guard.Against.FutureOrPresent — valid when Must.Be.PastDateOnly succeeds (past date)
    public static class FutureOrPresent
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateOnlyIsInPast.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateOnlyIsInPast.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.FutureDateOnly — valid when Must.Be.PastOrPresentDateOnly succeeds (past or present)
    public static class Future
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateOnlyIsInPast.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateOnlyIsInPast.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.PastOrPresent — valid when Must.Be.FutureDateOnly succeeds (future date)
    public static class PastOrPresent
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateOnlyIsInFuture.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateOnlyIsInFuture.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.PastDateOnly — valid when Must.Be.FutureOrPresentDateOnly succeeds (future or present)
    public static class Past
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.DateOnlyIsInFuture.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.DateOnlyIsInFuture.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotBetween — calls Must.Be.BetweenDateOnly; throws when NOT between (null/not-parseable/outside range)
    // ToGuardCases("value") cannot detect null inside tuple inputs; use explicit mapping so NullValue → ANE
    public static class NotBetween
    {
        public static TheoryData<GuardCase<(string? value, DateOnly min, DateOnly max, Common.Inclusion inclusion)>> ValidCases =>
            F.DateOnlyIsBetween.AllValid.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, DateOnly min, DateOnly max, Common.Inclusion inclusion)>> InvalidCases =>
            F.DateOnlyIsBetween.AllInvalid.ToGuardCases(s => s.Name switch
            {
                nameof(F.DateOnlyIsBetween.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
                _ => new GuardExpected(false, typeof(ArgumentException), "value")
            });
    }

    // Guard.Against.Between — calls Must.Be.NotBetweenDateOnly; throws when IS between or null/not-parseable
    // Valid only for true outside-range scenarios: InvalidEdgeScenarios (OnMinExclusive = at min with exclusive = outside)
    public static class Between
    {
        public static TheoryData<GuardCase<(string? value, DateOnly min, DateOnly max, Common.Inclusion inclusion)>> ValidCases =>
            F.DateOnlyIsBetween.InvalidEdgeScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, DateOnly min, DateOnly max, Common.Inclusion inclusion)>> InvalidCases =>
            [
                .. F.DateOnlyIsBetween.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
                .. F.DateOnlyIsBetween.InvalidScenarios.ToGuardCases(s => s.Name switch
                {
                    nameof(F.DateOnlyIsBetween.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
                    _ => new GuardExpected(false, typeof(ArgumentException), "value")
                })
            ];
    }

    // Guard.Against.NotWithinDaysDateOnly — calls Must.Be.WithinDaysDateOnly; throws when NOT within days
    // ValidCases: string IS within days of reference
    // InvalidCases: string NOT within days / null / unparseable
    public static class NotWithinDaysDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly? reference, int days)>> Cases =>
        [
            new("within-days", ("2020-01-15", D20200101, 15), new GuardExpected(true)),
            new("outside-days", ("2020-06-01", D20200101, 30), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200101, 30), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.WithinDaysDateOnly — calls Must.Be.NotWithinDaysDateOnly; throws when IS within days
    // ValidCases: string NOT within days of reference
    // InvalidCases: string IS within days / null / unparseable
    public static class WithinDaysDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly? reference, int days)>> Cases =>
        [
            new("outside-days", ("2020-06-01", D20200101, 30), new GuardExpected(true)),
            new("within-days", ("2020-01-15", D20200101, 15), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200101, 30), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotWithinCalendarMonthsDateOnly — calls Must.Be.WithinCalendarMonthsDateOnly; throws when NOT within months
    // ValidCases: string IS within calendar months of reference
    // InvalidCases: string NOT within calendar months / null / unparseable
    public static class NotWithinCalendarMonthsDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly? reference, int months)>> Cases =>
        [
            new("within-months", ("2020-01-15", D20200101, 1), new GuardExpected(true)),
            new("outside-months", ("2020-06-01", D20200101, 1), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200101, 1), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.WithinCalendarMonthsDateOnly — calls Must.Be.NotWithinCalendarMonthsDateOnly; throws when IS within months
    // ValidCases: string NOT within calendar months of reference
    // InvalidCases: string IS within calendar months / null / unparseable
    public static class WithinCalendarMonthsDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly? reference, int months)>> Cases =>
        [
            new("outside-months", ("2020-06-01", D20200101, 1), new GuardExpected(true)),
            new("within-months", ("2020-01-15", D20200101, 1), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200101, 1), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.BeforeDateOnly — calls Must.Be.NotBeforeDateOnly; throws when string IS before other
    // ValidCases: string NOT before other (same or after)
    // InvalidCases: string IS before other / null / unparseable
    public static class BeforeDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly other)>> Cases =>
        [
            new("after", ("2020-01-31", D20200115), new GuardExpected(true)),
            new("same", ("2020-01-15", D20200115), new GuardExpected(true)),
            new("before", ("2020-01-01", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200115), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.OnOrBeforeDateOnly — calls Must.Be.NotOnOrBeforeDateOnly; throws when IS on or before
    // ValidCases: strictly after
    // InvalidCases: on or before (same or before) / null / unparseable
    public static class OnOrBeforeDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly other)>> Cases =>
        [
            new("after", ("2020-01-31", D20200115), new GuardExpected(true)),
            new("same", ("2020-01-15", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("before", ("2020-01-01", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200115), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.AfterDateOnly — calls Must.Be.NotAfterDateOnly; throws when IS after
    // ValidCases: on or before (not strictly after)
    // InvalidCases: IS after / null / unparseable
    public static class AfterDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly other)>> Cases =>
        [
            new("before", ("2020-01-01", D20200115), new GuardExpected(true)),
            new("same", ("2020-01-15", D20200115), new GuardExpected(true)),
            new("after", ("2020-01-31", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200115), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.OnOrAfterDateOnly — calls Must.Be.NotOnOrAfterDateOnly; throws when IS on or after
    // ValidCases: strictly before
    // InvalidCases: IS on or after (same or after) / null / unparseable
    public static class OnOrAfterDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly other)>> Cases =>
        [
            new("before", ("2020-01-01", D20200115), new GuardExpected(true)),
            new("same", ("2020-01-15", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("after", ("2020-01-31", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200115), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.SameDateOnly — calls Must.Be.NotSameDateOnly; throws when IS same
    // ValidCases: different
    // InvalidCases: same / null / unparseable
    public static class SameDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly other)>> Cases =>
        [
            new("different", ("2020-01-31", D20200115), new GuardExpected(true)),
            new("same", ("2020-01-15", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200115), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.NotSameDateOnly — calls Must.Be.SameDateOnly; throws when NOT same
    // ValidCases: IS same
    // InvalidCases: different / null / unparseable
    public static class NotSameDateOnly
    {
        public static TheoryData<GuardCase<(string? value, DateOnly other)>> Cases =>
        [
            new("same", ("2020-01-15", D20200115), new GuardExpected(true)),
            new("different", ("2020-01-31", D20200115), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("null-value", (null, D20200115), new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.ChronologicalDateOnly — calls Must.Be.NotChronologicalDateOnly; throws when IS chronological
    // ValidCases: NOT chronological (start > end)
    // InvalidCases: IS chronological / null start / unparseable
    public static class ChronologicalDateOnly
    {
        public static TheoryData<GuardCase<(string? start, string? end)>> Cases =>
        [
            new("not-chrono", ("2020-01-31", "2020-01-01"), new GuardExpected(true)),
            new("chrono", ("2020-01-01", "2020-01-31"), new GuardExpected(false, typeof(ArgumentException), "start")),
            new("null-start", (null, "2020-01-31"), new GuardExpected(false, typeof(ArgumentNullException), "start"))
        ];
    }

    // Guard.Against.OverlappingDateOnly — calls Must.Be.NotOverlappingDateOnly; throws when IS overlapping
    // ValidCases: NOT overlapping (disjoint)
    // InvalidCases: IS overlapping / null start1 / unparseable
    public static class OverlappingDateOnly
    {
        public static TheoryData<GuardCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("disjoint", ("2020-01-01", "2020-01-10", "2020-01-15", "2020-01-31"), new GuardExpected(true)),
            new("overlapping", ("2020-01-01", "2020-01-20", "2020-01-10", "2020-01-31"), new GuardExpected(false, typeof(ArgumentException), "start1")),
            new("null-start1", (null, "2020-01-20", "2020-01-10", "2020-01-31"), new GuardExpected(false, typeof(ArgumentNullException), "start1"))
        ];
    }

    // Guard.Against.NotOverlappingDateOnly — calls Must.Be.OverlappingDateOnly; throws when NOT overlapping
    // ValidCases: IS overlapping
    // InvalidCases: NOT overlapping / null start1 / unparseable
    public static class NotOverlappingDateOnly
    {
        public static TheoryData<GuardCase<(string? start1, string? end1, string? start2, string? end2)>> Cases =>
        [
            new("overlapping", ("2020-01-01", "2020-01-20", "2020-01-10", "2020-01-31"), new GuardExpected(true)),
            new("disjoint", ("2020-01-01", "2020-01-10", "2020-01-15", "2020-01-31"), new GuardExpected(false, typeof(ArgumentException), "start1")),
            new("null-start1", (null, "2020-01-20", "2020-01-10", "2020-01-31"), new GuardExpected(false, typeof(ArgumentNullException), "start1"))
        ];
    }

    // Guard.Against.BelowMinimumAge — calls Must.Be.MinimumAge; throws when the birth date falls short
    // ValidCases: the birth date meets the minimum age
    // InvalidCases: falls short / unparseable / null / negative years
    // ToGuardCases("value") cannot detect null inside tuple inputs; use explicit mapping so NullValue → ANE
    public static class BelowMinimumAge
    {
        public static TheoryData<GuardCase<(string? value, int years)>> ValidCases =>
            F.DateOnlyHasMinimumAge.AllValid.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? value, int years)>> InvalidCases =>
            F.DateOnlyHasMinimumAge.AllInvalid.ToGuardCases(s => s.Name switch
            {
                nameof(F.DateOnlyHasMinimumAge.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Date.Age.BelowMinimum),
                nameof(F.DateOnlyHasMinimumAge.NotADate) => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Date.Format.Invalid),
                nameof(F.DateOnlyHasMinimumAge.NegativeYears) => new GuardExpected(false, typeof(ArgumentException), "years", Code: MustCodes.Date.Age.BelowMinimum),
                _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Date.Age.BelowMinimum)
            });
    }

    // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
    // here the boundary moves and the birth date stays put, which the shared provider cannot express.
    public static class BelowMinimumAgeOnLeapDay
    {
        public static TheoryData<GuardCase<(string? value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Date.Age.BelowMinimum)),
            new("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new GuardExpected(true)),
            new("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new GuardExpected(true))
        ];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
