using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class GuidRulesTestData
{
    public static class IsEmpty
    {
        public static TheoryData<RuleCase<Guid?>> Cases => F.IsEmpty.AllScenarios.ToRuleCases();
    }

    public static class IsNotEmpty
    {
        public static TheoryData<RuleCase<Guid?>> Cases => F.IsNotEmpty.AllScenarios.ToRuleCases();
    }

    public static class HasVersion
    {
        public static TheoryData<RuleCase<(Guid? value, int version)>> Cases => F.HasVersion.AllScenarios.ToRuleCases();
    }
}
