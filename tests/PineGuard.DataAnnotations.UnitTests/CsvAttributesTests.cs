using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class CsvAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CsvAttributesTestData.CsvLine.Cases), MemberType = typeof(CsvAttributesTestData.CsvLine))]
    public void CsvLine_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CsvLineAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(CsvAttributesTestData.CsvHeaderLine.Cases), MemberType = typeof(CsvAttributesTestData.CsvHeaderLine))]
    public void CsvHeaderLine_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CsvHeaderLineAttribute("Id", "Name");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(CsvAttributesTestData.ConfiguredSeparator.Cases), MemberType = typeof(CsvAttributesTestData.ConfiguredSeparator))]
    public void CsvAttributes_ForwardSeparatorAndComparison(char separator)
    {
        // Arrange
        var line = new CsvLineAttribute(separator);
        var header = new CsvHeaderLineAttribute("a", "b") { Separator = separator, Comparison = StringComparison.Ordinal };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var lineResult = line.GetValidationResult($"a{separator}\"b{separator}c\"", ctx);
        var headerResult = header.GetValidationResult($"a{separator}b", ctx);
        var wrongCase = header.GetValidationResult($"A{separator}B", ctx);

        // Assert
        Assert.Null(lineResult);
        Assert.Null(headerResult);
        Assert.NotNull(wrongCase);
        Assert.Equal(separator, line.Separator);
        Assert.Equal(separator, header.Separator);
        Assert.Equal(StringComparison.Ordinal, header.Comparison);
    }

    [Theory]
    [MemberData(nameof(CsvAttributesTestData.ConfiguredSeparator.InvalidCases), MemberType = typeof(CsvAttributesTestData.ConfiguredSeparator))]
    public void CsvAttributes_InvalidSeparatorReturnsValidationFailure(char separator)
    {
        // Arrange
        var line = new CsvLineAttribute(separator);
        var header = new CsvHeaderLineAttribute("a") { Separator = separator };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var lineResult = line.GetValidationResult(null, ctx);
        var headerResult = header.GetValidationResult(null, ctx);

        // Assert
        Assert.NotNull(lineResult);
        Assert.NotNull(headerResult);
        Assert.Contains("separator", lineResult.ErrorMessage, StringComparison.Ordinal);
        Assert.Contains("separator", headerResult.ErrorMessage, StringComparison.Ordinal);
    }
}
