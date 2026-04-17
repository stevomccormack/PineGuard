using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.JsonRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentJsonExtensionsTestData
{
    public static class Json
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsJson.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsJson.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be JSON.")
        });
    }

    public static class JsonObject
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsJsonObject.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsJsonObject.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a JSON object.")
        });
    }

    public static class JsonArray
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsJsonArray.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsJsonArray.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a JSON array.")
        });
    }

    public static class JsonContentType
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.IsJsonContentType.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsJsonContentType.NullHeaders) => new FluentExpected(false, "Value must contain a JSON Content-Type."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain a JSON Content-Type.")
        });
    }
}
