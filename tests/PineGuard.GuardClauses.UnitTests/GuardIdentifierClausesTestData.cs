using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.IdentifierRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardIdentifierClausesTestData
{
    public static class NotSlug
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsSlug.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsSlug.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotUlid — throws when value is NOT a canonical ULID (delegates to Must.Be.Ulid)
    public static class NotUlid
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsUlid.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsUlid.AllInvalid.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Identifier.Ulid.Invalid));
    }
}
