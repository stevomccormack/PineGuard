using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using System.Text.RegularExpressions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesCoreTests : BaseUnitTest
{
    [Fact]
    public void LengthRules_ReturnExpected()
    {
        Assert.True(PineGuard.Rules.StringRules.IsExactLength("abc", 3));
        Assert.False(PineGuard.Rules.StringRules.IsExactLength("abc", 2));
        Assert.False(PineGuard.Rules.StringRules.IsExactLength(null, 1));

        Assert.True(PineGuard.Rules.StringRules.IsLengthBetween("abc", 2, 3));
        Assert.False(PineGuard.Rules.StringRules.IsLengthBetween("abc", 4, 5));
        Assert.False(PineGuard.Rules.StringRules.IsLengthBetween(null, 1, 2));

        Assert.True(PineGuard.Rules.StringRules.IsLongerThan("abc", 2));
        Assert.False(PineGuard.Rules.StringRules.IsLongerThan("abc", 3, Inclusion.Exclusive));
        Assert.False(PineGuard.Rules.StringRules.IsLongerThan(null, 0));

        Assert.True(PineGuard.Rules.StringRules.IsShorterThan("abc", 4));
        Assert.False(PineGuard.Rules.StringRules.IsShorterThan("abc", 3, Inclusion.Exclusive));
        Assert.False(PineGuard.Rules.StringRules.IsShorterThan(null, 10));
    }

    [Fact]
    public void IsMatch_Throws_WhenPatternNull()
    {
        _ = Assert.Throws<ArgumentNullException>(() => PineGuard.Rules.StringRules.IsMatch("abc", pattern: null!));
    }

    [Fact]
    public void IsMatch_ReturnsFalse_WhenValueNull()
    {
        var regex = new Regex("^abc$", RegexOptions.CultureInvariant);

        Assert.False(PineGuard.Rules.StringRules.IsMatch(null, regex));
        Assert.True(PineGuard.Rules.StringRules.IsMatch("abc", regex));
        Assert.False(PineGuard.Rules.StringRules.IsMatch("abcd", regex));
    }

    [Theory]
    [MemberData(nameof(StringRulesCoreTestData.IsAlphabetic.Cases), MemberType = typeof(StringRulesCoreTestData.IsAlphabetic))]
    public void IsAlphabetic_ReturnsExpected(StringRulesCoreTestData.IsAlphabetic.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.IsAlphabetic(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);

        var resultWithDashInclusions = PineGuard.Rules.StringRules.IsAlphabetic(testCase.Value, inclusions: ['-']);
        Assert.Equal(testCase.ExpectedWithDashInclusions, resultWithDashInclusions);

        var resultWithUnderscoreInclusions = PineGuard.Rules.StringRules.IsAlphabetic(testCase.Value, inclusions: ['_']);
        Assert.Equal(testCase.ExpectedWithUnderscoreInclusions, resultWithUnderscoreInclusions);
    }

    [Theory]
    [MemberData(nameof(StringRulesCoreTestData.IsNumeric.Cases), MemberType = typeof(StringRulesCoreTestData.IsNumeric))]
    public void IsNumeric_ReturnsExpected(StringRulesCoreTestData.IsNumeric.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.IsNumeric(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);

        var resultWithDashInclusions = PineGuard.Rules.StringRules.IsNumeric(testCase.Value, inclusions: ['-']);
        Assert.Equal(testCase.ExpectedWithDashInclusions, resultWithDashInclusions);

        var resultWithUnderscoreInclusions = PineGuard.Rules.StringRules.IsNumeric(testCase.Value, inclusions: ['_']);
        Assert.Equal(testCase.ExpectedWithUnderscoreInclusions, resultWithUnderscoreInclusions);
    }

    [Theory]
    [MemberData(nameof(StringRulesCoreTestData.IsAlphanumeric.Cases), MemberType = typeof(StringRulesCoreTestData.IsAlphanumeric))]
    public void IsAlphanumeric_ReturnsExpected(StringRulesCoreTestData.IsAlphanumeric.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.IsAlphanumeric(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);

        var resultWithDashInclusions = PineGuard.Rules.StringRules.IsAlphanumeric(testCase.Value, inclusions: ['-']);
        Assert.Equal(testCase.ExpectedWithDashInclusions, resultWithDashInclusions);

        var resultWithUnderscoreInclusions = PineGuard.Rules.StringRules.IsAlphanumeric(testCase.Value, inclusions: ['_']);
        Assert.Equal(testCase.ExpectedWithUnderscoreInclusions, resultWithUnderscoreInclusions);
    }

    [Theory]
    [MemberData(nameof(StringRulesCoreTestData.IsDigitsOnly.Cases), MemberType = typeof(StringRulesCoreTestData.IsDigitsOnly))]
    public void IsDigitsOnly_Default_ReturnsExpected(StringRulesCoreTestData.IsDigitsOnly.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.IsDigitsOnly(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCoreTestData.IsDigitsOnlyWithAllowedNonDigitChars.Cases), MemberType = typeof(StringRulesCoreTestData.IsDigitsOnlyWithAllowedNonDigitChars))]
    public void IsDigitsOnly_WithAllowedNonDigitChars_ReturnsExpected(StringRulesCoreTestData.IsDigitsOnlyWithAllowedNonDigitChars.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.IsDigitsOnly(testCase.Value, allowedNonDigitChars: ['-']);
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Fact]
    public void CaseRules_HandleLettersOnlyAndRequireAtLeastOneLetter()
    {
        Assert.True(PineGuard.Rules.StringRules.IsUppercase("ABC123"));
        Assert.False(PineGuard.Rules.StringRules.IsUppercase("123"));
        Assert.False(PineGuard.Rules.StringRules.IsUppercase("AbC"));
        Assert.True(PineGuard.Rules.StringRules.IsUppercase("ABC", lettersOnly: true));
        Assert.False(PineGuard.Rules.StringRules.IsUppercase("ABC123", lettersOnly: true));

        Assert.True(PineGuard.Rules.StringRules.IsLowercase("abc123"));
        Assert.False(PineGuard.Rules.StringRules.IsLowercase("123"));
        Assert.False(PineGuard.Rules.StringRules.IsLowercase("aBc"));
        Assert.True(PineGuard.Rules.StringRules.IsLowercase("abc", lettersOnly: true));
        Assert.False(PineGuard.Rules.StringRules.IsLowercase("abc123", lettersOnly: true));
    }

    [Theory]
    [MemberData(nameof(StringRulesCoreTestData.RulesThatRequireTrim.Cases), MemberType = typeof(StringRulesCoreTestData.RulesThatRequireTrim))]
    public void RulesThatRequireTrim_ReturnFalse_ForNullOrWhitespace(StringRulesCoreTestData.RulesThatRequireTrim.Case testCase)
    {
        Assert.False(PineGuard.Rules.StringRules.IsUppercase(testCase.Value));
        Assert.False(PineGuard.Rules.StringRules.IsLowercase(testCase.Value));
        Assert.False(PineGuard.Rules.StringRules.IsAscii(testCase.Value));
        Assert.False(PineGuard.Rules.StringRules.IsPrintableAscii(testCase.Value));
        Assert.False(PineGuard.Rules.StringRules.DoesNotContainWhitespace(testCase.Value));
        Assert.False(PineGuard.Rules.StringRules.ContainsNoControlChars(testCase.Value));

        Assert.False(PineGuard.Rules.StringRules.ContainsOnlyAllowedChars(testCase.Value, allowedChars: ['a']));
        Assert.False(PineGuard.Rules.StringRules.ContainsAnyDisallowedChars(testCase.Value, allowedChars: ['a']));
    }

    [Fact]
    public void AsciiAndWhitespaceRules_ReturnExpected()
    {
        Assert.True(PineGuard.Rules.StringRules.IsAscii(" abc "));
        Assert.False(PineGuard.Rules.StringRules.IsAscii("café"));

        Assert.True(PineGuard.Rules.StringRules.IsPrintableAscii("Hello"));
        Assert.False(PineGuard.Rules.StringRules.IsPrintableAscii("A\tB"));
        Assert.True(PineGuard.Rules.StringRules.IsPrintableAscii("A\tB", allowCommonWhitespace: true));

        Assert.True(PineGuard.Rules.StringRules.DoesNotContainWhitespace("abc"));
        Assert.False(PineGuard.Rules.StringRules.DoesNotContainWhitespace("a b"));

        Assert.True(PineGuard.Rules.StringRules.ContainsNoControlChars("abc"));
        Assert.False(PineGuard.Rules.StringRules.ContainsNoControlChars("a\nb"));
    }

    [Fact]
    public void IsPrintableAscii_RespectsAllowCommonWhitespace_ForCrLfTab()
    {
        Assert.False(PineGuard.Rules.StringRules.IsPrintableAscii("A\tB"));
        Assert.False(PineGuard.Rules.StringRules.IsPrintableAscii("A\rB"));
        Assert.False(PineGuard.Rules.StringRules.IsPrintableAscii("A\nB"));

        Assert.True(PineGuard.Rules.StringRules.IsPrintableAscii("A\tB", allowCommonWhitespace: true));
        Assert.True(PineGuard.Rules.StringRules.IsPrintableAscii("A\rB", allowCommonWhitespace: true));
        Assert.True(PineGuard.Rules.StringRules.IsPrintableAscii("A\nB", allowCommonWhitespace: true));
    }

    [Fact]
    public void IsPrintableAscii_ReturnsFalse_ForOtherControlWhitespace_EvenWhenAllowedCommonWhitespace()
    {
        Assert.False(PineGuard.Rules.StringRules.IsPrintableAscii("A\vB", allowCommonWhitespace: true));
    }

    [Fact]
    public void AllowedCharsRules_ThrowOnNullAllowedChars()
    {
        _ = Assert.Throws<ArgumentNullException>(() => PineGuard.Rules.StringRules.ContainsOnlyAllowedChars("abc", allowedChars: null!));
        _ = Assert.Throws<ArgumentNullException>(() => PineGuard.Rules.StringRules.ContainsAnyDisallowedChars("abc", allowedChars: null!));
    }

    [Fact]
    public void AllowedCharsRules_ReturnExpected()
    {
        Assert.True(PineGuard.Rules.StringRules.ContainsOnlyAllowedChars("aba", allowedChars: ['a', 'b']));
        Assert.False(PineGuard.Rules.StringRules.ContainsOnlyAllowedChars("abc", allowedChars: ['a', 'b']));

        Assert.False(PineGuard.Rules.StringRules.ContainsAnyDisallowedChars("aba", allowedChars: ['a', 'b']));
        Assert.True(PineGuard.Rules.StringRules.ContainsAnyDisallowedChars("abc", allowedChars: ['a', 'b']));
    }
}
