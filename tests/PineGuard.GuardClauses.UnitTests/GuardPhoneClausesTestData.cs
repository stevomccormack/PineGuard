using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.PhoneRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardPhoneClausesTestData
{
    public static class NotPhoneNumber
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsPhoneNumber.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsPhoneNumber.AllInvalid.ToGuardCases("value");
    }

    public static class NotPhoneNumberString
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsPhoneNumber.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsPhoneNumber.AllInvalid.ToGuardCases("value");
    }
}
