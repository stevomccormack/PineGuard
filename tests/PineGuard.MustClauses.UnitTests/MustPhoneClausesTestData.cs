using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.PhoneRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustPhoneClausesTestData
{
    public static class PhoneNumber
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsPhoneNumber.AllValid.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsPhoneNumber.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsPhoneNumber.TooShort) => new MustExpected(false, "value must be a valid phone number.", "value", MustCodes.Phone.Number.Invalid),
            _ => new MustExpected(false, "value must be a valid phone number.", "value")
        });
    }

    public static class PhoneNumberString
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsPhoneNumber.AllValid.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsPhoneNumber.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsPhoneNumber.TooShort) => new MustExpected(false, "value must be a valid phone number.", "value", MustCodes.Phone.Number.Invalid),
            _ => new MustExpected(false, "value must be a valid phone number.", "value")
        });
    }
}
