using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.BufferRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class BufferAttributesTestData
{
    // Hex — valid when IS hex; null skipped by DA layer
    public static class Hex
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsHex.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsHex.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, Code: MustCodes.Encoding.Hex.Invalid)
        });
    }

    // NotHex — valid when NOT hex; null skipped by DA layer
    public static class NotHex
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsHex.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsHex.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, Code: MustCodes.Encoding.Hex.WellFormed),  // IS hex → invalid for NotHex
            _ => new DataAnnotationExpected(true)                                                            // NOT hex → valid for NotHex
        });
    }

    // Base64 — valid when IS base64; null skipped by DA layer
    public static class Base64
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsBase64.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsBase64.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, Code: MustCodes.Encoding.Base64.Invalid)
        });
    }

    // NotBase64 — valid when NOT base64; null skipped by DA layer
    public static class NotBase64
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsBase64.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsBase64.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, Code: MustCodes.Encoding.Base64.WellFormed),  // IS base64 → invalid for NotBase64
            _ => new DataAnnotationExpected(true)                                                               // NOT base64 → valid for NotBase64
        });
    }

    public static class Base64Url
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsBase64Url.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsBase64Url.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid base64url string.", Code: MustCodes.Encoding.Base64url.Invalid)
        });
    }

    public static class Utf8
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUtf8.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsUtf8.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid UTF-8 byte sequence.", Code: MustCodes.Encoding.Utf8.Invalid)
        });
    }
}
