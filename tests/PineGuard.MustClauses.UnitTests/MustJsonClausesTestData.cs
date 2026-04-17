using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.JsonRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustJsonClausesTestData
{
    public static class Json
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsJson.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsJson.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsJson.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be JSON.")
        });
    }

    public static class JsonObject
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsJsonObject.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsJsonObject.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsJsonObject.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a JSON object.")
        });
    }

    public static class JsonArray
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsJsonArray.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsJsonArray.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsJsonArray.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a JSON array.")
        });
    }

    public static class JsonContentType
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.IsJsonContentType.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.IsJsonContentType.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must contain a JSON Content-Type."));
    }
}
