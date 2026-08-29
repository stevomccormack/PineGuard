using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.ChecksumRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class ChecksumRulesTestData
{
    public static class IsLuhn
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsLuhn.AllScenarios.ToRuleCases();
    }
}
