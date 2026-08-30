using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class ChecksumAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ChecksumAttributesTestData.Luhn.Cases), MemberType = typeof(ChecksumAttributesTestData.Luhn))]
    public void Luhn_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LuhnAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
