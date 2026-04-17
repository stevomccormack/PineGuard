using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TimeSpanAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TimeSpanAttributesTestData.DurationBetweenTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.DurationBetweenTimeSpan))]
    public void DurationBetweenTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new DurationBetweenTimeSpanAttribute("00:00:01", "00:00:10");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanAttributesTestData.NotDurationBetweenTimeSpan.Cases), MemberType = typeof(TimeSpanAttributesTestData.NotDurationBetweenTimeSpan))]
    public void NotDurationBetweenTimeSpan_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotDurationBetweenTimeSpanAttribute("00:00:01", "00:00:10");
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
