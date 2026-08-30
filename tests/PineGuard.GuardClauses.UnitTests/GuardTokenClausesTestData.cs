using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.TokenRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardTokenClausesTestData
{
    // Guard.Against.NotJwt — throws when value is NOT a structurally valid JWT (delegates to Must.Be.Jwt)
    public static class NotJwt
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsJwt.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsJwt.AllInvalid.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Token.Jwt.Invalid));
    }
}
