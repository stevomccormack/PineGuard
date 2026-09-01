using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.TokenRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TokenAttributesTestData
{
    public static class Jwt
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsJwt.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsJwt.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid JWT.", Code: MustCodes.Token.Jwt.Invalid)
        });
    }
}
