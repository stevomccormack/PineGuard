using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NetworkRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardNetworkClausesTestData
{
    // Guard.Against.NotIpAddress — throws when value is NOT a valid IP address
    public static class NotIpAddress
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpAddress.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsIpAddress.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    // Guard.Against.NotIpv4 — throws when value is NOT a valid IPv4 address
    public static class NotIpv4
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv4.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsIpv4.InvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotIpv6 — throws when value is NOT a valid IPv6 address
    public static class NotIpv6
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv6.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsIpv6.InvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotIpAddressString — throws when value is NOT a valid IP address string
    public static class NotIpAddressString
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpAddress.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsIpAddress.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.IsIpAddress.Null) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    // Guard.Against.NotIpv4String — throws when value is NOT a valid IPv4 string
    public static class NotIpv4String
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv4.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsIpv4.InvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotIpv6String — throws when value is NOT a valid IPv6 string
    public static class NotIpv6String
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv6.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsIpv6.InvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotInCidrRange — throws when value is NOT in the CIDR range
    public static class NotInCidrRange
    {
        public static TheoryData<GuardCase<(string? ip, string cidr)>> ValidCases =>
            F.IsInCidr.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? ip, string cidr)>> InvalidCases =>
            F.IsInCidr.InvalidScenarios.ToGuardCases(s => s.Name switch
            {
                nameof(F.IsInCidr.NullIp) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
                _ => new GuardExpected(false, typeof(ArgumentException), "value")
            });
    }

    // Guard.Against.NotHostname — throws when value is NOT a valid hostname
    public static class NotHostname
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsValidHostname.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsValidHostname.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.IsValidHostname.Null) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    // Guard.Against.NotPortNumber — throws when value is NOT a valid port number
    public static class NotPortNumber
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsPortNumber.AllValid.Where(s => s.Inputs.HasValue)
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsPortNumber.AllInvalid.Where(s => s.Inputs.HasValue)
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.IpAddress — throws when value IS a valid IP address
    public static class IpAddress
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
            F.IsIpAddress.InvalidScenarios.Except(nameof(F.IsIpAddress.Null)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpAddress.Ipv4Loopback), F.IsIpAddress.Ipv4Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Ipv6Loopback), F.IsIpAddress.Ipv6Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Null), F.IsIpAddress.Null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Ipv4 — throws when value IS a valid IPv4 address
    public static class Ipv4
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv4.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpv4.Loopback), F.IsIpv4.Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Null), null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Ipv6 — throws when value IS a valid IPv6 address
    public static class Ipv6
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv6.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpv6.Loopback), F.IsIpv6.Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Null), null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.IpAddressString — throws when value IS a valid IP address string
    public static class IpAddressString
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
            F.IsIpAddress.InvalidScenarios.Except(nameof(F.IsIpAddress.Null)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpAddress.Ipv4Loopback), F.IsIpAddress.Ipv4Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Ipv6Loopback), F.IsIpAddress.Ipv6Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Null), F.IsIpAddress.Null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Ipv4String — throws when value IS a valid IPv4 string
    public static class Ipv4String
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv4.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpv4.Loopback), F.IsIpv4.Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Null), null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Ipv6String — throws when value IS a valid IPv6 string
    public static class Ipv6String
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsIpv6.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsIpv6.Loopback), F.IsIpv6.Loopback, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsIpAddress.Null), null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.InCidrRange — throws when value IS in the CIDR range
    public static class InCidrRange
    {
        public static TheoryData<GuardCase<(string? ip, string cidr)>> ValidCases =>
            F.IsInCidr.InvalidScenarios.Except(nameof(F.IsInCidr.NullIp)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? ip, string cidr)>> InvalidCases =>
        [
            new(nameof(F.IsInCidr.InRange), F.IsInCidr.InRange, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsInCidr.NullIp), F.IsInCidr.NullIp, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.Hostname — throws when value IS a valid hostname
    public static class Hostname
    {
        public static TheoryData<GuardCase<string?>> ValidCases =>
            F.IsValidHostname.InvalidScenarios.Except(nameof(F.IsValidHostname.Null)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.IsValidHostname.Simple),      F.IsValidHostname.Simple,      new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsValidHostname.SingleLabel),  F.IsValidHostname.SingleLabel,  new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsValidHostname.Trimmed),      F.IsValidHostname.Trimmed,      new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsValidHostname.TrailingDot),  F.IsValidHostname.TrailingDot,  new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsValidHostname.WithHyphen),   F.IsValidHostname.WithHyphen,   new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsValidHostname.Null),         F.IsValidHostname.Null,         new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    // Guard.Against.PortNumber — throws when value IS a valid port number
    public static class PortNumber
    {
        public static TheoryData<GuardCase<int>> ValidCases =>
            F.IsPortNumber.AllInvalid.Where(s => s.Inputs.HasValue)
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<int>> InvalidCases =>
            F.IsPortNumber.AllValid.Where(s => s.Inputs.HasValue)
            .Select(s => new RuleScenario<int>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
