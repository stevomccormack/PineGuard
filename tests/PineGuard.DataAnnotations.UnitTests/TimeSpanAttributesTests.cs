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
    [MemberData(nameof(TimeSpanAttributesTestData.LongerThanTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.LongerThanTimeSpan))]
    public void LongerThanTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LongerThanTimeSpanAttribute("00:00:05");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanAttributesTestData.ShorterThanTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.ShorterThanTimeSpan))]
    public void ShorterThanTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new ShorterThanTimeSpanAttribute("00:00:10");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
