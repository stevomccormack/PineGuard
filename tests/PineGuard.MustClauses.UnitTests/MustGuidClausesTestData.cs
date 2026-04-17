using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustGuidClausesTestData
{
    public static class NotEmpty
    {
        public static TheoryData<MustCase<Guid>> ValidCases => F.NotEmpty.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<Guid>> InvalidCases => F.NotEmpty.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be an empty GUID."));
    }
}
