using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class NetworkUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpAddress.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpAddress))]
    public void TryParseIpAddress_ReturnsTrue_ForParseableInputs(NetworkUtilityTestData.TryParseIpAddress.Case testCase)
    {
        var result = NetworkUtility.TryParseIpAddress(testCase.Input, out var ip);

        Assert.True(result);
        Assert.NotNull(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpAddress.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpAddress))]
    public void TryParseIpAddress_ReturnsFalse_ForInvalidInputs(NetworkUtilityTestData.TryParseIpAddress.Case testCase)
    {
        var result = NetworkUtility.TryParseIpAddress(testCase.Input, out var ip);

        Assert.False(result);
        Assert.Null(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv4.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv4))]
    public void TryParseIpv4_ReturnsTrue_ForStrictDottedQuad(NetworkUtilityTestData.TryParseIpv4.Case testCase)
    {
        var result = NetworkUtility.TryParseIpv4(testCase.Input, out var ip);

        Assert.True(result);
        Assert.NotNull(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv4.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv4))]
    public void TryParseIpv4_ReturnsFalse_ForInvalidInputs(NetworkUtilityTestData.TryParseIpv4.Case testCase)
    {
        var result = NetworkUtility.TryParseIpv4(testCase.Input, out var ip);

        Assert.False(result);
        Assert.Null(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv6.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv6))]
    public void TryParseIpv6_ReturnsTrue_ForIpv6Addresses(NetworkUtilityTestData.TryParseIpv6.Case testCase)
    {
        var result = NetworkUtility.TryParseIpv6(testCase.Input, out var ip);

        Assert.True(result);
        Assert.NotNull(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv6.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv6))]
    public void TryParseIpv6_ReturnsFalse_ForInvalidOrNonIpv6Inputs(NetworkUtilityTestData.TryParseIpv6.Case testCase)
    {
        var result = NetworkUtility.TryParseIpv6(testCase.Input, out var ip);

        Assert.False(result);
        Assert.Null(ip);
    }
}
