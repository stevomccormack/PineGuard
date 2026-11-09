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
