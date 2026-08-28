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
    [MemberData(nameof(StringTimeSpanAttributesTestData.DurationBetweenTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.DurationBetweenTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.DurationBetweenTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.DurationBetweenTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.DurationBetweenTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.DurationBetweenTimeSpanString))]
    public void DurationBetweenTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
    {
        var attribute = new DurationBetweenTimeSpanStringAttribute("00:05:00", "02:00:00");
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
    [MemberData(nameof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString))]
    public void NotDurationBetweenTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
        => Verify(new NotDurationBetweenTimeSpanStringAttribute("00:05:00", "02:00:00"), testCase);
}
