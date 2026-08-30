using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.TokenRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustTokenClausesTestData
{
    public static class Jwt
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsJwt.AllValid.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsJwt.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsJwt.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Token.Jwt.Invalid),
            _ => new MustExpected(false, "value must be a valid JWT.", Code: MustCodes.Token.Jwt.Invalid)
        });
    }
}
