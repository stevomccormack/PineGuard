using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesTestData.IsExactLength.Cases), MemberType = typeof(StringRulesTestData.IsExactLength))]
    public void IsExactLength_BehavesAsExpected(RuleCase<(string? value, int length)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsExactLength(tc.Value.value, tc.Value.length);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsLengthBetween.Cases), MemberType = typeof(StringRulesTestData.IsLengthBetween))]
    public void IsLengthBetween_BehavesAsExpected(RuleCase<(string? value, int min, int max)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsLengthBetween(tc.Value.value, tc.Value.min, tc.Value.max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsLongerThan.Cases), MemberType = typeof(StringRulesTestData.IsLongerThan))]
    public void IsLongerThan_BehavesAsExpected(RuleCase<(string? value, int length, PineGuard.Common.Inclusion inclusion)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsLongerThan(tc.Value.value, tc.Value.length, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsLongerThanDefaultInclusion.Cases), MemberType = typeof(StringRulesTestData.IsLongerThanDefaultInclusion))]
    public void IsLongerThan_DefaultInclusion_IsExclusive(RuleCase<(string? value, int length)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsLongerThan(tc.Value.value, tc.Value.length);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsShorterThan.Cases), MemberType = typeof(StringRulesTestData.IsShorterThan))]
    public void IsShorterThan_BehavesAsExpected(RuleCase<(string? value, int length, PineGuard.Common.Inclusion inclusion)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsShorterThan(tc.Value.value, tc.Value.length, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsShorterThanDefaultInclusion.Cases), MemberType = typeof(StringRulesTestData.IsShorterThanDefaultInclusion))]
    public void IsShorterThan_DefaultInclusion_IsExclusive(RuleCase<(string? value, int length)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsShorterThan(tc.Value.value, tc.Value.length);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsDigitsOnly.Cases), MemberType = typeof(StringRulesTestData.IsDigitsOnly))]
    public void IsDigitsOnly_Default_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsDigitsOnly(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsDigitsOnlyWithAllowedNonDigitChars.Cases), MemberType = typeof(StringRulesTestData.IsDigitsOnlyWithAllowedNonDigitChars))]
    public void IsDigitsOnly_WithAllowedNonDigitChars_BehavesAsExpected(RuleCase<(string value, char[] allowedNonDigitChars)> tc)
    {
        // Arrange
        var (value, allowedNonDigitChars) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.IsDigitsOnly(value, allowedNonDigitChars: allowedNonDigitChars);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsDigitsOnlyWithNullAllowedNonDigitChars.Cases), MemberType = typeof(StringRulesTestData.IsDigitsOnlyWithNullAllowedNonDigitChars))]
    public void IsDigitsOnly_WithNullAllowedNonDigitChars_MeansDigitsOnly(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsDigitsOnly(tc.Value, allowedNonDigitChars: null);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsUppercase.Cases), MemberType = typeof(StringRulesTestData.IsUppercase))]
    public void IsUppercase_BehavesAsExpected(RuleCase<(string? value, bool lettersOnly)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsUppercase(tc.Value.value, tc.Value.lettersOnly);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsLowercase.Cases), MemberType = typeof(StringRulesTestData.IsLowercase))]
    public void IsLowercase_BehavesAsExpected(RuleCase<(string? value, bool lettersOnly)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsLowercase(tc.Value.value, tc.Value.lettersOnly);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.RulesThatRequireTrim.Cases), MemberType = typeof(StringRulesTestData.RulesThatRequireTrim))]
    public void RulesThatRequireTrim_ReturnFalse_ForNullOrWhitespace(RuleCase<string?> tc)
    {
        // Act & Assert
        Assert.False(PineGuard.Rules.StringRules.IsUppercase(tc.Value));
        Assert.False(PineGuard.Rules.StringRules.IsLowercase(tc.Value));
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsAscii.Cases), MemberType = typeof(StringRulesTestData.IsAscii))]
    public void IsAscii_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsAscii(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsPrintableAscii.Cases), MemberType = typeof(StringRulesTestData.IsPrintableAscii))]
    public void IsPrintableAscii_BehavesAsExpected(RuleCase<(string? value, bool allowCommonWhitespace)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsPrintableAscii(tc.Value.value, tc.Value.allowCommonWhitespace);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsWhitespace.Cases), MemberType = typeof(StringRulesTestData.IsWhitespace))]
    public void IsWhitespace_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsWhitespace(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.ContainsWhitespace.Cases), MemberType = typeof(StringRulesTestData.ContainsWhitespace))]
    public void ContainsWhitespace_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.ContainsWhitespace(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.ContainsControlChars.Cases), MemberType = typeof(StringRulesTestData.ContainsControlChars))]
    public void ContainsControlChars_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.ContainsControlChars(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.NotContainsControlChars.Cases), MemberType = typeof(StringRulesTestData.NotContainsControlChars))]
    public void NotContainsControlChars_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.NotContainsControlChars(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsMatch.Cases), MemberType = typeof(StringRulesTestData.IsMatch))]
    public void IsMatch_BehavesAsExpected(RuleCase<(string? value, System.Text.RegularExpressions.Regex pattern)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsMatch(tc.Value.value, tc.Value.pattern);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsMatch.InvalidCases), MemberType = typeof(StringRulesTestData.IsMatch))]
    public void IsMatch_Throws_WhenPatternNull(StringRulesTestData.IsMatch.InvalidCase testCase)
    {
        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => PineGuard.Rules.StringRules.IsMatch(testCase.Input.Value, testCase.Input.Pattern));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsAlphabetic.Cases), MemberType = typeof(StringRulesTestData.IsAlphabetic))]
    public void IsAlphabetic_BehavesAsExpected(StringRulesTestData.IsAlphabetic.Case tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsAlphabetic(tc.Value);
        var resultWithDash = PineGuard.Rules.StringRules.IsAlphabetic(tc.Value, inclusions: ['-']);
        var resultWithUnderscore = PineGuard.Rules.StringRules.IsAlphabetic(tc.Value, inclusions: ['_']);

        // Assert
        Assert.Equal(tc.Expected, result);
        Assert.Equal(tc.ExpectedWithDashInclusions, resultWithDash);
        Assert.Equal(tc.ExpectedWithUnderscoreInclusions, resultWithUnderscore);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsAlphabetic.EmptyInclusions), MemberType = typeof(StringRulesTestData.IsAlphabetic))]
    public void IsAlphabetic_TreatsEmptyInclusionsAsNone_BehavesAsExpected(StringRulesTestData.IsAlphabetic.Case tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsAlphabetic(tc.Value, inclusions: []);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsNumeric.Cases), MemberType = typeof(StringRulesTestData.IsNumeric))]
    public void IsNumeric_BehavesAsExpected(StringRulesTestData.IsNumeric.Case tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsNumeric(tc.Value);
        var resultWithDash = PineGuard.Rules.StringRules.IsNumeric(tc.Value, inclusions: ['-']);
        var resultWithUnderscore = PineGuard.Rules.StringRules.IsNumeric(tc.Value, inclusions: ['_']);

        // Assert
        Assert.Equal(tc.Expected, result);
        Assert.Equal(tc.ExpectedWithDashInclusions, resultWithDash);
        Assert.Equal(tc.ExpectedWithUnderscoreInclusions, resultWithUnderscore);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsNumeric.EmptyInclusions), MemberType = typeof(StringRulesTestData.IsNumeric))]
    public void IsNumeric_TreatsEmptyInclusionsAsNone_BehavesAsExpected(StringRulesTestData.IsNumeric.Case tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsNumeric(tc.Value, inclusions: []);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsAlphanumeric.Cases), MemberType = typeof(StringRulesTestData.IsAlphanumeric))]
    public void IsAlphanumeric_BehavesAsExpected(StringRulesTestData.IsAlphanumeric.Case tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsAlphanumeric(tc.Value);
        var resultWithDash = PineGuard.Rules.StringRules.IsAlphanumeric(tc.Value, inclusions: ['-']);
        var resultWithUnderscore = PineGuard.Rules.StringRules.IsAlphanumeric(tc.Value, inclusions: ['_']);

        // Assert
        Assert.Equal(tc.Expected, result);
        Assert.Equal(tc.ExpectedWithDashInclusions, resultWithDash);
        Assert.Equal(tc.ExpectedWithUnderscoreInclusions, resultWithUnderscore);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.IsAlphanumeric.EmptyInclusions), MemberType = typeof(StringRulesTestData.IsAlphanumeric))]
    public void IsAlphanumeric_TreatsEmptyInclusionsAsNone_BehavesAsExpected(StringRulesTestData.IsAlphanumeric.Case tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsAlphanumeric(tc.Value, inclusions: []);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.ContainsAllowedOnly.Cases), MemberType = typeof(StringRulesTestData.ContainsAllowedOnly))]
    public void ContainsAllowedOnly_BehavesAsExpected(RuleCase<(string? value, char[] allowedChars)> tc)
    {
        // Arrange
        var (value, allowedChars) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.ContainsAllowedOnly(value, allowedChars);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.ContainsAllowedOnly.InvalidCases), MemberType = typeof(StringRulesTestData.ContainsAllowedOnly))]
    public void ContainsAllowedOnly_Throws_WhenAllowedCharsNull(StringRulesTestData.ContainsAllowedOnly.InvalidCase testCase)
    {
        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type,
            () => PineGuard.Rules.StringRules.ContainsAllowedOnly(testCase.Input.Value, testCase.Input.AllowedChars));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.ContainsDisallowed.Cases), MemberType = typeof(StringRulesTestData.ContainsDisallowed))]
    public void ContainsDisallowed_BehavesAsExpected(RuleCase<(string? value, char[] disallowedChars)> tc)
    {
        // Arrange
        var (value, disallowedChars) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.ContainsDisallowed(value, disallowedChars);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTestData.ContainsDisallowed.InvalidCases), MemberType = typeof(StringRulesTestData.ContainsDisallowed))]
    public void ContainsDisallowed_Throws_WhenDisallowedCharsNull(StringRulesTestData.ContainsDisallowed.InvalidCase testCase)
    {
        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type,
            () => PineGuard.Rules.StringRules.ContainsDisallowed(testCase.Input.Value, testCase.Input.DisallowedChars));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
