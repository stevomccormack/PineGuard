using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringBoolClausesTestData
{
    // Guard.Against.False(string?) — throws when value does NOT parse as true
    public static class False
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.BoolIsTrue.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.BoolIsTrue.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.True(string?) — throws when value does NOT parse as false
    public static class True
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.BoolIsFalse.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.BoolIsFalse.InvalidScenarios.ToGuardCases("value");
    }
}
