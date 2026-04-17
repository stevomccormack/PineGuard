using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.BoolRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentBoolExtensionsTestData
{
    // FluentBoolExtensions.True — validates bool? property
    public static class True
    {
        public static TheoryData<FluentCase<bool?>> Cases => F.IsTrue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsTrue.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be true.")
        });
    }

    // FluentBoolExtensions.False — validates bool? property
    public static class False
    {
        public static TheoryData<FluentCase<bool?>> Cases => F.IsFalse.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFalse.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be false.")
        });
    }
}
