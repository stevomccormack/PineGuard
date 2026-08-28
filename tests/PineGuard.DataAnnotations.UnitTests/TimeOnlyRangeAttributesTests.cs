using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TimeOnlyRangeAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, TimeOnlyRangeAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };
        var result = attribute.GetValidationResult(testCase.Value, ctx);
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.ChronologicalTimeOnlyRange.ValidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.ChronologicalTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.ChronologicalTimeOnlyRange.EdgeCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.ChronologicalTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.ChronologicalTimeOnlyRange.InvalidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.ChronologicalTimeOnlyRange))]
    public void ChronologicalTimeOnlyRange_ShouldReturnExpected(TimeOnlyRangeAttributesTestData.ValidCase testCase)
    {
        var attribute = new ChronologicalTimeOnlyRangeAttribute();
        Assert.Equal(MustCodes.Range.Order.NotChronological, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.OverlappingTimeOnlyRange.ValidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.OverlappingTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.OverlappingTimeOnlyRange.EdgeCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.OverlappingTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.OverlappingTimeOnlyRange.InvalidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.OverlappingTimeOnlyRange))]
    public void OverlappingTimeOnlyRange_ShouldReturnExpected(TimeOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new OverlappingTimeOnlyRangeAttribute("13:00", "17:00"), testCase);

    [Theory]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.NotOverlappingTimeOnlyRange.ValidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.NotOverlappingTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.NotOverlappingTimeOnlyRange.EdgeCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.NotOverlappingTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.NotOverlappingTimeOnlyRange.InvalidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.NotOverlappingTimeOnlyRange))]
    public void NotOverlappingTimeOnlyRange_ShouldReturnExpected(TimeOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotOverlappingTimeOnlyRangeAttribute("13:00", "17:00"), testCase);

    [Theory]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.ContainsTimeOnlyRange.ValidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.ContainsTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.ContainsTimeOnlyRange.EdgeCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.ContainsTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.ContainsTimeOnlyRange.InvalidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.ContainsTimeOnlyRange))]
    public void ContainsTimeOnlyRange_ShouldReturnExpected(TimeOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new ContainsTimeOnlyRangeAttribute("14:00"), testCase);

    [Theory]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.NotContainsTimeOnlyRange.ValidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.NotContainsTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.NotContainsTimeOnlyRange.EdgeCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.NotContainsTimeOnlyRange))]
    [MemberData(nameof(TimeOnlyRangeAttributesTestData.NotContainsTimeOnlyRange.InvalidCases), MemberType = typeof(TimeOnlyRangeAttributesTestData.NotContainsTimeOnlyRange))]
    public void NotContainsTimeOnlyRange_ShouldReturnExpected(TimeOnlyRangeAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsTimeOnlyRangeAttribute("14:00"), testCase);
}
