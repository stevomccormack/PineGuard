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
            .Except(nameof(F.IsInCidr.InvalidCidr))
            .ToDataAnnotationCases(inputs => inputs.ip, s => s.Name switch
            {
                nameof(F.IsInCidr.NullIp) => new DataAnnotationExpected(true),
                _ when s.IsValid => new DataAnnotationExpected(true),
                _ => new DataAnnotationExpected(false, "Value must be within the specified CIDR range.", Code: MustCodes.Network.Cidr.OutOfRange)
            });
    }

    public static class Hostname
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsValidHostname.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsValidHostname.Null) => new DataAnnotationExpected(true),
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
}
