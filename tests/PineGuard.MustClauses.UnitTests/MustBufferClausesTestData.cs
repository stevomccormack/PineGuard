using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.BufferRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustBufferClausesTestData
{
    public static class Hex
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHex.AllValid.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsHex.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsHex.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid hex string.", "value", MustCodes.Encoding.Hex.Invalid)
        });
    }

    public static class Base64
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsBase64.AllValid.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsBase64.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsBase64.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid base64 string.", "value", MustCodes.Encoding.Base64.Invalid)
        });
    }

    public static class NotHex
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHex.AllInvalid.Except(nameof(F.IsHex.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsHex.AllValid.ToMustCases(
            _ => new MustExpected(false, "value must not be a valid hex string.", "value", MustCodes.Encoding.Hex.WellFormed));

        public static TheoryData<MustCase<string?>> NullCases => F.IsHex.AllInvalid.Only(nameof(F.IsHex.Null)).ToMustCases(
            _ => new MustExpected(false, "value must not be null.", "value"));
    }

    public static class NotBase64
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsBase64.AllInvalid.Except(nameof(F.IsBase64.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsBase64.AllValid.ToMustCases(
            _ => new MustExpected(false, "value must not be a valid base64 string.", "value", MustCodes.Encoding.Base64.WellFormed));

        public static TheoryData<MustCase<string?>> NullCases => F.IsBase64.AllInvalid.Only(nameof(F.IsBase64.Null)).ToMustCases(
            _ => new MustExpected(false, "value must not be null.", "value"));
    }
}
