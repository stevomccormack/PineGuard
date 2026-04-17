using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeRulesTestData
{
    public static class IsInPast
    {
        public static TheoryData<RuleCase<DateTime?>> Cases
        {
            get
            {
                var now = DateTime.UtcNow;
                return
                [
                    new RuleCase<DateTime?>("Past", now.AddDays(-2), new RuleExpected(true)),
                    new RuleCase<DateTime?>("Future", now.AddDays(2), new RuleExpected(false)),
                    new RuleCase<DateTime?>("NullValue", null, new RuleExpected(false))
                ];
            }
        }
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<DateTime?>> Cases
        {
            get
            {
                var now = DateTime.UtcNow;
                return
                [
                    new RuleCase<DateTime?>("Future", now.AddDays(2), new RuleExpected(true)),
                    new RuleCase<DateTime?>("Past", now.AddDays(-2), new RuleExpected(false)),
                    new RuleCase<DateTime?>("NullValue", null, new RuleExpected(false))
                ];
            }
        }
    }

    public static class IsWithinDaysFromNow
    {
        public static TheoryData<RuleCase<(DateTime? value, int days)>> Cases
        {
            get
            {
                var now = DateTime.UtcNow;
                return
                [
                    new RuleCase<(DateTime? value, int days)>("WithinFuture", (now.AddHours(12), 1), new RuleExpected(true)),
                    new RuleCase<(DateTime? value, int days)>("WithinPast", (now.AddHours(-12), 1), new RuleExpected(true)),
                    new RuleCase<(DateTime? value, int days)>("OutsideWindow", (now.AddDays(5), 1), new RuleExpected(false)),
                    new RuleCase<(DateTime? value, int days)>("NullValue", (null, 1), new RuleExpected(false)),
                    new RuleCase<(DateTime? value, int days)>("NegativeDays", (now, -1), new RuleExpected(false))
                ];
            }
        }
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime min, DateTime max, Inclusion inclusion)>> Cases => F.IsBetween.AllScenarios.ToRuleCases();
    }

    public static class IsBefore
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>> Cases => F.IsBefore.AllScenarios.ToRuleCases();
    }

    public static class IsAfter
    {
        public static TheoryData<RuleCase<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>> Cases => F.IsAfter.AllScenarios.ToRuleCases();
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
}
