using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    private static void Verify<TAttribute>(TAttribute attribute, StringAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ExactLength.ValidCases), MemberType = typeof(StringAttributesTestData.ExactLength))]
    [MemberData(nameof(StringAttributesTestData.ExactLength.EdgeCases), MemberType = typeof(StringAttributesTestData.ExactLength))]
    [MemberData(nameof(StringAttributesTestData.ExactLength.InvalidCases), MemberType = typeof(StringAttributesTestData.ExactLength))]
    public void ExactLength_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ExactLengthAttribute(3);
        Assert.Equal(MustCodes.Text.Length.Mismatch, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.LengthBetween.ValidCases), MemberType = typeof(StringAttributesTestData.LengthBetween))]
    [MemberData(nameof(StringAttributesTestData.LengthBetween.EdgeCases), MemberType = typeof(StringAttributesTestData.LengthBetween))]
    [MemberData(nameof(StringAttributesTestData.LengthBetween.InvalidCases), MemberType = typeof(StringAttributesTestData.LengthBetween))]
    public void LengthBetween_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new LengthBetweenAttribute(3, 5);
        Assert.Equal(MustCodes.Text.Length.OutOfRange, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.LongerThan.ValidCases), MemberType = typeof(StringAttributesTestData.LongerThan))]
    [MemberData(nameof(StringAttributesTestData.LongerThan.EdgeCases), MemberType = typeof(StringAttributesTestData.LongerThan))]
    [MemberData(nameof(StringAttributesTestData.LongerThan.InvalidCases), MemberType = typeof(StringAttributesTestData.LongerThan))]
    public void LongerThan_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new LongerThanAttribute(3);
        Assert.Equal(MustCodes.Text.Length.TooShort, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ShorterThan.ValidCases), MemberType = typeof(StringAttributesTestData.ShorterThan))]
    [MemberData(nameof(StringAttributesTestData.ShorterThan.EdgeCases), MemberType = typeof(StringAttributesTestData.ShorterThan))]
    [MemberData(nameof(StringAttributesTestData.ShorterThan.InvalidCases), MemberType = typeof(StringAttributesTestData.ShorterThan))]
    public void ShorterThan_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ShorterThanAttribute(3);
        Assert.Equal(MustCodes.Text.Length.TooLong, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.Match.ValidCases), MemberType = typeof(StringAttributesTestData.Match))]
    [MemberData(nameof(StringAttributesTestData.Match.EdgeCases), MemberType = typeof(StringAttributesTestData.Match))]
    [MemberData(nameof(StringAttributesTestData.Match.InvalidCases), MemberType = typeof(StringAttributesTestData.Match))]
    public void Match_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new MatchAttribute(@"^\d+$");
        Assert.Equal(MustCodes.Text.Pattern.NoMatch, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotMatch.ValidCases), MemberType = typeof(StringAttributesTestData.NotMatch))]
    [MemberData(nameof(StringAttributesTestData.NotMatch.EdgeCases), MemberType = typeof(StringAttributesTestData.NotMatch))]
    [MemberData(nameof(StringAttributesTestData.NotMatch.InvalidCases), MemberType = typeof(StringAttributesTestData.NotMatch))]
    public void NotMatch_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotMatchAttribute(@"^\d+$");
        Assert.Equal(MustCodes.Text.Pattern.Match, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.Alphabetic.ValidCases), MemberType = typeof(StringAttributesTestData.Alphabetic))]
    [MemberData(nameof(StringAttributesTestData.Alphabetic.EdgeCases), MemberType = typeof(StringAttributesTestData.Alphabetic))]
    [MemberData(nameof(StringAttributesTestData.Alphabetic.InvalidCases), MemberType = typeof(StringAttributesTestData.Alphabetic))]
    public void Alphabetic_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new AlphabeticAttribute();
        Assert.Equal(MustCodes.Text.Charset.NotAlpha, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotAlphabetic.ValidCases), MemberType = typeof(StringAttributesTestData.NotAlphabetic))]
    [MemberData(nameof(StringAttributesTestData.NotAlphabetic.EdgeCases), MemberType = typeof(StringAttributesTestData.NotAlphabetic))]
    [MemberData(nameof(StringAttributesTestData.NotAlphabetic.InvalidCases), MemberType = typeof(StringAttributesTestData.NotAlphabetic))]
    public void NotAlphabetic_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotAlphabeticAttribute();
        Assert.Equal(MustCodes.Text.Charset.Alpha, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.Alphanumeric.ValidCases), MemberType = typeof(StringAttributesTestData.Alphanumeric))]
    [MemberData(nameof(StringAttributesTestData.Alphanumeric.EdgeCases), MemberType = typeof(StringAttributesTestData.Alphanumeric))]
    [MemberData(nameof(StringAttributesTestData.Alphanumeric.InvalidCases), MemberType = typeof(StringAttributesTestData.Alphanumeric))]
    public void Alphanumeric_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new AlphanumericAttribute();
        Assert.Equal(MustCodes.Text.Charset.NotAlphanumeric, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotAlphanumeric.ValidCases), MemberType = typeof(StringAttributesTestData.NotAlphanumeric))]
    [MemberData(nameof(StringAttributesTestData.NotAlphanumeric.EdgeCases), MemberType = typeof(StringAttributesTestData.NotAlphanumeric))]
    [MemberData(nameof(StringAttributesTestData.NotAlphanumeric.InvalidCases), MemberType = typeof(StringAttributesTestData.NotAlphanumeric))]
    public void NotAlphanumeric_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotAlphanumericAttribute();
        Assert.Equal(MustCodes.Text.Charset.Alphanumeric, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NumericString.ValidCases), MemberType = typeof(StringAttributesTestData.NumericString))]
    [MemberData(nameof(StringAttributesTestData.NumericString.EdgeCases), MemberType = typeof(StringAttributesTestData.NumericString))]
    [MemberData(nameof(StringAttributesTestData.NumericString.InvalidCases), MemberType = typeof(StringAttributesTestData.NumericString))]
    public void NumericString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NumericStringAttribute();
        Assert.Equal(MustCodes.Text.Charset.NotNumeric, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotNumericString.ValidCases), MemberType = typeof(StringAttributesTestData.NotNumericString))]
    [MemberData(nameof(StringAttributesTestData.NotNumericString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotNumericString))]
    [MemberData(nameof(StringAttributesTestData.NotNumericString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotNumericString))]
    public void NotNumericString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotNumericStringAttribute();
        Assert.Equal(MustCodes.Text.Charset.Numeric, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.DigitsOnly.ValidCases), MemberType = typeof(StringAttributesTestData.DigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.DigitsOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.DigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.DigitsOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.DigitsOnly))]
    public void DigitsOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new DigitsOnlyAttribute();
        Assert.Equal(MustCodes.Text.Charset.NotDigits, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotDigitsOnly.ValidCases), MemberType = typeof(StringAttributesTestData.NotDigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.NotDigitsOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.NotDigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.NotDigitsOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.NotDigitsOnly))]
    public void NotDigitsOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotDigitsOnlyAttribute();
        Assert.Equal(MustCodes.Text.Charset.Digits, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.EmptyString.ValidCases), MemberType = typeof(StringAttributesTestData.EmptyString))]
    [MemberData(nameof(StringAttributesTestData.EmptyString.EdgeCases), MemberType = typeof(StringAttributesTestData.EmptyString))]
    [MemberData(nameof(StringAttributesTestData.EmptyString.InvalidCases), MemberType = typeof(StringAttributesTestData.EmptyString))]
    public void EmptyString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new EmptyStringAttribute();
        Assert.Equal(MustCodes.Text.Content.NotEmpty, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NullOrEmptyString.ValidCases), MemberType = typeof(StringAttributesTestData.NullOrEmptyString))]
    [MemberData(nameof(StringAttributesTestData.NullOrEmptyString.InvalidCases), MemberType = typeof(StringAttributesTestData.NullOrEmptyString))]
    public void NullOrEmptyString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NullOrEmptyStringAttribute();
        Assert.Equal(MustCodes.Text.Content.NotNullOrEmpty, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotNullOrEmptyString.ValidCases), MemberType = typeof(StringAttributesTestData.NotNullOrEmptyString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrEmptyString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotNullOrEmptyString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrEmptyString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotNullOrEmptyString))]
    public void NotNullOrEmptyString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotNullOrEmptyStringAttribute();
        Assert.Equal(MustCodes.Text.Content.NullOrEmpty, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NullOrWhiteSpaceString.ValidCases), MemberType = typeof(StringAttributesTestData.NullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NullOrWhiteSpaceString.EdgeCases), MemberType = typeof(StringAttributesTestData.NullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NullOrWhiteSpaceString.InvalidCases), MemberType = typeof(StringAttributesTestData.NullOrWhiteSpaceString))]
    public void NullOrWhiteSpaceString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NullOrWhiteSpaceStringAttribute();
        Assert.Equal(MustCodes.Text.Content.NotBlank, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotNullOrWhiteSpaceString.ValidCases), MemberType = typeof(StringAttributesTestData.NotNullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrWhiteSpaceString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotNullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrWhiteSpaceString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotNullOrWhiteSpaceString))]
    public void NotNullOrWhiteSpaceString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotNullOrWhiteSpaceStringAttribute();
        Assert.Equal(MustCodes.Text.Content.Blank, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.LongerThanOrEqual.ValidCases), MemberType = typeof(StringAttributesTestData.LongerThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.LongerThanOrEqual.EdgeCases), MemberType = typeof(StringAttributesTestData.LongerThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.LongerThanOrEqual.InvalidCases), MemberType = typeof(StringAttributesTestData.LongerThanOrEqual))]
    public void LongerThanOrEqual_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new LongerThanOrEqualAttribute(3);
        Assert.Equal(MustCodes.Text.Length.TooShort, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ShorterThanOrEqual.ValidCases), MemberType = typeof(StringAttributesTestData.ShorterThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.ShorterThanOrEqual.EdgeCases), MemberType = typeof(StringAttributesTestData.ShorterThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.ShorterThanOrEqual.InvalidCases), MemberType = typeof(StringAttributesTestData.ShorterThanOrEqual))]
    public void ShorterThanOrEqual_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ShorterThanOrEqualAttribute(3);
        Assert.Equal(MustCodes.Text.Length.TooLong, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.AsciiString.ValidCases), MemberType = typeof(StringAttributesTestData.AsciiString))]
    [MemberData(nameof(StringAttributesTestData.AsciiString.EdgeCases), MemberType = typeof(StringAttributesTestData.AsciiString))]
    [MemberData(nameof(StringAttributesTestData.AsciiString.InvalidCases), MemberType = typeof(StringAttributesTestData.AsciiString))]
    public void AsciiString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new AsciiStringAttribute();
        Assert.Equal(MustCodes.Text.Charset.NotAscii, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotAsciiString.ValidCases), MemberType = typeof(StringAttributesTestData.NotAsciiString))]
    [MemberData(nameof(StringAttributesTestData.NotAsciiString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotAsciiString))]
    [MemberData(nameof(StringAttributesTestData.NotAsciiString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotAsciiString))]
    public void NotAsciiString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotAsciiStringAttribute();
        Assert.Equal(MustCodes.Text.Charset.Ascii, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsWhitespace.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.ContainsWhitespace.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.ContainsWhitespace.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsWhitespace))]
    public void ContainsWhitespace_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ContainsWhitespaceAttribute();
        Assert.Equal(MustCodes.Text.Charset.NotContainsWhitespace, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsWhitespace.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.NotContainsWhitespace.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.NotContainsWhitespace.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsWhitespace))]
    public void NotContainsWhitespace_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotContainsWhitespaceAttribute();
        Assert.Equal(MustCodes.Text.Charset.ContainsWhitespace, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsControlChars.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.ContainsControlChars.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.ContainsControlChars.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsControlChars))]
    public void ContainsControlChars_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ContainsControlCharsAttribute();
        Assert.Equal(MustCodes.Text.Charset.NotContainsControl, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsControlChars.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.NotContainsControlChars.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.NotContainsControlChars.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsControlChars))]
    public void NotContainsControlChars_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotContainsControlCharsAttribute();
        Assert.Equal(MustCodes.Text.Charset.ContainsControl, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsAllowedOnly.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.ContainsAllowedOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.ContainsAllowedOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsAllowedOnly))]
    public void ContainsAllowedOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ContainsAllowedOnlyAttribute(['a', 'b']);
        Assert.Equal(MustCodes.Text.Charset.NotSubset, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsAllowedOnly.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.NotContainsAllowedOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.NotContainsAllowedOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsAllowedOnly))]
    public void NotContainsAllowedOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotContainsAllowedOnlyAttribute(['a', 'b']);
        Assert.Equal(MustCodes.Text.Charset.Subset, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsDisallowed.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.ContainsDisallowed.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.ContainsDisallowed.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsDisallowed))]
    public void ContainsDisallowed_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ContainsDisallowedAttribute(['x', 'y']);
        Assert.Equal(MustCodes.Text.Charset.NotContainsDisallowed, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsDisallowed.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.NotContainsDisallowed.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.NotContainsDisallowed.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsDisallowed))]
    public void NotContainsDisallowed_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new NotContainsDisallowedAttribute(['x', 'y']);
        Assert.Equal(MustCodes.Text.Charset.ContainsDisallowed, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsAny.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsAny))]
    [MemberData(nameof(StringAttributesTestData.ContainsAny.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsAny))]
    [MemberData(nameof(StringAttributesTestData.ContainsAny.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsAny))]
    public void ContainsAny_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
    {
        var attribute = new ContainsAnyAttribute(['x', 'y']);
        Assert.Equal(MustCodes.Text.Charset.NotContainsAny, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.Contains.Cases), MemberType = typeof(StringAttributesTestData.Contains))]
    public void Contains_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new ContainsAttribute(StringAttributesTestData.Contains.Substring);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsIgnoringCase.Cases), MemberType = typeof(StringAttributesTestData.ContainsIgnoringCase))]
    public void ContainsIgnoringCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new ContainsAttribute(StringAttributesTestData.ContainsIgnoringCase.Substring) { Comparison = StringAttributesTestData.ContainsIgnoringCase.Comparison };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContains.Cases), MemberType = typeof(StringAttributesTestData.NotContains))]
    public void NotContains_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotContainsAttribute(StringAttributesTestData.NotContains.Substring);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsIgnoringCase.Cases), MemberType = typeof(StringAttributesTestData.NotContainsIgnoringCase))]
    public void NotContainsIgnoringCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotContainsAttribute(StringAttributesTestData.NotContainsIgnoringCase.Substring) { Comparison = StringAttributesTestData.NotContainsIgnoringCase.Comparison };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.StartsWith.Cases), MemberType = typeof(StringAttributesTestData.StartsWith))]
    public void StartsWith_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new StartsWithAttribute(StringAttributesTestData.StartsWith.Prefix);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.StartsWithIgnoringCase.Cases), MemberType = typeof(StringAttributesTestData.StartsWithIgnoringCase))]
    public void StartsWithIgnoringCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new StartsWithAttribute(StringAttributesTestData.StartsWithIgnoringCase.Prefix) { Comparison = StringAttributesTestData.StartsWithIgnoringCase.Comparison };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotStartsWith.Cases), MemberType = typeof(StringAttributesTestData.NotStartsWith))]
    public void NotStartsWith_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotStartsWithAttribute(StringAttributesTestData.NotStartsWith.Prefix);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotStartsWithIgnoringCase.Cases), MemberType = typeof(StringAttributesTestData.NotStartsWithIgnoringCase))]
    public void NotStartsWithIgnoringCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotStartsWithAttribute(StringAttributesTestData.NotStartsWithIgnoringCase.Prefix) { Comparison = StringAttributesTestData.NotStartsWithIgnoringCase.Comparison };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.EndsWith.Cases), MemberType = typeof(StringAttributesTestData.EndsWith))]
    public void EndsWith_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new EndsWithAttribute(StringAttributesTestData.EndsWith.Suffix);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.EndsWithIgnoringCase.Cases), MemberType = typeof(StringAttributesTestData.EndsWithIgnoringCase))]
    public void EndsWithIgnoringCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new EndsWithAttribute(StringAttributesTestData.EndsWithIgnoringCase.Suffix) { Comparison = StringAttributesTestData.EndsWithIgnoringCase.Comparison };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotEndsWith.Cases), MemberType = typeof(StringAttributesTestData.NotEndsWith))]
    public void NotEndsWith_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotEndsWithAttribute(StringAttributesTestData.NotEndsWith.Suffix);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotEndsWithIgnoringCase.Cases), MemberType = typeof(StringAttributesTestData.NotEndsWithIgnoringCase))]
    public void NotEndsWithIgnoringCase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotEndsWithAttribute(StringAttributesTestData.NotEndsWithIgnoringCase.Suffix) { Comparison = StringAttributesTestData.NotEndsWithIgnoringCase.Comparison };
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringAttributesTestData.RegexPattern.Cases), MemberType = typeof(StringAttributesTestData.RegexPattern))]
    public void RegexPattern_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new RegexPatternAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
