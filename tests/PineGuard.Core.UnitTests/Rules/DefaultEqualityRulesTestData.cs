using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DefaultEqualityRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DefaultEqualityRulesTestData
{
    public static class IsDefaultInt32
    {
        public static TheoryData<RuleCase<int>> Cases => F.IsDefaultInt32.AllScenarios.ToRuleCases();
    }

    public static class IsDefaultNullableInt32
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsDefaultNullableInt32.AllScenarios.ToRuleCases();
    }

    public static class IsDefaultString
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsDefaultString.AllScenarios.ToRuleCases();
    }

    public static class IsNullOrDefaultInt32
    {
        public static TheoryData<RuleCase<int>> Cases => F.IsNullOrDefaultInt32.AllScenarios.ToRuleCases();
    }

    public static class IsNullOrDefaultNullableInt32
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsNullOrDefaultNullableInt32.AllScenarios.ToRuleCases();
    }

    public static class IsNullOrDefaultString
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsNullOrDefaultString.AllScenarios.ToRuleCases();
    }
}
