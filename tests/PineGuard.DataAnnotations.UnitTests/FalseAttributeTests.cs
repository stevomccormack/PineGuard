using System.ComponentModel.DataAnnotations;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class FalseAttributeTests : BaseUnitTest
{
    private const string DisplayName = "MyProp";

    [Theory]
    [MemberData(nameof(FalseAttributeTestData.Validation.ValidCases), MemberType = typeof(FalseAttributeTestData.Validation))]
    [MemberData(nameof(FalseAttributeTestData.Validation.EdgeCases), MemberType = typeof(FalseAttributeTestData.Validation))]
    public void IsValid_ShouldReturnExpected_WhenValueIsValidOrInvalid(FalseAttributeTestData.Validation.ValidCase testCase)
    {
        // Arrange
        var attribute = new FalseAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Expected)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.False(testCase.Value!.Value, paramName: null);
        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);

        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
    }

    [Theory]
    [MemberData(nameof(FalseAttributeTestData.Validation.InvalidCases), MemberType = typeof(FalseAttributeTestData.Validation))]
    public void IsValid_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new FalseAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
