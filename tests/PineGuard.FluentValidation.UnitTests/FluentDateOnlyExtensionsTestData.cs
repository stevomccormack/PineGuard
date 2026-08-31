using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateOnlyRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDateOnlyExtensionsTestData
{
    public static class Past
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPast.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be in the past.")
        });
    }

    public static class PastOrPresent
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPast.NullValue) => new FluentExpected(true),
            nameof(F.IsPast.FutureDate) => new FluentExpected(false, "Value must be in the past or present."),
            _ => new FluentExpected(true)
        });
    }

    public static class Future
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPast.NullValue) => new FluentExpected(true),
            nameof(F.IsPast.FutureDate) => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be in the future.")
        });
    }

    public static class FutureOrPresent
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsPast.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPast.NullValue) => new FluentExpected(true),
            nameof(F.IsPast.PastDate) => new FluentExpected(false, "Value must be in the future or present."),
            _ => new FluentExpected(true)
        });
    }

    public static class Between
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly min, DateOnly max)>> Cases =>
            F.IsBetween.AllScenarios
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly min, DateOnly max)>(s.Name, (s.Inputs.value, s.Inputs.min, s.Inputs.max), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsBetween.NullValue) => new FluentExpected(true),
                nameof(F.IsBetween.AtMinExclusive) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be within the expected range.")
            });
    }

    public static class NotBetween
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly min, DateOnly max)>> Cases =>
            F.IsBetween.AllScenarios
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly min, DateOnly max)>(s.Name, (s.Inputs.value, s.Inputs.min, s.Inputs.max), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsBetween.NullValue) => new FluentExpected(true),
                nameof(F.IsBetween.AtMinExclusive) => new FluentExpected(false, "Value must not be within the expected range."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be within the expected range."),
                _ => new FluentExpected(true)
            });
    }

    public static class Before
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
            F.IsBefore.AllScenarios.Where(s => s.Inputs.other.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly other)>(s.Name, (s.Inputs.value, s.Inputs.other!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsBefore.NullValue) => new FluentExpected(true),
                nameof(F.IsBefore.SameDayInclusive) => new FluentExpected(false, "Value must be before the specified date.", Code: MustCodes.Date.Order.NotBefore),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be before the specified date.", Code: MustCodes.Date.Order.NotBefore)
            });
    }

    public static class OnOrBefore
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
            F.IsBefore.AllScenarios.Where(s => s.Inputs.other.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly other)>(s.Name, (s.Inputs.value, s.Inputs.other!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsBefore.NullValue) => new FluentExpected(true),
                nameof(F.IsBefore.BeforeInclusive) => new FluentExpected(true),
                nameof(F.IsBefore.SameDayInclusive) => new FluentExpected(true),
                nameof(F.IsBefore.SameDayExclusive) => new FluentExpected(true),
                nameof(F.IsBefore.YearPrecision) => new FluentExpected(true),
                nameof(F.IsBefore.UnknownPrecision) => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be on or before the specified date.")
            });
    }

    public static class After
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
            F.IsAfter.AllScenarios.Where(s => s.Inputs.other.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly other)>(s.Name, (s.Inputs.value, s.Inputs.other!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsAfter.NullValue) => new FluentExpected(true),
                nameof(F.IsAfter.SameDayInclusive) => new FluentExpected(false, "Value must be after the specified date."),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be after the specified date.")
            });
    }

    public static class OnOrAfter
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
            F.IsAfter.AllScenarios.Where(s => s.Inputs.other.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly other)>(s.Name, (s.Inputs.value, s.Inputs.other!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsAfter.NullValue) => new FluentExpected(true),
                nameof(F.IsAfter.AfterInclusive) => new FluentExpected(true),
                nameof(F.IsAfter.SameDayInclusive) => new FluentExpected(true),
                nameof(F.IsAfter.SameDayExclusive) => new FluentExpected(true),
                nameof(F.IsAfter.MonthPrecision) => new FluentExpected(true),
                nameof(F.IsAfter.UnknownPrecision) => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be on or after the specified date.")
            });
    }

    public static class Same
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
            F.IsSame.AllScenarios.Where(s => s.Inputs.other.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly other)>(s.Name, (s.Inputs.value, s.Inputs.other!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsSame.NullValue) => new FluentExpected(true),
                nameof(F.IsSame.YearPrecision) => new FluentExpected(false, "Value must be the same date."),
                nameof(F.IsSame.UnknownPrecision) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be the same date.")
            });
    }

    public static class NotSame
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
            F.IsSame.AllScenarios.Where(s => s.Inputs.other.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly other)>(s.Name, (s.Inputs.value, s.Inputs.other!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsSame.NullValue) => new FluentExpected(true),
                nameof(F.IsSame.YearPrecision) => new FluentExpected(true),
                nameof(F.IsSame.UnknownPrecision) => new FluentExpected(false, "Value must not be the same date."),
                _ when s.IsValid => new FluentExpected(false, "Value must not be the same date."),
                _ => new FluentExpected(true)
            });
    }

    public static class Chronological
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly end)>> Cases =>
            F.IsChronological.AllScenarios.Where(s => s.Inputs.end.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly end)>(s.Name, (s.Inputs.start, s.Inputs.end!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsChronological.StartNullEndSet) => new FluentExpected(true),
                nameof(F.IsChronological.SameDayInclusive) => new FluentExpected(false, "Value must be chronological."),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be chronological.")
            });
    }

    public static class NotChronological
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly end)>> Cases =>
            F.IsChronological.AllScenarios.Where(s => s.Inputs.end.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly end)>(s.Name, (s.Inputs.start, s.Inputs.end!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsChronological.StartNullEndSet) => new FluentExpected(true),
                nameof(F.IsChronological.SameDayInclusive) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not be chronological."),
                _ => new FluentExpected(true)
            });
    }

    public static class Overlapping
    {
        private static readonly DateOnly Ov2 = new(2024, 1, 5);
        private static readonly DateOnly Ov3 = new(2024, 1, 3);
        private static readonly DateOnly Ov4 = new(2024, 1, 8);

        public static TheoryData<FluentCase<(DateOnly? value, DateOnly end1, DateOnly start2, DateOnly end2)>> Cases =>
        [
            ..F.IsOverlapping.AllScenarios
                .Where(s => s.Inputs is { end1: not null, start2: not null, end2: not null })
                .Select(s => new RuleScenario<(DateOnly? value, DateOnly end1, DateOnly start2, DateOnly end2)>(s.Name, (s.Inputs.start1, s.Inputs.end1!.Value, s.Inputs.start2!.Value, s.Inputs.end2!.Value), s.IsValid)).ToArray()
                .ToFluentCases(s => s.Name switch
                {
                    nameof(F.IsOverlapping.TouchingInclusive) => new FluentExpected(false, "Value must be overlapping."),
                    _ when s.IsValid => new FluentExpected(true),
                    _ => new FluentExpected(false, "Value must be overlapping.")
                }),
            new("NullStart1", (null, Ov2, Ov3, Ov4), new FluentExpected(true))
        ];
    }

    public static class NotOverlapping
    {
        private static readonly DateOnly Ov2 = new(2024, 1, 5);
        private static readonly DateOnly Ov3 = new(2024, 1, 3);
        private static readonly DateOnly Ov4 = new(2024, 1, 8);

        public static TheoryData<FluentCase<(DateOnly? value, DateOnly end1, DateOnly start2, DateOnly end2)>> Cases =>
        [
            ..F.IsOverlapping.AllScenarios
                .Where(s => s.Inputs is { end1: not null, start2: not null, end2: not null })
                .Select(s => new RuleScenario<(DateOnly? value, DateOnly end1, DateOnly start2, DateOnly end2)>(s.Name, (s.Inputs.start1, s.Inputs.end1!.Value, s.Inputs.start2!.Value, s.Inputs.end2!.Value), s.IsValid)).ToArray()
                .ToFluentCases(s => s.Name switch
                {
                    nameof(F.IsOverlapping.TouchingInclusive) => new FluentExpected(true),
                    _ when s.IsValid => new FluentExpected(false, "Value must not be overlapping."),
                    _ => new FluentExpected(true)
                }),
            new("NullStart1", (null, Ov2, Ov3, Ov4), new FluentExpected(true))
        ];
    }

    public static class WithinDays
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly reference, int days)>> Cases =>
            F.IsWithin.AllScenarios.Where(s => s.Inputs.reference.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly reference, int days)>(s.Name, (s.Inputs.value, s.Inputs.reference!.Value, s.Inputs.days), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsWithin.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be within the expected number of days.")
            });
    }

    public static class NotWithinDays
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly reference, int days)>> Cases =>
            F.IsWithin.AllScenarios.Where(s => s.Inputs.reference.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly reference, int days)>(s.Name, (s.Inputs.value, s.Inputs.reference!.Value, s.Inputs.days), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsWithin.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not be within the expected number of days."),
                _ => new FluentExpected(true)
            });
    }

    public static class WithinCalendarMonths
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly reference, int months)>> Cases =>
            F.IsWithinCalendarMonths.AllScenarios.Where(s => s.Inputs.reference.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly reference, int months)>(s.Name, (s.Inputs.value, s.Inputs.reference!.Value, s.Inputs.months), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsWithinCalendarMonths.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be within the expected number of calendar months.")
            });
    }

    public static class NotWithinCalendarMonths
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly reference, int months)>> Cases =>
            F.IsWithinCalendarMonths.AllScenarios.Where(s => s.Inputs.reference.HasValue)
            .Select(s => new RuleScenario<(DateOnly? value, DateOnly reference, int months)>(s.Name, (s.Inputs.value, s.Inputs.reference!.Value, s.Inputs.months), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                nameof(F.IsWithinCalendarMonths.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not be within the expected number of calendar months."),
                _ => new FluentExpected(true)
            });
    }

    public static class Weekday
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsWeekday.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsWeekday.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a weekday.", Code: MustCodes.Date.Calendar.NotWeekday)
        });
    }

    public static class Weekend
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsWeekend.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsWeekend.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a weekend day.", Code: MustCodes.Date.Calendar.NotWeekend)
        });
    }

    public static class FirstDayOfMonth
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsFirstDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFirstDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be the first day of the month.", Code: MustCodes.Date.Calendar.NotFirstDayOfMonth)
        });
    }

    public static class NotFirstDayOfMonth
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsFirstDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFirstDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be the first day of the month.", Code: MustCodes.Date.Calendar.FirstDayOfMonth),
            _ => new FluentExpected(true)
        });
    }

    public static class LastDayOfMonth
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsLastDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLastDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be the last day of the month.", Code: MustCodes.Date.Calendar.NotLastDayOfMonth)
        });
    }

    public static class NotLastDayOfMonth
    {
        public static TheoryData<FluentCase<DateOnly?>> Cases => F.IsLastDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLastDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be the last day of the month.", Code: MustCodes.Date.Calendar.LastDayOfMonth),
            _ => new FluentExpected(true)
        });
    }

    public static class MinimumAge
    {
        public static TheoryData<FluentCase<(DateOnly? value, int years)>> Cases => F.HasMinimumAge.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasMinimumAge.NullValue) => new FluentExpected(true),
            nameof(F.HasMinimumAge.NegativeYears) => new FluentExpected(false, "years requires a non-negative number of years.", Code: MustCodes.Date.Age.BelowMinimum),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)
        });
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    private static readonly DateOnly PastDateOnly = new(2000, 1, 1);
    private static readonly DateOnly FutureDateOnly = new(2999, 1, 1);
    private static readonly DateOnly MinDate = new(2024, 1, 1);
    private static readonly DateOnly MaxDate = new(2024, 1, 3);
    private static readonly DateOnly ReferenceDate = new(2020, 6, 15);

    public static class PastNonNullable
    {
        private static RuleScenario<DateOnly>[] AllScenarios =>
        [
            new("PastDate",   PastDateOnly,   true),
            new("FutureDate", FutureDateOnly, false)
        ];

        public static TheoryData<FluentCase<DateOnly>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be in the past.")
        });
    }

    public static class PastOrPresentNonNullable
    {
        private static RuleScenario<DateOnly>[] AllScenarios =>
        [
            new("PastDate",   PastDateOnly,   true),
            new("FutureDate", FutureDateOnly, false)
        ];

        public static TheoryData<FluentCase<DateOnly>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be in the past or present.")
        });
    }

    public static class FutureNonNullable
    {
        private static RuleScenario<DateOnly>[] AllScenarios =>
        [
            new("FutureDate", FutureDateOnly, true),
            new("PastDate",   PastDateOnly,   false)
        ];

        public static TheoryData<FluentCase<DateOnly>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be in the future.")
        });
    }

    public static class FutureOrPresentNonNullable
    {
        private static RuleScenario<DateOnly>[] AllScenarios =>
        [
            new("FutureDate", FutureDateOnly, true),
            new("PastDate",   PastDateOnly,   false)
        ];

        public static TheoryData<FluentCase<DateOnly>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be in the future or present.")
        });
    }

    public static class BetweenNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly min, DateOnly max)>[] AllScenarios =>
        [
            new("Inside",   (new DateOnly(2024, 1, 2), MinDate, MaxDate), true),
            new("Outside",  (new DateOnly(2024, 1, 5), MinDate, MaxDate), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly min, DateOnly max)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be within the expected range.")
        });
    }

    public static class NotBetweenNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly min, DateOnly max)>[] AllScenarios =>
        [
            new("Outside",  (new DateOnly(2024, 1, 5), MinDate, MaxDate), true),
            new("Inside",   (new DateOnly(2024, 1, 2), MinDate, MaxDate), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly min, DateOnly max)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be within the expected range.")
        });
    }

    public static class BeforeNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly other)>[] AllScenarios =>
        [
            new("Before", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2)), true),
            new("After",  (new DateOnly(2024, 1, 3), new DateOnly(2024, 1, 2)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be before the specified date.")
        });
    }

    public static class OnOrBeforeNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly other)>[] AllScenarios =>
        [
            new("SameDay", (new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 2)), true),
            new("After",   (new DateOnly(2024, 1, 3), new DateOnly(2024, 1, 2)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be on or before the specified date.")
        });
    }

    public static class AfterNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly other)>[] AllScenarios =>
        [
            new("After",  (new DateOnly(2024, 1, 3), new DateOnly(2024, 1, 2)), true),
            new("Before", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be after the specified date.")
        });
    }

    public static class OnOrAfterNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly other)>[] AllScenarios =>
        [
            new("SameDay", (new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 2)), true),
            new("Before",  (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be on or after the specified date.")
        });
    }

    public static class SameNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly other)>[] AllScenarios =>
        [
            new("SameDay",    (new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 2)), true),
            new("DifferentDay", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be the same date.")
        });
    }

    public static class NotSameNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly other)>[] AllScenarios =>
        [
            new("DifferentDay", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2)), true),
            new("SameDay",      (new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 2)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be the same date.")
        });
    }

    public static class ChronologicalNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly end)>[] AllScenarios =>
        [
            new("Before", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 3)), true),
            new("After",  (new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 3)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly end)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be chronological.")
        });
    }

    public static class NotChronologicalNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly end)>[] AllScenarios =>
        [
            new("After",  (new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 3)), true),
            new("Before", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 3)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly end)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be chronological.")
        });
    }

    public static class OverlappingNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly end1, DateOnly start2, DateOnly end2)>[] AllScenarios =>
        [
            new("Overlapping",   (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 3), new DateOnly(2024, 1, 8)), true),
            new("NotOverlapping", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 8)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly end1, DateOnly start2, DateOnly end2)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be overlapping.")
        });
    }

    public static class NotOverlappingNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly end1, DateOnly start2, DateOnly end2)>[] AllScenarios =>
        [
            new("NotOverlapping", (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 8)), true),
            new("Overlapping",    (new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 3), new DateOnly(2024, 1, 8)), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly end1, DateOnly start2, DateOnly end2)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be overlapping.")
        });
    }

    public static class WithinDaysNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly reference, int days)>[] AllScenarios =>
        [
            new("WithinDays",   (ReferenceDate, ReferenceDate.AddDays(-3), 5), true),
            new("OutsideDays",  (ReferenceDate, ReferenceDate.AddDays(-10), 5), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly reference, int days)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be within the expected number of days.")
        });
    }

    public static class NotWithinDaysNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly reference, int days)>[] AllScenarios =>
        [
            new("OutsideDays",  (ReferenceDate, ReferenceDate.AddDays(-10), 5), true),
            new("WithinDays",   (ReferenceDate, ReferenceDate.AddDays(-3), 5), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly reference, int days)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be within the expected number of days.")
        });
    }

    public static class WithinCalendarMonthsNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly reference, int months)>[] AllScenarios =>
        [
            new("SameMonth",     (ReferenceDate, ReferenceDate.AddMonths(-1), 2), true),
            new("OutsideMonths", (ReferenceDate, ReferenceDate.AddMonths(-6), 2), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly reference, int months)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be within the expected number of calendar months.")
        });
    }

    public static class NotWithinCalendarMonthsNonNullable
    {
        private static RuleScenario<(DateOnly value, DateOnly reference, int months)>[] AllScenarios =>
        [
            new("OutsideMonths", (ReferenceDate, ReferenceDate.AddMonths(-6), 2), true),
            new("SameMonth",     (ReferenceDate, ReferenceDate.AddMonths(-1), 2), false)
        ];

        public static TheoryData<FluentCase<(DateOnly value, DateOnly reference, int months)>> Cases => AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be within the expected number of calendar months.")
        });
    }

    public static class WeekdayNonNullable
    {
        public static TheoryData<FluentCase<DateOnly>> Cases =>
            F.IsWeekday.AllScenarios.Except(nameof(F.IsWeekday.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be a weekday.", Code: MustCodes.Date.Calendar.NotWeekday));
    }

    public static class WeekendNonNullable
    {
        public static TheoryData<FluentCase<DateOnly>> Cases =>
            F.IsWeekend.AllScenarios.Except(nameof(F.IsWeekend.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be a weekend day.", Code: MustCodes.Date.Calendar.NotWeekend));
    }

    public static class FirstDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateOnly>> Cases =>
            F.IsFirstDayOfMonth.AllScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be the first day of the month.", Code: MustCodes.Date.Calendar.NotFirstDayOfMonth));
    }

    public static class NotFirstDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateOnly>> Cases =>
            F.IsFirstDayOfMonth.AllScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(false, "Value must not be the first day of the month.", Code: MustCodes.Date.Calendar.FirstDayOfMonth)
                : new FluentExpected(true));
    }

    public static class LastDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateOnly>> Cases =>
            F.IsLastDayOfMonth.AllScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be the last day of the month.", Code: MustCodes.Date.Calendar.NotLastDayOfMonth));
    }

    public static class NotLastDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateOnly>> Cases =>
            F.IsLastDayOfMonth.AllScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(false, "Value must not be the last day of the month.", Code: MustCodes.Date.Calendar.LastDayOfMonth)
                : new FluentExpected(true));
    }

    public static class MinimumAgeNonNullable
    {
        public static TheoryData<FluentCase<(DateOnly value, int years)>> Cases =>
            F.HasMinimumAge.AllScenarios.Except(nameof(F.HasMinimumAge.NullValue)).Project(v => (v.value!.Value, v.years)).ToFluentCases(s => s.Name switch
            {
                nameof(F.HasMinimumAge.NegativeYears) => new FluentExpected(false, "years requires a non-negative number of years.", Code: MustCodes.Date.Age.BelowMinimum),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)
            });
    }

    // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
    // here the boundary moves and the birth date stays put, which the shared provider cannot express.
    public static class MinimumAgeOnLeapDay
    {
        public static TheoryData<FluentCase<(DateOnly value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new FluentExpected(false, "Value must meet the expected minimum age.", Code: MustCodes.Date.Age.BelowMinimum)),
            new("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new FluentExpected(true)),
            new("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new FluentExpected(true))
        ];
    }

    // ── Cross-property expression overloads ──────────────────────────

    private static readonly DateOnly RefDate = new(2024, 6, 15);
    private static readonly DateOnly RefDateMinus1 = RefDate.AddDays(-1);
    private static readonly DateOnly RefDatePlus1 = RefDate.AddDays(1);

    public static class BeforeExpression
    {
        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases =>
        [
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be before the specified date.", Code: MustCodes.Date.Order.NotBefore))
        ];
    }

    public static class BeforeNullableExpression
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
        [
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be before the specified date.", Code: MustCodes.Date.Order.NotBefore))
        ];
    }

    public static class OnOrBeforeExpression
    {
        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be on or before the specified date.", Code: MustCodes.Date.Order.After))
        ];
    }

    public static class OnOrBeforeNullableExpression
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be on or before the specified date.", Code: MustCodes.Date.Order.After))
        ];
    }

    public static class AfterExpression
    {
        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases =>
        [
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be after the specified date.", Code: MustCodes.Date.Order.NotAfter))
        ];
    }

    public static class AfterNullableExpression
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
        [
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be after the specified date.", Code: MustCodes.Date.Order.NotAfter))
        ];
    }

    public static class OnOrAfterExpression
    {
        public static TheoryData<FluentCase<(DateOnly value, DateOnly other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be on or after the specified date.", Code: MustCodes.Date.Order.Before))
        ];
    }

    public static class OnOrAfterNullableExpression
    {
        public static TheoryData<FluentCase<(DateOnly? value, DateOnly other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be on or after the specified date.", Code: MustCodes.Date.Order.Before))
        ];
    }

    private static readonly DateOnly LeapDayBirth = new(2008, 02, 29);

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
