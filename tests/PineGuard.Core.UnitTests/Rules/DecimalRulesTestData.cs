using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DecimalRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DecimalRulesTestData
{
    public static class HasMaxScale
    {
        public static TheoryData<RuleCase<(decimal? value, int scale)>> Cases => F.HasMaxScale.AllScenarios.ToRuleCases();
    }

    public static class HasMaxPrecision
    {
        public static TheoryData<RuleCase<(decimal? value, int precision)>> Cases => F.HasMaxPrecision.AllScenarios.ToRuleCases();
    }

    public static class IsWithinPrecision
    {
        public static TheoryData<RuleCase<(decimal? value, int precision, int scale)>> Cases => F.IsWithinPrecision.AllScenarios.ToRuleCases();
    }
}
