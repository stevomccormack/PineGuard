using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardGuidClausesTestData
{
    public static class Empty
    {
        public static TheoryData<GuardCase<Guid>> ValidCases => F.NotEmpty.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<Guid>> InvalidCases => F.NotEmpty.InvalidScenarios.ToGuardCases("value");
    }
}
