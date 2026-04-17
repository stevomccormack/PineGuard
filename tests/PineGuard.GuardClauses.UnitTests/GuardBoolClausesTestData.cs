using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.BoolRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardBoolClausesTestData
{
    // Guard.Against.False — throws when value IS false
    public static class False
    {
        public static TheoryData<GuardCase<bool>> ValidCases => F.FalseRule.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<bool>> InvalidCases => F.FalseRule.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.True — throws when value IS true
    public static class True
    {
        public static TheoryData<GuardCase<bool>> ValidCases => F.TrueRule.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<bool>> InvalidCases => F.TrueRule.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
