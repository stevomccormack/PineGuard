using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class CharRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CharRulesTestData.Constants.Cases), MemberType = typeof(CharRulesTestData.Constants))]
    public void Constants_AreExpected(CharRulesTestData.Constants.ValidCase testCase)
    {
        // Act & Assert
        Assert.Equal(testCase.Expected, testCase.Actual);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsLetter.Cases), MemberType = typeof(CharRulesTestData.IsLetter))]
    public void IsLetter_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsLetter(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsDigit.Cases), MemberType = typeof(CharRulesTestData.IsDigit))]
    public void IsDigit_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsDigit(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsLetterOrDigit.Cases), MemberType = typeof(CharRulesTestData.IsLetterOrDigit))]
    public void IsLetterOrDigit_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsLetterOrDigit(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsAscii.Cases), MemberType = typeof(CharRulesTestData.IsAscii))]
    public void IsAscii_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsAscii(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsPrintableAscii.Cases), MemberType = typeof(CharRulesTestData.IsPrintableAscii))]
    public void IsPrintableAscii_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsPrintableAscii(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsWhitespace.Cases), MemberType = typeof(CharRulesTestData.IsWhitespace))]
    public void IsWhitespace_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsWhitespace(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsControl.Cases), MemberType = typeof(CharRulesTestData.IsControl))]
    public void IsControl_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsControl(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsUppercase.Cases), MemberType = typeof(CharRulesTestData.IsUppercase))]
    public void IsUppercase_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsUppercase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsLowercase.Cases), MemberType = typeof(CharRulesTestData.IsLowercase))]
    public void IsLowercase_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsLowercase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CharRulesTestData.IsHexDigit.Cases), MemberType = typeof(CharRulesTestData.IsHexDigit))]
    public void IsHexDigit_BehavesAsExpected(RuleCase<char?> tc)
    {
        // Act
        var result = CharRules.IsHexDigit(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
