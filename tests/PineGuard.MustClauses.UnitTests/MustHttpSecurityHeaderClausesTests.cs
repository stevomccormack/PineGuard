using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustHttpSecurityHeaderClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyHeader))]
    public void ContentSecurityPolicyHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.ContentSecurityPolicyHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityHeader))]
    public void StrictTransportSecurityHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.StrictTransportSecurityHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsHeader))]
    public void XContentTypeOptionsHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.XContentTypeOptionsHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsHeader))]
    public void XFrameOptionsHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.XFrameOptionsHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyHeader))]
    public void ReferrerPolicyHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.ReferrerPolicyHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyHeader))]
    public void PermissionsPolicyHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.PermissionsPolicyHeader(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicyWithDefaults))]
    public void ContentSecurityPolicyWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.ContentSecurityPolicyWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurityWithDefaults))]
    public void StrictTransportSecurityWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.StrictTransportSecurityWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptionsWithDefaults))]
    public void XContentTypeOptionsWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.XContentTypeOptionsWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XFrameOptionsWithDefaults))]
    public void XFrameOptionsWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.XFrameOptionsWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicyWithDefaults))]
    public void ReferrerPolicyWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.ReferrerPolicyWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyWithDefaults))]
    public void PermissionsPolicyWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        // Act
        var result = Must.Be.PermissionsPolicyWithDefaults(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptions.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptions))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptions.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XContentTypeOptions))]
    public void XContentTypeOptions_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = Must.Be.XContentTypeOptions(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XFrameOptions.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XFrameOptions))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.XFrameOptions.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.XFrameOptions))]
    public void XFrameOptions_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = Must.Be.XFrameOptions(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicy.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicy))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicy.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ReferrerPolicy))]
    public void ReferrerPolicy_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = Must.Be.ReferrerPolicy(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicy.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicy))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicy.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicy))]
    public void PermissionsPolicy_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, expectedValue) = tc.Value;

        // Act
        var result = Must.Be.PermissionsPolicy(headers, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurity.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurity))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurity.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.StrictTransportSecurity))]
    public void StrictTransportSecurity_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)> tc)
    {
        // Arrange
        var (headers, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload) = tc.Value;

        // Act
        var result = Must.Be.StrictTransportSecurity(headers, minMaxAgeSeconds, requireIncludeSubDomains, requirePreload);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicy.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicy))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicy.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.ContentSecurityPolicy))]
    public void ContentSecurityPolicy_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)> tc)
    {
        // Arrange
        var (headers, requiredDefaultSrcValue, requiredObjectSrcValue, requiredBaseUriValue, requiredFrameAncestorsValue) = tc.Value;

        // Act
        var result = Must.Be.ContentSecurityPolicy(headers, requiredDefaultSrcValue, requiredObjectSrcValue, requiredBaseUriValue, requiredFrameAncestorsValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyContaining.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyContaining))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyContaining.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.PermissionsPolicyContaining))]
    public void PermissionsPolicyContaining_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)> tc)
    {
        // Arrange
        var (headers, requiredFragments) = tc.Value;

        // Act
        var result = requiredFragments is null
            ? Must.Be.PermissionsPolicyContaining(headers, null!)
            : Must.Be.PermissionsPolicyContaining(headers, [requiredFragments]);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyHeader))]
    public void NotContentSecurityPolicyHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotContentSecurityPolicyHeader(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotContentSecurityPolicyWithDefaults))]
    public void NotContentSecurityPolicyWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotContentSecurityPolicyWithDefaults(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityHeader))]
    public void NotStrictTransportSecurityHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotStrictTransportSecurityHeader(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotStrictTransportSecurityWithDefaults))]
    public void NotStrictTransportSecurityWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotStrictTransportSecurityWithDefaults(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsHeader))]
    public void NotXContentTypeOptionsHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotXContentTypeOptionsHeader(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXContentTypeOptionsWithDefaults))]
    public void NotXContentTypeOptionsWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotXContentTypeOptionsWithDefaults(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsHeader))]
    public void NotXFrameOptionsHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotXFrameOptionsHeader(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotXFrameOptionsWithDefaults))]
    public void NotXFrameOptionsWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotXFrameOptionsWithDefaults(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyHeader))]
    public void NotReferrerPolicyHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotReferrerPolicyHeader(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotReferrerPolicyWithDefaults))]
    public void NotReferrerPolicyWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotReferrerPolicyWithDefaults(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyHeader.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyHeader))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyHeader.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyHeader))]
    public void NotPermissionsPolicyHeader_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotPermissionsPolicyHeader(tc.Value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyWithDefaults.ValidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyWithDefaults))]
    [MemberData(nameof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyWithDefaults.InvalidCases), MemberType = typeof(MustHttpSecurityHeaderClausesTestData.NotPermissionsPolicyWithDefaults))]
    public void NotPermissionsPolicyWithDefaults_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = Must.Be.NotPermissionsPolicyWithDefaults(tc.Value);
        AssertResult(tc, result);
    }
}
