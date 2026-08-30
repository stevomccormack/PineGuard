using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FileSignatureRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentFileSignatureExtensionsTestData
{
    public static class FileSignature
    {
        public static TheoryData<FluentCase<(byte[]? value, string extension)>> Cases =>
        [
            .. F.HasSignature.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasSignature.NullValue) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must match the file signature for the declared extension.", Code: MustCodes.File.Signature.Mismatch)
            }),
            .. F.IsKnownExtension.InvalidScenarios.Project<string?, (byte[]? value, string extension)>(extension => (F.Png, extension!)).ToFluentCases(_ => new FluentExpected(false, "extension must have a registered file signature.", Code: MustCodes.File.Signature.Mismatch))
        ];
    }

    public static class KnownFileSignature
    {
        public static TheoryData<FluentCase<byte[]?>> Cases => F.HasKnownSignature.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NullHeader) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must match a known file signature.", Code: MustCodes.File.Signature.Unknown)
        });
    }
}
