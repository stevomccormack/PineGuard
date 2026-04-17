using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DateTimeAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, DateTimeAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var value = testCase.Value();
        var result = attribute.GetValidationResult(value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DateTimeAttributesTestData.PastDateTime.ValidCases), MemberType = typeof(DateTimeAttributesTestData.PastDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.PastDateTime.EdgeCases), MemberType = typeof(DateTimeAttributesTestData.PastDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.PastDateTime.InvalidCases), MemberType = typeof(DateTimeAttributesTestData.PastDateTime))]
    public void PastDateTime_ShouldReturnExpected(DateTimeAttributesTestData.ValidCase testCase)
        => Verify(new PastDateTimeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeAttributesTestData.PastOrPresentDateTime.ValidCases), MemberType = typeof(DateTimeAttributesTestData.PastOrPresentDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.PastOrPresentDateTime.EdgeCases), MemberType = typeof(DateTimeAttributesTestData.PastOrPresentDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.PastOrPresentDateTime.InvalidCases), MemberType = typeof(DateTimeAttributesTestData.PastOrPresentDateTime))]
    public void PastOrPresentDateTime_ShouldReturnExpected(DateTimeAttributesTestData.ValidCase testCase)
        => Verify(new PastOrPresentDateTimeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeAttributesTestData.FutureDateTime.ValidCases), MemberType = typeof(DateTimeAttributesTestData.FutureDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.FutureDateTime.EdgeCases), MemberType = typeof(DateTimeAttributesTestData.FutureDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.FutureDateTime.InvalidCases), MemberType = typeof(DateTimeAttributesTestData.FutureDateTime))]
    public void FutureDateTime_ShouldReturnExpected(DateTimeAttributesTestData.ValidCase testCase)
        => Verify(new FutureDateTimeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeAttributesTestData.FutureOrPresentDateTime.ValidCases), MemberType = typeof(DateTimeAttributesTestData.FutureOrPresentDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.FutureOrPresentDateTime.EdgeCases), MemberType = typeof(DateTimeAttributesTestData.FutureOrPresentDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.FutureOrPresentDateTime.InvalidCases), MemberType = typeof(DateTimeAttributesTestData.FutureOrPresentDateTime))]
    public void FutureOrPresentDateTime_ShouldReturnExpected(DateTimeAttributesTestData.ValidCase testCase)
        => Verify(new FutureOrPresentDateTimeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeAttributesTestData.UtcDateTime.ValidCases), MemberType = typeof(DateTimeAttributesTestData.UtcDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.UtcDateTime.EdgeCases), MemberType = typeof(DateTimeAttributesTestData.UtcDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.UtcDateTime.InvalidCases), MemberType = typeof(DateTimeAttributesTestData.UtcDateTime))]
    public void UtcDateTime_ShouldReturnExpected(DateTimeAttributesTestData.ValidCase testCase)
        => Verify(new UtcDateTimeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeAttributesTestData.LocalDateTime.ValidCases), MemberType = typeof(DateTimeAttributesTestData.LocalDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.LocalDateTime.EdgeCases), MemberType = typeof(DateTimeAttributesTestData.LocalDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.LocalDateTime.InvalidCases), MemberType = typeof(DateTimeAttributesTestData.LocalDateTime))]
    public void LocalDateTime_ShouldReturnExpected(DateTimeAttributesTestData.ValidCase testCase)
        => Verify(new LocalDateTimeAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateTimeAttributesTestData.UnspecifiedDateTime.ValidCases), MemberType = typeof(DateTimeAttributesTestData.UnspecifiedDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.UnspecifiedDateTime.EdgeCases), MemberType = typeof(DateTimeAttributesTestData.UnspecifiedDateTime))]
    [MemberData(nameof(DateTimeAttributesTestData.UnspecifiedDateTime.InvalidCases), MemberType = typeof(DateTimeAttributesTestData.UnspecifiedDateTime))]
    public void UnspecifiedDateTime_ShouldReturnExpected(DateTimeAttributesTestData.ValidCase testCase)
        => Verify(new UnspecifiedDateTimeAttribute(), testCase);
}
