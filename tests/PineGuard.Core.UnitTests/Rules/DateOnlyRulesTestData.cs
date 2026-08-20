using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateOnlyRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateOnlyRulesTestData
{
    public static class IsInPast
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return
                [
                    new RuleCase<DateOnly?>("Past", today.AddDays(-2), new RuleExpected(true)),
                    new RuleCase<DateOnly?>("Future", today.AddDays(2), new RuleExpected(false)),
                    new RuleCase<DateOnly?>("Today", today, new RuleExpected(false)),
                    new RuleCase<DateOnly?>("NullValue", null, new RuleExpected(false))
                ];
            }
        }
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return
                [
                    new RuleCase<DateOnly?>("Future", today.AddDays(2), new RuleExpected(true)),
                    new RuleCase<DateOnly?>("Past", today.AddDays(-2), new RuleExpected(false)),
                    new RuleCase<DateOnly?>("Today", today, new RuleExpected(false)),
                    new RuleCase<DateOnly?>("NullValue", null, new RuleExpected(false))
                ];
            }
        }
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
}
