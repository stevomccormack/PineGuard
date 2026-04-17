using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.BufferRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class BufferRulesTestData
{
    public static class IsHex
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsHex.AllScenarios.ToRuleCases();
    }

    public static class IsBase64
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsBase64.AllScenarios.ToRuleCases();
    }
}
