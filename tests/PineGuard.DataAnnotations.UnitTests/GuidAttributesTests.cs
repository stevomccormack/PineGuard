using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class GuidAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuidAttributesTestData.NotEmptyGuid.Cases), MemberType = typeof(GuidAttributesTestData.NotEmptyGuid))]
    public void NotEmptyGuid_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotEmptyGuidAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(GuidAttributesTestData.HasGuidVersion.Cases), MemberType = typeof(GuidAttributesTestData.HasGuidVersion))]
    public void HasGuidVersion_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, version) = ((Guid? value, int version))tc.Value!;
        var attr = new HasGuidVersionAttribute(version);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
