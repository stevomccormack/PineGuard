using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeRulesTestData
{
    // Now as the pinned clock reports it, derived from FixedTimeProvider.Default rather than restated as a
    // literal. Because that instant is in the real past, "Future" is an instant the machine clock considers
    // past — so a rule that ignored the supplied provider would fail here.
    private static readonly DateTime PinnedNow = FixedTimeProvider.Default.GetUtcNow().UtcDateTime;

    private static readonly DateTime LeapDayBirth = new(2008, 02, 29, 0, 0, 0, DateTimeKind.Utc);

    public static class IsInPast
    {
        public static TheoryData<RuleCase<DateTime?>> Cases =>
        [
            new RuleCase<DateTime?>("Past", PinnedNow.AddDays(-2), new RuleExpected(true)),
            new RuleCase<DateTime?>("Future", PinnedNow.AddDays(2), new RuleExpected(false)),
            // Only an injected clock can put a value exactly on "now", so this boundary is untestable
            // against the machine clock: the default inclusion is exclusive, and the instant itself fails.
            new RuleCase<DateTime?>("ThisVeryInstant", PinnedNow, new RuleExpected(false)),
            new RuleCase<DateTime?>("NullValue", null, new RuleExpected(false))
        ];
    }

    public static class IsInPastSystemClock
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsPast.AllScenarios.ToRuleCases();
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<DateTime?>> Cases =>
        [
            new RuleCase<DateTime?>("Future", PinnedNow.AddDays(2), new RuleExpected(true)),
            new RuleCase<DateTime?>("Past", PinnedNow.AddDays(-2), new RuleExpected(false)),
            new RuleCase<DateTime?>("ThisVeryInstant", PinnedNow, new RuleExpected(false)),
            new RuleCase<DateTime?>("NullValue", null, new RuleExpected(false))
        ];
    }

    public static class IsInFutureSystemClock
    {
        public static TheoryData<RuleCase<DateTime?>> Cases =>
        [
            new RuleCase<DateTime?>(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate, new RuleExpected(true)),
            new RuleCase<DateTime?>(nameof(F.IsPast.PastDate), F.IsPast.PastDate, new RuleExpected(false)),
            new RuleCase<DateTime?>(nameof(F.IsPast.NullValue), F.IsPast.NullValue, new RuleExpected(false))
        ];
    }

    public static class IsWithinDaysFromNow
    {
        public static TheoryData<RuleCase<(DateTime? value, int days)>> Cases =>
        [
            new RuleCase<(DateTime? value, int days)>("WithinFuture", (PinnedNow.AddHours(12), 1), new RuleExpected(true)),
            new RuleCase<(DateTime? value, int days)>("WithinPast", (PinnedNow.AddHours(-12), 1), new RuleExpected(true)),
            new RuleCase<(DateTime? value, int days)>("OutsideWindow", (PinnedNow.AddDays(5), 1), new RuleExpected(false)),
            new RuleCase<(DateTime? value, int days)>("NullValue", (null, 1), new RuleExpected(false)),
            new RuleCase<(DateTime? value, int days)>("NegativeDays", (PinnedNow, -1), new RuleExpected(false)),
            // Regression pin for the ToUtc normalization: the value sits 5 minutes inside the 1-day
            // window once correctly converted from Local to UTC. Un-normalized (raw-ticks) comparison
            // adds the host's local UTC offset on top, which pushes the naive difference past 1 day
            // for any host whose local time zone is not exactly UTC+00:00. Derived from a UTC instant
            // via ToLocalTime() so it is correct (not flaky) on every host, regardless of local offset.
            new RuleCase<(DateTime? value, int days)>("LocalKindNearWindowBoundary", (PinnedNow.Add(TimeSpan.FromHours(24) - TimeSpan.FromMinutes(5)).ToLocalTime(), 1), new RuleExpected(true))
        ];
    }

    public static class IsWithinDaysFromNowSystemClock
    {
        // Windows chosen so the answer holds for any date the suite could realistically run on, which keeps
        // the system-clock path asserted without the test data reading the machine clock itself.
        public static TheoryData<RuleCase<(DateTime? value, int days)>> Cases =>
        [
            new RuleCase<(DateTime? value, int days)>("FarPastInsideAVeryWideWindow", (F.IsPast.PastDate, 200_000), new RuleExpected(true)),
            new RuleCase<(DateTime? value, int days)>("FarPastOutsideWindow", (F.IsPast.PastDate, 1), new RuleExpected(false)),
            new RuleCase<(DateTime? value, int days)>("FarFutureOutsideWindow", (F.IsPast.FutureDate, 1), new RuleExpected(false))
        ];
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime min, DateTime max, Inclusion inclusion)>> Cases => F.IsBetween.AllScenarios.ToRuleCases();
    }

    public static class IsBefore
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>> Cases => F.IsBefore.AllScenarios.ToRuleCases();
    }

    public static class IsBeforeDefaultInclusion
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other)>> Cases => F.IsBeforeDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsAfter
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>> Cases => F.IsAfter.AllScenarios.ToRuleCases();
    }

    public static class IsAfterDefaultInclusion
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other)>> Cases => F.IsAfterDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsSame
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other, DateTimePrecision? precision)>> Cases => F.IsSame.AllScenarios.ToRuleCases();
    }

    public static class IsChronological
    {
        public static TheoryData<RuleCase<(DateTime? start, DateTime? end, Inclusion inclusion)>> Cases => F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion)>> Cases => F.IsOverlapping.AllScenarios.ToRuleCases();
    }

    public static class IsWithin
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? reference, TimeSpan window)>> Cases => F.IsWithin.AllScenarios.ToRuleCases();
    }

    public static class IsWithinCalendarMonths
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? reference, int months)>> Cases => F.IsWithinCalendarMonths.AllScenarios.ToRuleCases();
    }

    public static class IsWeekday
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsWeekday.AllScenarios.ToRuleCases();
    }

    public static class IsWeekend
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsWeekend.AllScenarios.ToRuleCases();
    }

    public static class IsFirstDayOfMonth
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsFirstDayOfMonth.AllScenarios.ToRuleCases();
    }

    public static class IsLastDayOfMonth
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsLastDayOfMonth.AllScenarios.ToRuleCases();
    }

    public static class IsSameDay
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other)>> Cases => F.IsSameDay.AllScenarios.ToRuleCases();
    }

    public static class IsUtc
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsUtc.AllScenarios.ToRuleCases();
    }

    public static class IsLocal
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsLocal.AllScenarios.ToRuleCases();
    }

    public static class IsUnspecified
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsUnspecified.AllScenarios.ToRuleCases();
    }

    public static class HasExplicitKind
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.HasExplicitKind.AllScenarios.ToRuleCases();
    }

    public static class HasMinimumAge
    {
        public static TheoryData<RuleCase<(DateTime? value, int years)>> Cases => F.HasMinimumAge.AllScenarios.ToRuleCases();
    }

    public static class HasMinimumAgeSystemClock
    {
        public static TheoryData<RuleCase<(DateTime? value, int years)>> Cases =>
        [
            new RuleCase<(DateTime? value, int years)>("BornLongAgo", (new DateTime(1900, 01, 01, 0, 0, 0, DateTimeKind.Utc), 18), new RuleExpected(true)),
            new RuleCase<(DateTime? value, int years)>("BornFarInTheFuture", (new DateTime(2999, 01, 01, 0, 0, 0, DateTimeKind.Utc), 18), new RuleExpected(false))
        ];
    }

    public static class HasMinimumAgeOnLeapDay
    {
        // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
        // here the boundary moves and the birth date stays put, which the shared provider cannot express.
        public static TheoryData<RuleCase<(DateTime? value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new RuleCase<(DateTime? value, int years, DateTimeOffset utcNow)>("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new RuleExpected(false)),
            new RuleCase<(DateTime? value, int years, DateTimeOffset utcNow)>("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new RuleExpected(true)),
            new RuleCase<(DateTime? value, int years, DateTimeOffset utcNow)>("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new RuleExpected(true))
        ];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
