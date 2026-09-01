using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.IdentifierRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustIdentifierClausesTestData
{
    public static class Slug
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsSlug.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsSlug.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsSlug.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid slug.", Code: MustCodes.Identifier.Slug.Invalid)
        });
    }

    public static class Ulid
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsUlid.AllValid.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsUlid.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsUlid.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Identifier.Ulid.Invalid),
            _ => new MustExpected(false, "value must be a valid ULID.", Code: MustCodes.Identifier.Ulid.Invalid)
        });
    }
}
