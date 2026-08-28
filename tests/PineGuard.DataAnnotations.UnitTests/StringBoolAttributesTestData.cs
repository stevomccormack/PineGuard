using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringBoolAttributesTestData
{
    // TrueStringAttribute
    public static class TrueString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.BoolIsTrue.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.BoolIsTrue.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be true.", Code: MustCodes.Boolean.Value.False)
        });
    }

    // FalseStringAttribute
    public static class FalseString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.BoolIsFalse.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.BoolIsFalse.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be false.", Code: MustCodes.Boolean.Value.True)
        });
    }
}
