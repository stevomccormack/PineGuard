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

    public static class IpAddressString
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpAddress.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsIpAddress.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid IP address.")
        });
    }

    public static class Ipv4String
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

    public static class Ipv6String
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
                    _ => new MustExpected(false, "value must be within the specified CIDR range.")
                });
                data.Add(new MustCase<(string? ip, string cidr)>(nameof(F.IsInCidr.EmptyCidr), F.IsInCidr.EmptyCidr, new MustExpected(false, "cidr must not be null or whitespace.", "cidr")));
                return data;
            }
        }
    }

    public static class Hostname
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsValidHostname.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsValidHostname.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsValidHostname.Null) => new MustExpected(false, "value must not be null.", "value"),
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

    public static class NotIpAddressString
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpAddress.InvalidScenarios.Except(nameof(F.IsIpAddress.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpAddress.Ipv4Loopback), F.IsIpAddress.Ipv4Loopback, new MustExpected(false, "value must not be a valid IP address.")),
            new(nameof(F.IsIpAddress.Ipv6Loopback), F.IsIpAddress.Ipv6Loopback, new MustExpected(false, "value must not be a valid IP address.")),
            new(nameof(F.IsIpAddress.Null), F.IsIpAddress.Null, new MustExpected(false, "value must not be null.", "value"))
        ];
    }

    public static class NotIpv4String
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsIpv4.InvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpv4.Loopback), F.IsIpv4.Loopback, new MustExpected(false, "value must not be a valid IPv4 address.")),
            new(nameof(F.IsIpAddress.Null), F.IsIpAddress.Null, new MustExpected(false, "value must not be null.", "value"))
        ];
    }

    public static class NotIpv6String
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
        public static TheoryData<MustCase<(string? ip, string cidr)>> ValidCases => F.IsInCidr.InvalidScenarios.Except(nameof(F.IsInCidr.NullIp), nameof(F.IsInCidr.InvalidCidr)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(string? ip, string cidr)>> InvalidCases =>
        [
            new(nameof(F.IsInCidr.InRange), F.IsInCidr.InRange, new MustExpected(false, "value must not be within the specified CIDR range.")),
            new(nameof(F.IsInCidr.NullIp), F.IsInCidr.NullIp, new MustExpected(false, "value must not be null.", "value")),
            new(nameof(F.IsInCidr.EmptyCidr), F.IsInCidr.EmptyCidr, new MustExpected(false, "cidr must not be null or whitespace.", "cidr"))
        ];
    }

    public static class NotHostname
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsValidHostname.InvalidScenarios.Except(nameof(F.IsValidHostname.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsValidHostname.Simple), F.IsValidHostname.Simple, new MustExpected(false, "value must not be a valid hostname.")),
            new(nameof(F.IsValidHostname.Null), F.IsValidHostname.Null, new MustExpected(false, "value must not be null.", "value"))
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
}
