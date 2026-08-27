using PineGuard.Codes;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustStringClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustStringClausesTestData.NullOrEmpty.ValidCases), MemberType = typeof(MustStringClausesTestData.NullOrEmpty))]
    public void NullOrEmpty_Checks(MustStringClausesTestData.NullOrEmpty.ValidCase testCase)
    {
        var result = Must.Be.NullOrEmpty(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Content.NotNullOrEmpty, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.NotNullOrEmpty.ValidCases), MemberType = typeof(MustStringClausesTestData.NotNullOrEmpty))]
    public void NotNullOrEmpty_Checks(MustStringClausesTestData.NotNullOrEmpty.ValidCase testCase)
    {
        var result = Must.Be.NotNullOrEmpty(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Content.NullOrEmpty, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.NullOrWhiteSpace.ValidCases), MemberType = typeof(MustStringClausesTestData.NullOrWhiteSpace))]
    public void NullOrWhiteSpace_Checks(MustStringClausesTestData.NullOrWhiteSpace.ValidCase testCase)
    {
        var result = Must.Be.NullOrWhiteSpace(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Content.NotBlank, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.NotNullOrWhiteSpace.ValidCases), MemberType = typeof(MustStringClausesTestData.NotNullOrWhiteSpace))]
    public void NotNullOrWhiteSpace_Checks(MustStringClausesTestData.NotNullOrWhiteSpace.ValidCase testCase)
    {
        var result = Must.Be.NotNullOrWhiteSpace(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Content.Blank, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Empty.ValidCases), MemberType = typeof(MustStringClausesTestData.Empty))]
    public void Empty_Checks(MustStringClausesTestData.Empty.ValidCase testCase)
    {
        var result = Must.Be.Empty(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Content.NotEmpty, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.NotEmpty.ValidCases), MemberType = typeof(MustStringClausesTestData.NotEmpty))]
    public void NotEmpty_Checks(MustStringClausesTestData.NotEmpty.ValidCase testCase)
    {
        var result = Must.Be.NotEmpty(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Content.Empty, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ExactLength.ValidCases), MemberType = typeof(MustStringClausesTestData.ExactLength))]
    public void ExactLength_Checks(MustStringClausesTestData.ExactLength.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ExactLength(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Length.Mismatch, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ExactLength.EdgeCases), MemberType = typeof(MustStringClausesTestData.ExactLength))]
    public void ExactLength_EdgeChecks(MustStringClausesTestData.ExactLength.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ExactLength(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LengthBetween.ValidCases), MemberType = typeof(MustStringClausesTestData.LengthBetween))]
    public void LengthBetween_Checks(MustStringClausesTestData.LengthBetween.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.LengthBetween(input, testCase.Value.Min, testCase.Value.Max);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Length.OutOfRange, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LengthBetween.EdgeCases), MemberType = typeof(MustStringClausesTestData.LengthBetween))]
    public void LengthBetween_EdgeChecks(MustStringClausesTestData.LengthBetween.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.LengthBetween(input, testCase.Value.Min, testCase.Value.Max);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Match.ValidCases), MemberType = typeof(MustStringClausesTestData.Match))]
    public void Match_Checks(MustStringClausesTestData.Match.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Match(input, testCase.Value.Pattern);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Pattern.NoMatch, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Match.EdgeCases), MemberType = typeof(MustStringClausesTestData.Match))]
    public void Match_EdgeChecks(MustStringClausesTestData.Match.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Match(input, testCase.Value.Pattern);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);

        if (testCase.ParamName is not null)
        {
            Assert.Equal(testCase.ParamName, result.ParamName);
        }
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Match.ValidCases), MemberType = typeof(MustStringClausesTestData.Match))]
    public void NotMatch_Checks(MustStringClausesTestData.Match.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotMatch(input, testCase.Value.Pattern);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Pattern.Match, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Match.EdgeCases), MemberType = typeof(MustStringClausesTestData.Match))]
    public void NotMatch_EdgeChecks(MustStringClausesTestData.Match.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotMatch(input, testCase.Value.Pattern);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphabetic.ValidCases), MemberType = typeof(MustStringClausesTestData.Alphabetic))]
    public void Alphabetic_Checks(MustStringClausesTestData.Alphabetic.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Alphabetic(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotAlpha, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphabetic.ValidCases), MemberType = typeof(MustStringClausesTestData.Alphabetic))]
    public void NotAlphabetic_Checks(MustStringClausesTestData.Alphabetic.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotAlphabetic(input, testCase.Value.Inclusions);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.Alpha, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphabetic.EdgeCases), MemberType = typeof(MustStringClausesTestData.Alphabetic))]
    public void Alphabetic_EdgeChecks(MustStringClausesTestData.Alphabetic.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Alphabetic(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphabetic.EdgeCases), MemberType = typeof(MustStringClausesTestData.Alphabetic))]
    public void NotAlphabetic_EdgeChecks(MustStringClausesTestData.Alphabetic.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotAlphabetic(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Numeric.ValidCases), MemberType = typeof(MustStringClausesTestData.Numeric))]
    public void Numeric_Checks(MustStringClausesTestData.Numeric.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Numeric(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotNumeric, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Numeric.ValidCases), MemberType = typeof(MustStringClausesTestData.Numeric))]
    public void NotNumeric_Checks(MustStringClausesTestData.Numeric.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotNumeric(input, testCase.Value.Inclusions);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.Numeric, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Numeric.EdgeCases), MemberType = typeof(MustStringClausesTestData.Numeric))]
    public void Numeric_EdgeChecks(MustStringClausesTestData.Numeric.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Numeric(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Numeric.EdgeCases), MemberType = typeof(MustStringClausesTestData.Numeric))]
    public void NotNumeric_EdgeChecks(MustStringClausesTestData.Numeric.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotNumeric(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphanumeric.ValidCases), MemberType = typeof(MustStringClausesTestData.Alphanumeric))]
    public void Alphanumeric_Checks(MustStringClausesTestData.Alphanumeric.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Alphanumeric(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotAlphanumeric, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphanumeric.ValidCases), MemberType = typeof(MustStringClausesTestData.Alphanumeric))]
    public void NotAlphanumeric_Checks(MustStringClausesTestData.Alphanumeric.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotAlphanumeric(input, testCase.Value.Inclusions);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.Alphanumeric, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphanumeric.EdgeCases), MemberType = typeof(MustStringClausesTestData.Alphanumeric))]
    public void Alphanumeric_EdgeChecks(MustStringClausesTestData.Alphanumeric.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Alphanumeric(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Alphanumeric.EdgeCases), MemberType = typeof(MustStringClausesTestData.Alphanumeric))]
    public void NotAlphanumeric_EdgeChecks(MustStringClausesTestData.Alphanumeric.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotAlphanumeric(input, testCase.Value.Inclusions);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.DigitsOnly.ValidCases), MemberType = typeof(MustStringClausesTestData.DigitsOnly))]
    public void DigitsOnly_Checks(MustStringClausesTestData.DigitsOnly.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        if (testCase.Value.AllowedChars is null)
        {
            var result = Must.Be.DigitsOnly(input);
            Assert.Equal(testCase.Expected, result.Success);
            AssertCode(MustCodes.Text.Charset.NotDigits, result);
        }
        else
        {
            var result = Must.Be.DigitsOnly(input, testCase.Value.AllowedChars);
            Assert.Equal(testCase.Expected, result.Success);
            AssertCode(MustCodes.Text.Charset.NotDigits, result);
        }
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.DigitsOnly.EdgeCases), MemberType = typeof(MustStringClausesTestData.DigitsOnly))]
    public void DigitsOnly_EdgeChecks(MustStringClausesTestData.DigitsOnly.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = testCase.Value.AllowedChars is null
            ? Must.Be.DigitsOnly(input)
            : Must.Be.DigitsOnly(input, testCase.Value.AllowedChars);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.DigitsOnly.ValidCases), MemberType = typeof(MustStringClausesTestData.DigitsOnly))]
    public void NotDigitsOnly_Checks(MustStringClausesTestData.DigitsOnly.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        if (testCase.Value.AllowedChars is null)
        {
            var result = Must.Be.NotDigitsOnly(input);
            Assert.NotEqual(testCase.Expected, result.Success);
            AssertCode(MustCodes.Text.Charset.Digits, result);
        }
        else
        {
            var result = Must.Be.NotDigitsOnly(input, testCase.Value.AllowedChars);
            Assert.NotEqual(testCase.Expected, result.Success);
            AssertCode(MustCodes.Text.Charset.Digits, result);
        }
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.DigitsOnly.EdgeCases), MemberType = typeof(MustStringClausesTestData.DigitsOnly))]
    public void NotDigitsOnly_EdgeChecks(MustStringClausesTestData.DigitsOnly.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = testCase.Value.AllowedChars is null
            ? Must.Be.NotDigitsOnly(input)
            : Must.Be.NotDigitsOnly(input, testCase.Value.AllowedChars);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Uppercase.ValidCases), MemberType = typeof(MustStringClausesTestData.Uppercase))]
    public void Uppercase_Checks(MustStringClausesTestData.Uppercase.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Uppercase(input, testCase.Value.LettersOnly);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Casing.NotUpper, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Uppercase.EdgeCases), MemberType = typeof(MustStringClausesTestData.Uppercase))]
    public void Uppercase_EdgeChecks(MustStringClausesTestData.Uppercase.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Uppercase(input, testCase.Value.LettersOnly);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Uppercase.ValidCases), MemberType = typeof(MustStringClausesTestData.Uppercase))]
    public void NotUppercase_Checks(MustStringClausesTestData.Uppercase.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotUppercase(input, testCase.Value.LettersOnly);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Casing.Upper, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Uppercase.EdgeCases), MemberType = typeof(MustStringClausesTestData.Uppercase))]
    public void NotUppercase_EdgeChecks(MustStringClausesTestData.Uppercase.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotUppercase(input, testCase.Value.LettersOnly);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Lowercase.ValidCases), MemberType = typeof(MustStringClausesTestData.Lowercase))]
    public void Lowercase_Checks(MustStringClausesTestData.Lowercase.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Lowercase(input, testCase.Value.LettersOnly);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Casing.NotLower, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Lowercase.EdgeCases), MemberType = typeof(MustStringClausesTestData.Lowercase))]
    public void Lowercase_EdgeChecks(MustStringClausesTestData.Lowercase.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.Lowercase(input, testCase.Value.LettersOnly);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Lowercase.ValidCases), MemberType = typeof(MustStringClausesTestData.Lowercase))]
    public void NotLowercase_Checks(MustStringClausesTestData.Lowercase.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotLowercase(input, testCase.Value.LettersOnly);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Casing.Lower, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Lowercase.EdgeCases), MemberType = typeof(MustStringClausesTestData.Lowercase))]
    public void NotLowercase_EdgeChecks(MustStringClausesTestData.Lowercase.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotLowercase(input, testCase.Value.LettersOnly);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Ascii.ValidCases), MemberType = typeof(MustStringClausesTestData.Ascii))]
    public void Ascii_Checks(MustStringClausesTestData.Ascii.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.Ascii(input);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotAscii, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Ascii.EdgeCases), MemberType = typeof(MustStringClausesTestData.Ascii))]
    public void Ascii_EdgeChecks(MustStringClausesTestData.Ascii.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.Ascii(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Ascii.NotCases), MemberType = typeof(MustStringClausesTestData.Ascii))]
    public void NotAscii_Checks(MustStringClausesTestData.Ascii.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotAscii(input);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.Ascii, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Ascii.EdgeCases), MemberType = typeof(MustStringClausesTestData.Ascii))]
    public void NotAscii_EdgeChecks(MustStringClausesTestData.Ascii.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotAscii(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.PrintableAscii.ValidCases), MemberType = typeof(MustStringClausesTestData.PrintableAscii))]
    public void PrintableAscii_Checks(MustStringClausesTestData.PrintableAscii.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.PrintableAscii(input);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotPrintable, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.PrintableAscii.EdgeCases), MemberType = typeof(MustStringClausesTestData.PrintableAscii))]
    public void PrintableAscii_EdgeChecks(MustStringClausesTestData.PrintableAscii.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.PrintableAscii(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.PrintableAscii.ValidCases), MemberType = typeof(MustStringClausesTestData.PrintableAscii))]
    public void NotPrintableAscii_Checks(MustStringClausesTestData.PrintableAscii.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotPrintableAscii(input);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.Printable, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.PrintableAscii.EdgeCases), MemberType = typeof(MustStringClausesTestData.PrintableAscii))]
    public void NotPrintableAscii_EdgeChecks(MustStringClausesTestData.PrintableAscii.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotPrintableAscii(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Whitespace.NotWhitespaceCases), MemberType = typeof(MustStringClausesTestData.Whitespace))]
    public void NotWhitespace_Checks(MustStringClausesTestData.Whitespace.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotWhitespace(input);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Content.Whitespace, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Whitespace.EdgeCases), MemberType = typeof(MustStringClausesTestData.Whitespace))]
    public void NotWhitespace_EdgeChecks(MustStringClausesTestData.Whitespace.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotWhitespace(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Whitespace.ContainsWhitespaceCases), MemberType = typeof(MustStringClausesTestData.Whitespace))]
    public void ContainsWhitespace_Checks(MustStringClausesTestData.Whitespace.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.ContainsWhitespace(input);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotContainsWhitespace, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Whitespace.EdgeCases), MemberType = typeof(MustStringClausesTestData.Whitespace))]
    public void ContainsWhitespace_EdgeChecks(MustStringClausesTestData.Whitespace.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.ContainsWhitespace(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Whitespace.ContainsWhitespaceCases), MemberType = typeof(MustStringClausesTestData.Whitespace))]
    public void NotContainsWhitespace_Checks(MustStringClausesTestData.Whitespace.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotContainsWhitespace(input);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.ContainsWhitespace, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.Whitespace.EdgeCases), MemberType = typeof(MustStringClausesTestData.Whitespace))]
    public void NotContainsWhitespace_EdgeChecks(MustStringClausesTestData.Whitespace.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotContainsWhitespace(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ControlChars.ContainsCases), MemberType = typeof(MustStringClausesTestData.ControlChars))]
    public void ContainsControlChars_Checks(MustStringClausesTestData.ControlChars.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.ContainsControlChars(input);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotContainsControl, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ControlChars.EdgeCases), MemberType = typeof(MustStringClausesTestData.ControlChars))]
    public void ContainsControlChars_EdgeChecks(MustStringClausesTestData.ControlChars.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.ContainsControlChars(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ControlChars.NotContainsCases), MemberType = typeof(MustStringClausesTestData.ControlChars))]
    public void NotContainsControlChars_Checks(MustStringClausesTestData.ControlChars.ValidCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotContainsControlChars(input);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.ContainsControl, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ControlChars.EdgeCases), MemberType = typeof(MustStringClausesTestData.ControlChars))]
    public void NotContainsControlChars_EdgeChecks(MustStringClausesTestData.ControlChars.EdgeCase testCase)
    {
        var input = testCase.Value;

        var result = Must.Be.NotContainsControlChars(input);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.ContainsAllowedOnlyCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void ContainsAllowedOnly_Checks(MustStringClausesTestData.AllowedDisallowed.AllowedCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ContainsAllowedOnly(input, testCase.Value.Allowed);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotSubset, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.ContainsAllowedOnlyEdgeCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void ContainsAllowedOnly_EdgeChecks(MustStringClausesTestData.AllowedDisallowed.AllowedEdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ContainsAllowedOnly(input, testCase.Value.Allowed);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.ContainsAllowedOnlyCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void NotContainsAllowedOnly_Checks(MustStringClausesTestData.AllowedDisallowed.AllowedCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotContainsAllowedOnly(input, testCase.Value.Allowed);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.Subset, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.ContainsAllowedOnlyEdgeCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void NotContainsAllowedOnly_EdgeChecks(MustStringClausesTestData.AllowedDisallowed.AllowedEdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotContainsAllowedOnly(input, testCase.Value.Allowed);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.ContainsDisallowedCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void ContainsDisallowed_Checks(MustStringClausesTestData.AllowedDisallowed.AllowedCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ContainsDisallowed(input, testCase.Value.Allowed);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotContainsDisallowed, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.EdgeCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void ContainsDisallowed_EdgeChecks(MustStringClausesTestData.AllowedDisallowed.AllowedEdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ContainsDisallowed(input, testCase.Value.Allowed);
        Assert.Equal(testCase.Expected, result.Success);
        if (testCase.ParamName is not null)
            Assert.Equal(testCase.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.ContainsDisallowedCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void NotContainsDisallowed_Checks(MustStringClausesTestData.AllowedDisallowed.AllowedCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotContainsDisallowed(input, testCase.Value.Allowed);
        Assert.NotEqual(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.ContainsDisallowed, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.AllowedDisallowed.EdgeCases), MemberType = typeof(MustStringClausesTestData.AllowedDisallowed))]
    public void NotContainsDisallowed_EdgeChecks(MustStringClausesTestData.AllowedDisallowed.AllowedEdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.NotContainsDisallowed(input, testCase.Value.Allowed);
        Assert.Equal(testCase.Expected, result.Success);
        if (testCase.ParamName is not null)
            Assert.Equal(testCase.ParamName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.LongerThanCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void LongerThan_Checks(MustStringClausesTestData.LongerShorter.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.LongerThan(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Length.TooShort, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.EdgeCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void LongerThan_EdgeChecks(MustStringClausesTestData.LongerShorter.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.LongerThan(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.LongerThanOrEqualCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void LongerThanOrEqual_Checks(MustStringClausesTestData.LongerShorter.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.LongerThanOrEqual(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Length.TooShort, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.EdgeCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void LongerThanOrEqual_EdgeChecks(MustStringClausesTestData.LongerShorter.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.LongerThanOrEqual(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.ShorterThanCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void ShorterThan_Checks(MustStringClausesTestData.LongerShorter.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ShorterThan(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Length.TooLong, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.EdgeCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void ShorterThan_EdgeChecks(MustStringClausesTestData.LongerShorter.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ShorterThan(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.ShorterThanOrEqualCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void ShorterThanOrEqual_Checks(MustStringClausesTestData.LongerShorter.ValidCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ShorterThanOrEqual(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Length.TooLong, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.LongerShorter.EdgeCases), MemberType = typeof(MustStringClausesTestData.LongerShorter))]
    public void ShorterThanOrEqual_EdgeChecks(MustStringClausesTestData.LongerShorter.EdgeCase testCase)
    {
        var input = testCase.Value.Value;

        var result = Must.Be.ShorterThanOrEqual(input, testCase.Value.Length);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ExpectedMessage, result.Message);
    }


    // ContainsAny
    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ContainsAny.ValidCases), MemberType = typeof(MustStringClausesTestData.ContainsAny))]
    public void ContainsAny_Checks(MustStringClausesTestData.ContainsAny.AllowedCase testCase)
    {
        var input = testCase.Value.Value;
        var result = Must.Be.ContainsAny(input, testCase.Value.Chars);
        Assert.Equal(testCase.Expected, result.Success);
        AssertCode(MustCodes.Text.Charset.NotContainsAny, result);
    }

    [Theory]
    [MemberData(nameof(MustStringClausesTestData.ContainsAny.EdgeCases), MemberType = typeof(MustStringClausesTestData.ContainsAny))]
    public void ContainsAny_EdgeChecks(MustStringClausesTestData.ContainsAny.AllowedEdgeCase testCase)
    {
        var input = testCase.Value.Value;
        var result = Must.Be.ContainsAny(input, testCase.Value.Chars);
        Assert.Equal(testCase.Expected, result.Success);
        Assert.Equal(testCase.ParamName, result.ParamName);
    }

    private static void AssertCode(string expectedCode, MustResult<string> result)
    {
        if (result.Failed)
            Assert.Equal(expectedCode, result.Code);
    }
}
