using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.ChecksumRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustChecksumClausesTestData
{
    public static class Luhn
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsLuhn.AllValid.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsLuhn.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsLuhn.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Checksum.Luhn.Invalid),
            _ => new MustExpected(false, "value must satisfy the Luhn checksum.", Code: MustCodes.Checksum.Luhn.Invalid)
        });
    }
}
