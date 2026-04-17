using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentHttpSecurityHeaderExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public IReadOnlyDictionary<string, IEnumerable<string>>? Value { get; init; } }

    private sealed class ContentSecurityPolicyHeaderValidator : AbstractValidator<Model>
    {
        public ContentSecurityPolicyHeaderValidator() => RuleFor(x => x.Value).ContentSecurityPolicyHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.ContentSecurityPolicyHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.ContentSecurityPolicyHeader))]
    public void ContentSecurityPolicyHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new ContentSecurityPolicyHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class StrictTransportSecurityHeaderValidator : AbstractValidator<Model>
    {
        public StrictTransportSecurityHeaderValidator() => RuleFor(x => x.Value).StrictTransportSecurityHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.StrictTransportSecurityHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.StrictTransportSecurityHeader))]
    public void StrictTransportSecurityHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new StrictTransportSecurityHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class XContentTypeOptionsHeaderValidator : AbstractValidator<Model>
    {
        public XContentTypeOptionsHeaderValidator() => RuleFor(x => x.Value).XContentTypeOptionsHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.XContentTypeOptionsHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.XContentTypeOptionsHeader))]
    public void XContentTypeOptionsHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new XContentTypeOptionsHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class XFrameOptionsHeaderValidator : AbstractValidator<Model>
    {
        public XFrameOptionsHeaderValidator() => RuleFor(x => x.Value).XFrameOptionsHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.XFrameOptionsHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.XFrameOptionsHeader))]
    public void XFrameOptionsHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new XFrameOptionsHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ReferrerPolicyHeaderValidator : AbstractValidator<Model>
    {
        public ReferrerPolicyHeaderValidator() => RuleFor(x => x.Value).ReferrerPolicyHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.ReferrerPolicyHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.ReferrerPolicyHeader))]
    public void ReferrerPolicyHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new ReferrerPolicyHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class PermissionsPolicyHeaderValidator : AbstractValidator<Model>
    {
        public PermissionsPolicyHeaderValidator() => RuleFor(x => x.Value).PermissionsPolicyHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicyHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicyHeader))]
    public void PermissionsPolicyHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new PermissionsPolicyHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ContentSecurityPolicyWithDefaultsValidator : AbstractValidator<Model>
    {
        public ContentSecurityPolicyWithDefaultsValidator() => RuleFor(x => x.Value).ContentSecurityPolicyWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.ContentSecurityPolicyWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.ContentSecurityPolicyWithDefaults))]
    public void ContentSecurityPolicyWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new ContentSecurityPolicyWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class StrictTransportSecurityWithDefaultsValidator : AbstractValidator<Model>
    {
        public StrictTransportSecurityWithDefaultsValidator() => RuleFor(x => x.Value).StrictTransportSecurityWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.StrictTransportSecurityWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.StrictTransportSecurityWithDefaults))]
    public void StrictTransportSecurityWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new StrictTransportSecurityWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class XContentTypeOptionsWithDefaultsValidator : AbstractValidator<Model>
    {
        public XContentTypeOptionsWithDefaultsValidator() => RuleFor(x => x.Value).XContentTypeOptionsWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.XContentTypeOptionsWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.XContentTypeOptionsWithDefaults))]
    public void XContentTypeOptionsWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new XContentTypeOptionsWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class XFrameOptionsWithDefaultsValidator : AbstractValidator<Model>
    {
        public XFrameOptionsWithDefaultsValidator() => RuleFor(x => x.Value).XFrameOptionsWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.XFrameOptionsWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.XFrameOptionsWithDefaults))]
    public void XFrameOptionsWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new XFrameOptionsWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ReferrerPolicyWithDefaultsValidator : AbstractValidator<Model>
    {
        public ReferrerPolicyWithDefaultsValidator() => RuleFor(x => x.Value).ReferrerPolicyWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.ReferrerPolicyWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.ReferrerPolicyWithDefaults))]
    public void ReferrerPolicyWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new ReferrerPolicyWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class PermissionsPolicyWithDefaultsValidator : AbstractValidator<Model>
    {
        public PermissionsPolicyWithDefaultsValidator() => RuleFor(x => x.Value).PermissionsPolicyWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicyWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicyWithDefaults))]
    public void PermissionsPolicyWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new PermissionsPolicyWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotContentSecurityPolicyHeaderValidator : AbstractValidator<Model>
    {
        public NotContentSecurityPolicyHeaderValidator() => RuleFor(x => x.Value).NotContentSecurityPolicyHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotContentSecurityPolicyHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotContentSecurityPolicyHeader))]
    public void NotContentSecurityPolicyHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotContentSecurityPolicyHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotStrictTransportSecurityHeaderValidator : AbstractValidator<Model>
    {
        public NotStrictTransportSecurityHeaderValidator() => RuleFor(x => x.Value).NotStrictTransportSecurityHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotStrictTransportSecurityHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotStrictTransportSecurityHeader))]
    public void NotStrictTransportSecurityHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotStrictTransportSecurityHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotXContentTypeOptionsHeaderValidator : AbstractValidator<Model>
    {
        public NotXContentTypeOptionsHeaderValidator() => RuleFor(x => x.Value).NotXContentTypeOptionsHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotXContentTypeOptionsHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotXContentTypeOptionsHeader))]
    public void NotXContentTypeOptionsHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotXContentTypeOptionsHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotXFrameOptionsHeaderValidator : AbstractValidator<Model>
    {
        public NotXFrameOptionsHeaderValidator() => RuleFor(x => x.Value).NotXFrameOptionsHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotXFrameOptionsHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotXFrameOptionsHeader))]
    public void NotXFrameOptionsHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotXFrameOptionsHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotReferrerPolicyHeaderValidator : AbstractValidator<Model>
    {
        public NotReferrerPolicyHeaderValidator() => RuleFor(x => x.Value).NotReferrerPolicyHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotReferrerPolicyHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotReferrerPolicyHeader))]
    public void NotReferrerPolicyHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotReferrerPolicyHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotPermissionsPolicyHeaderValidator : AbstractValidator<Model>
    {
        public NotPermissionsPolicyHeaderValidator() => RuleFor(x => x.Value).NotPermissionsPolicyHeader();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotPermissionsPolicyHeader.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotPermissionsPolicyHeader))]
    public void NotPermissionsPolicyHeader_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotPermissionsPolicyHeaderValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotContentSecurityPolicyWithDefaultsValidator : AbstractValidator<Model>
    {
        public NotContentSecurityPolicyWithDefaultsValidator() => RuleFor(x => x.Value).NotContentSecurityPolicyWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotContentSecurityPolicyWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotContentSecurityPolicyWithDefaults))]
    public void NotContentSecurityPolicyWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotContentSecurityPolicyWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotStrictTransportSecurityWithDefaultsValidator : AbstractValidator<Model>
    {
        public NotStrictTransportSecurityWithDefaultsValidator() => RuleFor(x => x.Value).NotStrictTransportSecurityWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotStrictTransportSecurityWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotStrictTransportSecurityWithDefaults))]
    public void NotStrictTransportSecurityWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotStrictTransportSecurityWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotXContentTypeOptionsWithDefaultsValidator : AbstractValidator<Model>
    {
        public NotXContentTypeOptionsWithDefaultsValidator() => RuleFor(x => x.Value).NotXContentTypeOptionsWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotXContentTypeOptionsWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotXContentTypeOptionsWithDefaults))]
    public void NotXContentTypeOptionsWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotXContentTypeOptionsWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotXFrameOptionsWithDefaultsValidator : AbstractValidator<Model>
    {
        public NotXFrameOptionsWithDefaultsValidator() => RuleFor(x => x.Value).NotXFrameOptionsWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotXFrameOptionsWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotXFrameOptionsWithDefaults))]
    public void NotXFrameOptionsWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotXFrameOptionsWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotReferrerPolicyWithDefaultsValidator : AbstractValidator<Model>
    {
        public NotReferrerPolicyWithDefaultsValidator() => RuleFor(x => x.Value).NotReferrerPolicyWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotReferrerPolicyWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotReferrerPolicyWithDefaults))]
    public void NotReferrerPolicyWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotReferrerPolicyWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotPermissionsPolicyWithDefaultsValidator : AbstractValidator<Model>
    {
        public NotPermissionsPolicyWithDefaultsValidator() => RuleFor(x => x.Value).NotPermissionsPolicyWithDefaults();
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.NotPermissionsPolicyWithDefaults.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.NotPermissionsPolicyWithDefaults))]
    public void NotPermissionsPolicyWithDefaults_BehavesAsExpected(FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?> tc)
    {
        var result = new NotPermissionsPolicyWithDefaultsValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class ContentSecurityPolicyValidator : AbstractValidator<Model>
    {
        public ContentSecurityPolicyValidator(string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue) =>
            RuleFor(x => x.Value).ContentSecurityPolicy(requiredDefaultSrcValue, requiredObjectSrcValue, requiredBaseUriValue, requiredFrameAncestorsValue);
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.ContentSecurityPolicy.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.ContentSecurityPolicy))]
    public void ContentSecurityPolicy_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)> tc)
    {
        var result = new ContentSecurityPolicyValidator(tc.Value.requiredDefaultSrcValue, tc.Value.requiredObjectSrcValue, tc.Value.requiredBaseUriValue, tc.Value.requiredFrameAncestorsValue).Validate(new Model { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class StrictTransportSecurityValidator : AbstractValidator<Model>
    {
        public StrictTransportSecurityValidator(int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) =>
            RuleFor(x => x.Value).StrictTransportSecurity(minMaxAgeSeconds, requireIncludeSubDomains, requirePreload);
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.StrictTransportSecurity.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.StrictTransportSecurity))]
    public void StrictTransportSecurity_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)> tc)
    {
        var result = new StrictTransportSecurityValidator(tc.Value.minMaxAgeSeconds, tc.Value.requireIncludeSubDomains, tc.Value.requirePreload).Validate(new Model { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class XContentTypeOptionsValidator : AbstractValidator<Model>
    {
        public XContentTypeOptionsValidator(string? expectedValue) =>
            RuleFor(x => x.Value).XContentTypeOptions(expectedValue);
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.XContentTypeOptions.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.XContentTypeOptions))]
    public void XContentTypeOptions_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var result = new XContentTypeOptionsValidator(tc.Value.expectedValue).Validate(new Model { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class XFrameOptionsValidator : AbstractValidator<Model>
    {
        public XFrameOptionsValidator(string? expectedValue) =>
            RuleFor(x => x.Value).XFrameOptions(expectedValue);
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.XFrameOptions.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.XFrameOptions))]
    public void XFrameOptions_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var result = new XFrameOptionsValidator(tc.Value.expectedValue).Validate(new Model { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class ReferrerPolicyValidator : AbstractValidator<Model>
    {
        public ReferrerPolicyValidator(string? expectedValue) =>
            RuleFor(x => x.Value).ReferrerPolicy(expectedValue);
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.ReferrerPolicy.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.ReferrerPolicy))]
    public void ReferrerPolicy_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var result = new ReferrerPolicyValidator(tc.Value.expectedValue).Validate(new Model { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class PermissionsPolicyValidator : AbstractValidator<Model>
    {
        public PermissionsPolicyValidator(string? expectedValue) =>
            RuleFor(x => x.Value).PermissionsPolicy(expectedValue);
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicy.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicy))]
    public void PermissionsPolicy_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)> tc)
    {
        var result = new PermissionsPolicyValidator(tc.Value.expectedValue).Validate(new Model { Value = tc.Value.headers });
        AssertResult(tc, result);
    }

    private sealed class PermissionsPolicyContainingValidator : AbstractValidator<Model>
    {
        public PermissionsPolicyContainingValidator(string[]? requiredFragments) =>
            RuleFor(x => x.Value).PermissionsPolicyContaining(requiredFragments);
    }

    [Theory]
    [MemberData(nameof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicyContaining.Cases), MemberType = typeof(FluentHttpSecurityHeaderExtensionsTestData.PermissionsPolicyContaining))]
    public void PermissionsPolicyContaining_BehavesAsExpected(FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? requiredFragments)> tc)
    {
        var result = new PermissionsPolicyContainingValidator(tc.Value.requiredFragments).Validate(new Model { Value = tc.Value.headers });
        AssertResult(tc, result);
    }
}
