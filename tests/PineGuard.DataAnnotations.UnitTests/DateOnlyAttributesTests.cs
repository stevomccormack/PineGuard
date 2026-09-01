using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DateOnlyAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    private static void Verify<TAttribute>(TAttribute attribute, DateOnlyAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.PastDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.PastDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.PastDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.PastDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.PastDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.PastDateOnly))]
    public void PastDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
    {
        var attribute = new PastDateOnlyAttribute();
        Assert.Equal(MustCodes.Date.Relative.NotPast, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.PastOrPresentDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.PastOrPresentDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.PastOrPresentDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.PastOrPresentDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.PastOrPresentDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.PastOrPresentDateOnly))]
    public void PastOrPresentDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new PastOrPresentDateOnlyAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.FutureDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.FutureDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.FutureDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.FutureDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.FutureDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.FutureDateOnly))]
    public void FutureDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new FutureDateOnlyAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.FutureOrPresentDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.FutureOrPresentDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.FutureOrPresentDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.FutureOrPresentDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.FutureOrPresentDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.FutureOrPresentDateOnly))]
    public void FutureOrPresentDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new FutureOrPresentDateOnlyAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.BetweenDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.BetweenDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.BetweenDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.BetweenDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.BetweenDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.BetweenDateOnly))]
    public void BetweenDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new BetweenDateOnlyAttribute("2020-01-01", "2020-01-31"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.NotBetweenDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.NotBetweenDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotBetweenDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.NotBetweenDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotBetweenDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.NotBetweenDateOnly))]
    public void NotBetweenDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new NotBetweenDateOnlyAttribute("2020-01-01", "2020-01-31"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.BeforeDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.BeforeDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.BeforeDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.BeforeDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.BeforeDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.BeforeDateOnly))]
    public void BeforeDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new BeforeDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.OnOrBeforeDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.OnOrBeforeDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.OnOrBeforeDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.OnOrBeforeDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.OnOrBeforeDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.OnOrBeforeDateOnly))]
    public void OnOrBeforeDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new OnOrBeforeDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.AfterDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.AfterDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.AfterDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.AfterDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.AfterDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.AfterDateOnly))]
    public void AfterDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new AfterDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.OnOrAfterDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.OnOrAfterDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.OnOrAfterDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.OnOrAfterDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.OnOrAfterDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.OnOrAfterDateOnly))]
    public void OnOrAfterDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new OnOrAfterDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.SameDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.SameDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.SameDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.SameDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.SameDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.SameDateOnly))]
    public void SameDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new SameDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.NotSameDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.NotSameDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotSameDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.NotSameDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotSameDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.NotSameDateOnly))]
    public void NotSameDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new NotSameDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.ChronologicalDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.ChronologicalDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.ChronologicalDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.ChronologicalDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.ChronologicalDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.ChronologicalDateOnly))]
    public void ChronologicalDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new ChronologicalDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.OverlappingDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.OverlappingDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.OverlappingDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.OverlappingDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.OverlappingDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.OverlappingDateOnly))]
    public void OverlappingDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        // End1="2020-01-30", Start2="2020-01-10", End2="2020-01-20"
        => Verify(new OverlappingDateOnlyAttribute("2020-01-30", "2020-01-10", "2020-01-20"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.NotChronologicalDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.NotChronologicalDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotChronologicalDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.NotChronologicalDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotChronologicalDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.NotChronologicalDateOnly))]
    public void NotChronologicalDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new NotChronologicalDateOnlyAttribute("2020-01-10"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.NotOverlappingDateOnly.ValidCases), MemberType = typeof(DateOnlyAttributesTestData.NotOverlappingDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotOverlappingDateOnly.EdgeCases), MemberType = typeof(DateOnlyAttributesTestData.NotOverlappingDateOnly))]
    [MemberData(nameof(DateOnlyAttributesTestData.NotOverlappingDateOnly.InvalidCases), MemberType = typeof(DateOnlyAttributesTestData.NotOverlappingDateOnly))]
    public void NotOverlappingDateOnly_ShouldReturnExpected(DateOnlyAttributesTestData.ValidCase testCase)
        => Verify(new NotOverlappingDateOnlyAttribute("2020-01-30", "2020-01-10", "2020-01-20"), testCase);

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.PastDateOnlyOnAnInjectedClock.Cases), MemberType = typeof(DateOnlyAttributesTestData.PastDateOnlyOnAnInjectedClock))]
    public void PastDateOnly_HonoursTheInjectedClock(DataAnnotationCase tc)
    {
        // Arrange
        var (value, utcNow) = ((DateOnly value, DateTimeOffset utcNow))tc.Value!;
        var attr = new PastDateOnlyAttribute();
        var ctx = ValidationContextFactory.WithTimeProvider(new FixedTimeProvider(utcNow));

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.FutureDateOnlyOnAnInjectedClock.Cases), MemberType = typeof(DateOnlyAttributesTestData.FutureDateOnlyOnAnInjectedClock))]
    public void FutureDateOnly_HonoursTheInjectedClock(DataAnnotationCase tc)
    {
        // Arrange
        var (value, utcNow) = ((DateOnly value, DateTimeOffset utcNow))tc.Value!;
        var attr = new FutureDateOnlyAttribute();
        var ctx = ValidationContextFactory.WithTimeProvider(new FixedTimeProvider(utcNow));

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.WeekdayDateOnly.Cases), MemberType = typeof(DateOnlyAttributesTestData.WeekdayDateOnly))]
    public void WeekdayDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new WeekdayDateOnlyAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.WeekendDateOnly.Cases), MemberType = typeof(DateOnlyAttributesTestData.WeekendDateOnly))]
    public void WeekendDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new WeekendDateOnlyAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.FirstDayOfMonthDateOnly.Cases), MemberType = typeof(DateOnlyAttributesTestData.FirstDayOfMonthDateOnly))]
    public void FirstDayOfMonthDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FirstDayOfMonthDateOnlyAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.NotFirstDayOfMonthDateOnly.Cases), MemberType = typeof(DateOnlyAttributesTestData.NotFirstDayOfMonthDateOnly))]
    public void NotFirstDayOfMonthDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotFirstDayOfMonthDateOnlyAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.LastDayOfMonthDateOnly.Cases), MemberType = typeof(DateOnlyAttributesTestData.LastDayOfMonthDateOnly))]
    public void LastDayOfMonthDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LastDayOfMonthDateOnlyAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(DateOnlyAttributesTestData.NotLastDayOfMonthDateOnly.Cases), MemberType = typeof(DateOnlyAttributesTestData.NotLastDayOfMonthDateOnly))]
    public void NotLastDayOfMonthDateOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotLastDayOfMonthDateOnlyAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
