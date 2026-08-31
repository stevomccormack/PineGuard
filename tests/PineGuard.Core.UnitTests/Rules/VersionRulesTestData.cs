using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.VersionRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class VersionRulesTestData
{
    public static class IsSemVer
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsSemVer.AllScenarios.ToRuleCases();
    }
}
