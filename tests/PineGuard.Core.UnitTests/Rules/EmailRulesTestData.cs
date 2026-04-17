using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.EmailRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class EmailRulesTestData
{
    public static class IsEmail
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsEmail.AllScenarios.ToRuleCases();
    }

    public static class IsStrictEmail
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsStrictEmail.AllScenarios.ToRuleCases();
    }

    public static class HasAlias
    {
        public static TheoryData<RuleCase<string?>> Cases => F.HasAlias.AllScenarios.ToRuleCases();
    }
}
