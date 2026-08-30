using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.BufferRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardBufferClausesTestData
{
    public static class NotHex
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHex.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsHex.AllInvalid.ToGuardCases("value");
    }

    public static class NotBase64
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsBase64.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsBase64.AllInvalid.ToGuardCases("value");
    }

    public static class Hex
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHex.AllInvalid.Except(nameof(F.IsHex.Null)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsHex.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));

        public static TheoryData<GuardCase<string?>> NullCases => F.IsHex.AllInvalid.Only(nameof(F.IsHex.Null)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
    }

    public static class Base64
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsBase64.AllInvalid.Except(nameof(F.IsBase64.Null)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsBase64.AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));

        public static TheoryData<GuardCase<string?>> NullCases => F.IsBase64.AllInvalid.Only(nameof(F.IsBase64.Null)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
    }

    // Guard.Against.NotBase64Url — throws when value is NOT valid base64url (delegates to Must.Be.Base64Url)
    public static class NotBase64Url
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsBase64Url.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsBase64Url.AllInvalid.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Encoding.Base64url.Invalid));
    }
}
