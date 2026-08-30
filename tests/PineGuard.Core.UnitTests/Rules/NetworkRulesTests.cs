using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class NetworkRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsIpAddress.Cases), MemberType = typeof(NetworkRulesTestData.IsIpAddress))]
    public void IsIpAddress_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = NetworkRules.IsIpAddress(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsIpv4.Cases), MemberType = typeof(NetworkRulesTestData.IsIpv4))]
    public void IsIpv4_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = NetworkRules.IsIpv4(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsIpv6.Cases), MemberType = typeof(NetworkRulesTestData.IsIpv6))]
    public void IsIpv6_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = NetworkRules.IsIpv6(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsInCidr.Cases), MemberType = typeof(NetworkRulesTestData.IsInCidr))]
    public void IsInCidr_BehavesAsExpected(RuleCase<(string? ip, string cidr)> tc)
    {
        // Act
        var result = NetworkRules.IsInCidr(tc.Value.ip, tc.Value.cidr);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsValidHostname.Cases), MemberType = typeof(NetworkRulesTestData.IsValidHostname))]
    public void IsValidHostname_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = NetworkRules.IsValidHostname(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsPortNumber.Cases), MemberType = typeof(NetworkRulesTestData.IsPortNumber))]
    public void IsPortNumber_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = NetworkRules.IsPortNumber(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsMacAddress.Cases), MemberType = typeof(NetworkRulesTestData.IsMacAddress))]
    public void IsMacAddress_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = NetworkRules.IsMacAddress(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
