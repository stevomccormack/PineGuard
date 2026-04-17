using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardHttpSecurityHeaderClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardHttpSecurityHeaderClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.NotContentSecurityPolicyHeader.ValidCases), MemberType = typeof(TD.NotContentSecurityPolicyHeader))]
    [MemberData(nameof(TD.NotContentSecurityPolicyHeader.InvalidCases), MemberType = typeof(TD.NotContentSecurityPolicyHeader))]
    public void NotContentSecurityPolicyHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotContentSecurityPolicyHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContentSecurityPolicyWithDefaults.ValidCases), MemberType = typeof(TD.NotContentSecurityPolicyWithDefaults))]
    [MemberData(nameof(TD.NotContentSecurityPolicyWithDefaults.InvalidCases), MemberType = typeof(TD.NotContentSecurityPolicyWithDefaults))]
    public void NotContentSecurityPolicyWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotContentSecurityPolicyWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContentSecurityPolicy.ValidCases), MemberType = typeof(TD.NotContentSecurityPolicy))]
    [MemberData(nameof(TD.NotContentSecurityPolicy.InvalidCases), MemberType = typeof(TD.NotContentSecurityPolicy))]
    public void NotContentSecurityPolicy_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)> tc)
    {
        var headers = tc.Value.headers;
        var result = AssertResult(tc, () => Guard.Against.NotContentSecurityPolicy(headers, tc.Value.requiredDefaultSrcValue, tc.Value.requiredObjectSrcValue, tc.Value.requiredBaseUriValue, tc.Value.requiredFrameAncestorsValue));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotStrictTransportSecurityHeader.ValidCases), MemberType = typeof(TD.NotStrictTransportSecurityHeader))]
    [MemberData(nameof(TD.NotStrictTransportSecurityHeader.InvalidCases), MemberType = typeof(TD.NotStrictTransportSecurityHeader))]
    public void NotStrictTransportSecurityHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotStrictTransportSecurityHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotStrictTransportSecurityWithDefaults.ValidCases), MemberType = typeof(TD.NotStrictTransportSecurityWithDefaults))]
    [MemberData(nameof(TD.NotStrictTransportSecurityWithDefaults.InvalidCases), MemberType = typeof(TD.NotStrictTransportSecurityWithDefaults))]
    public void NotStrictTransportSecurityWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotStrictTransportSecurityWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotStrictTransportSecurity.ValidCases), MemberType = typeof(TD.NotStrictTransportSecurity))]
    [MemberData(nameof(TD.NotStrictTransportSecurity.InvalidCases), MemberType = typeof(TD.NotStrictTransportSecurity))]
    public void NotStrictTransportSecurity_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)> tc)
    {
        var headers = tc.Value.headers;
        var result = AssertResult(tc, () => Guard.Against.NotStrictTransportSecurity(headers, tc.Value.minMaxAgeSeconds, tc.Value.requireIncludeSubDomains, tc.Value.requirePreload));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotXContentTypeOptionsHeader.ValidCases), MemberType = typeof(TD.NotXContentTypeOptionsHeader))]
    [MemberData(nameof(TD.NotXContentTypeOptionsHeader.InvalidCases), MemberType = typeof(TD.NotXContentTypeOptionsHeader))]
    public void NotXContentTypeOptionsHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotXContentTypeOptionsHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotXContentTypeOptionsWithDefaults.ValidCases), MemberType = typeof(TD.NotXContentTypeOptionsWithDefaults))]
    [MemberData(nameof(TD.NotXContentTypeOptionsWithDefaults.InvalidCases), MemberType = typeof(TD.NotXContentTypeOptionsWithDefaults))]
    public void NotXContentTypeOptionsWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotXContentTypeOptionsWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotXContentTypeOptions.ValidCases), MemberType = typeof(TD.NotXContentTypeOptions))]
    [MemberData(nameof(TD.NotXContentTypeOptions.InvalidCases), MemberType = typeof(TD.NotXContentTypeOptions))]
    public void NotXContentTypeOptions_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var headers = tc.Value.headers;
        var result = AssertResult(tc, () => Guard.Against.NotXContentTypeOptions(headers, tc.Value.expectedValue));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotXFrameOptionsHeader.ValidCases), MemberType = typeof(TD.NotXFrameOptionsHeader))]
    [MemberData(nameof(TD.NotXFrameOptionsHeader.InvalidCases), MemberType = typeof(TD.NotXFrameOptionsHeader))]
    public void NotXFrameOptionsHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotXFrameOptionsHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotXFrameOptionsWithDefaults.ValidCases), MemberType = typeof(TD.NotXFrameOptionsWithDefaults))]
    [MemberData(nameof(TD.NotXFrameOptionsWithDefaults.InvalidCases), MemberType = typeof(TD.NotXFrameOptionsWithDefaults))]
    public void NotXFrameOptionsWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotXFrameOptionsWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotXFrameOptions.ValidCases), MemberType = typeof(TD.NotXFrameOptions))]
    [MemberData(nameof(TD.NotXFrameOptions.InvalidCases), MemberType = typeof(TD.NotXFrameOptions))]
    public void NotXFrameOptions_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var headers = tc.Value.headers;
        var result = AssertResult(tc, () => Guard.Against.NotXFrameOptions(headers, tc.Value.expectedValue));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotReferrerPolicyHeader.ValidCases), MemberType = typeof(TD.NotReferrerPolicyHeader))]
    [MemberData(nameof(TD.NotReferrerPolicyHeader.InvalidCases), MemberType = typeof(TD.NotReferrerPolicyHeader))]
    public void NotReferrerPolicyHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotReferrerPolicyHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotReferrerPolicyWithDefaults.ValidCases), MemberType = typeof(TD.NotReferrerPolicyWithDefaults))]
    [MemberData(nameof(TD.NotReferrerPolicyWithDefaults.InvalidCases), MemberType = typeof(TD.NotReferrerPolicyWithDefaults))]
    public void NotReferrerPolicyWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotReferrerPolicyWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotReferrerPolicy.ValidCases), MemberType = typeof(TD.NotReferrerPolicy))]
    [MemberData(nameof(TD.NotReferrerPolicy.InvalidCases), MemberType = typeof(TD.NotReferrerPolicy))]
    public void NotReferrerPolicy_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var headers = tc.Value.headers;
        var result = AssertResult(tc, () => Guard.Against.NotReferrerPolicy(headers, tc.Value.expectedValue));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotPermissionsPolicyHeader.ValidCases), MemberType = typeof(TD.NotPermissionsPolicyHeader))]
    [MemberData(nameof(TD.NotPermissionsPolicyHeader.InvalidCases), MemberType = typeof(TD.NotPermissionsPolicyHeader))]
    public void NotPermissionsPolicyHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotPermissionsPolicyHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotPermissionsPolicyWithDefaults.ValidCases), MemberType = typeof(TD.NotPermissionsPolicyWithDefaults))]
    [MemberData(nameof(TD.NotPermissionsPolicyWithDefaults.InvalidCases), MemberType = typeof(TD.NotPermissionsPolicyWithDefaults))]
    public void NotPermissionsPolicyWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotPermissionsPolicyWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotPermissionsPolicy.ValidCases), MemberType = typeof(TD.NotPermissionsPolicy))]
    [MemberData(nameof(TD.NotPermissionsPolicy.InvalidCases), MemberType = typeof(TD.NotPermissionsPolicy))]
    public void NotPermissionsPolicy_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var headers = tc.Value.headers;
        var result = AssertResult(tc, () => Guard.Against.NotPermissionsPolicy(headers, tc.Value.expectedValue));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotPermissionsPolicyContaining.ValidCases), MemberType = typeof(TD.NotPermissionsPolicyContaining))]
    [MemberData(nameof(TD.NotPermissionsPolicyContaining.InvalidCases), MemberType = typeof(TD.NotPermissionsPolicyContaining))]
    public void NotPermissionsPolicyContaining_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)> tc)
    {
        var headers = tc.Value.headers;
        var requiredFragments = tc.Value.requiredFragments is not null ? new[] { tc.Value.requiredFragments } : null;
        var result = AssertResult(tc, () => Guard.Against.NotPermissionsPolicyContaining(headers, requiredFragments));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.ContentSecurityPolicyHeader.ValidCases), MemberType = typeof(TD.ContentSecurityPolicyHeader))]
    [MemberData(nameof(TD.ContentSecurityPolicyHeader.InvalidCases), MemberType = typeof(TD.ContentSecurityPolicyHeader))]
    public void ContentSecurityPolicyHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ContentSecurityPolicyHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.ContentSecurityPolicyWithDefaults.ValidCases), MemberType = typeof(TD.ContentSecurityPolicyWithDefaults))]
    [MemberData(nameof(TD.ContentSecurityPolicyWithDefaults.InvalidCases), MemberType = typeof(TD.ContentSecurityPolicyWithDefaults))]
    public void ContentSecurityPolicyWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ContentSecurityPolicyWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.StrictTransportSecurityHeader.ValidCases), MemberType = typeof(TD.StrictTransportSecurityHeader))]
    [MemberData(nameof(TD.StrictTransportSecurityHeader.InvalidCases), MemberType = typeof(TD.StrictTransportSecurityHeader))]
    public void StrictTransportSecurityHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.StrictTransportSecurityHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.StrictTransportSecurityWithDefaults.ValidCases), MemberType = typeof(TD.StrictTransportSecurityWithDefaults))]
    [MemberData(nameof(TD.StrictTransportSecurityWithDefaults.InvalidCases), MemberType = typeof(TD.StrictTransportSecurityWithDefaults))]
    public void StrictTransportSecurityWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.StrictTransportSecurityWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.XContentTypeOptionsHeader.ValidCases), MemberType = typeof(TD.XContentTypeOptionsHeader))]
    [MemberData(nameof(TD.XContentTypeOptionsHeader.InvalidCases), MemberType = typeof(TD.XContentTypeOptionsHeader))]
    public void XContentTypeOptionsHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.XContentTypeOptionsHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.XContentTypeOptionsWithDefaults.ValidCases), MemberType = typeof(TD.XContentTypeOptionsWithDefaults))]
    [MemberData(nameof(TD.XContentTypeOptionsWithDefaults.InvalidCases), MemberType = typeof(TD.XContentTypeOptionsWithDefaults))]
    public void XContentTypeOptionsWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.XContentTypeOptionsWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.XFrameOptionsHeader.ValidCases), MemberType = typeof(TD.XFrameOptionsHeader))]
    [MemberData(nameof(TD.XFrameOptionsHeader.InvalidCases), MemberType = typeof(TD.XFrameOptionsHeader))]
    public void XFrameOptionsHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.XFrameOptionsHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.XFrameOptionsWithDefaults.ValidCases), MemberType = typeof(TD.XFrameOptionsWithDefaults))]
    [MemberData(nameof(TD.XFrameOptionsWithDefaults.InvalidCases), MemberType = typeof(TD.XFrameOptionsWithDefaults))]
    public void XFrameOptionsWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.XFrameOptionsWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.ReferrerPolicyHeader.ValidCases), MemberType = typeof(TD.ReferrerPolicyHeader))]
    [MemberData(nameof(TD.ReferrerPolicyHeader.InvalidCases), MemberType = typeof(TD.ReferrerPolicyHeader))]
    public void ReferrerPolicyHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ReferrerPolicyHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.ReferrerPolicyWithDefaults.ValidCases), MemberType = typeof(TD.ReferrerPolicyWithDefaults))]
    [MemberData(nameof(TD.ReferrerPolicyWithDefaults.InvalidCases), MemberType = typeof(TD.ReferrerPolicyWithDefaults))]
    public void ReferrerPolicyWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ReferrerPolicyWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.PermissionsPolicyHeader.ValidCases), MemberType = typeof(TD.PermissionsPolicyHeader))]
    [MemberData(nameof(TD.PermissionsPolicyHeader.InvalidCases), MemberType = typeof(TD.PermissionsPolicyHeader))]
    public void PermissionsPolicyHeader_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.PermissionsPolicyHeader(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }

    [Theory]
    [MemberData(nameof(TD.PermissionsPolicyWithDefaults.ValidCases), MemberType = typeof(TD.PermissionsPolicyWithDefaults))]
    [MemberData(nameof(TD.PermissionsPolicyWithDefaults.InvalidCases), MemberType = typeof(TD.PermissionsPolicyWithDefaults))]
    public void PermissionsPolicyWithDefaults_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var headers = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.PermissionsPolicyWithDefaults(headers));
        if (tc.Expected.IsValid) Assert.Equal(headers, result);
    }
}
