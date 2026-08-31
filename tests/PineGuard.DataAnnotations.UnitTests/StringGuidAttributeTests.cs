using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringGuidAttributeTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringGuidAttributeTestData.GuidString.Cases), MemberType = typeof(StringGuidAttributeTestData.GuidString))]
    public void GuidString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new StringGuidAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGuidAttributeTestData.HasGuidVersionString.Cases), MemberType = typeof(StringGuidAttributeTestData.HasGuidVersionString))]
    public void HasGuidVersionString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, version) = ((string? value, int version))tc.Value!;
        var attr = new HasGuidVersionStringAttribute(version);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
