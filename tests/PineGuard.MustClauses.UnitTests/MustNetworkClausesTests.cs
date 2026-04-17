using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustNetworkClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.IpAddress.ValidCases), MemberType = typeof(MustNetworkClausesTestData.IpAddress))]
    [MemberData(nameof(MustNetworkClausesTestData.IpAddress.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.IpAddress))]
    public void IpAddress_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.IpAddress(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv4.ValidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv4))]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv4.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv4))]
    public void Ipv4_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Ipv4(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv6.ValidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv6))]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv6.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv6))]
    public void Ipv6_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Ipv6(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.IpAddressString.ValidCases), MemberType = typeof(MustNetworkClausesTestData.IpAddressString))]
    [MemberData(nameof(MustNetworkClausesTestData.IpAddressString.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.IpAddressString))]
    public void IpAddressString_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.IpAddressString(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv4String.ValidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv4String))]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv4String.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv4String))]
    public void Ipv4String_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Ipv4String(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv6String.ValidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv6String))]
    [MemberData(nameof(MustNetworkClausesTestData.Ipv6String.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.Ipv6String))]
    public void Ipv6String_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Ipv6String(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.InCidrRange.ValidCases), MemberType = typeof(MustNetworkClausesTestData.InCidrRange))]
    [MemberData(nameof(MustNetworkClausesTestData.InCidrRange.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.InCidrRange))]
    public void InCidrRange_BehavesAsExpected(MustCase<(string? ip, string cidr)> tc)
    {
        var result = Must.Be.InCidrRange(tc.Value.ip, tc.Value.cidr, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.Hostname.ValidCases), MemberType = typeof(MustNetworkClausesTestData.Hostname))]
    [MemberData(nameof(MustNetworkClausesTestData.Hostname.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.Hostname))]
    public void Hostname_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Hostname(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.PortNumber.ValidCases), MemberType = typeof(MustNetworkClausesTestData.PortNumber))]
    [MemberData(nameof(MustNetworkClausesTestData.PortNumber.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.PortNumber))]
    public void PortNumber_BehavesAsExpected(MustCase<int?> tc)
    {
        var result = Must.Be.PortNumber(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpAddress.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpAddress))]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpAddress.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpAddress))]
    public void NotIpAddress_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotIpAddress(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv4.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv4))]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv4.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv4))]
    public void NotIpv4_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotIpv4(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv6.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv6))]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv6.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv6))]
    public void NotIpv6_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotIpv6(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpAddressString.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpAddressString))]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpAddressString.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpAddressString))]
    public void NotIpAddressString_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotIpAddressString(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv4String.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv4String))]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv4String.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv4String))]
    public void NotIpv4String_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotIpv4String(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv6String.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv6String))]
    [MemberData(nameof(MustNetworkClausesTestData.NotIpv6String.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotIpv6String))]
    public void NotIpv6String_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotIpv6String(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotInCidrRange.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotInCidrRange))]
    [MemberData(nameof(MustNetworkClausesTestData.NotInCidrRange.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotInCidrRange))]
    public void NotInCidrRange_BehavesAsExpected(MustCase<(string? ip, string cidr)> tc)
    {
        var result = Must.Be.NotInCidrRange(tc.Value.ip, tc.Value.cidr, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotHostname.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotHostname))]
    [MemberData(nameof(MustNetworkClausesTestData.NotHostname.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotHostname))]
    public void NotHostname_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotHostname(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNetworkClausesTestData.NotPortNumber.ValidCases), MemberType = typeof(MustNetworkClausesTestData.NotPortNumber))]
    [MemberData(nameof(MustNetworkClausesTestData.NotPortNumber.InvalidCases), MemberType = typeof(MustNetworkClausesTestData.NotPortNumber))]
    public void NotPortNumber_BehavesAsExpected(MustCase<int?> tc)
    {
        var result = Must.Be.NotPortNumber(tc.Value!.Value, paramName: "value");
        AssertResult(tc, result);
    }
}
