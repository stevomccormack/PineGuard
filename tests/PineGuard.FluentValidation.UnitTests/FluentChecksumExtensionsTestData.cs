using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.ChecksumRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentChecksumExtensionsTestData
{
    public static class Luhn
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsLuhn.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLuhn.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must satisfy the Luhn checksum.", Code: MustCodes.Checksum.Luhn.Invalid)
        });
    }
}
