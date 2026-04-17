using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TimeAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, TimeAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.Past.ValidCases), MemberType = typeof(TimeAttributesTestData.Past))]
    [MemberData(nameof(TimeAttributesTestData.Past.EdgeCases), MemberType = typeof(TimeAttributesTestData.Past))]
    [MemberData(nameof(TimeAttributesTestData.Past.InvalidCases), MemberType = typeof(TimeAttributesTestData.Past))]
    public void Past_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new PastAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.PastOrPresent.ValidCases), MemberType = typeof(TimeAttributesTestData.PastOrPresent))]
    [MemberData(nameof(TimeAttributesTestData.PastOrPresent.EdgeCases), MemberType = typeof(TimeAttributesTestData.PastOrPresent))]
    [MemberData(nameof(TimeAttributesTestData.PastOrPresent.InvalidCases), MemberType = typeof(TimeAttributesTestData.PastOrPresent))]
    public void PastOrPresent_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new PastOrPresentAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.Future.ValidCases), MemberType = typeof(TimeAttributesTestData.Future))]
    [MemberData(nameof(TimeAttributesTestData.Future.EdgeCases), MemberType = typeof(TimeAttributesTestData.Future))]
    [MemberData(nameof(TimeAttributesTestData.Future.InvalidCases), MemberType = typeof(TimeAttributesTestData.Future))]
    public void Future_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new FutureAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.FutureOrPresent.ValidCases), MemberType = typeof(TimeAttributesTestData.FutureOrPresent))]
    [MemberData(nameof(TimeAttributesTestData.FutureOrPresent.EdgeCases), MemberType = typeof(TimeAttributesTestData.FutureOrPresent))]
    [MemberData(nameof(TimeAttributesTestData.FutureOrPresent.InvalidCases), MemberType = typeof(TimeAttributesTestData.FutureOrPresent))]
    public void FutureOrPresent_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new FutureOrPresentAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.DateOnlyBetween.ValidCases), MemberType = typeof(TimeAttributesTestData.DateOnlyBetween))]
    [MemberData(nameof(TimeAttributesTestData.DateOnlyBetween.EdgeCases), MemberType = typeof(TimeAttributesTestData.DateOnlyBetween))]
    [MemberData(nameof(TimeAttributesTestData.DateOnlyBetween.InvalidCases), MemberType = typeof(TimeAttributesTestData.DateOnlyBetween))]
    public void DateOnlyBetween_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new DateOnlyBetweenAttribute("2020-01-01", "2020-01-31"), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.Utc.ValidCases), MemberType = typeof(TimeAttributesTestData.Utc))]
    [MemberData(nameof(TimeAttributesTestData.Utc.EdgeCases), MemberType = typeof(TimeAttributesTestData.Utc))]
    [MemberData(nameof(TimeAttributesTestData.Utc.InvalidCases), MemberType = typeof(TimeAttributesTestData.Utc))]
    public void Utc_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new UtcAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.Local.ValidCases), MemberType = typeof(TimeAttributesTestData.Local))]
    [MemberData(nameof(TimeAttributesTestData.Local.EdgeCases), MemberType = typeof(TimeAttributesTestData.Local))]
    [MemberData(nameof(TimeAttributesTestData.Local.InvalidCases), MemberType = typeof(TimeAttributesTestData.Local))]
    public void Local_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new LocalAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.Unspecified.ValidCases), MemberType = typeof(TimeAttributesTestData.Unspecified))]
    [MemberData(nameof(TimeAttributesTestData.Unspecified.EdgeCases), MemberType = typeof(TimeAttributesTestData.Unspecified))]
    [MemberData(nameof(TimeAttributesTestData.Unspecified.InvalidCases), MemberType = typeof(TimeAttributesTestData.Unspecified))]
    public void Unspecified_ShouldReturnExpected(TimeAttributesTestData.ValidCase testCase)
        => Verify(new UnspecifiedAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TimeAttributesTestData.UnsupportedType.Cases), MemberType = typeof(TimeAttributesTestData.UnsupportedType))]
    public void TimeAttribute_WithUnsupportedType_ShouldThrow(TimeAttributesTestData.UnsupportedType.ThrowCase testCase)
    {
        // Arrange & Act
        var ex = Assert.Throws<InvalidOperationException>(
            () => testCase.Attribute.GetValidationResult(testCase.Value, new ValidationContext(new object())));

        // Assert
        Assert.Contains(testCase.ExpectedMessageContains, ex.Message);
    }
}
