using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FileSignatureRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustFileSignatureClausesTestData
{
    public static class FileSignature
    {
        public static TheoryData<MustCase<(byte[]? value, string extension)>> ValidCases => F.HasSignature.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(byte[]? value, string extension)>> InvalidCases => F.HasSignature.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.HasSignature.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.File.Signature.Mismatch),
            _ => new MustExpected(false, "value must match the file signature for the declared extension.", "value", MustCodes.File.Signature.Mismatch)
        });

        public static TheoryData<MustCase<(byte[]? value, string extension)>> UnknownExtensionCases =>
            F.IsKnownExtension.InvalidScenarios
                .Project<string?, (byte[]? value, string extension)>(extension => (F.Png, extension!))
                .ToMustCases(_ => new MustExpected(false, "extension must have a registered file signature.", "extension", MustCodes.File.Signature.Unknown));
    }

    public static class KnownFileSignature
    {
        public static TheoryData<MustCase<byte[]?>> ValidCases => F.HasKnownSignature.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<byte[]?>> InvalidCases => F.HasKnownSignature.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NullHeader) => new MustExpected(false, "value must not be null.", "value", MustCodes.File.Signature.Unknown),
            _ => new MustExpected(false, "value must match a known file signature.", "value", MustCodes.File.Signature.Unknown)
        });
    }
}
