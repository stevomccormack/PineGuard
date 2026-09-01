using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDateTimeOffsetClausesTestData
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;
    private static readonly DateTimeOffset PastOffset = Now.AddDays(-1);
    private static readonly DateTimeOffset FutureOffset = Now.AddDays(1);

    public static class Past
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("past", F.IsPast.PastDate!.Value, true),
            new("future", F.IsPast.FutureDate!.Value, false)
        ];

        public sealed record ValidCase(string Name, DateTimeOffset Value, bool Expected) : IsCase<DateTimeOffset>(Name, Value, Expected);
    }

    public static class PastOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("past", F.IsPast.PastDate!.Value, true),
            new("present", Now, true),
            new("future", F.IsPast.FutureDate!.Value, false)
        ];

        public sealed record ValidCase(string Name, DateTimeOffset Value, bool Expected) : IsCase<DateTimeOffset>(Name, Value, Expected);
    }

    public static class Future
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("future", F.IsPast.FutureDate!.Value, true),
            new("past", F.IsPast.PastDate!.Value, false)
        ];

        public sealed record ValidCase(string Name, DateTimeOffset Value, bool Expected) : IsCase<DateTimeOffset>(Name, Value, Expected);
    }

    public static class FutureOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("future", F.IsPast.FutureDate!.Value, true),
            new("past", F.IsPast.PastDate!.Value, false)
        ];

        public sealed record ValidCase(string Name, DateTimeOffset Value, bool Expected) : IsCase<DateTimeOffset>(Name, Value, Expected);
    }

    public static class Between
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("between", (Now, PastOffset, FutureOffset), true),
            new("not between", (PastOffset.AddDays(-1), PastOffset, FutureOffset), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Min, DateTimeOffset Max) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Min, DateTimeOffset Max)>(Name, Value, Expected);
    }

    public static class NotBetween
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not between (lower)", (PastOffset.AddDays(-1), PastOffset, FutureOffset), true),
            new("not between (higher)", (FutureOffset.AddDays(1), PastOffset, FutureOffset), true),
            new("between", (Now, PastOffset, FutureOffset), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Min, DateTimeOffset Max) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Min, DateTimeOffset Max)>(Name, Value, Expected);
    }

    public static class Before
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("before", (PastOffset, Now), true),
            new("on (exclusive)", (Now, Now), false),
            new("after", (FutureOffset, Now), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Target) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Target)>(Name, Value, Expected);
    }

    public static class OnOrBefore
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("before", (PastOffset, Now), true),
            new("on (inclusive)", (Now, Now), true),
            new("after", (FutureOffset, Now), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Target) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Target)>(Name, Value, Expected);
    }

    public static class After
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("after", (FutureOffset, Now), true),
            new("on (exclusive)", (Now, Now), false),
            new("before", (PastOffset, Now), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Target) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Target)>(Name, Value, Expected);
    }

    public static class OnOrAfter
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("after", (FutureOffset, Now), true),
            new("on (inclusive)", (Now, Now), true),
            new("before", (PastOffset, Now), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Target) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Target)>(Name, Value, Expected);
    }

    public static class Same
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("same", (Now, Now), true),
            new("not same", (Now, PastOffset), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Target) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Target)>(Name, Value, Expected);
    }

    public static class NotSame
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not same", (Now, PastOffset), true),
            new("same", (Now, Now), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Target) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Target)>(Name, Value, Expected);
    }

    public static class Chronological
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("chronological", (PastOffset, FutureOffset), true),
            new("same (exclusive)", (Now, Now), false),
            new("reverse", (FutureOffset, PastOffset), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Start, DateTimeOffset End) Value, bool Expected) : IsCase<(DateTimeOffset Start, DateTimeOffset End)>(Name, Value, Expected);
    }

    public static class Overlapping
    {
        private static readonly DateTimeOffset S1 = Now;
        private static readonly DateTimeOffset E1 = Now.AddDays(2);
        private static readonly DateTimeOffset S2 = Now.AddDays(1);
        private static readonly DateTimeOffset E2 = Now.AddDays(3);
        private static readonly DateTimeOffset NoOverlapStart = Now.AddDays(10);
        private static readonly DateTimeOffset NoOverlapEnd = Now.AddDays(12);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("overlapping", (S1, E1, S2, E2), true),
            new("no overlap", (S1, E1, NoOverlapStart, NoOverlapEnd), false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid range 1", (E1, S1, S2, E2), false), // Start1 > End1
            new("invalid range 2", (S1, E1, E2, S2), false) // Start2 > End2
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Start1, DateTimeOffset End1, DateTimeOffset Start2, DateTimeOffset End2) Value, bool Expected) : IsCase<(DateTimeOffset S1, DateTimeOffset E1, DateTimeOffset S2, DateTimeOffset E2)>(Name, Value, Expected);
    }

    public static class NotOverlapping
    {
        private static readonly DateTimeOffset S1 = Now;
        private static readonly DateTimeOffset E1 = Now.AddDays(2);
        private static readonly DateTimeOffset S2 = Now.AddDays(1);
        private static readonly DateTimeOffset E2 = Now.AddDays(3);
        private static readonly DateTimeOffset NoOverlapStart = Now.AddDays(10);
        private static readonly DateTimeOffset NoOverlapEnd = Now.AddDays(12);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("no overlap", (S1, E1, NoOverlapStart, NoOverlapEnd), true),
            new("overlapping", (S1, E1, S2, E2), false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid range 1", (E1, S1, S2, E2), true), // Start1 > End1 -> !false -> true
            new("invalid range 2", (S1, E1, E2, S2), true) // Start2 > End2 -> !false -> true
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Start1, DateTimeOffset End1, DateTimeOffset Start2, DateTimeOffset End2) Value, bool Expected) : IsCase<(DateTimeOffset S1, DateTimeOffset E1, DateTimeOffset S2, DateTimeOffset E2)>(Name, Value, Expected);
    }

    public static class Within
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("within", (Now, Now.AddHours(1), TimeSpan.FromHours(2)), true),
            new("not within", (Now, Now.AddHours(5), TimeSpan.FromHours(2)), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Ref, TimeSpan Window) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Ref, TimeSpan Window)>(Name, Value, Expected);
    }

    public static class NotWithin
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not within", (Now, Now.AddHours(5), TimeSpan.FromHours(2)), true),
            new("within", (Now, Now.AddHours(1), TimeSpan.FromHours(2)), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Ref, TimeSpan Window) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Ref, TimeSpan Window)>(Name, Value, Expected);
    }

    public static class WithinCalendarMonths
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("within same month", (Now, Now, 1), true),
            new("not within", (Now, Now.AddMonths(2), 1), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Ref, int Months) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Ref, int Months)>(Name, Value, Expected);
    }

    public static class NotWithinCalendarMonths
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("not within", (Now, Now.AddMonths(2), 1), true),
            new("within", (Now, Now, 1), false)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset Value, DateTimeOffset Ref, int Months) Value, bool Expected) : IsCase<(DateTimeOffset Value, DateTimeOffset Ref, int Months)>(Name, Value, Expected);
    }

    // The calendar clauses take a non-nullable DateTimeOffset, so each group drops the fixture's NullValue scenario.
    public static class Weekday
    {
        public static TheoryData<MustCase<DateTimeOffset>> ValidCases => F.IsWeekday.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateTimeOffset>> InvalidCases => F.IsWeekday.InvalidScenarios.Except(nameof(F.IsWeekday.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be a weekday.", "value", MustCodes.Date.Calendar.NotWeekday));
    }

    public static class Weekend
    {
        public static TheoryData<MustCase<DateTimeOffset>> ValidCases => F.IsWeekend.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateTimeOffset>> InvalidCases => F.IsWeekend.InvalidScenarios.Except(nameof(F.IsWeekend.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be a weekend day.", "value", MustCodes.Date.Calendar.NotWeekend));
    }

    public static class FirstDayOfMonth
    {
        public static TheoryData<MustCase<DateTimeOffset>> ValidCases => F.IsFirstDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateTimeOffset>> InvalidCases => F.IsFirstDayOfMonth.InvalidScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be the first day of the month.", "value", MustCodes.Date.Calendar.NotFirstDayOfMonth));
    }

    public static class NotFirstDayOfMonth
    {
        public static TheoryData<MustCase<DateTimeOffset>> ValidCases => F.IsFirstDayOfMonth.InvalidScenarios.Except(nameof(F.IsFirstDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<DateTimeOffset>> InvalidCases => F.IsFirstDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be the first day of the month.", "value", MustCodes.Date.Calendar.FirstDayOfMonth));
    }

    public static class LastDayOfMonth
    {
        public static TheoryData<MustCase<DateTimeOffset>> ValidCases => F.IsLastDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<DateTimeOffset>> InvalidCases => F.IsLastDayOfMonth.InvalidScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must be the last day of the month.", "value", MustCodes.Date.Calendar.NotLastDayOfMonth));
    }

    public static class NotLastDayOfMonth
    {
        public static TheoryData<MustCase<DateTimeOffset>> ValidCases => F.IsLastDayOfMonth.InvalidScenarios.Except(nameof(F.IsLastDayOfMonth.NullValue)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<DateTimeOffset>> InvalidCases => F.IsLastDayOfMonth.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, "value must not be the last day of the month.", "value", MustCodes.Date.Calendar.LastDayOfMonth));
    }
}
