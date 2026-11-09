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
    [MemberData(nameof(StringTimeSpanAttributesTestData.LongerThanTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.LongerThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.LongerThanTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.LongerThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.LongerThanTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.LongerThanTimeSpanString))]
    public void LongerThanTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
        => Verify(new LongerThanTimeSpanStringAttribute("00:05:00"), testCase);

    [Theory]
    [MemberData(nameof(StringTimeSpanAttributesTestData.ShorterThanTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.ShorterThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.ShorterThanTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.ShorterThanTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.ShorterThanTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.ShorterThanTimeSpanString))]
    public void ShorterThanTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
        => Verify(new ShorterThanTimeSpanStringAttribute("02:00:00"), testCase);

    [Theory]
    [MemberData(nameof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString.ValidCases), MemberType = typeof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString.EdgeCases), MemberType = typeof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString))]
    [MemberData(nameof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString.InvalidCases), MemberType = typeof(StringTimeSpanAttributesTestData.NotDurationBetweenTimeSpanString))]
    public void NotDurationBetweenTimeSpanString_ShouldReturnExpected(StringTimeSpanAttributesTestData.ValidCase testCase)
        => Verify(new NotDurationBetweenTimeSpanStringAttribute("00:05:00", "02:00:00"), testCase);
}
