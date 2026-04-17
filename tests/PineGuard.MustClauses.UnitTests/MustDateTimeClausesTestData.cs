using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.DateTimeRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDateTimeClausesTestData
{
    private static readonly DateTime Now = DateTime.UtcNow;
    private static readonly DateTime PastDate = Now.AddDays(-1);
    private static readonly DateTime FutureDate = Now.AddDays(1);

    public static class Past
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("past", F.IsPast.PastDate!.Value, true),
            new("future", F.IsPast.FutureDate!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateTime.MinValue is past", DateTime.MinValue, true),
            new("DateTime.MaxValue is future", DateTime.MaxValue, false)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class PastOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("past", F.IsPast.PastDate!.Value, true),
            new("future", F.IsPast.FutureDate!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateTime.MinValue is past or present", DateTime.MinValue, true),
            new("DateTime.MaxValue is future only", DateTime.MaxValue, false)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class Future
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("future", F.IsPast.FutureDate!.Value, true),
            new("past", F.IsPast.PastDate!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateTime.MinValue is not future", DateTime.MinValue, false),
            new("DateTime.MaxValue is future", DateTime.MaxValue, true)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class FutureOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("future", F.IsPast.FutureDate!.Value, true),
            new("past", F.IsPast.PastDate!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateTime.MinValue is not future or present", DateTime.MinValue, false),
            new("DateTime.MaxValue is future or present", DateTime.MaxValue, true)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class Between
    {
        private static readonly DateTime D1 = new(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D2 = new(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D3 = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("between", (Now, PastDate, FutureDate), true),
            new("not between", (PastDate.AddDays(-1), PastDate, FutureDate), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("at min boundary inclusive", (D1, D1, D3), true),
            new("at max boundary inclusive", (D3, D1, D3), true),
            new("midnight boundary", (D2, D1, D3), true)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime min, DateTime max) Value, bool Expected)
            : IsCase<(DateTime value, DateTime min, DateTime max)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime min, DateTime max) Value, bool Expected)
            : IsCase<(DateTime value, DateTime min, DateTime max)>(Name, Value, Expected);
    }

    public static class NotBetween
    {
        private static readonly DateTime D1 = new(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D2 = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D3 = new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D4 = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not between", (PastDate.AddDays(-1), PastDate, FutureDate), true),
            new("between", (Now, PastDate, FutureDate), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("before range is not between", (D3, D1, D2), true),
            new("after range is not between", (D4, D1, D2), true)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime min, DateTime max) Value, bool Expected)
            : IsCase<(DateTime value, DateTime min, DateTime max)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime min, DateTime max) Value, bool Expected)
            : IsCase<(DateTime value, DateTime min, DateTime max)>(Name, Value, Expected);
    }

    public static class Before
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("before", (PastDate, Now), true),
            new("after", (Now, PastDate), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue before MaxValue", (DateTime.MinValue, DateTime.MaxValue), true),
            new("MaxValue not before MinValue", (DateTime.MaxValue, DateTime.MinValue), false)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
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
            new("MinValue on or before MaxValue", (DateTime.MinValue, DateTime.MaxValue), true),
            new("MaxValue on or before MaxValue", (DateTime.MaxValue, DateTime.MaxValue), true)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
    }

    public static class After
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("after", (FutureDate, Now), true),
            new("before", (Now, FutureDate), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MaxValue after MinValue", (DateTime.MaxValue, DateTime.MinValue), true),
            new("MinValue not after MaxValue", (DateTime.MinValue, DateTime.MaxValue), false)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
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
            new("MaxValue on or after MinValue", (DateTime.MaxValue, DateTime.MinValue), true),
            new("MinValue on or after MinValue", (DateTime.MinValue, DateTime.MinValue), true)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
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
            new("MinValue same as MinValue", (DateTime.MinValue, DateTime.MinValue), true),
            new("MaxValue not same as MinValue", (DateTime.MaxValue, DateTime.MinValue), false)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
    }

    public static class NotSame
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not same", (Now, PastDate), true),
            new("same", (Now, Now), false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue not same as MaxValue", (DateTime.MinValue, DateTime.MaxValue), true),
            new("MinValue same as MinValue is same", (DateTime.MinValue, DateTime.MinValue), false)
        ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
    }

    public static class Chronological
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("chronological", (PastDate, FutureDate), true),
            new("not chronological", (FutureDate, PastDate), false),
            new("same", (Now, Now), false)
        ];

        public sealed record ValidCase(string Name, (DateTime start, DateTime end) Value, bool Expected)
            : IsCase<(DateTime start, DateTime end)>(Name, Value, Expected);
    }

    public static class Overlapping
    {
        private static readonly DateTime BaseTime = new(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
       [
           new("overlapping", (BaseTime, BaseTime.AddHours(2), BaseTime.AddHours(1), BaseTime.AddHours(3)), true),
            new("not overlapping", (BaseTime, BaseTime.AddHours(2), BaseTime.AddHours(3), BaseTime.AddHours(4)), false)
       ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("invalid range1", (BaseTime.AddHours(2), BaseTime, BaseTime.AddHours(1), BaseTime.AddHours(3)), false),
            new("invalid range2", (BaseTime, BaseTime.AddHours(2), BaseTime.AddHours(3), BaseTime.AddHours(1)), false)
        ];

        public sealed record ValidCase(string Name, (DateTime start1, DateTime end1, DateTime start2, DateTime end2) Value, bool Expected)
            : IsCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateTime start1, DateTime end1, DateTime start2, DateTime end2) Value, bool Expected)
            : IsCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)>(Name, Value, Expected);
    }

    public static class NotOverlapping
    {
        private static readonly DateTime BaseTime = new(2023, 1, 1, 10, 0, 0, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
       [
           new("not overlapping", (BaseTime, BaseTime.AddHours(2), BaseTime.AddHours(3), BaseTime.AddHours(4)), true),
            new("overlapping", (BaseTime, BaseTime.AddHours(2), BaseTime.AddHours(1), BaseTime.AddHours(3)), false)
       ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("invalid range1", (BaseTime.AddHours(2), BaseTime, BaseTime.AddHours(1), BaseTime.AddHours(3)), true),
            new("invalid range2", (BaseTime, BaseTime.AddHours(2), BaseTime.AddHours(3), BaseTime.AddHours(1)), true)
        ];

        public sealed record ValidCase(string Name, (DateTime start1, DateTime end1, DateTime start2, DateTime end2) Value, bool Expected)
            : IsCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)>(Name, Value, Expected);

        public sealed record EdgeCase(string Name, (DateTime start1, DateTime end1, DateTime start2, DateTime end2) Value, bool Expected)
            : IsCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)>(Name, Value, Expected);
    }

    public static class WithinDaysFromNow
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("within", (DateTime.UtcNow.AddDays(1), 2), true),
             new("not within", (DateTime.UtcNow.AddDays(5), 2), false)
        ];

        public sealed record ValidCase(string Name, (DateTime value, int days) Value, bool Expected)
            : IsCase<(DateTime value, int days)>(Name, Value, Expected);
    }

    public static class NotWithinDaysFromNow
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
             new("not within", (DateTime.UtcNow.AddDays(5), 2), true),
             new("within", (DateTime.UtcNow.AddDays(1), 2), false)
        ];

        public sealed record ValidCase(string Name, (DateTime value, int days) Value, bool Expected)
            : IsCase<(DateTime value, int days)>(Name, Value, Expected);
    }

    public static class Within
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("within", (Now.AddMinutes(5), Now, TimeSpan.FromMinutes(10)), true),
            new("not within", (Now.AddMinutes(20), Now, TimeSpan.FromMinutes(10)), false)
       ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime reference, TimeSpan window) Value, bool Expected)
            : IsCase<(DateTime value, DateTime reference, TimeSpan window)>(Name, Value, Expected);
    }

    public static class NotWithin
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("not within", (Now.AddMinutes(20), Now, TimeSpan.FromMinutes(10)), true),
            new("within", (Now.AddMinutes(5), Now, TimeSpan.FromMinutes(10)), false)
       ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime reference, TimeSpan window) Value, bool Expected)
            : IsCase<(DateTime value, DateTime reference, TimeSpan window)>(Name, Value, Expected);
    }

    public static class WithinCalendarMonths
    {
        private static readonly DateTime D1 = new(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D2 = new(2023, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D3 = new(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
       [
           new("same month", (D2, D1, 0), true),
            new("within", (D3, D1, 3), true),
            new("not within", (D3, D1, 1), false)
       ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime reference, int months) Value, bool Expected)
            : IsCase<(DateTime value, DateTime reference, int months)>(Name, Value, Expected);
    }

    public static class NotWithinCalendarMonths
    {
        private static readonly DateTime D1 = new(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime D3 = new(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
       [
           new("not within", (D3, D1, 1), true),
            new("within", (D3, D1, 3), false)
       ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime reference, int months) Value, bool Expected)
            : IsCase<(DateTime value, DateTime reference, int months)>(Name, Value, Expected);
    }

    public static class Weekday
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("weekday", F.IsWeekday.Monday!.Value, true),
            new("weekend", F.IsWeekday.Saturday!.Value, false)
       ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class Weekend
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("weekend", F.IsWeekend.Saturday!.Value, true),
            new("weekday", F.IsWeekend.Monday!.Value, false)
       ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class FirstDayOfMonth
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("first day", F.IsFirstDayOfMonth.FirstDay!.Value, true),
            new("second day", F.IsFirstDayOfMonth.NotFirst!.Value, false)
       ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class NotFirstDayOfMonth
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("second day", F.IsFirstDayOfMonth.NotFirst!.Value, true),
            new("first day", F.IsFirstDayOfMonth.FirstDay!.Value, false)
       ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class LastDayOfMonth
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("last day", F.IsLastDayOfMonth.LastDay!.Value, true),
            new("second last", F.IsLastDayOfMonth.NotLast!.Value, false)
       ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class NotLastDayOfMonth
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("second last", F.IsLastDayOfMonth.NotLast!.Value, true),
            new("last day", F.IsLastDayOfMonth.LastDay!.Value, false)
       ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class SameDay
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("same day", (F.IsSameDay.SameDay.value!.Value, F.IsSameDay.SameDay.other!.Value), true),
            new("different day", (F.IsSameDay.DifferentDay.value!.Value, F.IsSameDay.DifferentDay.other!.Value), false)
       ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
    }

    public static class NotSameDay
    {
        public static TheoryData<ValidCase> ValidCases =>
       [
           new("different day", (F.IsSameDay.DifferentDay.value!.Value, F.IsSameDay.DifferentDay.other!.Value), true),
            new("same day", (F.IsSameDay.SameDay.value!.Value, F.IsSameDay.SameDay.other!.Value), false)
       ];

        public sealed record ValidCase(string Name, (DateTime value, DateTime other) Value, bool Expected)
            : IsCase<(DateTime value, DateTime other)>(Name, Value, Expected);
    }

    public static class Utc
    {
        private static readonly DateTime UtcMinValue = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc", F.IsUtc.Utc!.Value, true),
            new("local", F.IsUtc.Local!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue with UTC kind", UtcMinValue, true),
            new("MinValue with Unspecified kind", DateTime.MinValue, false)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class NotUtc
    {
        private static readonly DateTime LocalMinValue = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Local);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("local", F.IsUtc.Local!.Value, true),
            new("utc", F.IsUtc.Utc!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue with Local kind is not UTC", LocalMinValue, true),
            new("MinValue with Unspecified kind is not UTC", DateTime.MinValue, true)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class Local
    {
        private static readonly DateTime LocalMinValue = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Local);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("local", F.IsLocal.Local!.Value, true),
            new("utc", F.IsLocal.Utc!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue with Local kind", LocalMinValue, true),
            new("MinValue with Unspecified kind is not local", DateTime.MinValue, false)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class NotLocal
    {
        private static readonly DateTime UtcMinValue = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc", F.IsLocal.Utc!.Value, true),
            new("local", F.IsLocal.Local!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue with UTC kind is not local", UtcMinValue, true),
            new("MinValue with Unspecified kind is not local", DateTime.MinValue, true)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class Unspecified
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("unspecified", F.IsUnspecified.Unspecified!.Value, true),
            new("utc", F.IsUnspecified.Utc!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateTime.MinValue has Unspecified kind", DateTime.MinValue, true),
            new("DateTime.MaxValue has Unspecified kind", DateTime.MaxValue, true)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class NotUnspecified
    {
        private static readonly DateTime UtcMinValue = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc", F.IsUnspecified.Utc!.Value, true),
            new("unspecified", F.IsUnspecified.Unspecified!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue with UTC kind is not unspecified", UtcMinValue, true),
            new("DateTime.MinValue is unspecified", DateTime.MinValue, false)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class ExplicitKind
    {
        private static readonly DateTime UtcMinValue = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc", F.HasExplicitKind.Utc!.Value, true),
            new("unspecified", F.HasExplicitKind.Unspecified!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("MinValue with UTC kind has explicit kind", UtcMinValue, true),
            new("DateTime.MinValue has no explicit kind", DateTime.MinValue, false)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }

    public static class NotExplicitKind
    {
        private static readonly DateTime LocalMinValue = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Local);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("unspecified", F.HasExplicitKind.Unspecified!.Value, true),
            new("utc", F.HasExplicitKind.Utc!.Value, false)
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("DateTime.MinValue has no explicit kind", DateTime.MinValue, true),
            new("MinValue with Local kind has explicit kind", LocalMinValue, false)
        ];

        public sealed record ValidCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
        public sealed record EdgeCase(string Name, DateTime Value, bool Expected) : IsCase<DateTime>(Name, Value, Expected);
    }
}
