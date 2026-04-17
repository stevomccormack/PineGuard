using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class XmlRulesTestData
{
    public static class IsXml
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsXml.AllScenarios.ToRuleCases();
    }

    public static class IsXmlContentType
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.IsXmlContentType.AllScenarios.ToRuleCases();
    }
}
