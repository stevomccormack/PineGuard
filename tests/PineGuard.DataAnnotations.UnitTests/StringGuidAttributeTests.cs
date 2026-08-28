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
}
