using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CronRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class CronRulesTestData
{
    public static class IsCronExpression
    {
        public static TheoryData<RuleCase<(string? value, CronFormat format)>> Cases => F.IsCronExpression.AllScenarios.ToRuleCases();
    }
}
