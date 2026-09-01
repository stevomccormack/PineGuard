using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DateTimeOffsetAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    private static void Verify<TAttribute>(TAttribute attribute, DateTimeOffsetAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.PastDateTimeOffset.ValidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.PastDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.PastDateTimeOffset.EdgeCases), MemberType = typeof(DateTimeOffsetAttributesTestData.PastDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.PastDateTimeOffset.InvalidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.PastDateTimeOffset))]
    public void PastDateTimeOffset_ShouldReturnExpected(DateTimeOffsetAttributesTestData.ValidCase testCase)
        => Verify(new PastDateTimeOffsetAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.PastOrPresentDateTimeOffset.ValidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.PastOrPresentDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.PastOrPresentDateTimeOffset.EdgeCases), MemberType = typeof(DateTimeOffsetAttributesTestData.PastOrPresentDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.PastOrPresentDateTimeOffset.InvalidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.PastOrPresentDateTimeOffset))]
    public void PastOrPresentDateTimeOffset_ShouldReturnExpected(DateTimeOffsetAttributesTestData.ValidCase testCase)
        => Verify(new PastOrPresentDateTimeOffsetAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FutureDateTimeOffset.ValidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.FutureDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FutureDateTimeOffset.EdgeCases), MemberType = typeof(DateTimeOffsetAttributesTestData.FutureDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FutureDateTimeOffset.InvalidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.FutureDateTimeOffset))]
    public void FutureDateTimeOffset_ShouldReturnExpected(DateTimeOffsetAttributesTestData.ValidCase testCase)
        => Verify(new FutureDateTimeOffsetAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FutureOrPresentDateTimeOffset.ValidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.FutureOrPresentDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FutureOrPresentDateTimeOffset.EdgeCases), MemberType = typeof(DateTimeOffsetAttributesTestData.FutureOrPresentDateTimeOffset))]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FutureOrPresentDateTimeOffset.InvalidCases), MemberType = typeof(DateTimeOffsetAttributesTestData.FutureOrPresentDateTimeOffset))]
    public void FutureOrPresentDateTimeOffset_ShouldReturnExpected(DateTimeOffsetAttributesTestData.ValidCase testCase)
        => Verify(new FutureOrPresentDateTimeOffsetAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.PastDateTimeOffsetOnAnInjectedClock.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.PastDateTimeOffsetOnAnInjectedClock))]
    public void PastDateTimeOffset_HonoursTheInjectedClock(DataAnnotationCase tc)
    {
        // Arrange
        var (value, utcNow) = ((DateTimeOffset value, DateTimeOffset utcNow))tc.Value!;
        var attr = new PastDateTimeOffsetAttribute();
        var ctx = ValidationContextFactory.WithTimeProvider(new FixedTimeProvider(utcNow));

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FutureDateTimeOffsetOnAnInjectedClock.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.FutureDateTimeOffsetOnAnInjectedClock))]
    public void FutureDateTimeOffset_HonoursTheInjectedClock(DataAnnotationCase tc)
    {
        // Arrange
        var (value, utcNow) = ((DateTimeOffset value, DateTimeOffset utcNow))tc.Value!;
        var attr = new FutureDateTimeOffsetAttribute();
        var ctx = ValidationContextFactory.WithTimeProvider(new FixedTimeProvider(utcNow));

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.WeekdayDateTimeOffset.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.WeekdayDateTimeOffset))]
    public void WeekdayDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new WeekdayDateTimeOffsetAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.WeekendDateTimeOffset.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.WeekendDateTimeOffset))]
    public void WeekendDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new WeekendDateTimeOffsetAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.FirstDayOfMonthDateTimeOffset.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.FirstDayOfMonthDateTimeOffset))]
    public void FirstDayOfMonthDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FirstDayOfMonthDateTimeOffsetAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.NotFirstDayOfMonthDateTimeOffset.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.NotFirstDayOfMonthDateTimeOffset))]
    public void NotFirstDayOfMonthDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotFirstDayOfMonthDateTimeOffsetAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.LastDayOfMonthDateTimeOffset.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.LastDayOfMonthDateTimeOffset))]
    public void LastDayOfMonthDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LastDayOfMonthDateTimeOffsetAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetAttributesTestData.NotLastDayOfMonthDateTimeOffset.Cases), MemberType = typeof(DateTimeOffsetAttributesTestData.NotLastDayOfMonthDateTimeOffset))]
    public void NotLastDayOfMonthDateTimeOffset_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotLastDayOfMonthDateTimeOffsetAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
