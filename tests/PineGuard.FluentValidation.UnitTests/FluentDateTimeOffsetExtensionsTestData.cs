using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDateTimeOffsetExtensionsTestData
{
    private static readonly DateTimeOffset RefDate = new(2020, 1, 10, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset RefDateMinus1 = RefDate.AddDays(-1);
    private static readonly DateTimeOffset RefDatePlus1 = RefDate.AddDays(1);
    private static readonly DateTimeOffset RefDatePlus2 = RefDate.AddDays(2);

    // Now as the pinned clock reports it, derived from FixedTimeProvider.Default rather than restated as a
    // literal. That instant is in the real past, so the value two days after it is past on the machine clock
    // and future on the pinned one — an extension that dropped the supplied provider would fail these groups.
    private static readonly DateTimeOffset PinnedNow = FixedTimeProvider.Default.GetUtcNow();

    public static class Past
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Past", F.IsPast.PastDate, new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Future", F.IsPast.FutureDate, new FluentExpected(false, "Value must be in the past."))
        ];
    }

    public static class Future
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Future", F.IsPast.FutureDate, new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Past", F.IsPast.PastDate, new FluentExpected(false, "Value must be in the future."))
        ];
    }

    public static class PastOrPresent
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Past", F.IsPast.PastDate, new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Future", F.IsPast.FutureDate, new FluentExpected(false, "Value must be in the past or present."))
        ];
    }

    public static class FutureOrPresent
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Future", F.IsPast.FutureDate, new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Past", F.IsPast.PastDate, new FluentExpected(false, "Value must be in the future or present."))
        ];
    }

    public static class PastPinnedClock
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(false, "Value must be in the past.")),
            new("Future", PinnedNow.AddDays(2), new FluentExpected(false, "Value must be in the past."))
        ];
    }

    public static class PastOrPresentPinnedClock
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Future", PinnedNow.AddDays(2), new FluentExpected(false, "Value must be in the past or present."))
        ];
    }

    public static class FuturePinnedClock
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Future", PinnedNow.AddDays(2), new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(false, "Value must be in the future.")),
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(false, "Value must be in the future."))
        ];
    }

    public static class FutureOrPresentPinnedClock
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases =>
        [
            new("Future", PinnedNow.AddDays(2), new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(true)),
            new("Null", null, new FluentExpected(true)),
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(false, "Value must be in the future or present."))
        ];
    }

    public static class Between
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max)>> Cases =>
        [
            new("In range", (RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Null", (null, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Too early", (RefDateMinus1.AddTicks(-1), RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must be within the expected range.")),
            new("Too late", (RefDatePlus1.AddTicks(1), RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must be within the expected range."))
        ];
    }

    public static class NotBetween
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max)>> Cases =>
        [
            new("Outside range", (RefDatePlus2, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Null", (null, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Inside range", (RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must not be within the expected range."))
        ];
    }

    public static class Before
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be before the specified date/time.", Code: MustCodes.Date.Order.NotBefore))
        ];
    }

    public static class OnOrBefore
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be on or before the specified date/time."))
        ];
    }

    public static class After
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be after the specified date/time."))
        ];
    }

    public static class OnOrAfter
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be on or after the specified date/time."))
        ];
    }

    public static class Same
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Different", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be the same date/time."))
        ];
    }

    public static class NotSame
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Different", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Same", (RefDate, RefDate), new FluentExpected(false, "Value must not be the same date/time."))
        ];
    }

    public static class Chronological
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset end)>> Cases =>
        [
            new("Start before end", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Start after end", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be chronological."))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<FluentCase<(DateTimeOffset? start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)>> Cases =>
        [
            new("Overlaps", (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Null", (null, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Does not overlap", (RefDateMinus1, RefDate, RefDatePlus1, RefDatePlus2), new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<FluentCase<(DateTimeOffset? start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)>> Cases =>
        [
            new("Does not overlap", (RefDateMinus1, RefDate, RefDatePlus1, RefDatePlus2), new FluentExpected(true)),
            new("Null", (null, RefDate, RefDatePlus1, RefDatePlus2), new FluentExpected(true)),
            new("Overlaps", (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    public static class Within
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset reference, TimeSpan window)>> Cases =>
        [
            new("Within window", (RefDatePlus1, RefDate, TimeSpan.FromDays(2)), new FluentExpected(true)),
            new("Null", (null, RefDate, TimeSpan.FromDays(2)), new FluentExpected(true)),
            new("Outside", (RefDatePlus2.AddHours(1), RefDate, TimeSpan.FromDays(2)), new FluentExpected(false, "Value must be within the expected time window."))
        ];
    }

    public static class NotWithin
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset reference, TimeSpan window)>> Cases =>
        [
            new("Outside window", (RefDatePlus2.AddHours(1), RefDate, TimeSpan.FromDays(2)), new FluentExpected(true)),
            new("Null", (null, RefDate, TimeSpan.FromDays(2)), new FluentExpected(true)),
            new("Within", (RefDatePlus1, RefDate, TimeSpan.FromDays(2)), new FluentExpected(false, "Value must not be within the expected time window."))
        ];
    }

    public static class WithinCalendarMonths
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset reference, int months)>> Cases =>
        [
            new("Within", (new DateTimeOffset(2020, 1, 15, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(true)),
            new("Null", (null, new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(true)),
            new("Outside", (new DateTimeOffset(2020, 3, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(false, "Value must be within the expected number of calendar months."))
        ];
    }

    public static class NotWithinCalendarMonths
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset reference, int months)>> Cases =>
        [
            new("Outside", (new DateTimeOffset(2020, 3, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(true)),
            new("Null", (null, new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(true)),
            new("Within", (new DateTimeOffset(2020, 1, 15, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(false, "Value must not be within the expected number of calendar months."))
        ];
    }

    public static class Weekday
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases => F.IsWeekday.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsWeekday.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a weekday.", Code: MustCodes.Date.Calendar.NotWeekday)
        });
    }

    public static class Weekend
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases => F.IsWeekend.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsWeekend.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a weekend day.", Code: MustCodes.Date.Calendar.NotWeekend)
        });
    }

    public static class FirstDayOfMonth
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases => F.IsFirstDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFirstDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be the first day of the month.", Code: MustCodes.Date.Calendar.NotFirstDayOfMonth)
        });
    }

    public static class NotFirstDayOfMonth
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases => F.IsFirstDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFirstDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be the first day of the month.", Code: MustCodes.Date.Calendar.FirstDayOfMonth),
            _ => new FluentExpected(true)
        });
    }

    public static class LastDayOfMonth
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases => F.IsLastDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLastDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be the last day of the month.", Code: MustCodes.Date.Calendar.NotLastDayOfMonth)
        });
    }

    public static class NotLastDayOfMonth
    {
        public static TheoryData<FluentCase<DateTimeOffset?>> Cases => F.IsLastDayOfMonth.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLastDayOfMonth.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be the last day of the month.", Code: MustCodes.Date.Calendar.LastDayOfMonth),
            _ => new FluentExpected(true)
        });
    }

    public static class ChronologicalExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset start, DateTimeOffset end)>> Cases =>
        [
            new("Start before end", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("Start after end", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be chronological."))
        ];
    }

    public static class OverlappingExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)>> Cases =>
        [
            new("Overlaps", (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Does not overlap", (RefDateMinus1, RefDate, RefDatePlus1, RefDatePlus2), new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlappingExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)>> Cases =>
        [
            new("Does not overlap", (RefDateMinus1, RefDate, RefDatePlus1, RefDatePlus2), new FluentExpected(true)),
            new("Overlaps", (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    public static class PastNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(true)),
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(false, "Value must be in the past."))
        ];
    }

    public static class PastOrPresentNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(true)),
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(false, "Value must be in the past or present."))
        ];
    }

    public static class FutureNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(true)),
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(false, "Value must be in the future."))
        ];
    }

    public static class FutureOrPresentNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(true)),
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(false, "Value must be in the future or present."))
        ];
    }

    public static class PastPinnedClockNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(false, "Value must be in the past.")),
            new("Future", PinnedNow.AddDays(2), new FluentExpected(false, "Value must be in the past."))
        ];
    }

    public static class PastOrPresentPinnedClockNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(true)),
            new("Future", PinnedNow.AddDays(2), new FluentExpected(false, "Value must be in the past or present."))
        ];
    }

    public static class FuturePinnedClockNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Future", PinnedNow.AddDays(2), new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(false, "Value must be in the future.")),
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(false, "Value must be in the future."))
        ];
    }

    public static class FutureOrPresentPinnedClockNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
        [
            new("Future", PinnedNow.AddDays(2), new FluentExpected(true)),
            new("ThisVeryInstant", PinnedNow, new FluentExpected(true)),
            new("Past", PinnedNow.AddDays(-2), new FluentExpected(false, "Value must be in the future or present."))
        ];
    }

    public static class BetweenNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max)>> Cases =>
        [
            new("In range",  (RefDate,                 RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Too early", (RefDateMinus1.AddTicks(-1), RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must be within the expected range."))
        ];
    }

    public static class NotBetweenNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max)>> Cases =>
        [
            new("Outside range", (RefDatePlus2,  RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Inside range",  (RefDate,        RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must not be within the expected range."))
        ];
    }

    public static class BeforeNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Before", (RefDateMinus1, RefDate),    new FluentExpected(true)),
            new("After",  (RefDatePlus1,  RefDate),    new FluentExpected(false, "Value must be before the specified date/time."))
        ];
    }

    public static class OnOrBeforeNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Same",  (RefDate,        RefDate),    new FluentExpected(true)),
            new("After", (RefDatePlus1,   RefDate),    new FluentExpected(false, "Value must be on or before the specified date/time."))
        ];
    }

    public static class AfterNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("After",  (RefDatePlus1,  RefDate),    new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate),    new FluentExpected(false, "Value must be after the specified date/time."))
        ];
    }

    public static class OnOrAfterNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Same",   (RefDate,        RefDate),   new FluentExpected(true)),
            new("Before", (RefDateMinus1,  RefDate),   new FluentExpected(false, "Value must be on or after the specified date/time."))
        ];
    }

    public static class SameNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Same",      (RefDate,      RefDate),  new FluentExpected(true)),
            new("Different", (RefDatePlus1, RefDate),  new FluentExpected(false, "Value must be the same date/time."))
        ];
    }

    public static class NotSameNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Different", (RefDatePlus1, RefDate),  new FluentExpected(true)),
            new("Same",      (RefDate,      RefDate),  new FluentExpected(false, "Value must not be the same date/time."))
        ];
    }

    public static class ChronologicalNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset end)>> Cases =>
        [
            new("Start before end", (RefDateMinus1, RefDate),   new FluentExpected(true)),
            new("Start after end",  (RefDatePlus1,  RefDate),   new FluentExpected(false, "Value must be chronological."))
        ];
    }

    public static class OverlappingNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)>> Cases =>
        [
            new("Overlaps",          (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Does not overlap",  (RefDateMinus1, RefDate, RefDatePlus1,  RefDatePlus2), new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlappingNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)>> Cases =>
        [
            new("Does not overlap", (RefDateMinus1, RefDate, RefDatePlus1,  RefDatePlus2), new FluentExpected(true)),
            new("Overlaps",         (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    public static class WithinNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>> Cases =>
        [
            new("Within window", (RefDatePlus1, RefDate, TimeSpan.FromDays(2)),         new FluentExpected(true)),
            new("Outside",       (RefDatePlus2.AddHours(1), RefDate, TimeSpan.FromDays(2)), new FluentExpected(false, "Value must be within the expected time window."))
        ];
    }

    public static class NotWithinNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>> Cases =>
        [
            new("Outside window", (RefDatePlus2.AddHours(1), RefDate, TimeSpan.FromDays(2)), new FluentExpected(true)),
            new("Within",         (RefDatePlus1, RefDate, TimeSpan.FromDays(2)),              new FluentExpected(false, "Value must not be within the expected time window."))
        ];
    }

    public static class WithinCalendarMonthsNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset reference, int months)>> Cases =>
        [
            new("Within",  (new DateTimeOffset(2020, 1, 15, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(true)),
            new("Outside", (new DateTimeOffset(2020, 3, 1,  0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(false, "Value must be within the expected number of calendar months."))
        ];
    }

    public static class NotWithinCalendarMonthsNonNullable
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset reference, int months)>> Cases =>
        [
            new("Outside", (new DateTimeOffset(2020, 3, 1,  0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(true)),
            new("Within",  (new DateTimeOffset(2020, 1, 15, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), 1), new FluentExpected(false, "Value must not be within the expected number of calendar months."))
        ];
    }

    public static class WeekdayNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
            F.IsWeekday.AllScenarios.Except(nameof(F.IsWeekday.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be a weekday.", Code: MustCodes.Date.Calendar.NotWeekday));
    }

    public static class WeekendNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
            F.IsWeekend.AllScenarios.Except(nameof(F.IsWeekend.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be a weekend day.", Code: MustCodes.Date.Calendar.NotWeekend));
    }

    public static class FirstDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
            F.IsFirstDayOfMonth.AllScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be the first day of the month.", Code: MustCodes.Date.Calendar.NotFirstDayOfMonth));
    }

    public static class NotFirstDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
            F.IsFirstDayOfMonth.AllScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(false, "Value must not be the first day of the month.", Code: MustCodes.Date.Calendar.FirstDayOfMonth)
                : new FluentExpected(true));
    }

    public static class LastDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
            F.IsLastDayOfMonth.AllScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be the last day of the month.", Code: MustCodes.Date.Calendar.NotLastDayOfMonth));
    }

    public static class NotLastDayOfMonthNonNullable
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases =>
            F.IsLastDayOfMonth.AllScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToFluentCases(s => s.IsValid
                ? new FluentExpected(false, "Value must not be the last day of the month.", Code: MustCodes.Date.Calendar.LastDayOfMonth)
                : new FluentExpected(true));
    }

    // ── Cross-property expression overloads ──────────────────────────

    public static class BeforeExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be before the specified date/time.", Code: MustCodes.Date.Order.NotBefore))
        ];
    }

    public static class BeforeNullableExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be before the specified date/time.", Code: MustCodes.Date.Order.NotBefore))
        ];
    }

    public static class OnOrBeforeExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be on or before the specified date/time.", Code: MustCodes.Date.Order.After))
        ];
    }

    public static class OnOrBeforeNullableExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be on or before the specified date/time.", Code: MustCodes.Date.Order.After))
        ];
    }

    public static class AfterExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be after the specified date/time.", Code: MustCodes.Date.Order.NotAfter))
        ];
    }

    public static class AfterNullableExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be after the specified date/time.", Code: MustCodes.Date.Order.NotAfter))
        ];
    }

    public static class OnOrAfterExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset value, DateTimeOffset other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be on or after the specified date/time.", Code: MustCodes.Date.Order.Before))
        ];
    }

    public static class OnOrAfterNullableExpression
    {
        public static TheoryData<FluentCase<(DateTimeOffset? value, DateTimeOffset other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Null", (null, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be on or after the specified date/time.", Code: MustCodes.Date.Order.Before))
        ];
    }
}
