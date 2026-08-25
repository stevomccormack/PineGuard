using PineGuard.Rules;
using PineGuard.Rules.Owasp;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.Owasp;

public sealed class OwaspRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.HtmlTagRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void HtmlTagRegex_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = OwaspRegex.Xss.HtmlTagRegex().IsMatch(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.HtmlEntityEncodedAngleBracketRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void HtmlEntityEncodedAngleBracketRegex_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = OwaspRegex.Xss.HtmlEntityEncodedAngleBracketRegex().IsMatch(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.PercentEncodedAngleBracketRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void PercentEncodedAngleBracketRegex_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = OwaspRegex.Xss.PercentEncodedAngleBracketRegex().IsMatch(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.ScriptProtocolRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void ScriptProtocolRegex_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = OwaspRegex.Xss.ScriptProtocolRegex().IsMatch(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.HtmlEventHandlerAttributeRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void HtmlEventHandlerAttributeRegex_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = OwaspRegex.Xss.HtmlEventHandlerAttributeRegex().IsMatch(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsOwaspSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsOwaspSafe))]
    public void IsOwaspSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsOwaspSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsXssSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsXssSafe))]
    public void IsXssSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsXssSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsSqlInjectionSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsSqlInjectionSafe))]
    public void IsSqlInjectionSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsSqlInjectionSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsPathTraversalSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsPathTraversalSafe))]
    public void IsPathTraversalSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsPathTraversalSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsCommandInjectionSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsCommandInjectionSafe))]
    public void IsCommandInjectionSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsCommandInjectionSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsCrLfSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsCrLfSafe))]
    public void IsCrLfSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsCrLfSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsLdapFilterSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsLdapFilterSafe))]
    public void IsLdapFilterSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsLdapFilterSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsOpenRedirectSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsOpenRedirectSafe))]
    public void IsOpenRedirectSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsOpenRedirectSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsSsrfSchemeSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsSsrfSchemeSafe))]
    public void IsSsrfSchemeSafe_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = OwaspRules.IsSsrfSchemeSafe(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
