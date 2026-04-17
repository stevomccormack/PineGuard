using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringGuidClausesTestData
{
    public static class NotGuid
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.GuidIsGuid.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.GuidIsGuid.InvalidScenarios.ToGuardCases("value");
    }

    public static class EmptyGuid
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.GuidIsNotEmpty.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.GuidIsNotEmpty.InvalidScenarios.ToGuardCases("value");
    }
}
