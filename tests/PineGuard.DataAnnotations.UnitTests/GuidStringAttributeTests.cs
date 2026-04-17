using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class GuidStringAttributeTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuidStringAttributeTestData.GuidString.Cases), MemberType = typeof(GuidStringAttributeTestData.GuidString))]
    public void GuidString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new GuidStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
