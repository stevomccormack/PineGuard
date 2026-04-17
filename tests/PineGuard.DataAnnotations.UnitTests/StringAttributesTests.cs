using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringAttributesTests
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
        => Verify(new ExactLengthAttribute(3), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.LengthBetween.ValidCases), MemberType = typeof(StringAttributesTestData.LengthBetween))]
    [MemberData(nameof(StringAttributesTestData.LengthBetween.EdgeCases), MemberType = typeof(StringAttributesTestData.LengthBetween))]
    [MemberData(nameof(StringAttributesTestData.LengthBetween.InvalidCases), MemberType = typeof(StringAttributesTestData.LengthBetween))]
    public void LengthBetween_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new LengthBetweenAttribute(3, 5), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.LongerThan.ValidCases), MemberType = typeof(StringAttributesTestData.LongerThan))]
    [MemberData(nameof(StringAttributesTestData.LongerThan.EdgeCases), MemberType = typeof(StringAttributesTestData.LongerThan))]
    [MemberData(nameof(StringAttributesTestData.LongerThan.InvalidCases), MemberType = typeof(StringAttributesTestData.LongerThan))]
    public void LongerThan_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new LongerThanAttribute(3), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ShorterThan.ValidCases), MemberType = typeof(StringAttributesTestData.ShorterThan))]
    [MemberData(nameof(StringAttributesTestData.ShorterThan.EdgeCases), MemberType = typeof(StringAttributesTestData.ShorterThan))]
    [MemberData(nameof(StringAttributesTestData.ShorterThan.InvalidCases), MemberType = typeof(StringAttributesTestData.ShorterThan))]
    public void ShorterThan_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new ShorterThanAttribute(3), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.Match.ValidCases), MemberType = typeof(StringAttributesTestData.Match))]
    [MemberData(nameof(StringAttributesTestData.Match.EdgeCases), MemberType = typeof(StringAttributesTestData.Match))]
    [MemberData(nameof(StringAttributesTestData.Match.InvalidCases), MemberType = typeof(StringAttributesTestData.Match))]
    public void Match_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new MatchAttribute(@"^\d+$"), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotMatch.ValidCases), MemberType = typeof(StringAttributesTestData.NotMatch))]
    [MemberData(nameof(StringAttributesTestData.NotMatch.EdgeCases), MemberType = typeof(StringAttributesTestData.NotMatch))]
    [MemberData(nameof(StringAttributesTestData.NotMatch.InvalidCases), MemberType = typeof(StringAttributesTestData.NotMatch))]
    public void NotMatch_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotMatchAttribute(@"^\d+$"), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.Alphabetic.ValidCases), MemberType = typeof(StringAttributesTestData.Alphabetic))]
    [MemberData(nameof(StringAttributesTestData.Alphabetic.EdgeCases), MemberType = typeof(StringAttributesTestData.Alphabetic))]
    [MemberData(nameof(StringAttributesTestData.Alphabetic.InvalidCases), MemberType = typeof(StringAttributesTestData.Alphabetic))]
    public void Alphabetic_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new AlphabeticAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotAlphabetic.ValidCases), MemberType = typeof(StringAttributesTestData.NotAlphabetic))]
    [MemberData(nameof(StringAttributesTestData.NotAlphabetic.EdgeCases), MemberType = typeof(StringAttributesTestData.NotAlphabetic))]
    [MemberData(nameof(StringAttributesTestData.NotAlphabetic.InvalidCases), MemberType = typeof(StringAttributesTestData.NotAlphabetic))]
    public void NotAlphabetic_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotAlphabeticAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.Alphanumeric.ValidCases), MemberType = typeof(StringAttributesTestData.Alphanumeric))]
    [MemberData(nameof(StringAttributesTestData.Alphanumeric.EdgeCases), MemberType = typeof(StringAttributesTestData.Alphanumeric))]
    [MemberData(nameof(StringAttributesTestData.Alphanumeric.InvalidCases), MemberType = typeof(StringAttributesTestData.Alphanumeric))]
    public void Alphanumeric_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new AlphanumericAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotAlphanumeric.ValidCases), MemberType = typeof(StringAttributesTestData.NotAlphanumeric))]
    [MemberData(nameof(StringAttributesTestData.NotAlphanumeric.EdgeCases), MemberType = typeof(StringAttributesTestData.NotAlphanumeric))]
    [MemberData(nameof(StringAttributesTestData.NotAlphanumeric.InvalidCases), MemberType = typeof(StringAttributesTestData.NotAlphanumeric))]
    public void NotAlphanumeric_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotAlphanumericAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NumericString.ValidCases), MemberType = typeof(StringAttributesTestData.NumericString))]
    [MemberData(nameof(StringAttributesTestData.NumericString.EdgeCases), MemberType = typeof(StringAttributesTestData.NumericString))]
    [MemberData(nameof(StringAttributesTestData.NumericString.InvalidCases), MemberType = typeof(StringAttributesTestData.NumericString))]
    public void NumericString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NumericStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotNumericString.ValidCases), MemberType = typeof(StringAttributesTestData.NotNumericString))]
    [MemberData(nameof(StringAttributesTestData.NotNumericString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotNumericString))]
    [MemberData(nameof(StringAttributesTestData.NotNumericString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotNumericString))]
    public void NotNumericString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotNumericStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.DigitsOnly.ValidCases), MemberType = typeof(StringAttributesTestData.DigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.DigitsOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.DigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.DigitsOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.DigitsOnly))]
    public void DigitsOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new DigitsOnlyAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotDigitsOnly.ValidCases), MemberType = typeof(StringAttributesTestData.NotDigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.NotDigitsOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.NotDigitsOnly))]
    [MemberData(nameof(StringAttributesTestData.NotDigitsOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.NotDigitsOnly))]
    public void NotDigitsOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotDigitsOnlyAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.EmptyString.ValidCases), MemberType = typeof(StringAttributesTestData.EmptyString))]
    [MemberData(nameof(StringAttributesTestData.EmptyString.EdgeCases), MemberType = typeof(StringAttributesTestData.EmptyString))]
    [MemberData(nameof(StringAttributesTestData.EmptyString.InvalidCases), MemberType = typeof(StringAttributesTestData.EmptyString))]
    public void EmptyString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new EmptyStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NullOrEmptyString.ValidCases), MemberType = typeof(StringAttributesTestData.NullOrEmptyString))]
    [MemberData(nameof(StringAttributesTestData.NullOrEmptyString.InvalidCases), MemberType = typeof(StringAttributesTestData.NullOrEmptyString))]
    public void NullOrEmptyString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NullOrEmptyStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotNullOrEmptyString.ValidCases), MemberType = typeof(StringAttributesTestData.NotNullOrEmptyString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrEmptyString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotNullOrEmptyString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrEmptyString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotNullOrEmptyString))]
    public void NotNullOrEmptyString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotNullOrEmptyStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NullOrWhiteSpaceString.ValidCases), MemberType = typeof(StringAttributesTestData.NullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NullOrWhiteSpaceString.EdgeCases), MemberType = typeof(StringAttributesTestData.NullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NullOrWhiteSpaceString.InvalidCases), MemberType = typeof(StringAttributesTestData.NullOrWhiteSpaceString))]
    public void NullOrWhiteSpaceString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NullOrWhiteSpaceStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotNullOrWhiteSpaceString.ValidCases), MemberType = typeof(StringAttributesTestData.NotNullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrWhiteSpaceString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotNullOrWhiteSpaceString))]
    [MemberData(nameof(StringAttributesTestData.NotNullOrWhiteSpaceString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotNullOrWhiteSpaceString))]
    public void NotNullOrWhiteSpaceString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotNullOrWhiteSpaceStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.LongerThanOrEqual.ValidCases), MemberType = typeof(StringAttributesTestData.LongerThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.LongerThanOrEqual.EdgeCases), MemberType = typeof(StringAttributesTestData.LongerThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.LongerThanOrEqual.InvalidCases), MemberType = typeof(StringAttributesTestData.LongerThanOrEqual))]
    public void LongerThanOrEqual_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new LongerThanOrEqualAttribute(3), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ShorterThanOrEqual.ValidCases), MemberType = typeof(StringAttributesTestData.ShorterThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.ShorterThanOrEqual.EdgeCases), MemberType = typeof(StringAttributesTestData.ShorterThanOrEqual))]
    [MemberData(nameof(StringAttributesTestData.ShorterThanOrEqual.InvalidCases), MemberType = typeof(StringAttributesTestData.ShorterThanOrEqual))]
    public void ShorterThanOrEqual_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new ShorterThanOrEqualAttribute(3), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.AsciiString.ValidCases), MemberType = typeof(StringAttributesTestData.AsciiString))]
    [MemberData(nameof(StringAttributesTestData.AsciiString.EdgeCases), MemberType = typeof(StringAttributesTestData.AsciiString))]
    [MemberData(nameof(StringAttributesTestData.AsciiString.InvalidCases), MemberType = typeof(StringAttributesTestData.AsciiString))]
    public void AsciiString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new AsciiStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotAsciiString.ValidCases), MemberType = typeof(StringAttributesTestData.NotAsciiString))]
    [MemberData(nameof(StringAttributesTestData.NotAsciiString.EdgeCases), MemberType = typeof(StringAttributesTestData.NotAsciiString))]
    [MemberData(nameof(StringAttributesTestData.NotAsciiString.InvalidCases), MemberType = typeof(StringAttributesTestData.NotAsciiString))]
    public void NotAsciiString_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotAsciiStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsWhitespace.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.ContainsWhitespace.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.ContainsWhitespace.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsWhitespace))]
    public void ContainsWhitespace_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new ContainsWhitespaceAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsWhitespace.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.NotContainsWhitespace.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsWhitespace))]
    [MemberData(nameof(StringAttributesTestData.NotContainsWhitespace.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsWhitespace))]
    public void NotContainsWhitespace_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsWhitespaceAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsControlChars.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.ContainsControlChars.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.ContainsControlChars.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsControlChars))]
    public void ContainsControlChars_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new ContainsControlCharsAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsControlChars.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.NotContainsControlChars.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsControlChars))]
    [MemberData(nameof(StringAttributesTestData.NotContainsControlChars.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsControlChars))]
    public void NotContainsControlChars_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsControlCharsAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsAllowedOnly.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.ContainsAllowedOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.ContainsAllowedOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsAllowedOnly))]
    public void ContainsAllowedOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new ContainsAllowedOnlyAttribute(['a', 'b']), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsAllowedOnly.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.NotContainsAllowedOnly.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsAllowedOnly))]
    [MemberData(nameof(StringAttributesTestData.NotContainsAllowedOnly.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsAllowedOnly))]
    public void NotContainsAllowedOnly_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsAllowedOnlyAttribute(['a', 'b']), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsDisallowed.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.ContainsDisallowed.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.ContainsDisallowed.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsDisallowed))]
    public void ContainsDisallowed_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new ContainsDisallowedAttribute(['x', 'y']), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.NotContainsDisallowed.ValidCases), MemberType = typeof(StringAttributesTestData.NotContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.NotContainsDisallowed.EdgeCases), MemberType = typeof(StringAttributesTestData.NotContainsDisallowed))]
    [MemberData(nameof(StringAttributesTestData.NotContainsDisallowed.InvalidCases), MemberType = typeof(StringAttributesTestData.NotContainsDisallowed))]
    public void NotContainsDisallowed_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new NotContainsDisallowedAttribute(['x', 'y']), testCase);

    [Theory]
    [MemberData(nameof(StringAttributesTestData.ContainsAny.ValidCases), MemberType = typeof(StringAttributesTestData.ContainsAny))]
    [MemberData(nameof(StringAttributesTestData.ContainsAny.EdgeCases), MemberType = typeof(StringAttributesTestData.ContainsAny))]
    [MemberData(nameof(StringAttributesTestData.ContainsAny.InvalidCases), MemberType = typeof(StringAttributesTestData.ContainsAny))]
    public void ContainsAny_ShouldReturnExpected(StringAttributesTestData.ValidCase testCase)
        => Verify(new ContainsAnyAttribute(['x', 'y']), testCase);
}
