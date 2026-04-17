using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.DateTimeRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDateTimeExtensionsTestData
{
    private static readonly DateTime RefDate = new(2020, 1, 10, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime RefDateMinus1 = RefDate.AddDays(-1);
    private static readonly DateTime RefDatePlus1 = RefDate.AddDays(1);
    private static readonly DateTime RefDatePlus2 = RefDate.AddDays(2);

    public static class Past
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(true)),
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(false, "Value must be in the past."))
        ];
    }

    public static class Future
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(true)),
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(false, "Value must be in the future."))
        ];
    }

    public static class PastOrPresent
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(true)),
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(false, "Value must be in the past or present."))
        ];
    }

    public static class FutureOrPresent
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Future", F.IsPast.FutureDate!.Value, new FluentExpected(true)),
            new("Past", F.IsPast.PastDate!.Value, new FluentExpected(false, "Value must be in the future or present."))
        ];
    }

    public static class Between
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime min, DateTime max)>> Cases =>
        [
            new("In range", (RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Too early", (RefDateMinus1.AddTicks(-1), RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must be within the expected range.")),
            new("Too late", (RefDatePlus1.AddTicks(1), RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must be within the expected range."))
        ];
    }

    public static class NotBetween
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime min, DateTime max)>> Cases =>
        [
            new("Outside range", (RefDatePlus2, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Inside range", (RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must not be within the expected range."))
        ];
    }

    public static class Before
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be before the specified date/time."))
        ];
    }

    public static class OnOrBefore
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be on or before the specified date/time."))
        ];
    }

    public static class After
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be after the specified date/time."))
        ];
    }

    public static class OnOrAfter
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("After", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Before", (RefDateMinus1, RefDate), new FluentExpected(false, "Value must be on or after the specified date/time."))
        ];
    }

    public static class Same
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("Same", (RefDate, RefDate), new FluentExpected(true)),
            new("Different", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be the same date/time."))
        ];
    }

    public static class NotSame
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("Different", (RefDatePlus1, RefDate), new FluentExpected(true)),
            new("Same", (RefDate, RefDate), new FluentExpected(false, "Value must not be the same date/time."))
        ];
    }

    public static class Chronological
    {
        public static TheoryData<FluentCase<(DateTime start, DateTime end)>> Cases =>
        [
            new("Start before end", (RefDateMinus1, RefDate), new FluentExpected(true)),
            new("Start after end", (RefDatePlus1, RefDate), new FluentExpected(false, "Value must be chronological."))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<FluentCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)>> Cases =>
        [
            new("Overlaps", (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(true)),
            new("Does not overlap", (RefDateMinus1, RefDate, RefDatePlus1, RefDatePlus2), new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<FluentCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)>> Cases =>
        [
            new("Does not overlap", (RefDateMinus1, RefDate, RefDatePlus1, RefDatePlus2), new FluentExpected(true)),
            new("Overlaps", (RefDateMinus1, RefDate, RefDateMinus1, RefDatePlus1), new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    public static class WithinDaysFromNow
    {
        public static TheoryData<FluentCase<(DateTime value, int days)>> Cases =>
        [
            new("Within", (DateTime.UtcNow.AddHours(1), 1), new FluentExpected(true)),
            new("Outside", (DateTime.UtcNow.AddDays(2), 1), new FluentExpected(false, "Value must be within the expected number of days from now."))
        ];
    }

    public static class NotWithinDaysFromNow
    {
        public static TheoryData<FluentCase<(DateTime value, int days)>> Cases =>
        [
            new("Outside", (DateTime.UtcNow.AddDays(2), 1), new FluentExpected(true)),
            new("Within", (DateTime.UtcNow.AddHours(1), 1), new FluentExpected(false, "Value must not be within the expected number of days from now."))
        ];
    }

    public static class Within
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime reference, TimeSpan window)>> Cases =>
        [
            new("Within window", (RefDatePlus1, RefDate, TimeSpan.FromDays(2)), new FluentExpected(true)),
            new("Outside", (RefDatePlus2.AddHours(1), RefDate, TimeSpan.FromDays(2)), new FluentExpected(false, "Value must be within the expected time window."))
        ];
    }

    public static class NotWithin
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime reference, TimeSpan window)>> Cases =>
        [
            new("Outside window", (RefDatePlus2.AddHours(1), RefDate, TimeSpan.FromDays(2)), new FluentExpected(true)),
            new("Within", (RefDatePlus1, RefDate, TimeSpan.FromDays(2)), new FluentExpected(false, "Value must not be within the expected time window."))
        ];
    }

    public static class WithinCalendarMonths
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime reference, int months)>> Cases =>
        [
            new("Within", (new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1), new FluentExpected(true)),
            new("Outside", (new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1), new FluentExpected(false, "Value must be within the expected number of calendar months."))
        ];
    }

    public static class NotWithinCalendarMonths
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime reference, int months)>> Cases =>
        [
            new("Outside", (new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1), new FluentExpected(true)),
            new("Within", (new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1), new FluentExpected(false, "Value must not be within the expected number of calendar months."))
        ];
    }

    public static class Weekday
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Monday", F.IsWeekday.Monday!.Value, new FluentExpected(true)),
            new("Saturday", F.IsWeekday.Saturday!.Value, new FluentExpected(false, "Value must be a weekday."))
        ];
    }

    public static class Weekend
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Saturday", F.IsWeekend.Saturday!.Value, new FluentExpected(true)),
            new("Monday", F.IsWeekend.Monday!.Value, new FluentExpected(false, "Value must be a weekend day."))
        ];
    }

    public static class FirstDayOfMonth
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("First day", F.IsFirstDayOfMonth.FirstDay!.Value, new FluentExpected(true)),
            new("Second day", F.IsFirstDayOfMonth.NotFirst!.Value, new FluentExpected(false, "Value must be the first day of the month."))
        ];
    }

    public static class NotFirstDayOfMonth
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Second day", F.IsFirstDayOfMonth.NotFirst!.Value, new FluentExpected(true)),
            new("First day", F.IsFirstDayOfMonth.FirstDay!.Value, new FluentExpected(false, "Value must not be the first day of the month."))
        ];
    }

    public static class LastDayOfMonth
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Last day", F.IsLastDayOfMonth.LastDay!.Value, new FluentExpected(true)),
            new("Not last day", F.IsLastDayOfMonth.NotLast!.Value, new FluentExpected(false, "Value must be the last day of the month."))
        ];
    }

    public static class NotLastDayOfMonth
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Not last day", F.IsLastDayOfMonth.NotLast!.Value, new FluentExpected(true)),
            new("Last day", F.IsLastDayOfMonth.LastDay!.Value, new FluentExpected(false, "Value must not be the last day of the month."))
        ];
    }

    public static class SameDay
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("Same day", (F.IsSameDay.SameDay.value!.Value, F.IsSameDay.SameDay.other!.Value), new FluentExpected(true)),
            new("Different day", (F.IsSameDay.DifferentDay.value!.Value, F.IsSameDay.DifferentDay.other!.Value), new FluentExpected(false, "Value must be the same day."))
        ];
    }

    public static class NotSameDay
    {
        public static TheoryData<FluentCase<(DateTime value, DateTime other)>> Cases =>
        [
            new("Different day", (F.IsSameDay.DifferentDay.value!.Value, F.IsSameDay.DifferentDay.other!.Value), new FluentExpected(true)),
            new("Same day", (F.IsSameDay.SameDay.value!.Value, F.IsSameDay.SameDay.other!.Value), new FluentExpected(false, "Value must not be the same day."))
        ];
    }

    public static class Utc
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Utc", F.IsUtc.Utc!.Value, new FluentExpected(true)),
            new("Unspecified", F.IsUnspecified.Unspecified!.Value, new FluentExpected(false, "Value must be UTC."))
        ];
    }

    public static class NotUtc
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Unspecified", F.IsUnspecified.Unspecified!.Value, new FluentExpected(true)),
            new("Utc", F.IsUtc.Utc!.Value, new FluentExpected(false, "Value must not be UTC."))
        ];
    }

    public static class Local
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Local", F.IsLocal.Local!.Value, new FluentExpected(true)),
            new("Utc", F.IsLocal.Utc!.Value, new FluentExpected(false, "Value must be local."))
        ];
    }

    public static class NotLocal
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Utc", F.IsLocal.Utc!.Value, new FluentExpected(true)),
            new("Local", F.IsLocal.Local!.Value, new FluentExpected(false, "Value must not be local."))
        ];
    }

    public static class Unspecified
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Unspecified", F.IsUnspecified.Unspecified!.Value, new FluentExpected(true)),
            new("Utc", F.IsUnspecified.Utc!.Value, new FluentExpected(false, "Value must have an unspecified kind."))
        ];
    }

    public static class NotUnspecified
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Utc", F.IsUnspecified.Utc!.Value, new FluentExpected(true)),
            new("Unspecified", F.IsUnspecified.Unspecified!.Value, new FluentExpected(false, "Value must not have an unspecified kind."))
        ];
    }

    public static class ExplicitKind
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Utc", F.HasExplicitKind.Utc!.Value, new FluentExpected(true)),
            new("Local", F.HasExplicitKind.Local!.Value, new FluentExpected(true)),
            new("Unspecified", F.HasExplicitKind.Unspecified!.Value, new FluentExpected(false, "Value must have an explicit kind."))
        ];
    }

    public static class NotExplicitKind
    {
        public static TheoryData<FluentCase<DateTime>> Cases =>
        [
            new("Unspecified", F.HasExplicitKind.Unspecified!.Value, new FluentExpected(true)),
            new("Utc", F.HasExplicitKind.Utc!.Value, new FluentExpected(false, "Value must not have an explicit kind."))
        ];
    }
}
