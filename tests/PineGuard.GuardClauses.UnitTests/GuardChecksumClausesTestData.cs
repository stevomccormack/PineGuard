using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.ChecksumRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardChecksumClausesTestData
{
    // Guard.Against.NotLuhn — throws when value does NOT satisfy the Luhn checksum (delegates to Must.Be.Luhn)
    public static class NotLuhn
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLuhn.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLuhn.AllInvalid.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Checksum.Luhn.Invalid));
    }
}
