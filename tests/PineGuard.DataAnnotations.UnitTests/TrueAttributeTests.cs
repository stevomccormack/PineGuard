using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TrueAttributeTests : BaseUnitTest
{
    private const string DisplayName = "MyProp";

    [Theory]
    [MemberData(nameof(TrueAttributeTestData.Validation.ValidCases), MemberType = typeof(TrueAttributeTestData.Validation))]
    [MemberData(nameof(TrueAttributeTestData.Validation.EdgeCases), MemberType = typeof(TrueAttributeTestData.Validation))]
    public void IsValid_ShouldReturnExpected_WhenValueIsValidOrInvalid(TrueAttributeTestData.Validation.ValidCase testCase)
    {
        // Arrange
        var attribute = new TrueAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Expected)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.True(testCase.Value!.Value, paramName: null);
        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);

        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Boolean.Value.False, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(TrueAttributeTestData.Validation.InvalidCases), MemberType = typeof(TrueAttributeTestData.Validation))]
    public void IsValid_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new TrueAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
