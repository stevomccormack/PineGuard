using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.TokenRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentTokenExtensionsTestData
{
    public static class Jwt
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsJwt.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsJwt.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid JWT.", Code: MustCodes.Token.Jwt.Invalid)
        });
    }
}
