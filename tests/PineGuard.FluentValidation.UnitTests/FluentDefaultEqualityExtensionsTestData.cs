using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.DefaultEqualityRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDefaultEqualityExtensionsTestData
{
    public static class DefaultInt32
    {
        public static TheoryData<FluentCase<int>> Cases => F.IsDefaultInt32.AllScenarios.ToFluentCases(s => s.IsValid
            ? new FluentExpected(true)
            : new FluentExpected(false, "Value must be the default value."));
    }

    public static class NotDefaultInt32
    {
        public static TheoryData<FluentCase<int>> Cases => F.IsDefaultInt32.AllScenarios.ToFluentCases(s => s.IsValid
            ? new FluentExpected(false, "Value must not be the default value.")
            : new FluentExpected(true));
    }

    public static class NullOrDefaultString
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsNullOrDefaultString.AllScenarios.ToFluentCases(s => s.IsValid
            ? new FluentExpected(true)
            : new FluentExpected(false, "Value must be null or the default value."));
    }

    public static class NotNullOrDefaultString
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsNullOrDefaultString.AllScenarios.ToFluentCases(s => s.IsValid
            ? new FluentExpected(false, "Value must not be null or the default value.")
            : new FluentExpected(true));
    }
}
