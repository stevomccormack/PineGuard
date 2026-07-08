using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DateTimeRangeAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, DateTimeRangeAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };
        var result = attribute.GetValidationResult(testCase.Value, ctx);
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeAttributesTestData.ChronologicalDateTimeRange.ValidCases), MemberType = typeof(DateTimeRangeAttributesTestData.ChronologicalDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.ChronologicalDateTimeRange.EdgeCases), MemberType = typeof(DateTimeRangeAttributesTestData.ChronologicalDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.ChronologicalDateTimeRange.InvalidCases), MemberType = typeof(DateTimeRangeAttributesTestData.ChronologicalDateTimeRange))]
    public void ChronologicalDateTimeRange_ShouldReturnExpected(DateTimeRangeAttributesTestData.ValidCase testCase)
        => Verify(new ChronologicalDateTimeRangeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeRangeAttributesTestData.OverlappingDateTimeRange.ValidCases), MemberType = typeof(DateTimeRangeAttributesTestData.OverlappingDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.OverlappingDateTimeRange.EdgeCases), MemberType = typeof(DateTimeRangeAttributesTestData.OverlappingDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.OverlappingDateTimeRange.InvalidCases), MemberType = typeof(DateTimeRangeAttributesTestData.OverlappingDateTimeRange))]
    public void OverlappingDateTimeRange_ShouldReturnExpected(DateTimeRangeAttributesTestData.ValidCase testCase)
        => Verify(new OverlappingDateTimeRangeAttribute("2020-01-15T00:00:00", "2020-01-25T00:00:00"), testCase);

    [Theory]
    [MemberData(nameof(DateTimeRangeAttributesTestData.NotOverlappingDateTimeRange.ValidCases), MemberType = typeof(DateTimeRangeAttributesTestData.NotOverlappingDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.NotOverlappingDateTimeRange.EdgeCases), MemberType = typeof(DateTimeRangeAttributesTestData.NotOverlappingDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.NotOverlappingDateTimeRange.InvalidCases), MemberType = typeof(DateTimeRangeAttributesTestData.NotOverlappingDateTimeRange))]
    public void NotOverlappingDateTimeRange_ShouldReturnExpected(DateTimeRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotOverlappingDateTimeRangeAttribute("2020-01-15T00:00:00", "2020-01-25T00:00:00"), testCase);

    [Theory]
    [MemberData(nameof(DateTimeRangeAttributesTestData.ContainsDateTimeRange.ValidCases), MemberType = typeof(DateTimeRangeAttributesTestData.ContainsDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.ContainsDateTimeRange.EdgeCases), MemberType = typeof(DateTimeRangeAttributesTestData.ContainsDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.ContainsDateTimeRange.InvalidCases), MemberType = typeof(DateTimeRangeAttributesTestData.ContainsDateTimeRange))]
    public void ContainsDateTimeRange_ShouldReturnExpected(DateTimeRangeAttributesTestData.ValidCase testCase)
        => Verify(new ContainsDateTimeRangeAttribute("2020-01-15T00:00:00"), testCase);

    [Theory]
    [MemberData(nameof(DateTimeRangeAttributesTestData.NotContainsDateTimeRange.ValidCases), MemberType = typeof(DateTimeRangeAttributesTestData.NotContainsDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.NotContainsDateTimeRange.EdgeCases), MemberType = typeof(DateTimeRangeAttributesTestData.NotContainsDateTimeRange))]
    [MemberData(nameof(DateTimeRangeAttributesTestData.NotContainsDateTimeRange.InvalidCases), MemberType = typeof(DateTimeRangeAttributesTestData.NotContainsDateTimeRange))]
    public void NotContainsDateTimeRange_ShouldReturnExpected(DateTimeRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsDateTimeRangeAttribute("2020-01-15T00:00:00"), testCase);
}
