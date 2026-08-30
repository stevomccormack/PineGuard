using System.ComponentModel.DataAnnotations;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class CronAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CronAttributesTestData.CronExpression.Cases), MemberType = typeof(CronAttributesTestData.CronExpression))]
    public void CronExpression_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (value, format) = ((string? value, CronFormat format))tc.Value!;
        var attr = new CronExpressionAttribute { Format = format };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
