using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringBoolExtensionsTestData
{
    // FluentStringBoolExtensions.True — validates string? property
    public static class True
    {
        public static TheoryData<FluentCase<string?>> Cases => F.BoolIsTrue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.BoolIsTrue.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be true.", Code: MustCodes.Boolean.Value.False)
        });
    }

    // FluentStringBoolExtensions.False — validates string? property
    public static class False
    {
        public static TheoryData<FluentCase<string?>> Cases => F.BoolIsFalse.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.BoolIsFalse.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be false.")
        });
    }
}
