using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TokenRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class TokenRulesTestData
{
    public static class IsJwt
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsJwt.AllScenarios.ToRuleCases();
    }
}
