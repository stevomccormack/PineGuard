using System.Net;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class NetworkUtilityTestData
{
    public static class TryParseIpAddress
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("Parses IPv4 loopback", "127.0.0.1"),
            V("Parses IPv4 with whitespace", " 127.0.0.1 "),
            V("Parses IPv6 loopback", "::1"),
            V("Parses IPv6 with whitespace", " 2001:db8::1 "),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            E("Null returns false", null),
            E("Empty returns false", string.Empty),
            E("Whitespace returns false", " "),
            E("Control whitespace returns false", "\t\r\n"),
            E("Not an IP returns false", "not-an-ip"),
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        private static Case V(string name, string? input) => new(name, input, true, IPAddress.Parse(input!.Trim()));

        private static Case E(string name, string? input) => new(name, input, false, IPAddress.Parse("0.0.0.0"));

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, IPAddress ExpectedIpAddress)
            : TryCase<string?, IPAddress>(Name, Input, ExpectedSuccess, ExpectedIpAddress);

        #endregion Cases
    }

    public static class TryParseIpv4
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("Parses dotted-quad", "127.0.0.1"),
            V("Parses dotted-quad with whitespace", " 127.0.0.1 "),
            V("Parses leading zeros", "001.002.003.004"),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            E("Null returns false", null),
            E("Empty returns false", string.Empty),
            E("Whitespace returns false", " "),
            E("Out-of-range octet returns false", "256.0.0.1"),
            E("Missing octet returns false", "127.0.0"),
            E("Extra octet returns false", "127.0.0.1.1"),
            E("Non-numeric returns false", "127.0.0.01a"),
            E("Empty segment returns false", "1..2.3"),
            E("Trailing dot returns false", "127.0.0."),
            E("Too many digits returns false", "0000.0.0.0"),
            E("Negative octet returns false", "-1.0.0.0"),
            E("Plus-signed octet returns false", "+1.0.0.0"),
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        private static Case V(string name, string? input) => new(name, input, true, IPAddress.Parse(input!.Trim()));

        private static Case E(string name, string? input) => new(name, input, false, IPAddress.Parse("0.0.0.0"));

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, IPAddress ExpectedIpAddress)
            : TryCase<string?, IPAddress>(Name, Input, ExpectedSuccess, ExpectedIpAddress);

        #endregion Cases
    }

    public static class TryParseIpv6
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("Parses IPv6 loopback", "::1"),
            V("Parses IPv6 with whitespace", " 2001:db8::1 "),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            E("Null returns false", null),
            E("Empty returns false", string.Empty),
            E("Whitespace returns false", " "),
            E("IPv4 is not IPv6", "127.0.0.1"),
            E("Not an IP returns false", "not-an-ip"),
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        private static Case V(string name, string? input) => new(name, input, true, IPAddress.Parse(input!.Trim()));

        private static Case E(string name, string? input) => new(name, input, false, IPAddress.Parse("0.0.0.0"));

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, IPAddress ExpectedIpAddress)
            : TryCase<string?, IPAddress>(Name, Input, ExpectedSuccess, ExpectedIpAddress);

        #endregion Cases
    }
}
