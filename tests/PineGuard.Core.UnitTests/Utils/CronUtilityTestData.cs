using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CronRulesFixtures;

namespace PineGuard.Core.UnitTests.Utils;

public static class CronUtilityTestData
{
    public static class TryParse
    {
        public static TheoryData<RuleCase<(string? value, CronFormat format, string[]? fields)>> Cases => F.TryParse.AllScenarios.ToRuleCases();
    }
}
