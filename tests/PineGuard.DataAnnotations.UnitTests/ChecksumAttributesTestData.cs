using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.ChecksumRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class ChecksumAttributesTestData
{
    public static class Luhn
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLuhn.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsLuhn.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must satisfy the Luhn checksum.", Code: MustCodes.Checksum.Luhn.Invalid)
        });
    }
}
