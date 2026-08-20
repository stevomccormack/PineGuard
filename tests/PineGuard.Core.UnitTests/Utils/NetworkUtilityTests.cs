using System.Net;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class NetworkUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpAddress.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpAddress))]
    public void TryParseIpAddress_ReturnsTrue_ForParseableInputs(NetworkUtilityTestData.TryParseIpAddress.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseIpAddress(testCase.Value, out var ip);

        // Assert
        Assert.True(result);
        Assert.NotNull(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpAddress.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpAddress))]
    public void TryParseIpAddress_ReturnsFalse_ForInvalidInputs(NetworkUtilityTestData.TryParseIpAddress.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseIpAddress(testCase.Value, out var ip);

        // Assert
        Assert.False(result);
        Assert.Null(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv4.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv4))]
    public void TryParseIpv4_ReturnsTrue_ForStrictDottedQuad(NetworkUtilityTestData.TryParseIpv4.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseIpv4(testCase.Value, out var ip);

        // Assert
        Assert.True(result);
        Assert.NotNull(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv4.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv4))]
    public void TryParseIpv4_ReturnsFalse_ForInvalidInputs(NetworkUtilityTestData.TryParseIpv4.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseIpv4(testCase.Value, out var ip);

        // Assert
        Assert.False(result);
        Assert.Null(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv6.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv6))]
    public void TryParseIpv6_ReturnsTrue_ForIpv6Addresses(NetworkUtilityTestData.TryParseIpv6.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseIpv6(testCase.Value, out var ip);

        // Assert
        Assert.True(result);
        Assert.NotNull(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseIpv6.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryParseIpv6))]
    public void TryParseIpv6_ReturnsFalse_ForInvalidOrNonIpv6Inputs(NetworkUtilityTestData.TryParseIpv6.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseIpv6(testCase.Value, out var ip);

        // Assert
        Assert.False(result);
        Assert.Null(ip);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseCidr.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryParseCidr))]
    public void TryParseCidr_ReturnsTrue_ForValidCidr(NetworkUtilityTestData.TryParseCidr.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseCidr(testCase.Value, out var network, out var prefixLength);

        // Assert
        Assert.True(result);
        Assert.NotNull(network);
        Assert.InRange(prefixLength, 0, 128);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryParseCidr.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryParseCidr))]
    public void TryParseCidr_ReturnsFalse_ForInvalidCidr(NetworkUtilityTestData.TryParseCidr.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryParseCidr(testCase.Value, out var network, out var prefixLength);

        // Assert
        Assert.False(result);
        Assert.Null(network);
        Assert.Equal(0, prefixLength);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.IsInCidr.ValidCases), MemberType = typeof(NetworkUtilityTestData.IsInCidr))]
    [MemberData(nameof(NetworkUtilityTestData.IsInCidr.EdgeCases), MemberType = typeof(NetworkUtilityTestData.IsInCidr))]
    public void IsInCidr_ReturnsExpected(NetworkUtilityTestData.IsInCidr.ValidCase testCase)
    {
        // Arrange
        // Parsing IP inputs first
        Assert.True(IPAddress.TryParse(testCase.Value.Ip, out var ip));
        Assert.True(IPAddress.TryParse(testCase.Value.NetworkIp, out var network));

        // Act
        var result = NetworkUtility.IsInCidr(ip, network, testCase.Value.PrefixLength);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryGetAsciiHostname.ValidCases), MemberType = typeof(NetworkUtilityTestData.TryGetAsciiHostname))]
    public void TryGetAsciiHostname_ReturnsTrue_ForValidHostnames(NetworkUtilityTestData.TryGetAsciiHostname.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryGetAsciiHostname(testCase.Value, out var hostname);

        // Assert
        Assert.True(result);
        Assert.NotNull(hostname);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.TryGetAsciiHostname.EdgeCases), MemberType = typeof(NetworkUtilityTestData.TryGetAsciiHostname))]
    public void TryGetAsciiHostname_ReturnsFalse_ForInvalidHostnames(NetworkUtilityTestData.TryGetAsciiHostname.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.TryGetAsciiHostname(testCase.Value, out var hostname);

        // Assert
        Assert.False(result);
        Assert.Null(hostname);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.ValidateHostnameLabels.ValidCases), MemberType = typeof(NetworkUtilityTestData.ValidateHostnameLabels))]
    public void ValidateHostnameLabels_ReturnsTrue_ForValidLabels(NetworkUtilityTestData.ValidateHostnameLabels.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.ValidateHostnameLabels(testCase.Value);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.ValidateHostnameLabels.EdgeCases), MemberType = typeof(NetworkUtilityTestData.ValidateHostnameLabels))]
    public void ValidateHostnameLabels_ReturnsFalse_ForInvalidLabels(NetworkUtilityTestData.ValidateHostnameLabels.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.ValidateHostnameLabels(testCase.Value);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.IsValidHostnameLabel.ValidCases), MemberType = typeof(NetworkUtilityTestData.IsValidHostnameLabel))]
    public void IsValidHostnameLabel_ReturnsTrue_ForValidLabels(NetworkUtilityTestData.IsValidHostnameLabel.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.IsValidHostnameLabel(testCase.Value);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.IsValidHostnameLabel.EdgeCases), MemberType = typeof(NetworkUtilityTestData.IsValidHostnameLabel))]
    public void IsValidHostnameLabel_ReturnsFalse_ForInvalidLabels(NetworkUtilityTestData.IsValidHostnameLabel.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.IsValidHostnameLabel(testCase.Value);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.IsValidIpv4Segment.ValidCases), MemberType = typeof(NetworkUtilityTestData.IsValidIpv4Segment))]
    public void IsValidIpv4Segment_ReturnsTrue_ForValidSegments(NetworkUtilityTestData.IsValidIpv4Segment.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.IsValidIpv4Segment(testCase.Value);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NetworkUtilityTestData.IsValidIpv4Segment.EdgeCases), MemberType = typeof(NetworkUtilityTestData.IsValidIpv4Segment))]
    public void IsValidIpv4Segment_ReturnsFalse_ForInvalidSegments(NetworkUtilityTestData.IsValidIpv4Segment.ValidCase testCase)
    {
        // Act
        var result = NetworkUtility.IsValidIpv4Segment(testCase.Value);

        // Assert
        Assert.False(result);
    }
}
