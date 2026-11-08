using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NetworkRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class NetworkAttributesTestData
{
    public static class IpAddress
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsIpAddress.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid IP address.", Code: MustCodes.Network.Address.Invalid)
        });
    }

    public static class Ipv4
    {
        public static TheoryData<DataAnnotationCase> Cases
        {
            get
            {
                var td = F.IsIpv4.AllScenarios.ToDataAnnotationCases(s => s.IsValid
                    ? new DataAnnotationExpected(true)
                    : new DataAnnotationExpected(false, "Value must be a valid IPv4 address.", Code: MustCodes.Network.Address.NotIpv4));
                td.Add(new DataAnnotationCase("NullValue", null, new DataAnnotationExpected(true)));
                return td;
            }
        }
    }

    public static class Ipv6
    {
        public static TheoryData<DataAnnotationCase> Cases
        {
            get
            {
                var td = F.IsIpv6.AllScenarios.ToDataAnnotationCases(s => s.IsValid
                    ? new DataAnnotationExpected(true)
                    : new DataAnnotationExpected(false, "Value must be a valid IPv6 address.", Code: MustCodes.Network.Address.NotIpv6));
                td.Add(new DataAnnotationCase("NullValue", null, new DataAnnotationExpected(true)));
                return td;
            }
        }
    }

    public static class InCidrRange
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsInCidr.AllScenarios
            .Except(nameof(F.IsInCidr.InvalidCidr), nameof(F.IsInCidr.CidrPrefixWithSign))
            .ToDataAnnotationCases(inputs => inputs.ip, s => s.Name switch
            {
                nameof(F.IsInCidr.NullIp) => new DataAnnotationExpected(true),
                _ when s.IsValid => new DataAnnotationExpected(true),
                _ => new DataAnnotationExpected(false, "Value must be within the specified CIDR range.", Code: MustCodes.Network.Cidr.OutOfRange)
            });
    }

    public static class Hostname
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsHostname.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsHostname.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid hostname.", Code: MustCodes.Network.Hostname.Invalid)
        });
    }

    public static class PortNumber
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsPortNumber.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsPortNumber.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid port number.", Code: MustCodes.Network.Port.Invalid)
        });
    }

    public static class MacAddress
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsMacAddress.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsMacAddress.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid MAC address.", Code: MustCodes.Network.Mac.Invalid)
        });
    }

    public static class CidrConfiguration
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("NullMalformedCidr", ((string?)null, "invalid"), new(false, "cidr must be a valid CIDR range.", Code: MustCodes.Network.Cidr.OutOfRange)),
            new("NullMissingCidr", ((string?)null, (string?)null), new(false, "cidr must be a valid CIDR range.", Code: MustCodes.Network.Cidr.OutOfRange)),
            new("OptionalValue", ((string?)null, "192.168.0.0/24"), new(true))
        ];
    }
}
