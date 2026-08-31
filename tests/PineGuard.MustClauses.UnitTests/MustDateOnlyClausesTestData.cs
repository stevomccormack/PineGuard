using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateOnlyRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDateOnlyClausesTestData
{
    private static readonly DateOnly Now = DateOnly.FromDateTime(DateTime.UtcNow);
    private static readonly DateOnly LeapDayBirth = new(2008, 02, 29);
    private static readonly DateOnly PastDate = Now.AddDays(-1);
    private static readonly DateOnly FutureDate = Now.AddDays(1);
    private static readonly DateOnly WayFutureDate = Now.AddDays(5);

    public static class Past
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("past", F.IsPast.PastDate!.Value, true),
               new("future", F.IsPast.FutureDate!.Value, false),
               new("present", Now, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateOnly.MinValue is past", DateOnly.MinValue, true),
            new("DateOnly.MaxValue is future", DateOnly.MaxValue, false)
        ];

        public sealed record ValidCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);
    }

    public static class PastOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("past", F.IsPast.PastDate!.Value, true),
               new("present", Now, true),
               new("future", F.IsPast.FutureDate!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateOnly.MinValue is past or present", DateOnly.MinValue, true),
            new("DateOnly.MaxValue is future only", DateOnly.MaxValue, false)
        ];

        public sealed record ValidCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);
    }

    public static class Future
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("future", F.IsPast.FutureDate!.Value, true),
               new("past", F.IsPast.PastDate!.Value, false),
               new("present", Now, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateOnly.MinValue is not future", DateOnly.MinValue, false),
            new("DateOnly.MaxValue is future", DateOnly.MaxValue, true)
        ];

        public sealed record ValidCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);
    }

    public static class FutureOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("future", F.IsPast.FutureDate!.Value, true),
               new("present", Now, true),
               new("past", F.IsPast.PastDate!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateOnly.MinValue is not future or present", DateOnly.MinValue, false),
            new("DateOnly.MaxValue is future or present", DateOnly.MaxValue, true)
        ];

        public sealed record ValidCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, DateOnly Value, bool Expected) : IsCase<DateOnly>(Name, Value, Expected);
    }

    public static class Between
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("between", (Now, PastDate, FutureDate), true),
               new("not between", (PastDate.AddDays(-1), PastDate, FutureDate), false),
               new("start boundary", (PastDate, PastDate, FutureDate), true),
               new("end boundary", (FutureDate, PastDate, FutureDate), true)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("min boundary inclusive", (DateOnly.MinValue, DateOnly.MinValue, DateOnly.MaxValue), true),
            new("max boundary inclusive", (DateOnly.MaxValue, DateOnly.MinValue, DateOnly.MaxValue), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly min, DateOnly max) Value, bool Expected) : IsCase<(DateOnly value, DateOnly min, DateOnly max)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly min, DateOnly max) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly min, DateOnly max)>(Name, Value, Expected);
    }

    public static class NotBetween
    {
        private static readonly DateOnly D1 = new(2023, 1, 1);
        private static readonly DateOnly D2 = new(2025, 1, 1);

        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not between", (PastDate.AddDays(-1), PastDate, FutureDate), true),
               new("between", (Now, PastDate, FutureDate), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateOnly.MinValue outside range", (DateOnly.MinValue, D1, D2), true),
            new("DateOnly.MaxValue outside range", (DateOnly.MaxValue, D1, D2), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly min, DateOnly max) Value, bool Expected) : IsCase<(DateOnly value, DateOnly min, DateOnly max)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly min, DateOnly max) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly min, DateOnly max)>(Name, Value, Expected);
    }

    public static class Before
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("before", (PastDate, Now), true),
               new("after", (Now, PastDate), false),
               new("same", (Now, Now), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue before MaxValue", (DateOnly.MinValue, DateOnly.MaxValue), true),
            new("MaxValue not before MinValue", (DateOnly.MaxValue, DateOnly.MinValue), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);
    }

    public static class OnOrBefore
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("before", (PastDate, Now), true),
               new("same", (Now, Now), true),
               new("after", (FutureDate, Now), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue on or before MaxValue", (DateOnly.MinValue, DateOnly.MaxValue), true),
            new("MaxValue on or before MaxValue", (DateOnly.MaxValue, DateOnly.MaxValue), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);
    }

    public static class After
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("after", (FutureDate, Now), true),
               new("before", (Now, FutureDate), false),
               new("same", (Now, Now), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MaxValue after MinValue", (DateOnly.MaxValue, DateOnly.MinValue), true),
            new("MinValue not after MaxValue", (DateOnly.MinValue, DateOnly.MaxValue), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);
    }

    public static class OnOrAfter
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("after", (FutureDate, Now), true),
               new("same", (Now, Now), true),
               new("before", (PastDate, Now), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MaxValue on or after MinValue", (DateOnly.MaxValue, DateOnly.MinValue), true),
            new("MinValue on or after MinValue", (DateOnly.MinValue, DateOnly.MinValue), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);
    }

    public static class Same
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("same", (Now, Now), true),
               new("not same", (Now, PastDate), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue same as MinValue", (DateOnly.MinValue, DateOnly.MinValue), true),
            new("MaxValue not same as MinValue", (DateOnly.MaxValue, DateOnly.MinValue), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);
    }

    public static class NotSame
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not same", (Now, PastDate), true),
               new("same", (Now, Now), false)
        ];

        // NotSame allows null

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target)>(Name, Value, Expected);

        // EdgeCases? If logic allows null, maybe check valid case with null input?
        // IsCase handles ValidCase.
        // If we want explicit EdgeCase usage we can add it, but NotSame doesn't return Fail, it returns Ok(true/false).
        // So valid case covers it.
    }

    public static class Chronological
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("chronological", (PastDate, FutureDate), true),
               new("not chronological", (FutureDate, PastDate), false),
               new("same", (Now, Now), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue before MaxValue", (DateOnly.MinValue, DateOnly.MaxValue), true),
            new("MaxValue same as MaxValue is not chronological", (DateOnly.MaxValue, DateOnly.MaxValue), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly min, DateOnly max) Value, bool Expected) : IsCase<(DateOnly min, DateOnly max)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly min, DateOnly max) Value, bool Expected)
             : IsCase<(DateOnly min, DateOnly max)>(Name, Value, Expected);
    }

    public static class NotChronological
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not chronological", (FutureDate, PastDate), true),
               new("chronological", (PastDate, FutureDate), false),
               new("same", (Now, Now), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly min, DateOnly max) Value, bool Expected) : IsCase<(DateOnly min, DateOnly max)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly min, DateOnly max) Value, bool Expected)
             : IsCase<(DateOnly min, DateOnly max)>(Name, Value, Expected);
    }

    public static class Overlapping
    {
        private static readonly DateOnly D1 = new(2023, 1, 1);
        private static readonly DateOnly D2 = new(2023, 1, 5);
        private static readonly DateOnly D3 = new(2023, 1, 3);
        private static readonly DateOnly D4 = new(2023, 1, 8);
        private static readonly DateOnly D5 = new(2023, 1, 10);

        public static TheoryData<ValidCase> ValidCases =>
        [
             new("overlapping", (D1, D2, D3, D4), true),
               new("not overlapping", (D1, D2, D4, D5), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("invalid range1", (D2, D1, D3, D4), false),
               new("invalid range2", (D1, D2, D4, D3), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2) Value, bool Expected)
             : IsCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2) Value, bool Expected)
             : IsCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2)>(Name, Value, Expected);
    }

    public static class NotOverlapping
    {
        private static readonly DateOnly D1 = new(2023, 1, 1);
        private static readonly DateOnly D2 = new(2023, 1, 5);
        private static readonly DateOnly D3 = new(2023, 1, 3);
        private static readonly DateOnly D4 = new(2023, 1, 8);
        private static readonly DateOnly D5 = new(2023, 1, 6);

        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not overlapping", (D1, D2, D5, D4), true),
               new("overlapping", (D1, D2, D3, D4), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
             new("invalid range1", (D2, D1, D3, D4), true),
               new("invalid range2", (D1, D2, D4, D3), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2) Value, bool Expected)
             : IsCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2) Value, bool Expected)
             : IsCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2)>(Name, Value, Expected);
    }

    public static class WithinDays
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("within", (FutureDate, Now, 2), true),
               new("not within", (WayFutureDate, Now, 2), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("same day zero days", (Now, Now, 0), true),
            new("one day apart zero tolerance", (Now.AddDays(1), Now, 0), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target, int days) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target, int days)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target, int days) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target, int days)>(Name, Value, Expected);
    }

    public static class NotWithinDays
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not within", (WayFutureDate, Now, 2), true),
               new("within", (FutureDate, Now, 2), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("same day zero days is within", (Now, Now, 0), false),
            new("one day apart zero tolerance is not within", (Now.AddDays(1), Now, 0), true)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target, int days) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target, int days)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target, int days) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target, int days)>(Name, Value, Expected);
    }

    public static class WithinCalendarMonths
    {
        private static readonly DateOnly D1 = new(2023, 1, 1);
        private static readonly DateOnly D2 = new(2023, 3, 1);

        public static TheoryData<ValidCase> ValidCases =>
        [
             new("within", (D2, D1, 3), true),
               new("not within", (D2, D1, 1), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("same date zero months", (D1, D1, 0), true),
            new("different month zero tolerance", (D2, D1, 0), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target, int months) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target, int months)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target, int months) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target, int months)>(Name, Value, Expected);
    }

    public static class NotWithinCalendarMonths
    {
        private static readonly DateOnly D1 = new(2023, 1, 1);
        private static readonly DateOnly D2 = new(2023, 3, 1);

        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not within", (D2, D1, 1), true),
               new("within", (D2, D1, 3), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("different month zero tolerance is not within", (D2, D1, 0), true),
            new("same date zero months is within", (D1, D1, 0), false)
        ];

        public sealed record ValidCase(string Name, (DateOnly value, DateOnly target, int months) Value, bool Expected) : IsCase<(DateOnly value, DateOnly target, int months)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateOnly value, DateOnly target, int months) Value, bool Expected)
             : IsCase<(DateOnly value, DateOnly target, int months)>(Name, Value, Expected);
    }

    // The calendar clauses take a non-nullable DateOnly, so each group drops the fixture's NullValue scenario.
    public static class Weekday
    {
        public static TheoryData<MustCase<DateOnly>> ValidCases => F.IsWeekday.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateOnly>> InvalidCases => F.IsWeekday.InvalidScenarios.Except(nameof(F.IsWeekday.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be a weekday.", "value", MustCodes.Date.Calendar.NotWeekday));
    }

    public static class Weekend
    {
        public static TheoryData<MustCase<DateOnly>> ValidCases => F.IsWeekend.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateOnly>> InvalidCases => F.IsWeekend.InvalidScenarios.Except(nameof(F.IsWeekend.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be a weekend day.", "value", MustCodes.Date.Calendar.NotWeekend));
    }

    public static class FirstDayOfMonth
    {
        public static TheoryData<MustCase<DateOnly>> ValidCases => F.IsFirstDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateOnly>> InvalidCases => F.IsFirstDayOfMonth.InvalidScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be the first day of the month.", "value", MustCodes.Date.Calendar.NotFirstDayOfMonth));
    }

    public static class NotFirstDayOfMonth
    {
        public static TheoryData<MustCase<DateOnly>> ValidCases => F.IsFirstDayOfMonth.InvalidScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<DateOnly>> InvalidCases => F.IsFirstDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be the first day of the month.", "value", MustCodes.Date.Calendar.FirstDayOfMonth));
    }

    public static class LastDayOfMonth
    {
        public static TheoryData<MustCase<DateOnly>> ValidCases => F.IsLastDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateOnly>> InvalidCases => F.IsLastDayOfMonth.InvalidScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be the last day of the month.", "value", MustCodes.Date.Calendar.NotLastDayOfMonth));
    }

    public static class NotLastDayOfMonth
    {
        public static TheoryData<MustCase<DateOnly>> ValidCases => F.IsLastDayOfMonth.InvalidScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<DateOnly>> InvalidCases => F.IsLastDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be the last day of the month.", "value", MustCodes.Date.Calendar.LastDayOfMonth));
    }

    public static class MinimumAge
    {
        public static TheoryData<MustCase<(DateOnly value, int years)>> ValidCases => F.HasMinimumAge.AllValid.Project(v => (v.value!.Value, v.years)).ToMustCases();

        public static TheoryData<MustCase<(DateOnly value, int years)>> InvalidCases => F.HasMinimumAge.AllInvalid.Except(nameof(F.HasMinimumAge.NullValue)).Project(v => (v.value!.Value, v.years)).ToMustCases(s => s.Name switch
        {
            nameof(F.HasMinimumAge.NegativeYears) => new MustExpected(false, "years requires a non-negative number of years.", "years", Code: MustCodes.Date.Age.BelowMinimum),
            _ => new MustExpected(false, "value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)
        });
    }

    public static class MinimumAgeOnLeapDay
    {
        // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
        // here the boundary moves and the birth date stays put, which the shared provider cannot express.
        public static TheoryData<MustCase<(DateOnly value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new MustCase<(DateOnly value, int years, DateTimeOffset utcNow)>("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new MustExpected(false, "value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)),
            new MustCase<(DateOnly value, int years, DateTimeOffset utcNow)>("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new MustExpected(true)),
            new MustCase<(DateOnly value, int years, DateTimeOffset utcNow)>("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new MustExpected(true))
        ];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
