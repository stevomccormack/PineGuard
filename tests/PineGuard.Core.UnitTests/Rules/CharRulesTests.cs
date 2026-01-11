using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class CharRulesTests : BaseUnitTest
{
    [Fact]
    public void Constants_AreExpected()
    {
        // Act

        // Assert
        Assert.Equal((char)0x00, CharRules.AsciiMinValue);
        Assert.Equal((char)0x7F, CharRules.AsciiMaxValue);
        Assert.Equal((char)0x20, CharRules.PrintableAsciiMinValue);
        Assert.Equal((char)0x7E, CharRules.PrintableAsciiMaxValue);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.ClassificationMethods.ValidCases), MemberType = typeof(CharRulesTestData.ClassificationMethods))]
    [MemberData(nameof(CharRulesTestData.ClassificationMethods.EdgeCases), MemberType = typeof(CharRulesTestData.ClassificationMethods))]
    public void ClassificationMethods_ReturnExpected(
        CharRulesTestData.ClassificationMethods.Case testCase)
    {
        // Act

        // Assert
        Assert.Equal(testCase.ExpectedIsLetter, CharRules.IsLetter(testCase.Value));
        Assert.Equal(testCase.ExpectedIsDigit, CharRules.IsDigit(testCase.Value));
        Assert.Equal(testCase.ExpectedIsLetterOrDigit, CharRules.IsLetterOrDigit(testCase.Value));
        Assert.Equal(testCase.ExpectedIsAscii, CharRules.IsAscii(testCase.Value));
        Assert.Equal(testCase.ExpectedIsPrintableAscii, CharRules.IsPrintableAscii(testCase.Value));
        Assert.Equal(testCase.ExpectedIsWhitespace, CharRules.IsWhitespace(testCase.Value));
        Assert.Equal(testCase.ExpectedIsControl, CharRules.IsControl(testCase.Value));
        Assert.Equal(testCase.ExpectedIsUppercase, CharRules.IsUppercase(testCase.Value));
        Assert.Equal(testCase.ExpectedIsLowercase, CharRules.IsLowercase(testCase.Value));
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsHexDigit.ValidCases), MemberType = typeof(CharRulesTestData.IsHexDigit))]
    [MemberData(nameof(CharRulesTestData.IsHexDigit.EdgeCases), MemberType = typeof(CharRulesTestData.IsHexDigit))]
    public void IsHexDigit_ReturnsExpected(CharRulesTestData.IsHexDigit.Case testCase)
    {
        // Act
        var result = CharRules.IsHexDigit(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
