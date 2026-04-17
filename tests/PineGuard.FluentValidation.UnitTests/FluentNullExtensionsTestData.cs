using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.NullRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentNullExtensionsTestData
{
    // FluentNullExtensions.NotRequired — validates object? property
    public static class NotRequired
    {
        public static TheoryData<FluentCase<object?>> Cases => F.IsNull.AllScenarios.ToFluentCases(s =>
            s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be null."));
    }

    // FluentNullExtensions.Required — validates object? property
    public static class Required
    {
        public static TheoryData<FluentCase<object?>> Cases => F.IsNotNull.AllScenarios.ToFluentCases(s =>
            s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must not be null."));
    }
}
