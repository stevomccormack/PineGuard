using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.IdentifierRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentIdentifierExtensionsTestData
{
    public static class Slug
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsSlug.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsSlug.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid slug.")
        });
    }
}
