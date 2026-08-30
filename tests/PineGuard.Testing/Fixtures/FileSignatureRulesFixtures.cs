using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Utils;

namespace PineGuard.Testing.Fixtures;

public static class FileSignatureRulesFixtures
{
    public static readonly byte[]? Png = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    public static readonly byte[]? Jpeg = [0xFF, 0xD8, 0xFF, 0xE0];
    public static readonly byte[]? Gif87 = [0x47, 0x49, 0x46, 0x38, 0x37, 0x61];
    public static readonly byte[]? Gif89 = [0x47, 0x49, 0x46, 0x38, 0x39, 0x61];
    public static readonly byte[]? Bmp = [0x42, 0x4D, 0x36, 0x00];
    public static readonly byte[]? Webp = [0x52, 0x49, 0x46, 0x46, 0x1A, 0x00, 0x00, 0x00, 0x57, 0x45, 0x42, 0x50];
    public static readonly byte[]? TiffLittleEndian = [0x49, 0x49, 0x2A, 0x00];
    public static readonly byte[]? TiffBigEndian = [0x4D, 0x4D, 0x00, 0x2A];
    public static readonly byte[]? Ico = [0x00, 0x00, 0x01, 0x00];
    public static readonly byte[]? Pdf = [0x25, 0x50, 0x44, 0x46, 0x2D];
    public static readonly byte[]? Zip = [0x50, 0x4B, 0x03, 0x04];
    public static readonly byte[]? ZipEmptyArchive = [0x50, 0x4B, 0x05, 0x06];
    public static readonly byte[]? ZipSpannedArchive = [0x50, 0x4B, 0x07, 0x08];
    public static readonly byte[]? Gzip = [0x1F, 0x8B, 0x08];
    public static readonly byte[]? SevenZip = [0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C];
    public static readonly byte[]? Rar4 = [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00];
    public static readonly byte[]? Rar5 = [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00];
    public static readonly byte[]? Mp3 = [0x49, 0x44, 0x33, 0x03];
    public static readonly byte[]? Mp4 = [0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70];
    public static readonly byte[]? PngWithTrailingContent = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52];
    public static readonly byte[]? TruncatedPng = [0x89, 0x50];
    public static readonly byte[]? Unrecognized = new byte[FileSignatureUtility.MaxSignatureLength];
    public static readonly byte[]? EmptyHeader = [];
    public static readonly byte[]? NullHeader = null;

    public static class HasSignature
    {
        public static readonly (byte[]? value, string extension) PngHeader = (Png, ".png");
        public static readonly (byte[]? value, string extension) JpegHeaderAsJpeg = (Jpeg, ".jpeg");
        public static readonly (byte[]? value, string extension) ZipHeaderAsDocx = (Zip, ".docx");
        public static readonly (byte[]? value, string extension) WebpHeader = (Webp, ".webp");
        public static readonly (byte[]? value, string extension) Mp4Header = (Mp4, ".mp4");
        public static readonly (byte[]? value, string extension) ExtensionWithoutDot = (Pdf, "pdf");
        public static readonly (byte[]? value, string extension) ExtensionUppercase = (Png, ".PNG");
        public static readonly (byte[]? value, string extension) ExtensionPadded = (Png, "  .png  ");
        public static readonly (byte[]? value, string extension) TrailingContentIgnored = (PngWithTrailingContent, ".png");
        public static readonly (byte[]? value, string extension) SpoofedJpegAsPng = (Jpeg, ".png");
        public static readonly (byte[]? value, string extension) TruncatedHeader = (TruncatedPng, ".png");
        public static readonly (byte[]? value, string extension) UnrecognizedHeader = (Unrecognized, ".png");
        public static readonly (byte[]? value, string extension) EmptyValue = (EmptyHeader, ".png");
        public static readonly (byte[]? value, string extension) NullValue = (NullHeader, ".png");

        public static RuleScenario<(byte[]? value, string extension)>[] ValidScenarios =>
        [
            new(nameof(PngHeader), PngHeader, true),
            new(nameof(JpegHeaderAsJpeg), JpegHeaderAsJpeg, true),
            new(nameof(ZipHeaderAsDocx), ZipHeaderAsDocx, true),
            new(nameof(WebpHeader), WebpHeader, true),
            new(nameof(Mp4Header), Mp4Header, true),
            new(nameof(ExtensionWithoutDot), ExtensionWithoutDot, true),
            new(nameof(ExtensionUppercase), ExtensionUppercase, true),
            new(nameof(ExtensionPadded), ExtensionPadded, true),
            new(nameof(TrailingContentIgnored), TrailingContentIgnored, true)
        ];

        public static RuleScenario<(byte[]? value, string extension)>[] InvalidScenarios =>
        [
            new(nameof(SpoofedJpegAsPng), SpoofedJpegAsPng, false),
            new(nameof(TruncatedHeader), TruncatedHeader, false),
            new(nameof(UnrecognizedHeader), UnrecognizedHeader, false),
            new(nameof(EmptyValue), EmptyValue, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<(byte[]? value, string extension)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasKnownSignature
    {
        public static RuleScenario<byte[]?>[] ValidScenarios =>
        [
            new(nameof(Png), Png, true),
            new(nameof(Jpeg), Jpeg, true),
            new(nameof(Gif87), Gif87, true),
            new(nameof(Gif89), Gif89, true),
            new(nameof(Bmp), Bmp, true),
            new(nameof(Webp), Webp, true),
            new(nameof(TiffLittleEndian), TiffLittleEndian, true),
            new(nameof(TiffBigEndian), TiffBigEndian, true),
            new(nameof(Ico), Ico, true),
            new(nameof(Pdf), Pdf, true),
            new(nameof(Zip), Zip, true),
            new(nameof(ZipEmptyArchive), ZipEmptyArchive, true),
            new(nameof(ZipSpannedArchive), ZipSpannedArchive, true),
            new(nameof(Gzip), Gzip, true),
            new(nameof(SevenZip), SevenZip, true),
            new(nameof(Rar4), Rar4, true),
            new(nameof(Rar5), Rar5, true),
            new(nameof(Mp3), Mp3, true),
            new(nameof(Mp4), Mp4, true),
            new(nameof(PngWithTrailingContent), PngWithTrailingContent, true)
        ];

        public static RuleScenario<byte[]?>[] InvalidScenarios =>
        [
            new(nameof(TruncatedPng), TruncatedPng, false),
            new(nameof(Unrecognized), Unrecognized, false),
            new(nameof(EmptyHeader), EmptyHeader, false),
            new(nameof(NullHeader), NullHeader, false)
        ];

        public static RuleScenario<byte[]?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class TryDetectExtension
    {
        public static readonly (byte[]? header, string? extension) PngDetected = (Png, ".png");
        public static readonly (byte[]? header, string? extension) JpegDetectedAsJpg = (Jpeg, ".jpg");
        public static readonly (byte[]? header, string? extension) ZipDetectedAsZip = (Zip, ".zip");
        public static readonly (byte[]? header, string? extension) WebpDetected = (Webp, ".webp");
        public static readonly (byte[]? header, string? extension) Mp4Detected = (Mp4, ".mp4");
        public static readonly (byte[]? header, string? extension) TruncatedUndetected = (TruncatedPng, null);
        public static readonly (byte[]? header, string? extension) UnrecognizedUndetected = (Unrecognized, null);
        public static readonly (byte[]? header, string? extension) EmptyUndetected = (EmptyHeader, null);
        public static readonly (byte[]? header, string? extension) NullUndetected = (NullHeader, null);

        public static RuleScenario<(byte[]? header, string? extension)>[] ValidScenarios =>
        [
            new(nameof(PngDetected), PngDetected, true),
            new(nameof(JpegDetectedAsJpg), JpegDetectedAsJpg, true),
            new(nameof(ZipDetectedAsZip), ZipDetectedAsZip, true),
            new(nameof(WebpDetected), WebpDetected, true),
            new(nameof(Mp4Detected), Mp4Detected, true)
        ];

        public static RuleScenario<(byte[]? header, string? extension)>[] InvalidScenarios =>
        [
            new(nameof(TruncatedUndetected), TruncatedUndetected, false),
            new(nameof(UnrecognizedUndetected), UnrecognizedUndetected, false),
            new(nameof(EmptyUndetected), EmptyUndetected, false),
            new(nameof(NullUndetected), NullUndetected, false)
        ];

        public static RuleScenario<(byte[]? header, string? extension)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsKnownExtension
    {
        public static readonly string? WithDot = ".png";
        public static readonly string? WithoutDot = "png";
        public static readonly string? Uppercase = ".PNG";
        public static readonly string? Padded = "  .png  ";
        public static readonly string? OoxmlDocument = ".docx";
        public static readonly string? SevenZipWithoutDot = "7z";
        public static readonly string? Unregistered = ".exe";
        public static readonly string? DotOnly = ".";
        public static readonly string? WhitespaceExtension = "   ";
        public static readonly string? EmptyExtension = "";
        public static readonly string? NullExtension = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(WithDot), WithDot, true),
            new(nameof(WithoutDot), WithoutDot, true),
            new(nameof(Uppercase), Uppercase, true),
            new(nameof(Padded), Padded, true),
            new(nameof(OoxmlDocument), OoxmlDocument, true),
            new(nameof(SevenZipWithoutDot), SevenZipWithoutDot, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Unregistered), Unregistered, false),
            new(nameof(DotOnly), DotOnly, false),
            new(nameof(WhitespaceExtension), WhitespaceExtension, false),
            new(nameof(EmptyExtension), EmptyExtension, false),
            new(nameof(NullExtension), NullExtension, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
