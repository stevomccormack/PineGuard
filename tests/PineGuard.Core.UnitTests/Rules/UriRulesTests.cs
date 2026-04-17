using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class UriRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(UriRulesTestData.IsAbsoluteUri.Cases), MemberType = typeof(UriRulesTestData.IsAbsoluteUri))]
    public void IsAbsoluteUri_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = UriRules.IsAbsoluteUri(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsRelativeUri.Cases), MemberType = typeof(UriRulesTestData.IsRelativeUri))]
    public void IsRelativeUri_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = UriRules.IsRelativeUri(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsUrl.Cases), MemberType = typeof(UriRulesTestData.IsUrl))]
    public void IsUrl_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = UriRules.IsUrl(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsHttpsUrl.Cases), MemberType = typeof(UriRulesTestData.IsHttpsUrl))]
    public void IsHttpsUrl_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = UriRules.IsHttpsUrl(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsHttpUrl.Cases), MemberType = typeof(UriRulesTestData.IsHttpUrl))]
    public void IsHttpUrl_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = UriRules.IsHttpUrl(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsFileUri.Cases), MemberType = typeof(UriRulesTestData.IsFileUri))]
    public void IsFileUri_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = UriRules.IsFileUri(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsFilePath.Cases), MemberType = typeof(UriRulesTestData.IsFilePath))]
    public void IsFilePath_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = UriRules.IsFilePath(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.HasScheme.Cases), MemberType = typeof(UriRulesTestData.HasScheme))]
    public void HasScheme_BehavesAsExpected(RuleCase<(string? value, string scheme)> tc)
    {
        // Act
        var result = UriRules.HasScheme(tc.Value.value, tc.Value.scheme);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.HasScheme.InvalidCases), MemberType = typeof(UriRulesTestData.HasScheme))]
    public void HasScheme_Throws_WhenSchemeIsNull(UriRulesTestData.HasScheme.InvalidCase tc)
    {
        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => UriRules.HasScheme(tc.Input.Value, tc.Input.Scheme));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
