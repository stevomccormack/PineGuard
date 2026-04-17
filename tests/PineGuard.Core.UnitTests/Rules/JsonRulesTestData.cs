using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.JsonRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class JsonRulesTestData
{
    public static class IsJson
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsJson.AllScenarios.ToRuleCases();
    }

    public static class IsJsonObject
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsJsonObject.AllScenarios.ToRuleCases();
    }

    public static class IsJsonArray
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsJsonArray.AllScenarios.ToRuleCases();
    }

    public static class IsJsonContentType
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.IsJsonContentType.AllScenarios.ToRuleCases();
    }
}
