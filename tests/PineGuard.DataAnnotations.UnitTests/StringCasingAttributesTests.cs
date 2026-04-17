using System.ComponentModel.DataAnnotations;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringCasingAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.CaseStyle.Cases), MemberType = typeof(StringCasingAttributesTestData.CaseStyle))]
    public void CaseStyle_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CaseStyleAttribute(StringCasing.CamelCase);
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotCaseStyle.Cases), MemberType = typeof(StringCasingAttributesTestData.NotCaseStyle))]
    public void NotCaseStyle_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotCaseStyleAttribute(StringCasing.CamelCase);
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.CamelCase.Cases), MemberType = typeof(StringCasingAttributesTestData.CamelCase))]
    public void CamelCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CamelCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotCamelCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotCamelCase))]
    public void NotCamelCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotCamelCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.PascalCase.Cases), MemberType = typeof(StringCasingAttributesTestData.PascalCase))]
    public void PascalCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new PascalCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotPascalCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotPascalCase))]
    public void NotPascalCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotPascalCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.SnakeCase.Cases), MemberType = typeof(StringCasingAttributesTestData.SnakeCase))]
    public void SnakeCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new SnakeCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotSnakeCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotSnakeCase))]
    public void NotSnakeCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotSnakeCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.UpperSnakeCase.Cases), MemberType = typeof(StringCasingAttributesTestData.UpperSnakeCase))]
    public void UpperSnakeCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new UpperSnakeCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotUpperSnakeCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotUpperSnakeCase))]
    public void NotUpperSnakeCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotUpperSnakeCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.KebabCase.Cases), MemberType = typeof(StringCasingAttributesTestData.KebabCase))]
    public void KebabCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new KebabCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotKebabCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotKebabCase))]
    public void NotKebabCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotKebabCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.TrainCase.Cases), MemberType = typeof(StringCasingAttributesTestData.TrainCase))]
    public void TrainCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new TrainCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotTrainCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotTrainCase))]
    public void NotTrainCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotTrainCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.DotCase.Cases), MemberType = typeof(StringCasingAttributesTestData.DotCase))]
    public void DotCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new DotCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotDotCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotDotCase))]
    public void NotDotCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotDotCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.SpaceCase.Cases), MemberType = typeof(StringCasingAttributesTestData.SpaceCase))]
    public void SpaceCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new SpaceCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotSpaceCase.Cases), MemberType = typeof(StringCasingAttributesTestData.NotSpaceCase))]
    public void NotSpaceCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotSpaceCaseAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.UpperInvariant.Cases), MemberType = typeof(StringCasingAttributesTestData.UpperInvariant))]
    public void UpperInvariant_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new UpperInvariantAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotUpperInvariant.Cases), MemberType = typeof(StringCasingAttributesTestData.NotUpperInvariant))]
    public void NotUpperInvariant_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotUpperInvariantAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.LowerInvariant.Cases), MemberType = typeof(StringCasingAttributesTestData.LowerInvariant))]
    public void LowerInvariant_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LowerInvariantAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotLowerInvariant.Cases), MemberType = typeof(StringCasingAttributesTestData.NotLowerInvariant))]
    public void NotLowerInvariant_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotLowerInvariantAttribute();
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.UppercaseString.Cases), MemberType = typeof(StringCasingAttributesTestData.UppercaseString))]
    public void UppercaseString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new UppercaseStringAttribute { LettersOnly = false };
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotUppercaseString.Cases), MemberType = typeof(StringCasingAttributesTestData.NotUppercaseString))]
    public void NotUppercaseString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotUppercaseStringAttribute { LettersOnly = false };
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.LowercaseString.Cases), MemberType = typeof(StringCasingAttributesTestData.LowercaseString))]
    public void LowercaseString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LowercaseStringAttribute { LettersOnly = false };
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringCasingAttributesTestData.NotLowercaseString.Cases), MemberType = typeof(StringCasingAttributesTestData.NotLowercaseString))]
    public void NotLowercaseString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotLowercaseStringAttribute { LettersOnly = false };
        var ctx = new ValidationContext(new object());

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
