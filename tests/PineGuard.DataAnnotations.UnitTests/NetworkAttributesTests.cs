using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class NetworkAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(NetworkAttributesTestData.IpAddress.Cases), MemberType = typeof(NetworkAttributesTestData.IpAddress))]
    public void IpAddress_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new IpAddressAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(NetworkAttributesTestData.Ipv4.Cases), MemberType = typeof(NetworkAttributesTestData.Ipv4))]
    public void Ipv4_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Ipv4Attribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(NetworkAttributesTestData.Ipv6.Cases), MemberType = typeof(NetworkAttributesTestData.Ipv6))]
    public void Ipv6_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Ipv6Attribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(NetworkAttributesTestData.InCidrRange.Cases), MemberType = typeof(NetworkAttributesTestData.InCidrRange))]
    public void InCidrRange_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new InCidrRangeAttribute("192.168.1.0/24");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(NetworkAttributesTestData.Hostname.Cases), MemberType = typeof(NetworkAttributesTestData.Hostname))]
    public void Hostname_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new HostnameAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(NetworkAttributesTestData.PortNumber.Cases), MemberType = typeof(NetworkAttributesTestData.PortNumber))]
    public void PortNumber_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new PortNumberAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(NetworkAttributesTestData.MacAddress.Cases), MemberType = typeof(NetworkAttributesTestData.MacAddress))]
    public void MacAddress_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new MacAddressAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
