using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TimeSpanAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TimeSpanAttributesTestData.InRangeTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.InRangeTimeSpan))]
    public void InRangeTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        InRangeTimeSpanAttribute attr = new InRangeTimeSpanAttribute("00:00:01", "00:00:10");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanAttributesTestData.OutOfRangeTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.OutOfRangeTimeSpan))]
    public void OutOfRangeTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        OutOfRangeTimeSpanAttribute attr = new OutOfRangeTimeSpanAttribute("00:00:01", "00:00:10");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanAttributesTestData.GreaterThanTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.GreaterThanTimeSpan))]
    public void GreaterThanTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new GreaterThanTimeSpanAttribute("00:00:05");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanAttributesTestData.LessThanTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.LessThanTimeSpan))]
    public void LessThanTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LessThanTimeSpanAttribute("00:00:10");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
