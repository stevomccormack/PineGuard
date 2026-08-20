using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardNetworkClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardNetworkClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotIpAddress
    [Theory]
    [MemberData(nameof(TD.NotIpAddress.ValidCases), MemberType = typeof(TD.NotIpAddress))]
    [MemberData(nameof(TD.NotIpAddress.InvalidCases), MemberType = typeof(TD.NotIpAddress))]
    public void NotIpAddress_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotIpAddress(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotIpAddress(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    // Guard.Against.NotIpv4
    [Theory]
    [MemberData(nameof(TD.NotIpv4.ValidCases), MemberType = typeof(TD.NotIpv4))]
    [MemberData(nameof(TD.NotIpv4.InvalidCases), MemberType = typeof(TD.NotIpv4))]
    public void NotIpv4_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotIpv4(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotIpv4(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    // Guard.Against.NotIpv6
    [Theory]
    [MemberData(nameof(TD.NotIpv6.ValidCases), MemberType = typeof(TD.NotIpv6))]
    [MemberData(nameof(TD.NotIpv6.InvalidCases), MemberType = typeof(TD.NotIpv6))]
    public void NotIpv6_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotIpv6(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotIpv6(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    // Guard.Against.NotIpAddressString
    [Theory]
    [MemberData(nameof(TD.NotIpAddressString.ValidCases), MemberType = typeof(TD.NotIpAddressString))]
    [MemberData(nameof(TD.NotIpAddressString.InvalidCases), MemberType = typeof(TD.NotIpAddressString))]
    public void NotIpAddressString_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotIpAddressString(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotIpAddressString(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotIpv4String
    [Theory]
    [MemberData(nameof(TD.NotIpv4String.ValidCases), MemberType = typeof(TD.NotIpv4String))]
    [MemberData(nameof(TD.NotIpv4String.InvalidCases), MemberType = typeof(TD.NotIpv4String))]
    public void NotIpv4String_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotIpv4String(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotIpv4String(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotIpv6String
    [Theory]
    [MemberData(nameof(TD.NotIpv6String.ValidCases), MemberType = typeof(TD.NotIpv6String))]
    [MemberData(nameof(TD.NotIpv6String.InvalidCases), MemberType = typeof(TD.NotIpv6String))]
    public void NotIpv6String_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotIpv6String(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotIpv6String(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotInCidrRange
    [Theory]
    [MemberData(nameof(TD.NotInCidrRange.ValidCases), MemberType = typeof(TD.NotInCidrRange))]
    [MemberData(nameof(TD.NotInCidrRange.InvalidCases), MemberType = typeof(TD.NotInCidrRange))]
    public void NotInCidrRange_BehavesAsExpected(GuardCase<(string? ip, string cidr)> tc)
    {
        var value = tc.Value.ip;
        var result = AssertResult(tc, () => Guard.Against.NotInCidrRange(value!, tc.Value.cidr));
        AssertCustomMessage(tc, () => Guard.Against.NotInCidrRange(value!, tc.Value.cidr, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    // Guard.Against.NotHostname
    [Theory]
    [MemberData(nameof(TD.NotHostname.ValidCases), MemberType = typeof(TD.NotHostname))]
    [MemberData(nameof(TD.NotHostname.InvalidCases), MemberType = typeof(TD.NotHostname))]
    public void NotHostname_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHostname(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotHostname(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.NotNull(result);
    }

    // Guard.Against.NotPortNumber
    [Theory]
    [MemberData(nameof(TD.NotPortNumber.ValidCases), MemberType = typeof(TD.NotPortNumber))]
    [MemberData(nameof(TD.NotPortNumber.InvalidCases), MemberType = typeof(TD.NotPortNumber))]
    public void NotPortNumber_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotPortNumber(value));
        AssertCustomMessage(tc, () => Guard.Against.NotPortNumber(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.IpAddress
    [Theory]
    [MemberData(nameof(TD.IpAddress.ValidCases), MemberType = typeof(TD.IpAddress))]
    [MemberData(nameof(TD.IpAddress.InvalidCases), MemberType = typeof(TD.IpAddress))]
    public void IpAddress_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.IpAddress(value!));
        AssertCustomMessage(tc, () => Guard.Against.IpAddress(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Ipv4
    [Theory]
    [MemberData(nameof(TD.Ipv4.ValidCases), MemberType = typeof(TD.Ipv4))]
    [MemberData(nameof(TD.Ipv4.InvalidCases), MemberType = typeof(TD.Ipv4))]
    public void Ipv4_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Ipv4(value!));
        AssertCustomMessage(tc, () => Guard.Against.Ipv4(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Ipv6
    [Theory]
    [MemberData(nameof(TD.Ipv6.ValidCases), MemberType = typeof(TD.Ipv6))]
    [MemberData(nameof(TD.Ipv6.InvalidCases), MemberType = typeof(TD.Ipv6))]
    public void Ipv6_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Ipv6(value!));
        AssertCustomMessage(tc, () => Guard.Against.Ipv6(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.IpAddressString
    [Theory]
    [MemberData(nameof(TD.IpAddressString.ValidCases), MemberType = typeof(TD.IpAddressString))]
    [MemberData(nameof(TD.IpAddressString.InvalidCases), MemberType = typeof(TD.IpAddressString))]
    public void IpAddressString_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.IpAddressString(value!));
        AssertCustomMessage(tc, () => Guard.Against.IpAddressString(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Ipv4String
    [Theory]
    [MemberData(nameof(TD.Ipv4String.ValidCases), MemberType = typeof(TD.Ipv4String))]
    [MemberData(nameof(TD.Ipv4String.InvalidCases), MemberType = typeof(TD.Ipv4String))]
    public void Ipv4String_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Ipv4String(value!));
        AssertCustomMessage(tc, () => Guard.Against.Ipv4String(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Ipv6String
    [Theory]
    [MemberData(nameof(TD.Ipv6String.ValidCases), MemberType = typeof(TD.Ipv6String))]
    [MemberData(nameof(TD.Ipv6String.InvalidCases), MemberType = typeof(TD.Ipv6String))]
    public void Ipv6String_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Ipv6String(value!));
        AssertCustomMessage(tc, () => Guard.Against.Ipv6String(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.InCidrRange
    [Theory]
    [MemberData(nameof(TD.InCidrRange.ValidCases), MemberType = typeof(TD.InCidrRange))]
    [MemberData(nameof(TD.InCidrRange.InvalidCases), MemberType = typeof(TD.InCidrRange))]
    public void InCidrRange_BehavesAsExpected(GuardCase<(string? ip, string cidr)> tc)
    {
        var value = tc.Value.ip;
        var result = AssertResult(tc, () => Guard.Against.InCidrRange(value!, tc.Value.cidr));
        AssertCustomMessage(tc, () => Guard.Against.InCidrRange(value!, tc.Value.cidr, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Hostname
    [Theory]
    [MemberData(nameof(TD.Hostname.ValidCases), MemberType = typeof(TD.Hostname))]
    [MemberData(nameof(TD.Hostname.InvalidCases), MemberType = typeof(TD.Hostname))]
    public void Hostname_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Hostname(value!));
        AssertCustomMessage(tc, () => Guard.Against.Hostname(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.PortNumber
    [Theory]
    [MemberData(nameof(TD.PortNumber.ValidCases), MemberType = typeof(TD.PortNumber))]
    [MemberData(nameof(TD.PortNumber.InvalidCases), MemberType = typeof(TD.PortNumber))]
    public void PortNumber_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.PortNumber(value));
        AssertCustomMessage(tc, () => Guard.Against.PortNumber(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
