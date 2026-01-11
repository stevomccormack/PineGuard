using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class NetworkRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsIpAddress.ValidCases), MemberType = typeof(NetworkRulesTestData.IsIpAddress))]
    [MemberData(nameof(NetworkRulesTestData.IsIpAddress.EdgeCases), MemberType = typeof(NetworkRulesTestData.IsIpAddress))]
    public void IsIpAddress_ReturnsExpected(NetworkRulesTestData.IsIpAddress.Case testCase)
    {
        var result = NetworkRules.IsIpAddress(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsIpv4.ValidCases), MemberType = typeof(NetworkRulesTestData.IsIpv4))]
    [MemberData(nameof(NetworkRulesTestData.IsIpv4.EdgeCases), MemberType = typeof(NetworkRulesTestData.IsIpv4))]
    public void IsIpv4_ReturnsExpected(NetworkRulesTestData.IsIpv4.Case testCase)
    {
        var result = NetworkRules.IsIpv4(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(NetworkRulesTestData.IsIpv6.ValidCases), MemberType = typeof(NetworkRulesTestData.IsIpv6))]
    [MemberData(nameof(NetworkRulesTestData.IsIpv6.EdgeCases), MemberType = typeof(NetworkRulesTestData.IsIpv6))]
    public void IsIpv6_ReturnsExpected(NetworkRulesTestData.IsIpv6.Case testCase)
    {
        var result = NetworkRules.IsIpv6(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
