using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringDateTimeOffsetAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    // PastDateTimeOffsetStringAttribute
    [Theory]
    [MemberData(nameof(StringDateTimeOffsetAttributesTestData.PastDateTimeOffsetString.Cases), MemberType = typeof(StringDateTimeOffsetAttributesTestData.PastDateTimeOffsetString))]
    public void PastDateTimeOffsetString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new PastDateTimeOffsetStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    // FutureDateTimeOffsetStringAttribute
    [Theory]
    [MemberData(nameof(StringDateTimeOffsetAttributesTestData.FutureDateTimeOffsetString.Cases), MemberType = typeof(StringDateTimeOffsetAttributesTestData.FutureDateTimeOffsetString))]
    public void FutureDateTimeOffsetString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FutureDateTimeOffsetStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    // BetweenDateTimeOffsetStringAttribute
    [Theory]
    [MemberData(nameof(StringDateTimeOffsetAttributesTestData.BetweenDateTimeOffsetString.Cases), MemberType = typeof(StringDateTimeOffsetAttributesTestData.BetweenDateTimeOffsetString))]
    public void BetweenDateTimeOffsetString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BetweenDateTimeOffsetStringAttribute("2020-01-01T00:00:00Z", "2020-01-02T00:00:00Z");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    // BetweenDateTimeOffsetStringAttribute — offset-less values are assumed UTC on any host time zone
    [Theory]
    [MemberData(nameof(StringDateTimeOffsetAttributesTestData.BetweenDateTimeOffsetStringAssumeUtc.Cases), MemberType = typeof(StringDateTimeOffsetAttributesTestData.BetweenDateTimeOffsetStringAssumeUtc))]
    public void BetweenDateTimeOffsetString_OffsetLessValue_IsAssumedUtc(DataAnnotationCase tc)
    {
        // Arrange — a one-minute UTC window around the offset-less value; only UTC-assumed parsing lands inside it
        var attr = new BetweenDateTimeOffsetStringAttribute("2024-01-15T10:29:30Z", "2024-01-15T10:30:30Z");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateTimeOffsetAttributesTestData.PastDateTimeOffsetStringOnAnInjectedClock.Cases), MemberType = typeof(StringDateTimeOffsetAttributesTestData.PastDateTimeOffsetStringOnAnInjectedClock))]
    public void PastDateTimeOffsetString_HonoursTheInjectedClock(DataAnnotationCase tc)
    {
        // Arrange
        var (value, utcNow) = ((string value, DateTimeOffset utcNow))tc.Value!;
        var attr = new PastDateTimeOffsetStringAttribute();
        var ctx = ValidationContextFactory.WithTimeProvider(new FixedTimeProvider(utcNow));

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringDateTimeOffsetAttributesTestData.FutureDateTimeOffsetStringOnAnInjectedClock.Cases), MemberType = typeof(StringDateTimeOffsetAttributesTestData.FutureDateTimeOffsetStringOnAnInjectedClock))]
    public void FutureDateTimeOffsetString_HonoursTheInjectedClock(DataAnnotationCase tc)
    {
        // Arrange
        var (value, utcNow) = ((string value, DateTimeOffset utcNow))tc.Value!;
        var attr = new FutureDateTimeOffsetStringAttribute();
        var ctx = ValidationContextFactory.WithTimeProvider(new FixedTimeProvider(utcNow));

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
