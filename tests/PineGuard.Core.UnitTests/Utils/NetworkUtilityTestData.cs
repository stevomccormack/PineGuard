using System.Net;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class NetworkUtilityTestData
{
    public static class TryParseIpAddress
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Parses IPv4 loopback", "127.0.0.1", true, IPAddress.Parse("127.0.0.1")),
            new("Parses IPv4 with whitespace", " 127.0.0.1 ", true, IPAddress.Parse("127.0.0.1")),
            new("Parses IPv6 loopback", "::1", true, IPAddress.Parse("::1")),
            new("Parses IPv6 with whitespace", " 2001:db8::1 ", true, IPAddress.Parse("2001:db8::1"))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null returns false", null, false, IPAddress.Parse("0.0.0.0")),
            new("Empty returns false", string.Empty, false, IPAddress.Parse("0.0.0.0")),
            new("Whitespace returns false", " ", false, IPAddress.Parse("0.0.0.0")),
            new("Control whitespace returns false", "\t\r\n", false, IPAddress.Parse("0.0.0.0")),
            new("Not an IP returns false", "not-an-ip", false, IPAddress.Parse("0.0.0.0"))
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, IPAddress ExpectedOutValue)
            : TryCase<string?, IPAddress>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseIpv4
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Parses dotted-quad", "127.0.0.1", true, IPAddress.Parse("127.0.0.1")),
            new("Parses dotted-quad with whitespace", " 127.0.0.1 ", true, IPAddress.Parse("127.0.0.1")),
            new("Parses leading zeros", "001.002.003.004", true, IPAddress.Parse("1.2.3.4"))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null returns false", null, false, IPAddress.Parse("0.0.0.0")),
            new("Empty returns false", string.Empty, false, IPAddress.Parse("0.0.0.0")),
            new("Whitespace returns false", " ", false, IPAddress.Parse("0.0.0.0")),
            new("Out-of-range octet returns false", "256.0.0.1", false, IPAddress.Parse("0.0.0.0")),
            new("Missing octet returns false", "127.0.0", false, IPAddress.Parse("0.0.0.0")),
            new("Extra octet returns false", "127.0.0.1.1", false, IPAddress.Parse("0.0.0.0")),
            new("Non-numeric returns false", "127.0.0.01a", false, IPAddress.Parse("0.0.0.0")),
            new("Empty segment returns false", "1..2.3", false, IPAddress.Parse("0.0.0.0")),
            new("Trailing dot returns false", "127.0.0.", false, IPAddress.Parse("0.0.0.0")),
            new("Too many digits returns false", "0000.0.0.0", false, IPAddress.Parse("0.0.0.0")),
            new("Negative octet returns false", "-1.0.0.0", false, IPAddress.Parse("0.0.0.0")),
            new("Plus-signed octet returns false", "+1.0.0.0", false, IPAddress.Parse("0.0.0.0"))
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, IPAddress ExpectedOutValue)
            : TryCase<string?, IPAddress>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseIpv6
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Parses IPv6 loopback", "::1", true, IPAddress.Parse("::1")),
            new("Parses IPv6 with whitespace", " 2001:db8::1 ", true, IPAddress.Parse("2001:db8::1"))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null returns false", null, false, IPAddress.Parse("0.0.0.0")),
            new("Empty returns false", string.Empty, false, IPAddress.Parse("0.0.0.0")),
            new("Whitespace returns false", " ", false, IPAddress.Parse("0.0.0.0")),
            new("IPv4 is not IPv6", "127.0.0.1", false, IPAddress.Parse("0.0.0.0")),
            new("Not an IP returns false", "not-an-ip", false, IPAddress.Parse("0.0.0.0"))
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, IPAddress ExpectedOutValue)
            : TryCase<string?, IPAddress>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseCidr
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ipv4 /24", "192.168.1.1/24", true),
            new("ipv4 /25 (partial-byte prefix)", "192.168.1.200/25", true),
            new("ipv6 /64", "2001:db8::1/64", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("missing slash", "192.168.1.1", false),
            new("slash first", "/24", false),
            new("slash last", "192.168.1.1/", false),
            new("bad address", "x/24", false),
            new("bad prefix", "192.168.1.1/x", false),
            new("prefix too big v4", "192.168.1.1/33", false),
            new("prefix too big v6", "2001:db8::1/129", false),
            new("negative prefix", "192.168.1.1/-1", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class IsInCidr
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("in v4", ("192.168.1.10", "192.168.1.0", 24), true),
            new("out v4", ("192.168.2.10", "192.168.1.0", 24), false),
            new("in v4 /25", ("192.168.1.200", "192.168.1.128", 25), true),
            new("out v4 /25", ("192.168.1.127", "192.168.1.128", 25), false),
            new("in v6", ("2001:db8::1", "2001:db8::", 64), true),
            new("out v6", ("2001:db8:1::1", "2001:db8::", 64), false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("family mismatch", ("192.168.1.10", "2001:db8::", 64), false),
            new("negative prefix", ("192.168.1.10", "192.168.1.0", -1), false),
            new("too big prefix", ("192.168.1.10", "192.168.1.0", 33), false)
        ];

        public sealed record ValidCase(string Name, (string Ip, string NetworkIp, int PrefixLength) Value, bool Expected)
            : IsCase<(string Ip, string NetworkIp, int PrefixLength)>(Name, Value, Expected);
    }

    public static class TryGetAsciiHostname
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "example.com", true),
            new("trim + trailing dot", "  example.com.  ", true),
            new("contains space (normalizes, not validates)", "exa mple.com", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("whitespace", "  ", false),
            new("just dot", ".", false),
            new("invalid control char", "exa\u0001mple.com", false)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class ValidateHostnameLabels
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple hostname", "example.com", true),
            new("single label", "localhost", true),
            new("max label length (63)", new string('a', 63) + ".com", true),
            new("uppercase hostname", "EXAMPLE.COM", true),
            new("mixed case hostname", "Example.Com", true),
            new("digits in label", "host123.example.com", true),
            new("all digits label", "123.456.com", true),
            new("hyphen with digits", "a-1.com", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("overall > 253 chars", new string('a', 63) + "." + new string('b', 63) + "." + new string('c', 63) + "." + new string('d', 63) + ".com", false),
            new("label > 63 chars", new string('a', 64) + ".com", false),
            new("leading hyphen label", "-abc.com", false),
            new("trailing hyphen label", "abc-.com", false),
            new("invalid char underscore", "ab_c.com", false),
            new("invalid char at boundary", "ab" + (char)('0' - 1) + ".com", false)
        ];

        public sealed record ValidCase(string Name, string Value, bool Expected)
            : IsCase<string>(Name, Value, Expected);
    }
}
