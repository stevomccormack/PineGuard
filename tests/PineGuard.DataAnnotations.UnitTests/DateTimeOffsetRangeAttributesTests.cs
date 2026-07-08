using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DateTimeOffsetRangeAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, DateTimeOffsetRangeAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };
        var result = attribute.GetValidationResult(testCase.Value, ctx);
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.ChronologicalDateTimeOffsetRange.ValidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.ChronologicalDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.ChronologicalDateTimeOffsetRange.EdgeCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.ChronologicalDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.ChronologicalDateTimeOffsetRange.InvalidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.ChronologicalDateTimeOffsetRange))]
    public void ChronologicalDateTimeOffsetRange_ShouldReturnExpected(DateTimeOffsetRangeAttributesTestData.ValidCase testCase)
        => Verify(new ChronologicalDateTimeOffsetRangeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.OverlappingDateTimeOffsetRange.ValidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.OverlappingDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.OverlappingDateTimeOffsetRange.EdgeCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.OverlappingDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.OverlappingDateTimeOffsetRange.InvalidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.OverlappingDateTimeOffsetRange))]
    public void OverlappingDateTimeOffsetRange_ShouldReturnExpected(DateTimeOffsetRangeAttributesTestData.ValidCase testCase)
        => Verify(new OverlappingDateTimeOffsetRangeAttribute("2020-01-15T00:00:00+00:00", "2020-01-25T00:00:00+00:00"), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.NotOverlappingDateTimeOffsetRange.ValidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.NotOverlappingDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.NotOverlappingDateTimeOffsetRange.EdgeCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.NotOverlappingDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.NotOverlappingDateTimeOffsetRange.InvalidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.NotOverlappingDateTimeOffsetRange))]
    public void NotOverlappingDateTimeOffsetRange_ShouldReturnExpected(DateTimeOffsetRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotOverlappingDateTimeOffsetRangeAttribute("2020-01-15T00:00:00+00:00", "2020-01-25T00:00:00+00:00"), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.ContainsDateTimeOffsetRange.ValidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.ContainsDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.ContainsDateTimeOffsetRange.EdgeCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.ContainsDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.ContainsDateTimeOffsetRange.InvalidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.ContainsDateTimeOffsetRange))]
    public void ContainsDateTimeOffsetRange_ShouldReturnExpected(DateTimeOffsetRangeAttributesTestData.ValidCase testCase)
        => Verify(new ContainsDateTimeOffsetRangeAttribute("2020-01-15T00:00:00+00:00"), testCase);

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.NotContainsDateTimeOffsetRange.ValidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.NotContainsDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.NotContainsDateTimeOffsetRange.EdgeCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.NotContainsDateTimeOffsetRange))]
    [MemberData(nameof(DateTimeOffsetRangeAttributesTestData.NotContainsDateTimeOffsetRange.InvalidCases), MemberType = typeof(DateTimeOffsetRangeAttributesTestData.NotContainsDateTimeOffsetRange))]
    public void NotContainsDateTimeOffsetRange_ShouldReturnExpected(DateTimeOffsetRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsDateTimeOffsetRangeAttribute("2020-01-15T00:00:00+00:00"), testCase);
}
