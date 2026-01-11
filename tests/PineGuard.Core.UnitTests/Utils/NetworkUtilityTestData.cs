using System.Net;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class NetworkUtilityTestData
{
    public static class TryParseIpAddress
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Parses IPv4 loopback", "127.0.0.1", true, IPAddress.Parse("127.0.0.1")),
            new("Parses IPv4 with whitespace", " 127.0.0.1 ", true, IPAddress.Parse("127.0.0.1")),
            new("Parses IPv6 loopback", "::1", true, IPAddress.Parse("::1")),
            new("Parses IPv6 with whitespace", " 2001:db8::1 ", true, IPAddress.Parse("2001:db8::1"))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null returns false", null, false, IPAddress.Parse("0.0.0.0")),
            new("Empty returns false", string.Empty, false, IPAddress.Parse("0.0.0.0")),
            new("Whitespace returns false", " ", false, IPAddress.Parse("0.0.0.0")),
            new("Control whitespace returns false", "\t\r\n", false, IPAddress.Parse("0.0.0.0")),
            new("Not an IP returns false", "not-an-ip", false, IPAddress.Parse("0.0.0.0"))
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, IPAddress ExpectedIpAddress)
            : TryCase<string?, IPAddress>(Name, Value, ExpectedSuccess, ExpectedIpAddress);

        #endregion Cases
    }

    public static class TryParseIpv4
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Parses dotted-quad", "127.0.0.1", true, IPAddress.Parse("127.0.0.1")),
            new("Parses dotted-quad with whitespace", " 127.0.0.1 ", true, IPAddress.Parse("127.0.0.1")),
            new("Parses leading zeros", "001.002.003.004", true, IPAddress.Parse("1.2.3.4"))
        ];

        public static TheoryData<Case> EdgeCases =>
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

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, IPAddress ExpectedIpAddress)
            : TryCase<string?, IPAddress>(Name, Value, ExpectedSuccess, ExpectedIpAddress);

        #endregion Cases
    }

    public static class TryParseIpv6
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Parses IPv6 loopback", "::1", true, IPAddress.Parse("::1")),
            new("Parses IPv6 with whitespace", " 2001:db8::1 ", true, IPAddress.Parse("2001:db8::1"))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null returns false", null, false, IPAddress.Parse("0.0.0.0")),
            new("Empty returns false", string.Empty, false, IPAddress.Parse("0.0.0.0")),
            new("Whitespace returns false", " ", false, IPAddress.Parse("0.0.0.0")),
            new("IPv4 is not IPv6", "127.0.0.1", false, IPAddress.Parse("0.0.0.0")),
            new("Not an IP returns false", "not-an-ip", false, IPAddress.Parse("0.0.0.0"))
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, IPAddress ExpectedIpAddress)
            : TryCase<string?, IPAddress>(Name, Value, ExpectedSuccess, ExpectedIpAddress);

        #endregion Cases
    }
}
