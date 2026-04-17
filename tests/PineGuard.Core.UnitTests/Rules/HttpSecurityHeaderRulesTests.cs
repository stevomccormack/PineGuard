using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class HttpSecurityHeaderRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasContentSecurityPolicyHeader.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasContentSecurityPolicyHeader))]
    public void HasContentSecurityPolicyHeader_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasContentSecurityPolicyHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasStrictTransportSecurityHeader.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasStrictTransportSecurityHeader))]
    public void HasStrictTransportSecurityHeader_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasStrictTransportSecurityHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasXFrameOptionsHeader.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasXFrameOptionsHeader))]
    public void HasXFrameOptionsHeader_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasXFrameOptionsHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasReferrerPolicyHeader.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasReferrerPolicyHeader))]
    public void HasReferrerPolicyHeader_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasReferrerPolicyHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicyHeader.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicyHeader))]
    public void HasPermissionsPolicyHeader_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasPermissionsPolicyHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasXContentTypeOptionsHeader.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasXContentTypeOptionsHeader))]
    public void HasXContentTypeOptionsHeader_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasXContentTypeOptionsHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasXContentTypeOptions.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasXContentTypeOptions))]
    public void HasXContentTypeOptions_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = HttpSecurityHeaderRules.HasXContentTypeOptions(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasXFrameOptions.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasXFrameOptions))]
    public void HasXFrameOptions_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = HttpSecurityHeaderRules.HasXFrameOptions(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasReferrerPolicy.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasReferrerPolicy))]
    public void HasReferrerPolicy_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = HttpSecurityHeaderRules.HasReferrerPolicy(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicy.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicy))]
    public void HasPermissionsPolicy_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = HttpSecurityHeaderRules.HasPermissionsPolicy(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasStrictTransportSecurity.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasStrictTransportSecurity))]
    public void HasStrictTransportSecurity_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)> tc)
    {
        // Arrange
        var (headers, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload) = tc.Value;

        // Act
        var result = HttpSecurityHeaderRules.HasStrictTransportSecurity(headers, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasStrictTransportSecurityWithDefaults.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasStrictTransportSecurityWithDefaults))]
    public void HasStrictTransportSecurityWithDefaults_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasStrictTransportSecurityWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasContentSecurityPolicy.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasContentSecurityPolicy))]
    public void HasContentSecurityPolicy_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)> tc)
    {
        // Arrange
        var (headers, requiredDefaultSrcValue, requiredObjectSrcValue, requiredBaseUriValue, requiredFrameAncestorsValue) = tc.Value;

        // Act
        var result = HttpSecurityHeaderRules.HasContentSecurityPolicy(headers, requiredDefaultSrcValue, requiredObjectSrcValue, requiredBaseUriValue, requiredFrameAncestorsValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasContentSecurityPolicyWithDefaults.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasContentSecurityPolicyWithDefaults))]
    public void HasContentSecurityPolicyWithDefaults_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasContentSecurityPolicyWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicyContaining.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicyContaining))]
    public void HasPermissionsPolicyContaining_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)> tc)
    {
        // Arrange
        var (headers, requiredFragments) = tc.Value;

        // Act
        var result = requiredFragments is null
            ? HttpSecurityHeaderRules.HasPermissionsPolicyContaining(headers, null!)
            : HttpSecurityHeaderRules.HasPermissionsPolicyContaining(headers, requiredFragments);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicyWithDefaults.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasPermissionsPolicyWithDefaults))]
    public void HasPermissionsPolicyWithDefaults_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasPermissionsPolicyWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasXContentTypeOptionsWithDefaults.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasXContentTypeOptionsWithDefaults))]
    public void HasXContentTypeOptionsWithDefaults_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasXContentTypeOptionsWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasXFrameOptionsWithDefaults.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasXFrameOptionsWithDefaults))]
    public void HasXFrameOptionsWithDefaults_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasXFrameOptionsWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderRulesTestData.HasReferrerPolicyWithDefaults.Cases), MemberType = typeof(HttpSecurityHeaderRulesTestData.HasReferrerPolicyWithDefaults))]
    public void HasReferrerPolicyWithDefaults_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = HttpSecurityHeaderRules.HasReferrerPolicyWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
