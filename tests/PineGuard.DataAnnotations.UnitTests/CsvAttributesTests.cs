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
        AssertResult(tc, result);
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
        AssertResult(tc, result);
    }
}
