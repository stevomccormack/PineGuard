using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DecimalAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DecimalAttributesTestData.ScaleAtMost.Cases), MemberType = typeof(DecimalAttributesTestData.ScaleAtMost))]
    public void ScaleAtMost_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, scale) = ((decimal? value, int scale))tc.Value!;
        var attr = new ScaleAtMostAttribute(scale);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DecimalAttributesTestData.PrecisionAtMost.Cases), MemberType = typeof(DecimalAttributesTestData.PrecisionAtMost))]
    public void PrecisionAtMost_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, precision) = ((decimal? value, int precision))tc.Value!;
        var attr = new PrecisionAtMostAttribute(precision);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DecimalAttributesTestData.WithinPrecision.Cases), MemberType = typeof(DecimalAttributesTestData.WithinPrecision))]
    public void WithinPrecision_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, precision, scale) = ((decimal? value, int precision, int scale))tc.Value!;
        var attr = new WithinPrecisionAttribute(precision, scale);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
