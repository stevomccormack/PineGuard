using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FileSignatureRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class FileSignatureAttributesTestData
{
    public static class FileSignature
    {
        public static string Extension => F.HasSignature.PngHeader.extension;

        public static TheoryData<DataAnnotationCase> Cases => F.HasSignature.AllScenarios
            .Only(nameof(F.HasSignature.PngHeader), nameof(F.HasSignature.TrailingContentIgnored), nameof(F.HasSignature.SpoofedJpegAsPng), nameof(F.HasSignature.TruncatedHeader), nameof(F.HasSignature.UnrecognizedHeader), nameof(F.HasSignature.EmptyValue), nameof(F.HasSignature.NullValue))
            .ToDataAnnotationCases(inputs => inputs.value, s => s.Name switch
            {
                nameof(F.HasSignature.NullValue) => new DataAnnotationExpected(true),
                _ when s.IsValid => new DataAnnotationExpected(true),
                _ => new DataAnnotationExpected(false, "Value must match the file signature for the declared extension.", Code: MustCodes.File.Signature.Mismatch)
            });
    }

    public static class FileSignatureContainerExtension
    {
        public static string Extension => F.HasSignature.ZipHeaderAsDocx.extension;

        public static TheoryData<DataAnnotationCase> Cases => F.HasSignature.AllScenarios
            .Only(nameof(F.HasSignature.ZipHeaderAsDocx))
            .ToDataAnnotationCases(inputs => inputs.value, _ => new DataAnnotationExpected(true));
    }

    public static class FileSignatureExtensionWithoutDot
    {
        public static string Extension => F.HasSignature.ExtensionWithoutDot.extension;

        public static TheoryData<DataAnnotationCase> Cases => F.HasSignature.AllScenarios
            .Only(nameof(F.HasSignature.ExtensionWithoutDot))
            .ToDataAnnotationCases(inputs => inputs.value, _ => new DataAnnotationExpected(true));
    }

    public static class FileSignatureExtensionUppercase
    {
        public static string Extension => F.HasSignature.ExtensionUppercase.extension;

        public static TheoryData<DataAnnotationCase> Cases => F.HasSignature.AllScenarios
            .Only(nameof(F.HasSignature.ExtensionUppercase))
            .ToDataAnnotationCases(inputs => inputs.value, _ => new DataAnnotationExpected(true));
    }

    public static class FileSignatureExtensionPadded
    {
        public static string Extension => F.HasSignature.ExtensionPadded.extension;

        public static TheoryData<DataAnnotationCase> Cases => F.HasSignature.AllScenarios
            .Only(nameof(F.HasSignature.ExtensionPadded))
            .ToDataAnnotationCases(inputs => inputs.value, _ => new DataAnnotationExpected(true));
    }

    public static class FileSignatureUnknownExtension
    {
        public static string Extension => F.IsKnownExtension.Unregistered!;

        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new(nameof(F.Png), F.Png, new DataAnnotationExpected(false, "extension must have a registered file signature.", Code: MustCodes.File.Signature.Mismatch)),
            new(nameof(F.NullHeader), F.NullHeader, new DataAnnotationExpected(true))
        ];
    }

    public static class KnownFileSignature
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasKnownSignature.AllScenarios.ToDataAnnotationCases(value => (object?)value, s => s.Name switch
        {
            nameof(F.NullHeader) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must match a known file signature.", Code: MustCodes.File.Signature.Unknown)
        });
    }
}
