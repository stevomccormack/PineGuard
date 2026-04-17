using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesDateTimeOffsetTestData
{
    public static class IsInPast
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateTimeOffsetIsInPast.AllScenarios.ToRuleCases();
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateTimeOffsetIsInFuture.AllScenarios.ToRuleCases();
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> Cases => F.DateTimeOffsetIsBetween.AllScenarios.ToRuleCases();
    }
}
