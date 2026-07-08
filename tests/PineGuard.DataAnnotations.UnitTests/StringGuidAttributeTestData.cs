using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringGuidAttributeTestData
{
    public static class GuidString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GuidIsGuid.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.GuidIsGuid.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid GUID.")
        });
    }
}
