using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringTimeSpanAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, StringTimeSpanAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };
        var result = attribute.GetValidationResult(testCase.Value, ctx);
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(StringTimeSpanAttributesTestData.InRangeTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.InRangeTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.InRangeTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.InRangeTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.InRangeTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.InRangeTimeSpanString))]
    public void InRangeTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
    {
        InRangeTimeSpanStringAttribute attribute = new InRangeTimeSpanStringAttribute("00:05:00", "02:00:00");
        Assert.Equal(MustCodes.Time.Duration.OutOfRange, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringTimeSpanAttributesTestData.GreaterThanTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.GreaterThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.GreaterThanTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.GreaterThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.GreaterThanTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.GreaterThanTimeSpanString))]
    public void GreaterThanTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
        => Verify(new GreaterThanTimeSpanStringAttribute("00:05:00"), testCase);

    [Theory]
    [MemberData(nameof(StringTimeSpanAttributesTestData.LessThanTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.LessThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.LessThanTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.LessThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.LessThanTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.LessThanTimeSpanString))]
    public void LessThanTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
        => Verify(new LessThanTimeSpanStringAttribute("02:00:00"), testCase);

    [Theory]
    [MemberData(nameof(StringTimeSpanAttributesTestData.OutOfRangeTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.OutOfRangeTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.OutOfRangeTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.OutOfRangeTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.OutOfRangeTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.OutOfRangeTimeSpanString))]
    public void OutOfRangeTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
        => Verify(new OutOfRangeTimeSpanStringAttribute("00:05:00", "02:00:00"), testCase);
}
