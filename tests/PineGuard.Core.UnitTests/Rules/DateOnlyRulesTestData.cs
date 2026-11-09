using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateOnlyRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateOnlyRulesTestData
{
    // Today as the pinned clock reports it, derived from FixedTimeProvider.Default rather than
    // restated as a literal. Because that instant is in the real past, "Tomorrow" is a date the
    // machine clock considers past — so a rule that ignored the supplied provider would fail here.
    private static readonly DateOnly PinnedToday = DateOnly.FromDateTime(FixedTimeProvider.Default.GetUtcNow().UtcDateTime);

    private static readonly DateOnly LeapDayBirth = new(2008, 02, 29);

    // A zone exactly at the ±14h boundary DateTimeOffsetUtility.TryGetDateAtOffset enforces, paired with an
    // instant at the corresponding DateTimeOffset extreme, so the guarded reference projection fails on
    // tick range representability, not on the offset check.
    private static readonly TimeZoneInfo ProjectionFailsZone = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.Rules.DateOnly.MinusFourteen", TimeSpan.FromHours(-14), "PineGuard Test -14", "PineGuard Test -14");
    private static readonly TimeProvider ProjectionFailsProvider = new FixedTimeProvider(DateTimeOffset.MinValue, ProjectionFailsZone);
    private static readonly TimeZoneInfo ProjectionFailsMaxZone = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.Rules.DateOnly.PlusFourteen", TimeSpan.FromHours(14), "PineGuard Test +14", "PineGuard Test +14");
    private static readonly TimeProvider ProjectionFailsMaxProvider = new FixedTimeProvider(DateTimeOffset.MaxValue, ProjectionFailsMaxZone);

    public static class IsInPast
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases =>
        [
            new RuleCase<DateOnly?>("Yesterday", PinnedToday.AddDays(-1), new RuleExpected(true)),
            new RuleCase<DateOnly?>("Tomorrow", PinnedToday.AddDays(1), new RuleExpected(false)),
            new RuleCase<DateOnly?>("Today", PinnedToday, new RuleExpected(false)),
            new RuleCase<DateOnly?>("NullValue", null, new RuleExpected(false))
        ];
    }

    public static class IsInPastSystemClock
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases => F.IsPast.AllScenarios.ToRuleCases();
    }

    public static class IsInPastAcrossTimeZones
    {
        public static TheoryData<RuleCase<(DateOnly? value, TimeProvider timeProvider)>> Cases => F.IsInPastAcrossTimeZones.AllScenarios.ToRuleCases();
    }

    public static class IsInPastReferenceProjection
    {
        public static TheoryData<RuleCase<(DateOnly? value, TimeProvider timeProvider)>> Cases =>
        [
            new RuleCase<(DateOnly? value, TimeProvider timeProvider)>("MinValueUnderflow", (new DateOnly(2000, 01, 01), ProjectionFailsProvider), new RuleExpected(false)),
            new RuleCase<(DateOnly? value, TimeProvider timeProvider)>("MaxValueOverflow", (new DateOnly(2000, 01, 01), ProjectionFailsMaxProvider), new RuleExpected(false))
        ];
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases =>
        [
            new RuleCase<DateOnly?>("Tomorrow", PinnedToday.AddDays(1), new RuleExpected(true)),
            new RuleCase<DateOnly?>("Yesterday", PinnedToday.AddDays(-1), new RuleExpected(false)),
            new RuleCase<DateOnly?>("Today", PinnedToday, new RuleExpected(false)),
            new RuleCase<DateOnly?>("NullValue", null, new RuleExpected(false))
        ];
    }

    public static class IsInFutureSystemClock
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases =>
        [
            new RuleCase<DateOnly?>(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate, new RuleExpected(true)),
            new RuleCase<DateOnly?>(nameof(F.IsPast.PastDate), F.IsPast.PastDate, new RuleExpected(false)),
            new RuleCase<DateOnly?>(nameof(F.IsPast.NullValue), F.IsPast.NullValue, new RuleExpected(false))
        ];
    }

    public static class IsInFutureAcrossTimeZones
    {
        public static TheoryData<RuleCase<(DateOnly? value, TimeProvider timeProvider)>> Cases => F.IsInFutureAcrossTimeZones.AllScenarios.ToRuleCases();
    }

    public static class IsInFutureReferenceProjection
    {
        public static TheoryData<RuleCase<(DateOnly? value, TimeProvider timeProvider)>> Cases =>
        [
            new RuleCase<(DateOnly? value, TimeProvider timeProvider)>("MinValueUnderflow", (new DateOnly(2000, 01, 01), ProjectionFailsProvider), new RuleExpected(false)),
            new RuleCase<(DateOnly? value, TimeProvider timeProvider)>("MaxValueOverflow", (new DateOnly(2000, 01, 01), ProjectionFailsMaxProvider), new RuleExpected(false))
        ];
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion)>> Cases => F.IsBetween.AllScenarios.ToRuleCases();
    }

    public static class IsBefore
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>> Cases => F.IsBefore.AllScenarios.ToRuleCases();
    }

    public static class IsBeforeDefaultInclusion
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly? other)>> Cases => F.IsBeforeDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsAfter
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>> Cases => F.IsAfter.AllScenarios.ToRuleCases();
    }

    public static class IsAfterDefaultInclusion
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly? other)>> Cases => F.IsAfterDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsSame
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly? other, DatePrecision? precision)>> Cases => F.IsSame.AllScenarios.ToRuleCases();
    }

    public static class IsChronological
    {
        public static TheoryData<RuleCase<(DateOnly? start, DateOnly? end, Inclusion inclusion)>> Cases => F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion)>> Cases => F.IsOverlapping.AllScenarios.ToRuleCases();
    }

    public static class IsWithinCalendarMonths
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly? reference, int months)>> Cases => F.IsWithinCalendarMonths.AllScenarios.ToRuleCases();
    }

    public static class IsWithin
    {
        public static TheoryData<RuleCase<(DateOnly? value, DateOnly? reference, int days)>> Cases => F.IsWithin.AllScenarios.ToRuleCases();
    }

    public static class IsWeekday
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases => F.IsWeekday.AllScenarios.ToRuleCases();
    }

    public static class IsWeekend
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases => F.IsWeekend.AllScenarios.ToRuleCases();
    }

    public static class IsFirstDayOfMonth
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases => F.IsFirstDayOfMonth.AllScenarios.ToRuleCases();
    }

    public static class IsLastDayOfMonth
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases => F.IsLastDayOfMonth.AllScenarios.ToRuleCases();
    }

    public static class HasMinimumAge
    {
        public static TheoryData<RuleCase<(DateOnly? value, int years)>> Cases => F.HasMinimumAge.AllScenarios.ToRuleCases();
    }

    public static class HasMinimumAgeSystemClock
    {
        public static TheoryData<RuleCase<(DateOnly? value, int years)>> Cases =>
        [
            new RuleCase<(DateOnly? value, int years)>("BornLongAgo", (new DateOnly(1900, 01, 01), 18), new RuleExpected(true)),
            new RuleCase<(DateOnly? value, int years)>("BornFarInTheFuture", (new DateOnly(2999, 01, 01), 18), new RuleExpected(false))
        ];
    }

    public static class HasMinimumAgeAcrossTimeZones
    {
        public static TheoryData<RuleCase<(DateOnly? value, int years, TimeProvider timeProvider)>> Cases => F.HasMinimumAgeAcrossTimeZones.AllScenarios.ToRuleCases();
    }

    // Proves the guarded reference derivation's failure path: when TryGetTodayDate cannot project the
    // sampled instant, HasMinimumAge returns false rather than throwing or evaluating age.
    public static class HasMinimumAgeReferenceProjection
    {
        public static TheoryData<RuleCase<(DateOnly? value, int years, TimeProvider timeProvider)>> Cases =>
        [
            new RuleCase<(DateOnly? value, int years, TimeProvider timeProvider)>("MinValueUnderflow", (new DateOnly(2000, 01, 01), 18, ProjectionFailsProvider), new RuleExpected(false)),
            new RuleCase<(DateOnly? value, int years, TimeProvider timeProvider)>("MaxValueOverflow", (new DateOnly(2000, 01, 01), 18, ProjectionFailsMaxProvider), new RuleExpected(false))
        ];
    }

    public static class HasMinimumAgeOnLeapDay
    {
        // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
        // here the boundary moves and the birth date stays put, which the shared provider cannot express.
        public static TheoryData<RuleCase<(DateOnly? value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new RuleCase<(DateOnly? value, int years, DateTimeOffset utcNow)>("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new RuleExpected(false)),
            new RuleCase<(DateOnly? value, int years, DateTimeOffset utcNow)>("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new RuleExpected(true)),
            new RuleCase<(DateOnly? value, int years, DateTimeOffset utcNow)>("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new RuleExpected(true))
        ];
    }

    // Only one scenario is meaningful per predicate: proving exactly one GetUtcNow() read per call. Asserted
    // directly against CountingTimeProvider.UtcNowCallCount in the test method, not through AssertResult.
    public static class OneReadBehavior
    {
        public static TheoryData<string> Cases => ["SingleRead"];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
