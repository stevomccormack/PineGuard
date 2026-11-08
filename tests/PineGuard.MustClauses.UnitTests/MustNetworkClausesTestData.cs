using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NetworkRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustNetworkClausesTestData
{
    public static class IpAddress
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpAddress.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsIpAddress.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid IP address.")
        });
    }

    public static class Ipv4
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpv4.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases
        {
            get
            {
                var data = F.IsIpv4.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be a valid IPv4 address."));
                data.Add(new MustCase<string?>(nameof(F.IsIpAddress.Null), null, new MustExpected(false, "value must not be null.", "value")));
                return data;
            }
        }
    }

    public static class Ipv6
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpv6.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases
        {
            get
            {
                var data = F.IsIpv6.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must be a valid IPv6 address."));
                data.Add(new MustCase<string?>(nameof(F.IsIpAddress.Null), null, new MustExpected(false, "value must not be null.", "value")));
                return data;
            }
        }
    }

    public static class InCidrRange
    {
        public static TheoryData<MustCase<(string? ip, string cidr)>> ValidCases => F.IsInCidr.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(string? ip, string cidr)>> InvalidCases
        {
            get
            {
                var data = F.IsInCidr.InvalidScenarios.ToMustCases(s => s.Name switch
                {
                    nameof(F.IsInCidr.NullIp) => new MustExpected(false, "value must not be null.", "value"),
                    nameof(F.IsInCidr.InvalidCidr) or nameof(F.IsInCidr.CidrPrefixWithSign) => new MustExpected(false, "cidr must be a valid CIDR range.", "cidr"),
                    _ => new MustExpected(false, "value must be within the specified CIDR range.")
                });
                data.Add(new MustCase<(string? ip, string cidr)>(nameof(F.IsInCidr.EmptyCidr), F.IsInCidr.EmptyCidr, new MustExpected(false, "cidr must be a valid CIDR range.", "cidr")));
                return data;
            }
        }
    }

    public static class Hostname
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHostname.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsHostname.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsHostname.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid hostname.")
        });
    }

    public static class PortNumber
    {
        public static TheoryData<MustCase<int?>> ValidCases => F.IsPortNumber.AllValid.ToMustCases();

        public static TheoryData<MustCase<int?>> InvalidCases => F.IsPortNumber.AllInvalid.Except(nameof(F.IsPortNumber.Null)).ToMustCases(_ => new MustExpected(false, "value must be a valid port number."));
    }

    public static class MacAddress
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsMacAddress.AllValid.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsMacAddress.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsMacAddress.Null) => new MustExpected(false, "value must not be null.", "value", MustCodes.Network.Mac.Invalid),
            _ => new MustExpected(false, "value must be a valid MAC address.", Code: MustCodes.Network.Mac.Invalid)
        });
    }

    public static class NotIpAddress
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpAddress.InvalidScenarios.Except(nameof(F.IsIpAddress.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpAddress.Ipv4Loopback), F.IsIpAddress.Ipv4Loopback, new MustExpected(false, "value must not be a valid IP address.")),
            new(nameof(F.IsIpAddress.Ipv6Loopback), F.IsIpAddress.Ipv6Loopback, new MustExpected(false, "value must not be a valid IP address.")),
            new(nameof(F.IsIpAddress.Null), F.IsIpAddress.Null, new MustExpected(false, "value must not be null.", "value"))
        ];
    }

    public static class NotIpv4
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpv4.InvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpv4.Loopback), F.IsIpv4.Loopback, new MustExpected(false, "value must not be a valid IPv4 address.")),
            new(nameof(F.IsIpAddress.Null), F.IsIpAddress.Null, new MustExpected(false, "value must not be null.", "value"))
        ];
    }

    public static class NotIpv6
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpv6.InvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpv6.Loopback), F.IsIpv6.Loopback, new MustExpected(false, "value must not be a valid IPv6 address.")),
            new(nameof(F.IsIpAddress.Null), F.IsIpAddress.Null, new MustExpected(false, "value must not be null.", "value"))
        ];
    }

    public static class NotInCidrRange
    {
        public static TheoryData<MustCase<(string? ip, string cidr)>> ValidCases => F.IsInCidr.InvalidScenarios.Where(s => s.Name == nameof(F.IsInCidr.OutOfRange)).ToArray().ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(string? ip, string cidr)>> InvalidCases =>
        [
            new(nameof(F.IsInCidr.InRange), F.IsInCidr.InRange, new MustExpected(false, "value must not be within the specified CIDR range.")),
            new(nameof(F.IsInCidr.InvalidIp), F.IsInCidr.InvalidIp, new MustExpected(false, "value must not be within the specified CIDR range.")),
            new(nameof(F.IsInCidr.InvalidCidr), F.IsInCidr.InvalidCidr, new MustExpected(false, "cidr must be a valid CIDR range.", "cidr")),
            new(nameof(F.IsInCidr.CidrPrefixWithSign), F.IsInCidr.CidrPrefixWithSign, new MustExpected(false, "cidr must be a valid CIDR range.", "cidr")),
            new(nameof(F.IsInCidr.NullIp), F.IsInCidr.NullIp, new MustExpected(false, "value must not be null.", "value")),
            new(nameof(F.IsInCidr.EmptyCidr), F.IsInCidr.EmptyCidr, new MustExpected(false, "cidr must be a valid CIDR range.", "cidr"))
        ];
    }

    public static class NotHostname
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHostname.InvalidScenarios.Except(nameof(F.IsHostname.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsHostname.Simple), F.IsHostname.Simple, new MustExpected(false, "value must not be a valid hostname.")),
            new(nameof(F.IsHostname.Null), F.IsHostname.Null, new MustExpected(false, "value must not be null.", "value"))
        ];
    }

    public static class NotPortNumber
    {
        public static TheoryData<MustCase<int?>> ValidCases => F.IsPortNumber.AllInvalid.Except(nameof(F.IsPortNumber.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<int?>> InvalidCases =>
        [
            new(nameof(F.IsPortNumber.Mid), F.IsPortNumber.Mid, new MustExpected(false, "value must not be a valid port number."))
        ];
    }

    public static class CidrConfiguration
    {
        public static TheoryData<MustCase<Func<MustResult<System.Net.IPAddress>>>> Cases =>
        [
            new("PositiveNullInputInvalidCidr", () => Must.Be.InCidrRange(null, "invalid"), new(false, "cidr must be a valid CIDR range.", "cidr", MustCodes.Network.Cidr.OutOfRange)),
            new("NegativeNullInputInvalidCidr", () => Must.Be.NotInCidrRange(null, "invalid"), new(false, "cidr must be a valid CIDR range.", "cidr", MustCodes.Network.Cidr.InRange)),
            new("PositiveNullCidr", () => Must.Be.InCidrRange("127.0.0.1", null!), new(false, "cidr must be a valid CIDR range.", "cidr", MustCodes.Network.Cidr.OutOfRange)),
            new("NegativeNullCidr", () => Must.Be.NotInCidrRange("127.0.0.1", null!), new(false, "cidr must be a valid CIDR range.", "cidr", MustCodes.Network.Cidr.InRange))
        ];
    }
}
