using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class CharAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharAscii.Cases), MemberType = typeof(CharAttributesTestData.CharAscii))]
    public void CharAscii_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharAsciiAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNonAscii.Cases), MemberType = typeof(CharAttributesTestData.CharNonAscii))]
    public void CharNonAscii_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNonAsciiAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharDigit.Cases), MemberType = typeof(CharAttributesTestData.CharDigit))]
    public void CharDigit_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharDigitAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNotDigit.Cases), MemberType = typeof(CharAttributesTestData.CharNotDigit))]
    public void CharNotDigit_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNotDigitAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharLetter.Cases), MemberType = typeof(CharAttributesTestData.CharLetter))]
    public void CharLetter_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharLetterAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNotLetter.Cases), MemberType = typeof(CharAttributesTestData.CharNotLetter))]
    public void CharNotLetter_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNotLetterAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharLetterOrDigit.Cases), MemberType = typeof(CharAttributesTestData.CharLetterOrDigit))]
    public void CharLetterOrDigit_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharLetterOrDigitAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNonLetterOrDigit.Cases), MemberType = typeof(CharAttributesTestData.CharNonLetterOrDigit))]
    public void CharNonLetterOrDigit_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNonLetterOrDigitAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharLowercase.Cases), MemberType = typeof(CharAttributesTestData.CharLowercase))]
    public void CharLowercase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharLowercaseAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharUppercase.Cases), MemberType = typeof(CharAttributesTestData.CharUppercase))]
    public void CharUppercase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharUppercaseAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharHexDigit.Cases), MemberType = typeof(CharAttributesTestData.CharHexDigit))]
    public void CharHexDigit_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharHexDigitAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNotHexDigit.Cases), MemberType = typeof(CharAttributesTestData.CharNotHexDigit))]
    public void CharNotHexDigit_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNotHexDigitAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharPrintableAscii.Cases), MemberType = typeof(CharAttributesTestData.CharPrintableAscii))]
    public void CharPrintableAscii_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharPrintableAsciiAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNonPrintableAscii.Cases), MemberType = typeof(CharAttributesTestData.CharNonPrintableAscii))]
    public void CharNonPrintableAscii_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNonPrintableAsciiAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNonWhitespace.Cases), MemberType = typeof(CharAttributesTestData.CharNonWhitespace))]
    public void CharNonWhitespace_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNonWhitespaceAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharControl.Cases), MemberType = typeof(CharAttributesTestData.CharControl))]
    public void CharControl_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharControlAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharAttributesTestData.CharNotControl.Cases), MemberType = typeof(CharAttributesTestData.CharNotControl))]
    public void CharNotControl_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CharNotControlAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
