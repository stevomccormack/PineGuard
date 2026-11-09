using System.ComponentModel.DataAnnotations;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringGraphemesAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.HasExactGraphemeCount.Cases), MemberType = typeof(StringGraphemesAttributesTestData.HasExactGraphemeCount))]
    public void HasExactGraphemeCount_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, count) = ((string? value, int count))tc.Value!;
        var attr = new HasExactGraphemeCountAttribute(count);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.NotHasExactGraphemeCount.Cases), MemberType = typeof(StringGraphemesAttributesTestData.NotHasExactGraphemeCount))]
    public void NotHasExactGraphemeCount_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, count) = ((string? value, int count))tc.Value!;
        var attr = new NotHasExactGraphemeCountAttribute(count);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.HasMinGraphemeCount.Cases), MemberType = typeof(StringGraphemesAttributesTestData.HasMinGraphemeCount))]
    public void HasMinGraphemeCount_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, min) = ((string? value, int min))tc.Value!;
        var attr = new HasMinGraphemeCountAttribute(min);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.NotHasMinGraphemeCount.Cases), MemberType = typeof(StringGraphemesAttributesTestData.NotHasMinGraphemeCount))]
    public void NotHasMinGraphemeCount_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, min) = ((string? value, int min))tc.Value!;
        var attr = new NotHasMinGraphemeCountAttribute(min);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.HasMaxGraphemeCount.Cases), MemberType = typeof(StringGraphemesAttributesTestData.HasMaxGraphemeCount))]
    public void HasMaxGraphemeCount_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, max) = ((string? value, int max))tc.Value!;
        var attr = new HasMaxGraphemeCountAttribute(max);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.NotHasMaxGraphemeCount.Cases), MemberType = typeof(StringGraphemesAttributesTestData.NotHasMaxGraphemeCount))]
    public void NotHasMaxGraphemeCount_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, max) = ((string? value, int max))tc.Value!;
        var attr = new NotHasMaxGraphemeCountAttribute(max);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.HasGraphemeCountInRange.Cases), MemberType = typeof(StringGraphemesAttributesTestData.HasGraphemeCountInRange))]
    public void HasGraphemeCountInRange_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, min, max, inclusion) = ((string? value, int min, int max, Inclusion inclusion))tc.Value!;
        HasGraphemeCountInRangeAttribute attr = new HasGraphemeCountInRangeAttribute(min, max, inclusion);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.HasGraphemeCountOutOfRange.Cases), MemberType = typeof(StringGraphemesAttributesTestData.HasGraphemeCountOutOfRange))]
    public void HasGraphemeCountOutOfRange_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, min, max, inclusion) = ((string? value, int min, int max, Inclusion inclusion))tc.Value!;
        HasGraphemeCountOutOfRangeAttribute attr = new HasGraphemeCountOutOfRangeAttribute(min, max, inclusion);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringGraphemesAttributesTestData.NullConfiguration.Cases), MemberType = typeof(StringGraphemesAttributesTestData.NullConfiguration))]
    public void NullConfiguration_ValidatesConfigurationBeforeSkippingNull(DataAnnotationCase tc)
    {
        // Arrange
        var attribute = (ValidationAttribute)tc.Value!;
        var context = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attribute.GetValidationResult(null, context);

        // Assert
        AssertResult(tc, result);
    }
}
