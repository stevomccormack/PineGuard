using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.BufferRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentBufferExtensionsTestData
{
    public static class Hex
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHex.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHex.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid hex string.", Code: MustCodes.Encoding.Hex.Invalid)
        });
    }

    public static class NotHex
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHex.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHex.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid hex string."),
            _ => new FluentExpected(true)
        });
    }

    public static class Base64
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsBase64.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsBase64.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid base64 string.")
        });
    }

    public static class NotBase64
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsBase64.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsBase64.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid base64 string."),
            _ => new FluentExpected(true)
        });
    }

    public static class Base64Url
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsBase64Url.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsBase64Url.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid base64url string.", Code: MustCodes.Encoding.Base64url.Invalid)
        });
    }

    public static class Utf8
    {
        public static TheoryData<FluentCase<byte[]?>> Cases => F.IsUtf8.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsUtf8.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid UTF-8 byte sequence.", Code: MustCodes.Encoding.Utf8.Invalid)
        });
    }
}
