using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

/// <summary>
/// Tests custom ErrorMessage paths across different attribute base classes.
/// Covers ValidationAttributeBase.FromMustResult, InvokeAndMapResult, and per-attribute error handling.
/// </summary>
public sealed class ErrorMessageAttributesTests
{
    private static readonly int[] EmptyInts = [];

    // --- FromMustResult ErrorMessage path (simple attributes using FromMustResult directly) ---

    [Theory]
    [InlineData("Custom error for {0}")]
    public void SimpleAttribute_WithCustomErrorMessage_ShouldReturnCustomError(string errorMessage)
    {
        // Arrange — EmailAttribute is a simple string attribute using FromMustResult
        var attribute = new EmailAttribute { ErrorMessage = errorMessage };

        // Act
        var result = attribute.GetValidationResult("not-an-email", new ValidationContext(new object()));

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("Custom error for", result.ErrorMessage!);
    }

    // --- FormatErrorMessage {paramName} substitution path (ValidationAttributeBase.FormatErrorMessage) ---

    [Theory]
    [InlineData("The {paramName} must be a valid email.")]
    public void SimpleAttribute_WithParamNameToken_ShouldSubstituteParamName(string errorMessage)
    {
        // Arrange — a custom ErrorMessage reusing the library's {paramName} convention takes the
        // FormatErrorMessage override's substitution branch instead of the string.Format base fallback.
        var attribute = new EmailAttribute { ErrorMessage = errorMessage };
        var context = new ValidationContext(new object()) { DisplayName = "Email" };

        // Act
        var result = attribute.GetValidationResult("not-an-email", context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("The Email must be a valid email.", result.ErrorMessage);
    }

    // --- InvokeAndMapResult ErrorMessage path (Number attributes using reflection + InvokeAndMapResult) ---

    [Theory]
    [InlineData("Number must be positive: {0}")]
    public void NumberAttribute_WithCustomErrorMessage_ShouldReturnCustomError(string errorMessage)
    {
        // Arrange
        var attribute = new PositiveNumberAttribute { ErrorMessage = errorMessage };

        // Act
        var result = attribute.GetValidationResult(-1, new ValidationContext(new object()));

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("Number must be positive:", result.ErrorMessage!);
    }

    // --- Collection attribute InvokeAndMapResult ErrorMessage path ---

    [Theory]
    [InlineData("Collection error: {0}")]
    public void CollectionAttribute_WithCustomErrorMessage_ShouldReturnCustomError(string errorMessage)
    {
        // Arrange
        var attribute = new NotEmptyCollectionAttribute { ErrorMessage = errorMessage };

        // Act
        var result = attribute.GetValidationResult(EmptyInts, new ValidationContext(new object()));

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("Collection error:", result.ErrorMessage!);
    }

    // --- OfType ErrorMessage path ---

    [Theory]
    [InlineData("Type error: {0}")]
    public void OfTypeAttribute_WithCustomErrorMessage_ShouldReturnCustomError(string errorMessage)
    {
        // Arrange
        var attribute = new OfTypeAttribute(typeof(string)) { ErrorMessage = errorMessage };

        // Act — pass an int, which is not a string
        var result = attribute.GetValidationResult(42, new ValidationContext(new object()));

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("Type error:", result.ErrorMessage!);
    }

    // --- NotOfType ErrorMessage path ---

    [Theory]
    [InlineData("NotOfType error: {0}")]
    public void NotOfTypeAttribute_WithCustomErrorMessage_ShouldReturnCustomError(string errorMessage)
    {
        // Arrange
        var attribute = new NotOfTypeAttribute(typeof(string)) { ErrorMessage = errorMessage };

        // Act — pass a string, which IS of type string
        var result = attribute.GetValidationResult("hello", new ValidationContext(new object()));

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("NotOfType error:", result.ErrorMessage!);
    }

    // --- ObjectAttributeBase.CheckArgCompatibility type mismatch ---

    [Theory]
    [InlineData("abc")]
    public void EqualToAttribute_WithTypeMismatch_ShouldReturnError(string comparisonValue)
    {
        // Arrange — EqualTo("abc") compared against int value. Type mismatch in CheckArgCompatibility.
        var attribute = new EqualToAttribute(comparisonValue);

        // Act — pass an int (type mismatch with string comparison value)
        var result = attribute.GetValidationResult(42, new ValidationContext(new object()));

        // Assert — should return a type mismatch ValidationResult
        Assert.NotNull(result);
        Assert.Contains("Type mismatch", result.ErrorMessage!);
    }
}
