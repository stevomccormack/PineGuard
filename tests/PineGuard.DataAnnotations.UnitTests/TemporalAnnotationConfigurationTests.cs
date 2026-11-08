using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TemporalAnnotationConfigurationTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TemporalAnnotationConfigurationTestData.OptionalValue.Cases), MemberType = typeof(TemporalAnnotationConfigurationTestData.OptionalValue))]
    public void OptionalValue_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var (attribute, value) = ((ValidationAttributeBase attribute, object? value))tc.Value!;

        // Act
        var result = attribute.GetValidationResult(value, new ValidationContext(new object()) { MemberName = "Value" });

        // Assert
        AssertResult(tc, result, attribute.Code);
    }
}
