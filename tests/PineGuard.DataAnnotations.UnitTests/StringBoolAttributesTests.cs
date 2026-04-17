using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringBoolAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    // TrueStringAttribute
    [Theory]
    [MemberData(nameof(StringBoolAttributesTestData.TrueString.Cases), MemberType = typeof(StringBoolAttributesTestData.TrueString))]
    public void TrueString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new TrueStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    // FalseStringAttribute
    [Theory]
    [MemberData(nameof(StringBoolAttributesTestData.FalseString.Cases), MemberType = typeof(StringBoolAttributesTestData.FalseString))]
    public void FalseString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FalseStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
