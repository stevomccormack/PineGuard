using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.JsonRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardJsonClausesTestData
{
    public static class NotJson
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsJson.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsJson.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotJsonObject
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsJsonObject.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsJsonObject.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotJsonArray
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsJsonArray.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsJsonArray.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotJsonContentType
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.IsJsonContentType.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.IsJsonContentType.InvalidScenarios.ToGuardCases("value");
    }
}
