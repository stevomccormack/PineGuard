using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class BitWiseAttributesTests : BaseUnitTest
{
    private const string DisplayName = "MyProp";

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.BitwiseEqualTo.ValidCases), MemberType = typeof(BitWiseAttributesTestData.BitwiseEqualTo))]
    [MemberData(nameof(BitWiseAttributesTestData.BitwiseEqualTo.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.BitwiseEqualTo))]
    public void BitwiseEqualTo_ShouldReturnExpected(BitWiseAttributesTestData.BitwiseEqualTo.ValidCase testCase)
    {
        // Arrange
        var attribute = new BitwiseEqualToAttribute(testCase.EqualTo);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.BitwiseEqualTo(testCase.Value.Value, testCase.EqualTo, mask: null, paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Equality.NotEqual, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.BitwiseEqualTo.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.BitwiseEqualTo))]
    public void BitwiseEqualTo_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new BitwiseEqualToAttribute(5);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.NotBitwiseEqualTo.ValidCases), MemberType = typeof(BitWiseAttributesTestData.NotBitwiseEqualTo))]
    [MemberData(nameof(BitWiseAttributesTestData.NotBitwiseEqualTo.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.NotBitwiseEqualTo))]
    public void NotBitwiseEqualTo_ShouldReturnExpected(BitWiseAttributesTestData.NotBitwiseEqualTo.ValidCase testCase)
    {
        // Arrange
        var attribute = new NotBitwiseEqualToAttribute(testCase.EqualTo);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.NotBitwiseEqualTo(testCase.Value.Value, testCase.EqualTo, mask: null, paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Equality.Equal, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.NotBitwiseEqualTo.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.NotBitwiseEqualTo))]
    public void NotBitwiseEqualTo_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new NotBitwiseEqualToAttribute(5);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasAllBits.ValidCases), MemberType = typeof(BitWiseAttributesTestData.HasAllBits))]
    [MemberData(nameof(BitWiseAttributesTestData.HasAllBits.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.HasAllBits))]
    public void HasAllBits_ShouldReturnExpected(BitWiseAttributesTestData.HasAllBits.ValidCase testCase)
    {
        // Arrange
        var attribute = new HasAllBitsAttribute(testCase.Mask);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.HasAllBits(testCase.Value.Value, testCase.Mask.ToString(), paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Bits.NotAllSet, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasAllBits.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.HasAllBits))]
    public void HasAllBits_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new HasAllBitsAttribute(5);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasAnyBits.ValidCases), MemberType = typeof(BitWiseAttributesTestData.HasAnyBits))]
    [MemberData(nameof(BitWiseAttributesTestData.HasAnyBits.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.HasAnyBits))]
    public void HasAnyBits_ShouldReturnExpected(BitWiseAttributesTestData.HasAnyBits.ValidCase testCase)
    {
        // Arrange
        var attribute = new HasAnyBitsAttribute(testCase.Mask);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.HasAnyBits(testCase.Value.Value, testCase.Mask.ToString(), paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Bits.NoneSet, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasAnyBits.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.HasAnyBits))]
    public void HasAnyBits_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new HasAnyBitsAttribute(5);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasNoBits.ValidCases), MemberType = typeof(BitWiseAttributesTestData.HasNoBits))]
    [MemberData(nameof(BitWiseAttributesTestData.HasNoBits.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.HasNoBits))]
    public void HasNoBits_ShouldReturnExpected(BitWiseAttributesTestData.HasNoBits.ValidCase testCase)
    {
        // Arrange
        var attribute = new HasNoBitsAttribute(testCase.Mask);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.HasNoBits(testCase.Value.Value, testCase.Mask.ToString(), paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Bits.AnySet, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasNoBits.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.HasNoBits))]
    public void HasNoBits_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new HasNoBitsAttribute(5);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasOnlyBits.ValidCases), MemberType = typeof(BitWiseAttributesTestData.HasOnlyBits))]
    [MemberData(nameof(BitWiseAttributesTestData.HasOnlyBits.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.HasOnlyBits))]
    public void HasOnlyBits_ShouldReturnExpected(BitWiseAttributesTestData.HasOnlyBits.ValidCase testCase)
    {
        // Arrange
        var attribute = new HasOnlyBitsAttribute(testCase.Mask);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.HasOnlyBits(testCase.Value.Value, testCase.Mask.ToString(), paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Bits.NotSubset, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.HasOnlyBits.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.HasOnlyBits))]
    public void HasOnlyBits_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new HasOnlyBitsAttribute(7);
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.PowerOfTwo.ValidCases), MemberType = typeof(BitWiseAttributesTestData.PowerOfTwo))]
    [MemberData(nameof(BitWiseAttributesTestData.PowerOfTwo.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.PowerOfTwo))]
    public void PowerOfTwo_ShouldReturnExpected(BitWiseAttributesTestData.PowerOfTwo.ValidCase testCase)
    {
        // Arrange
        var attribute = new PowerOfTwoAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.PowerOfTwo(testCase.Value.Value, paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Value.NotPowerOfTwo, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.PowerOfTwo.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.PowerOfTwo))]
    public void PowerOfTwo_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new PowerOfTwoAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.NotPowerOfTwo.ValidCases), MemberType = typeof(BitWiseAttributesTestData.NotPowerOfTwo))]
    [MemberData(nameof(BitWiseAttributesTestData.NotPowerOfTwo.EdgeCases), MemberType = typeof(BitWiseAttributesTestData.NotPowerOfTwo))]
    public void NotPowerOfTwo_ShouldReturnExpected(BitWiseAttributesTestData.NotPowerOfTwo.ValidCase testCase)
    {
        // Arrange
        var attribute = new NotPowerOfTwoAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var result = attribute.GetValidationResult(testCase.Value, context);

        // Assert
        if (testCase.Value is null)
        {
            Assert.Equal(ValidationResult.Success, result);
            return;
        }

        var mustResult = Must.Be.NotPowerOfTwo(testCase.Value.Value, paramName: null);
        Assert.Equal(mustResult.Success, result == ValidationResult.Success);

        if (mustResult.Success)
            return;

        var expectedMessage = mustResult.Message.Replace("{paramName}", DisplayName);
        Assert.Equal(expectedMessage, result!.ErrorMessage);
        Assert.Equal(MustCodes.Bitwise.Value.PowerOfTwo, attribute.Code);
    }

    [Theory]
    [MemberData(nameof(BitWiseAttributesTestData.NotPowerOfTwo.InvalidCases), MemberType = typeof(BitWiseAttributesTestData.NotPowerOfTwo))]
    public void NotPowerOfTwo_ShouldThrow_WhenValueTypeIsInvalid(IThrowsCase testCase)
    {
        // Arrange
        var attribute = new NotPowerOfTwoAttribute();
        var context = new ValidationContext(new object()) { DisplayName = DisplayName };

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => attribute.GetValidationResult(((ThrowsCase<object?>)testCase).Value, context));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
