using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.JsonRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class JsonAttributesTestData
{
    public static class Json
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsJson.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsJson.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be JSON.", Code: MustCodes.Json.Document.Invalid)
        });
    }

    public static class JsonObject
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsJsonObject.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsJsonObject.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a JSON object.")
        });
    }

    public static class JsonArray
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsJsonArray.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsJsonArray.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a JSON array.")
        });
    }
}
