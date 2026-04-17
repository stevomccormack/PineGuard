using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.IdentifierRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class IdentifierRulesTestData
{
    public static class IsSlug
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsSlug.AllScenarios.ToRuleCases();
    }
}
