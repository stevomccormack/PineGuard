using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringBoolClausesTestData
{
    // MustStringBoolClauses.True
    public static class True
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.BoolIsTrue.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.BoolIsTrue.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.BoolIsTrue.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Boolean.Value.False),
            _ => new MustExpected(false, "value must be true.", Code: MustCodes.Boolean.Value.False)
        });
    }

    // MustStringBoolClauses.False
    public static class False
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.BoolIsFalse.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.BoolIsFalse.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.BoolIsFalse.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Boolean.Value.True),
            _ => new MustExpected(false, "value must be false.", Code: MustCodes.Boolean.Value.True)
        });
    }
}
