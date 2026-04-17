using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.PhoneRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class PhoneRulesTestData
{
    public static class IsPhoneNumber
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsPhoneNumber.AllScenarios.ToRuleCases();
    }
}
