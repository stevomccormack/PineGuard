using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TemporalConfigurationTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.Configuration.Cases), MemberType = typeof(TemporalConfigurationTestData.Configuration))]
    public void Configuration_IsForwardedWithOptionalNullSkip(TemporalConfigurationTestData.Case tc)
    {
        // Arrange
        var attribute = new ChronologicalDateOnlyStringAttribute("2024-02-01", tc.Value.Inclusion) { Styles = tc.Value.Styles };

        // Act
        var result = attribute.GetValidationResult(tc.Value.Input, new ValidationContext(new object()));

        // Assert
        Assert.Equal(tc.Expected, result == ValidationResult.Success);
        Assert.Equal(MustCodes.Date.Order.NotChronological, attribute.Code);
    }
}
