using System.Globalization;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesDateOnlyTestData
{
    public static class IsInPast
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateOnlyIsInPast.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<string?>> DynamicCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return
                [
                    new RuleCase<string?>("TodayIsNotInPast", today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), new RuleExpected(false)),
                    new RuleCase<string?>("YesterdayIsInPast", today.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), new RuleExpected(true))
                ];
            }
        }
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateOnlyIsInFuture.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<string?>> DynamicCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return
                [
                    new RuleCase<string?>("TodayIsNotInFuture", today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), new RuleExpected(false)),
                    new RuleCase<string?>("TomorrowIsInFuture", today.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), new RuleExpected(true))
                ];
            }
        }
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>> Cases => F.DateOnlyIsBetween.AllScenarios.ToRuleCases();
    }
}
