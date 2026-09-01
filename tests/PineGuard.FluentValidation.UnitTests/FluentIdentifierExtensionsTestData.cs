using PineGuard.Codes;
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
            _ => new FluentExpected(false, "Value must be a valid slug.", Code: MustCodes.Identifier.Slug.Invalid)
        });
    }

    public static class Ulid
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsUlid.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsUlid.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid ULID.", Code: MustCodes.Identifier.Ulid.Invalid)
        });
    }
}
