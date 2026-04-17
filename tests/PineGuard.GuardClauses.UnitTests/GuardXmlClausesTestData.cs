using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.XmlRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardXmlClausesTestData
{
    public static class NotXml
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsXml.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsXml.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotXmlDocument
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsXml.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsXml.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotXmlContentType
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.IsXmlContentType.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.IsXmlContentType.InvalidScenarios.ToGuardCases("headers");
    }
}
