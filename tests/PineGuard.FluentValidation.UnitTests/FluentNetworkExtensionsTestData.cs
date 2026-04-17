using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NetworkRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

#pragma warning disable CS0618

public static class FluentNetworkExtensionsTestData
{
    public static class IpAddress
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsIpAddress.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid IP address.")
        });
    }

    public static class NotIpAddress
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsIpAddress.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid IP address."),
            _ => new FluentExpected(true)
        });
    }

    public static class Ipv4
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv4.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be a valid IPv4 address."));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class NotIpv4
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv4.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(false, "Value must not be a valid IPv4 address.") : new FluentExpected(true));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class Ipv6
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv6.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be a valid IPv6 address."));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class NotIpv6
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv6.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(false, "Value must not be a valid IPv6 address.") : new FluentExpected(true));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class IpAddressString
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsIpAddress.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid IP address.")
        });
    }

    public static class NotIpAddressString
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsIpAddress.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid IP address."),
            _ => new FluentExpected(true)
        });
    }

    public static class Ipv4String
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv4.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be a valid IPv4 address."));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class NotIpv4String
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv4.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(false, "Value must not be a valid IPv4 address.") : new FluentExpected(true));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class Ipv6String
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv6.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be a valid IPv6 address."));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class NotIpv6String
    {
        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = F.IsIpv6.AllScenarios.ToFluentCases(s =>
                    s.IsValid ? new FluentExpected(false, "Value must not be a valid IPv6 address.") : new FluentExpected(true));
                td.Add(new FluentCase<string?>("NullValue", null, new FluentExpected(false, "Value must not be null.")));
                return td;
            }
        }
    }

    public static class Hostname
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsValidHostname.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsValidHostname.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid hostname.")
        });
    }

    public static class NotHostname
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsValidHostname.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsValidHostname.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid hostname."),
            _ => new FluentExpected(true)
        });
    }

    public static class InCidrRange
    {
        public static string Cidr => F.IsInCidr.InRange.cidr;

        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = new TheoryData<FluentCase<string?>>();
                foreach (var s in F.IsInCidr.AllScenarios.Except(nameof(F.IsInCidr.InvalidCidr)))
                {
                    var expected = s.Name switch
                    {
                        nameof(F.IsInCidr.NullIp) => new FluentExpected(false, "Value must not be null."),
                        _ when s.IsValid => new FluentExpected(true),
                        _ => new FluentExpected(false, "Value must be within the specified CIDR range.")
                    };
                    td.Add(new FluentCase<string?>(s.Name, s.Inputs.ip, expected));
                }
                return td;
            }
        }
    }

    public static class NotInCidrRange
    {
        public static string Cidr => F.IsInCidr.InRange.cidr;

        public static TheoryData<FluentCase<string?>> Cases
        {
            get
            {
                var td = new TheoryData<FluentCase<string?>>();
                foreach (var s in F.IsInCidr.AllScenarios.Except(nameof(F.IsInCidr.InvalidCidr)))
                {
                    var expected = s.Name switch
                    {
                        nameof(F.IsInCidr.NullIp) => new FluentExpected(false, "Value must not be null."),
                        _ when s.IsValid => new FluentExpected(false, "Value must not be within the specified CIDR range."),
                        _ => new FluentExpected(true)
                    };
                    td.Add(new FluentCase<string?>(s.Name, s.Inputs.ip, expected));
                }
                return td;
            }
        }
    }

    public static class PortNumber
    {
        public static TheoryData<FluentCase<int>> Cases => F.IsPortNumber.AllScenarios
            .Except(nameof(F.IsPortNumber.Null))
            .Project(inputs => inputs!.Value)
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be a valid port number."));
    }

    public static class NotPortNumber
    {
        public static TheoryData<FluentCase<int>> Cases => F.IsPortNumber.AllScenarios
            .Except(nameof(F.IsPortNumber.Null))
            .Project(inputs => inputs!.Value)
            .ToFluentCases(s => s.IsValid ? new FluentExpected(false, "Value must not be a valid port number.") : new FluentExpected(true));
    }

    public static class PortNumberNullable
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsPortNumber.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPortNumber.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid port number.")
        });
    }

    public static class NotPortNumberNullable
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsPortNumber.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsPortNumber.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid port number."),
            _ => new FluentExpected(true)
        });
    }
}
