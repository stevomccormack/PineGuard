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
}
