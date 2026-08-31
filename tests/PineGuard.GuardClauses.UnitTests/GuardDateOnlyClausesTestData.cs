using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateOnlyRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardDateOnlyClausesTestData
{
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.UtcNow);
    private static readonly DateOnly Tomorrow = Today.AddDays(1);
    private static readonly DateOnly Yesterday = Today.AddDays(-1);
    private static readonly DateOnly LeapDayBirth = new(2008, 02, 29);

    // Guard.Against.FutureOrPresent — throws when value IS future or present (calls Must.Be.Past)
    // ValidCases: value IS past — Guard does not throw
    // InvalidCases: value IS future or present — Guard throws
    public static class FutureOrPresent
    {
        public static TheoryData<GuardCase<DateOnly>> ValidCases =>
        [
            new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateOnly>> InvalidCases =>
        [
            new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, new GuardExpected(false, typeof(ArgumentException), "value")),
            new("today", Today, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Future — throws when value IS future (calls Must.Be.PastOrPresent)
    // ValidCases: value is past or present — Guard does not throw
    // InvalidCases: value IS future — Guard throws
    public static class Future
    {
        public static TheoryData<GuardCase<DateOnly>> ValidCases =>
        [
            new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, new GuardExpected(true)),
            new("today", Today, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateOnly>> InvalidCases =>
        [
            new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.PastOrPresent — throws when value IS past or present (calls Must.Be.Future)
    // ValidCases: value IS future — Guard does not throw
    // InvalidCases: value IS past or present — Guard throws
    public static class PastOrPresent
    {
        public static TheoryData<GuardCase<DateOnly>> ValidCases =>
        [
            new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateOnly>> InvalidCases =>
        [
            new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, new GuardExpected(false, typeof(ArgumentException), "value")),
            new("today", Today, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Past — throws when value IS past (calls Must.Be.FutureOrPresent)
    // ValidCases: value IS future or present — Guard does not throw
    // InvalidCases: value IS past — Guard throws
    public static class Past
    {
        public static TheoryData<GuardCase<DateOnly>> ValidCases =>
        [
            new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, new GuardExpected(true)),
            new("today", Today, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateOnly>> InvalidCases =>
        [
            new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Between — throws when value IS between (calls Must.Be.NotBetween — complement)
    // ValidCases: value is NOT between — Guard does not throw
    // InvalidCases: value IS between — Guard throws
    public static class Between
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly min, DateOnly max, Inclusion inclusion)>> ValidCases =>
        [
            new("out of range low", (Yesterday.AddDays(-1), Yesterday, Tomorrow, Inclusion.Inclusive), new GuardExpected(true)),
            new("out of range high", (Tomorrow.AddDays(1), Yesterday, Tomorrow, Inclusion.Inclusive), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly min, DateOnly max, Inclusion inclusion)>> InvalidCases =>
        [
            new("in range", (Today, Yesterday, Tomorrow, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotBetween — throws when value IS NOT between (calls Must.Be.Between)
    // ValidCases: value IS between — Guard does not throw
    // InvalidCases: value is NOT between — Guard throws
    public static class NotBetween
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly min, DateOnly max, Inclusion inclusion)>> ValidCases =>
        [
            new("in range", (Today, Yesterday, Tomorrow, Inclusion.Inclusive), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly min, DateOnly max, Inclusion inclusion)>> InvalidCases =>
        [
            new("out of range low", (Yesterday.AddDays(-1), Yesterday, Tomorrow, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("out of range high", (Tomorrow.AddDays(1), Yesterday, Tomorrow, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.OnOrAfter — throws when value IS on or after other (calls Must.Be.Before)
    // ValidCases: value IS strictly before other — Guard does not throw
    // InvalidCases: value IS on or after other — Guard throws
    public static class OnOrAfter
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> ValidCases =>
        [
            new("before", (Yesterday, Today), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> InvalidCases =>
        [
            new("same", (Today, Today), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("after", (Tomorrow, Today), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.After — throws when value IS strictly after other (calls Must.Be.OnOrBefore)
    // ValidCases: value is on or before other — Guard does not throw
    // InvalidCases: value IS strictly after other — Guard throws
    public static class After
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> ValidCases =>
        [
            new("before", (Yesterday, Today), new GuardExpected(true)),
            new("same", (Today, Today), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> InvalidCases =>
        [
            new("after", (Tomorrow, Today), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.OnOrBefore — throws when value IS on or before other (calls Must.Be.After)
    // ValidCases: value IS strictly after other — Guard does not throw
    // InvalidCases: value IS on or before other — Guard throws
    public static class OnOrBefore
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> ValidCases =>
        [
            new("after", (Tomorrow, Today), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> InvalidCases =>
        [
            new("same", (Today, Today), new GuardExpected(false, typeof(ArgumentException), "value")),
            new("before", (Yesterday, Today), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Before — throws when value IS strictly before other (calls Must.Be.OnOrAfter)
    // ValidCases: value is on or after other — Guard does not throw
    // InvalidCases: value IS strictly before other — Guard throws
    public static class Before
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> ValidCases =>
        [
            new("after", (Tomorrow, Today), new GuardExpected(true)),
            new("same", (Today, Today), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> InvalidCases =>
        [
            new("before", (Yesterday, Today), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotSame — throws when value IS NOT same as other (calls Must.Be.Same)
    // ValidCases: value IS same as other — Guard does not throw
    // InvalidCases: value IS NOT same as other — Guard throws
    public static class NotSame
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> ValidCases =>
        [
            new("same", (Today, Today), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> InvalidCases =>
        [
            new("different", (Tomorrow, Today), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Same — throws when value IS same as other (calls Must.Be.NotSame — complement)
    // ValidCases: value IS NOT same as other — Guard does not throw
    // InvalidCases: value IS same as other — Guard throws
    public static class Same
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> ValidCases =>
        [
            new("different", (Tomorrow, Today), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly other)>> InvalidCases =>
        [
            new("same", (Today, Today), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotChronological — throws when NOT chronological (calls Must.Be.Chronological)
    // ValidCases: IS chronological — Guard does not throw
    // InvalidCases: NOT chronological — Guard throws
    public static class NotChronological
    {
        public static TheoryData<GuardCase<(DateOnly start, DateOnly end, Inclusion inclusion)>> ValidCases =>
        [
            new("chronological", (Yesterday, Today, Inclusion.Exclusive), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly start, DateOnly end, Inclusion inclusion)>> InvalidCases =>
        [
            new("reverse", (Today, Yesterday, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "start"))
        ];
    }

    // Guard.Against.Chronological — throws when IS chronological (calls Must.Be.NotChronological — complement)
    // ValidCases: NOT chronological — Guard does not throw
    // InvalidCases: IS chronological — Guard throws
    public static class Chronological
    {
        public static TheoryData<GuardCase<(DateOnly start, DateOnly end, Inclusion inclusion)>> ValidCases =>
        [
            new("reverse", (Today, Yesterday, Inclusion.Exclusive), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly start, DateOnly end, Inclusion inclusion)>> InvalidCases =>
        [
            new("chronological", (Yesterday, Today, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "start"))
        ];
    }

    // Guard.Against.Overlapping — throws when IS overlapping (calls Must.Be.NotOverlapping — complement)
    // ValidCases: NOT overlapping — Guard does not throw
    // InvalidCases: IS overlapping — Guard throws
    public static class Overlapping
    {
        private static readonly DateOnly D1 = new(2024, 1, 1);
        private static readonly DateOnly D2 = new(2024, 1, 5);
        private static readonly DateOnly D3 = new(2024, 1, 3);
        private static readonly DateOnly D4 = new(2024, 1, 8);
        private static readonly DateOnly D5 = new(2024, 1, 10);

        public static TheoryData<GuardCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2, Inclusion inclusion)>> ValidCases =>
        [
            new("not overlapping", (D1, D2, D4, D5, Inclusion.Exclusive), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2, Inclusion inclusion)>> InvalidCases =>
        [
            new("overlapping", (D1, D2, D3, D4, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "start1"))
        ];
    }

    // Guard.Against.NotOverlapping — throws when NOT overlapping (calls Must.Be.Overlapping)
    // ValidCases: IS overlapping — Guard does not throw
    // InvalidCases: NOT overlapping — Guard throws
    public static class NotOverlapping
    {
        private static readonly DateOnly D1 = new(2024, 1, 1);
        private static readonly DateOnly D2 = new(2024, 1, 5);
        private static readonly DateOnly D3 = new(2024, 1, 3);
        private static readonly DateOnly D4 = new(2024, 1, 8);
        private static readonly DateOnly D5 = new(2024, 1, 10);

        public static TheoryData<GuardCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2, Inclusion inclusion)>> ValidCases =>
        [
            new("overlapping", (D1, D2, D3, D4, Inclusion.Exclusive), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2, Inclusion inclusion)>> InvalidCases =>
        [
            new("not overlapping", (D1, D2, D4, D5, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "start1"))
        ];
    }

    // Guard.Against.NotWithinDays — throws when value IS within days of reference (calls Must.Be.WithinDays)
    // ValidCases: value IS within days — Guard does not throw (Must.Be.WithinDays succeeds)
    // InvalidCases: value is NOT within days — Guard throws (Must.Be.WithinDays fails)
    public static class NotWithinDays
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int days)>> ValidCases =>
        [
            new("within 1 day", (Today, Tomorrow, 1), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int days)>> InvalidCases =>
        [
            new("not within 1 day", (Today, Today.AddDays(2), 1), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.WithinDays — throws when value IS NOT within days (calls Must.Be.NotWithinDays — complement)
    // ValidCases: value is NOT within days — Guard does not throw (Must.Be.NotWithinDays succeeds)
    // InvalidCases: value IS within days — Guard throws (Must.Be.NotWithinDays fails)
    public static class WithinDays
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int days)>> ValidCases =>
        [
            new("not within 1 day", (Today, Today.AddDays(2), 1), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int days)>> InvalidCases =>
        [
            new("within 1 day", (Today, Tomorrow, 1), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotWithinCalendarMonths — throws when IS within calendar months (calls Must.Be.WithinCalendarMonths)
    // ValidCases: value IS within months — Guard does not throw (Must.Be.WithinCalendarMonths succeeds)
    // InvalidCases: value is NOT within months — Guard throws (Must.Be.WithinCalendarMonths fails)
    public static class NotWithinCalendarMonths
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int months)>> ValidCases =>
        [
            new("within 1 month", (Today, Today.AddMonths(1), 1), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int months)>> InvalidCases =>
        [
            new("not within 1 month", (Today, Today.AddMonths(2), 1), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.WithinCalendarMonths — throws when NOT within calendar months (calls Must.Be.NotWithinCalendarMonths — complement)
    // ValidCases: value is NOT within months — Guard does not throw (Must.Be.NotWithinCalendarMonths succeeds)
    // InvalidCases: value IS within months — Guard throws (Must.Be.NotWithinCalendarMonths fails)
    public static class WithinCalendarMonths
    {
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int months)>> ValidCases =>
        [
            new("not within 1 month", (Today, Today.AddMonths(2), 1), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateOnly value, DateOnly reference, int months)>> InvalidCases =>
        [
            new("within 1 month", (Today, Today.AddMonths(1), 1), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.BelowMinimumAge — throws when the birth date does NOT meet the minimum age (calls Must.Be.MinimumAge)
    // ValidCases: the birth date meets the minimum age — Guard does not throw
    // InvalidCases: the birth date falls short, or years is negative — Guard throws
    // The fixture's NullValue scenario is dropped: this overload takes a non-nullable DateOnly.
    public static class BelowMinimumAge
    {
        public static TheoryData<GuardCase<(DateOnly value, int years)>> ValidCases =>
            F.HasMinimumAge.AllValid.Project(v => (v.value!.Value, v.years)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateOnly value, int years)>> InvalidCases =>
            F.HasMinimumAge.AllInvalid.Except(nameof(F.HasMinimumAge.NullValue)).Project(v => (v.value!.Value, v.years)).ToGuardCases(s => s.Name switch
            {
                nameof(F.HasMinimumAge.NegativeYears) => new GuardExpected(false, typeof(ArgumentException), "years", Code: MustCodes.Date.Age.BelowMinimum),
                _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Date.Age.BelowMinimum)
            });
    }

    // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
    // here the boundary moves and the birth date stays put, which the shared provider cannot express.
    public static class BelowMinimumAgeOnLeapDay
    {
        public static TheoryData<GuardCase<(DateOnly value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Date.Age.BelowMinimum)),
            new("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new GuardExpected(true)),
            new("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new GuardExpected(true))
        ];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
