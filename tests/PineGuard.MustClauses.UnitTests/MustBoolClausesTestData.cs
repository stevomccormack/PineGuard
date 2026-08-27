using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.BoolRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustBoolClausesTestData
{
    public static class True
    {
        public static TheoryData<MustCase<bool>> ValidCases => F.TrueRule.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<bool>> InvalidCases => F.TrueRule.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be true.", Code: MustCodes.Boolean.Value.False));
    }

    public static class False
    {
        public static TheoryData<MustCase<bool>> ValidCases => F.FalseRule.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<bool>> InvalidCases => F.FalseRule.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be false.", Code: MustCodes.Boolean.Value.True));
    }
}
