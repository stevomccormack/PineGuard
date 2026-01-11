using PineGuard.Rules.Owasp;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Owasp;

public sealed class OwaspRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.HtmlTagRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void HtmlTagRegex_ReturnsExpected(OwaspRulesTestData.OwaspRegexXss.Case testCase)
    {
        var result = OwaspRegex.Xss.HtmlTagRegex().IsMatch(testCase.Value!);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.HtmlEntityEncodedAngleBracketRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void HtmlEntityEncodedAngleBracketRegex_ReturnsExpected(OwaspRulesTestData.OwaspRegexXss.Case testCase)
    {
        var result = OwaspRegex.Xss.HtmlEntityEncodedAngleBracketRegex().IsMatch(testCase.Value!);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.ScriptProtocolRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void ScriptProtocolRegex_ReturnsExpected(OwaspRulesTestData.OwaspRegexXss.Case testCase)
    {
        var result = OwaspRegex.Xss.ScriptProtocolRegex().IsMatch(testCase.Value!);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.OwaspRegexXss.HtmlEventHandlerAttributeRegexCases), MemberType = typeof(OwaspRulesTestData.OwaspRegexXss))]
    public void HtmlEventHandlerAttributeRegex_ReturnsExpected(OwaspRulesTestData.OwaspRegexXss.Case testCase)
    {
        var result = OwaspRegex.Xss.HtmlEventHandlerAttributeRegex().IsMatch(testCase.Value!);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsOwaspSafe.ValidCases), MemberType = typeof(OwaspRulesTestData.IsOwaspSafe))]
    public void IsOwaspSafe_ReturnsTrue_ForSafeStrings(OwaspRulesTestData.IsOwaspSafe.Case testCase)
    {
        var result = OwaspRules.IsOwaspSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsOwaspDangerous.Cases), MemberType = typeof(OwaspRulesTestData.IsOwaspDangerous))]
    public void IsOwaspDangerous_ReturnsTrue_ForDangerousStrings(OwaspRulesTestData.IsOwaspDangerous.Case testCase)
    {
        var result = OwaspRules.IsOwaspDangerous(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsXssSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsXssSafe))]
    public void IsXssSafe_ReturnsExpected(OwaspRulesTestData.IsXssSafe.Case testCase)
    {
        var result = OwaspRules.IsXssSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsSqlInjectionSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsSqlInjectionSafe))]
    public void IsSqlInjectionSafe_ReturnsExpected(OwaspRulesTestData.IsSqlInjectionSafe.Case testCase)
    {
        var result = OwaspRules.IsSqlInjectionSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsPathTraversalSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsPathTraversalSafe))]
    public void IsPathTraversalSafe_ReturnsExpected(OwaspRulesTestData.IsPathTraversalSafe.Case testCase)
    {
        var result = OwaspRules.IsPathTraversalSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsCommandInjectionSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsCommandInjectionSafe))]
    public void IsCommandInjectionSafe_ReturnsExpected(OwaspRulesTestData.IsCommandInjectionSafe.Case testCase)
    {
        var result = OwaspRules.IsCommandInjectionSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsCrLfSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsCrLfSafe))]
    public void IsCrLfSafe_ReturnsExpected(OwaspRulesTestData.IsCrLfSafe.Case testCase)
    {
        var result = OwaspRules.IsCrLfSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsLdapFilterSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsLdapFilterSafe))]
    public void IsLdapFilterSafe_ReturnsExpected(OwaspRulesTestData.IsLdapFilterSafe.Case testCase)
    {
        var result = OwaspRules.IsLdapFilterSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsOpenRedirectSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsOpenRedirectSafe))]
    public void IsOpenRedirectSafe_ReturnsExpected(OwaspRulesTestData.IsOpenRedirectSafe.Case testCase)
    {
        var result = OwaspRules.IsOpenRedirectSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspRulesTestData.IsSsrfSchemeSafe.Cases), MemberType = typeof(OwaspRulesTestData.IsSsrfSchemeSafe))]
    public void IsSsrfSchemeSafe_ReturnsExpected(OwaspRulesTestData.IsSsrfSchemeSafe.Case testCase)
    {
        var result = OwaspRules.IsSsrfSchemeSafe(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
