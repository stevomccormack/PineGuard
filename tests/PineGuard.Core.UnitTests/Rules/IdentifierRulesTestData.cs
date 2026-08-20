using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.IdentifierRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class IdentifierRulesTestData
{
    public static class IsSlug
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsSlug.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<string?>> AdHocCases =>
        [
            new("Digits and letters", "page-2", new RuleExpected(true)),
            new("Digits only", "2024", new RuleExpected(true)),
            new("Leading digit", "2-page", new RuleExpected(true)),
            new("Underscore rejected", "page_2", new RuleExpected(false)),
            new("Period rejected", "page.2", new RuleExpected(false))
        ];
    }
}
