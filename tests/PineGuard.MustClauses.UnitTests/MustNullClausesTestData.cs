using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.NullRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustNullClausesTestData
{
    public static class Null
    {
        public static TheoryData<MustCase<object?>> ValidCases => F.IsNull.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<object?>> InvalidCases => F.IsNull.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be null."));
    }

    public static class NotNull
    {
        public static TheoryData<MustCase<object?>> ValidCases => F.IsNotNull.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<object?>> InvalidCases => F.IsNotNull.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be null."));
    }
}
