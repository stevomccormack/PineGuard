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

    // Guard.Against.NotMacAddress
    [Theory]
    [MemberData(nameof(TD.NotMacAddress.ValidCases), MemberType = typeof(TD.NotMacAddress))]
    [MemberData(nameof(TD.NotMacAddress.InvalidCases), MemberType = typeof(TD.NotMacAddress))]
    public void NotMacAddress_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotMacAddress(value));
        AssertCustomMessage(tc, () => Guard.Against.NotMacAddress(value, message: CustomMessage));
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

    // Guard.Against.InCidrRange
    [Theory]
    [MemberData(nameof(TD.InCidrRange.ValidCases), MemberType = typeof(TD.InCidrRange))]
    [MemberData(nameof(TD.InCidrRange.InvalidCases), MemberType = typeof(TD.InCidrRange))]
    public void InCidrRange_BehavesAsExpected(GuardCase<(string? ip, string cidr)> tc)
    {
        var value = tc.Value.ip;
        var result = AssertResult(tc, () => Guard.Against.InCidrRange(value!, tc.Value.cidr));
        AssertCustomMessage(tc, () => Guard.Against.InCidrRange(value!, tc.Value.cidr, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(System.Net.IPAddress.Parse(value!), result);
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
