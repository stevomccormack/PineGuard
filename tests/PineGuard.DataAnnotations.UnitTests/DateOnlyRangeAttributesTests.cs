using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DateOnlyRangeAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, DateOnlyRangeAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };
        var result = attribute.GetValidationResult(testCase.Value, ctx);
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.ChronologicalDateOnlyRange.ValidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.ChronologicalDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.ChronologicalDateOnlyRange.EdgeCases), MemberType = typeof(DateOnlyRangeAttributesTestData.ChronologicalDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.ChronologicalDateOnlyRange.InvalidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.ChronologicalDateOnlyRange))]
    public void ChronologicalDateOnlyRange_ShouldReturnExpected(DateOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new ChronologicalDateOnlyRangeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.OverlappingDateOnlyRange.ValidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.OverlappingDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.OverlappingDateOnlyRange.EdgeCases), MemberType = typeof(DateOnlyRangeAttributesTestData.OverlappingDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.OverlappingDateOnlyRange.InvalidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.OverlappingDateOnlyRange))]
    public void OverlappingDateOnlyRange_ShouldReturnExpected(DateOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new OverlappingDateOnlyRangeAttribute("2020-01-15", "2020-01-25"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.NotOverlappingDateOnlyRange.ValidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.NotOverlappingDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.NotOverlappingDateOnlyRange.EdgeCases), MemberType = typeof(DateOnlyRangeAttributesTestData.NotOverlappingDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.NotOverlappingDateOnlyRange.InvalidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.NotOverlappingDateOnlyRange))]
    public void NotOverlappingDateOnlyRange_ShouldReturnExpected(DateOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotOverlappingDateOnlyRangeAttribute("2020-01-15", "2020-01-25"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.ContainsDateOnlyRange.ValidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.ContainsDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.ContainsDateOnlyRange.EdgeCases), MemberType = typeof(DateOnlyRangeAttributesTestData.ContainsDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.ContainsDateOnlyRange.InvalidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.ContainsDateOnlyRange))]
    public void ContainsDateOnlyRange_ShouldReturnExpected(DateOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new ContainsDateOnlyRangeAttribute("2020-01-15"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.NotContainsDateOnlyRange.ValidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.NotContainsDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.NotContainsDateOnlyRange.EdgeCases), MemberType = typeof(DateOnlyRangeAttributesTestData.NotContainsDateOnlyRange))]
    [MemberData(nameof(DateOnlyRangeAttributesTestData.NotContainsDateOnlyRange.InvalidCases), MemberType = typeof(DateOnlyRangeAttributesTestData.NotContainsDateOnlyRange))]
    public void NotContainsDateOnlyRange_ShouldReturnExpected(DateOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsDateOnlyRangeAttribute("2020-01-15"), testCase);
}
