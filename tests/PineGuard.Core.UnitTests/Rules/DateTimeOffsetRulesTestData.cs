using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeOffsetRulesTestData
{
    // Now as the pinned clock reports it, derived from FixedTimeProvider.Default rather than restated as a
    // literal. Because that instant is in the real past, "Future" is an instant the machine clock considers
    // past — so a rule that ignored the supplied provider would fail here.
    private static readonly DateTimeOffset PinnedNow = FixedTimeProvider.Default.GetUtcNow();

    public static class IsInPast
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases =>
        [
            new RuleCase<DateTimeOffset?>("Past", PinnedNow.AddDays(-2), new RuleExpected(true)),
            new RuleCase<DateTimeOffset?>("Future", PinnedNow.AddDays(2), new RuleExpected(false)),
            // Only an injected clock can put a value exactly on "now", so this boundary is untestable
            // against the machine clock: the default inclusion is exclusive, and the instant itself fails.
            new RuleCase<DateTimeOffset?>("ThisVeryInstant", PinnedNow, new RuleExpected(false)),
            new RuleCase<DateTimeOffset?>("NullValue", null, new RuleExpected(false))
        ];
    }

    public static class IsInPastSystemClock
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases => F.IsPast.AllScenarios.ToRuleCases();
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases =>
        [
            new RuleCase<DateTimeOffset?>("Future", PinnedNow.AddDays(2), new RuleExpected(true)),
            new RuleCase<DateTimeOffset?>("Past", PinnedNow.AddDays(-2), new RuleExpected(false)),
            new RuleCase<DateTimeOffset?>("ThisVeryInstant", PinnedNow, new RuleExpected(false)),
            new RuleCase<DateTimeOffset?>("NullValue", null, new RuleExpected(false))
        ];
    }

    public static class IsInFutureSystemClock
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases =>
        [
            new RuleCase<DateTimeOffset?>(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate, new RuleExpected(true)),
            new RuleCase<DateTimeOffset?>(nameof(F.IsPast.PastDate), F.IsPast.PastDate, new RuleExpected(false)),
            new RuleCase<DateTimeOffset?>(nameof(F.IsPast.NullValue), F.IsPast.NullValue, new RuleExpected(false))
        ];
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> Cases => F.IsBetween.AllScenarios.ToRuleCases();
    }

    public static class IsBefore
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>> Cases => F.IsBefore.AllScenarios.ToRuleCases();
    }

    public static class IsBeforeDefaultInclusion
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset? other)>> Cases => F.IsBeforeDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsAfter
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>> Cases => F.IsAfter.AllScenarios.ToRuleCases();
    }

    public static class IsAfterDefaultInclusion
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset? other)>> Cases => F.IsAfterDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsSame
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision)>> Cases => F.IsSame.AllScenarios.ToRuleCases();
    }

    public static class IsChronological
    {
        public static TheoryData<RuleCase<(DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion)>> Cases => F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion)>> Cases => F.IsOverlapping.AllScenarios.ToRuleCases();
    }

    public static class IsWithin
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window)>> Cases => F.IsWithin.AllScenarios.ToRuleCases();
    }

    public static class IsWithinCalendarMonths
    {
        public static TheoryData<RuleCase<(DateTimeOffset? value, DateTimeOffset? reference, int months)>> Cases => F.IsWithinCalendarMonths.AllScenarios.ToRuleCases();
    }

    public static class IsWeekday
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases => F.IsWeekday.AllScenarios.ToRuleCases();
    }

    public static class IsWeekend
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases => F.IsWeekend.AllScenarios.ToRuleCases();
    }

    public static class IsFirstDayOfMonth
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases => F.IsFirstDayOfMonth.AllScenarios.ToRuleCases();
    }

    public static class IsLastDayOfMonth
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases => F.IsLastDayOfMonth.AllScenarios.ToRuleCases();
    }
}
