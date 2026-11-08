using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public static class TryReferenceOutputContractsTestData
{
    public sealed record OutputCase(string Name, Func<string> Value, string Expected)
        : ReturnCase<Func<string>, string>(Name, Value, Expected);

    public static TheoryData<OutputCase> Cases =>
    [
        new("Cron fields", () => CronUtility.TryParse("* * * * *", CronFormat.Standard, out var fields) ? fields[0] : "failure", "*"),
        new("Culture", () => CultureInfoUtility.TryGetCultureInfo("en", out var culture) ? culture.Name : "failure", "en"),
        new("Regional culture", () => CultureInfoUtility.TryGetCultureInfo("en", "US", out var culture) ? culture.Name : "failure", "en-US"),
        new("UTF8 text", () => BufferUtility.TryDecodeUtf8([65], out var text) ? text.ToLowerInvariant() : "failure", "a"),
        new("Security header", () => HttpSecurityHeaderUtility.TrySplitSemicolonSeparatedSegments("max-age=1; preload", out var segments) ? segments[1] : "failure", "preload"),
        new("Content types", () => HttpContentTypeUtility.TryGetContentTypeMediaTypes(new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/json"] }, out var types) ? types[0] : "failure", "application/json"),
        new("Single header", () => HttpUtility.TryGetSingleHeaderValue(new Dictionary<string, IEnumerable<string>> { ["x-name"] = [" name "] }, "x-name", out var value) ? value.ToUpperInvariant() : "failure", "NAME"),
        new("Signature", () => FileSignatureUtility.TryDetectExtension([0x25, 0x50, 0x44, 0x46, 0x2D], out var extension) ? extension.ToUpperInvariant() : "failure", ".PDF"),
        new("IP address", () => NetworkUtility.TryParseIpAddress("127.0.0.1", out var address) ? address.ToString() : "failure", "127.0.0.1"),
        new("IPv4", () => NetworkUtility.TryParseIpv4("127.0.0.1", out var address) ? address.ToString() : "failure", "127.0.0.1"),
        new("IPv6", () => NetworkUtility.TryParseIpv6("::1", out var address) ? address.ToString() : "failure", "::1"),
        new("CIDR network", () => NetworkUtility.TryParseCidr("192.168.1.42/24", out var network, out _) ? network.ToString() : "failure", "192.168.1.0"),
        new("ASCII hostname", () => NetworkUtility.TryGetAsciiHostname("example.com", out var hostname) ? hostname.ToUpperInvariant() : "failure", "EXAMPLE.COM"),
        new("Absolute URI", () => UriUtility.TryParseAbsolute("https://example.com", out var uri) ? uri.Scheme : "failure", "https"),
        new("Relative URI", () => UriUtility.TryParseRelative("path", out var uri) ? uri.OriginalString : "failure", "path"),
        new("URL", () => UriUtility.TryParseUrl("http://example.com", out var uri) ? uri.Scheme : "failure", "http"),
        new("HTTPS URL", () => UriUtility.TryParseHttpsUrl("https://example.com", out var uri) ? uri.Scheme : "failure", "https"),
        new("HTTP URL", () => UriUtility.TryParseHttpUrl("http://example.com", out var uri) ? uri.Scheme : "failure", "http"),
        new("File URI", () => UriUtility.TryParseFileUri("file:///C:/test.txt", out var uri) ? uri.Scheme : "failure", "file"),
        new("XML root", () => XmlUtility.TryGetRootName("<root />", out var name) ? name.Name : "failure", "root"),
    ];
}
