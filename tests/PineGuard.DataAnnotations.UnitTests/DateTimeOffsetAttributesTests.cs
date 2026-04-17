using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DateTimeOffsetAttributesTests
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
}
