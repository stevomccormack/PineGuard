using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.NullRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardNullClausesTestData
{
    public static class NotNull
    {
        public static TheoryData<GuardCase<object?>> ValidCases => F.IsNull.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<object?>> InvalidCases => F.IsNull.InvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class Null
    {
        public static TheoryData<GuardCase<object?>> ValidCases => F.IsNotNull.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<object?>> InvalidCases => F.IsNotNull.InvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
    }
}
