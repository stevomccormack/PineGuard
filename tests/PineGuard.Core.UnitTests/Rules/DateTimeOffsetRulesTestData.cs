using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeOffsetRulesTestData
{
    public static class IsInPast
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases
        {
            get
            {
                var now = DateTimeOffset.UtcNow;
                return
                [
                    new RuleCase<DateTimeOffset?>("Past", now.AddDays(-2), new RuleExpected(true)),
                    new RuleCase<DateTimeOffset?>("Future", now.AddDays(2), new RuleExpected(false)),
                    new RuleCase<DateTimeOffset?>("NullValue", null, new RuleExpected(false))
                ];
            }
        }
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases
        {
            get
            {
                var now = DateTimeOffset.UtcNow;
                return
                [
                    new RuleCase<DateTimeOffset?>("Future", now.AddDays(2), new RuleExpected(true)),
                    new RuleCase<DateTimeOffset?>("Past", now.AddDays(-2), new RuleExpected(false)),
                    new RuleCase<DateTimeOffset?>("NullValue", null, new RuleExpected(false))
                ];
            }
        }
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
