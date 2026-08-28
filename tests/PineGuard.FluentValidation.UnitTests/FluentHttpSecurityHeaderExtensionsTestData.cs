using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.HttpSecurityHeaderRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentHttpSecurityHeaderExtensionsTestData
{
    public static class ContentSecurityPolicyHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasContentSecurityPolicyHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasContentSecurityPolicyHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Content-Security-Policy header.", Code: MustCodes.Http.ContentSecurityPolicy.Missing)
            });
    }

    public static class StrictTransportSecurityHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasStrictTransportSecurityHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasStrictTransportSecurityHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Strict-Transport-Security header.")
            });
    }

    public static class XContentTypeOptionsHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXContentTypeOptionsHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXContentTypeOptionsHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain an X-Content-Type-Options header.")
            });
    }

    public static class XFrameOptionsHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXFrameOptionsHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXFrameOptionsHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain an X-Frame-Options header.")
            });
    }

    public static class ReferrerPolicyHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasReferrerPolicyHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasReferrerPolicyHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Referrer-Policy header.")
            });
    }

    public static class PermissionsPolicyHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasPermissionsPolicyHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasPermissionsPolicyHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Permissions-Policy header.")
            });
    }

    public static class ContentSecurityPolicyWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasContentSecurityPolicyWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasContentSecurityPolicyWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Content-Security-Policy header with secure default values.")
            });
    }

    public static class StrictTransportSecurityWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasStrictTransportSecurityWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasStrictTransportSecurityWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Strict-Transport-Security header with secure default values.")
            });
    }

    public static class XContentTypeOptionsWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXContentTypeOptionsWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXContentTypeOptionsWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain an X-Content-Type-Options header with secure default values.")
            });
    }

    public static class XFrameOptionsWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXFrameOptionsWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXFrameOptionsWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain an X-Frame-Options header with secure default values.")
            });
    }

    public static class ReferrerPolicyWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasReferrerPolicyWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasReferrerPolicyWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Referrer-Policy header with secure default values.")
            });
    }

    public static class PermissionsPolicyWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasPermissionsPolicyWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasPermissionsPolicyWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain a Permissions-Policy header with secure default values.")
            });
    }

    public static class NotContentSecurityPolicyHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasContentSecurityPolicyHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasContentSecurityPolicyHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Content-Security-Policy header."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotStrictTransportSecurityHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasStrictTransportSecurityHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasStrictTransportSecurityHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Strict-Transport-Security header."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotXContentTypeOptionsHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXContentTypeOptionsHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXContentTypeOptionsHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain an X-Content-Type-Options header."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotXFrameOptionsHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXFrameOptionsHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXFrameOptionsHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain an X-Frame-Options header."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotReferrerPolicyHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasReferrerPolicyHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasReferrerPolicyHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Referrer-Policy header."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotPermissionsPolicyHeader
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasPermissionsPolicyHeader.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasPermissionsPolicyHeader.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Permissions-Policy header."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotContentSecurityPolicyWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasContentSecurityPolicyWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasContentSecurityPolicyWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Content-Security-Policy header with defaults."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotStrictTransportSecurityWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasStrictTransportSecurityWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasStrictTransportSecurityWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Strict-Transport-Security header with defaults."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotXContentTypeOptionsWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXContentTypeOptionsWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXContentTypeOptionsWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain an X-Content-Type-Options header with defaults."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotXFrameOptionsWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasXFrameOptionsWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasXFrameOptionsWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain an X-Frame-Options header with defaults."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotReferrerPolicyWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasReferrerPolicyWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasReferrerPolicyWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Referrer-Policy header with defaults."),
                _ => new FluentExpected(true)
            });
    }

    public static class NotPermissionsPolicyWithDefaults
    {
        public static TheoryData<FluentCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases =>
            F.HasPermissionsPolicyWithDefaults.AllScenarios.ToFluentCases(s => s.Name switch
            {
                nameof(F.HasPermissionsPolicyWithDefaults.Null) => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain a Permissions-Policy header with defaults."),
                _ => new FluentExpected(true)
            });
    }

    public static class ContentSecurityPolicy
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)>> Cases =>
        [
            new(nameof(F.HasContentSecurityPolicy.AllNullRequirements), (F.HasContentSecurityPolicy.AllNullRequirements.headers, F.HasContentSecurityPolicy.AllNullRequirements.requiredDefaultSrcValue, F.HasContentSecurityPolicy.AllNullRequirements.requiredObjectSrcValue, F.HasContentSecurityPolicy.AllNullRequirements.requiredBaseUriValue, F.HasContentSecurityPolicy.AllNullRequirements.requiredFrameAncestorsValue), new FluentExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.OverrideDefaultSrc), (F.HasContentSecurityPolicy.OverrideDefaultSrc.headers, F.HasContentSecurityPolicy.OverrideDefaultSrc.requiredDefaultSrcValue, F.HasContentSecurityPolicy.OverrideDefaultSrc.requiredObjectSrcValue, F.HasContentSecurityPolicy.OverrideDefaultSrc.requiredBaseUriValue, F.HasContentSecurityPolicy.OverrideDefaultSrc.requiredFrameAncestorsValue), new FluentExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.AllRequiredWhitespace), (F.HasContentSecurityPolicy.AllRequiredWhitespace.headers, F.HasContentSecurityPolicy.AllRequiredWhitespace.requiredDefaultSrcValue, F.HasContentSecurityPolicy.AllRequiredWhitespace.requiredObjectSrcValue, F.HasContentSecurityPolicy.AllRequiredWhitespace.requiredBaseUriValue, F.HasContentSecurityPolicy.AllRequiredWhitespace.requiredFrameAncestorsValue), new FluentExpected(true)),
            new("NullHeaders", (null, null, null, null, null), new FluentExpected(true))
        ];
    }

    public static class StrictTransportSecurity
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)>> Cases =>
        [
            new(nameof(F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired), (F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired.headers, F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired.minMaxAgeSeconds, F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired.requireIncludeSubDomains, F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired.requirePreload), new FluentExpected(true)),
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadTrue), (F.HasStrictTransportSecurity.RequiresPreloadTrue.headers, F.HasStrictTransportSecurity.RequiresPreloadTrue.minMaxAgeSeconds, F.HasStrictTransportSecurity.RequiresPreloadTrue.requireIncludeSubDomains, F.HasStrictTransportSecurity.RequiresPreloadTrue.requirePreload), new FluentExpected(true)),
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadFalse), (F.HasStrictTransportSecurity.RequiresPreloadFalse.headers, F.HasStrictTransportSecurity.RequiresPreloadFalse.minMaxAgeSeconds, F.HasStrictTransportSecurity.RequiresPreloadFalse.requireIncludeSubDomains, F.HasStrictTransportSecurity.RequiresPreloadFalse.requirePreload), new FluentExpected(false, "Value must contain a Strict-Transport-Security header that meets requirements.")),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeLessThanMin), (F.HasStrictTransportSecurity.MaxAgeLessThanMin.headers, F.HasStrictTransportSecurity.MaxAgeLessThanMin.minMaxAgeSeconds, F.HasStrictTransportSecurity.MaxAgeLessThanMin.requireIncludeSubDomains, F.HasStrictTransportSecurity.MaxAgeLessThanMin.requirePreload), new FluentExpected(false, "Value must contain a Strict-Transport-Security header that meets requirements.")),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeNonNumeric), (F.HasStrictTransportSecurity.MaxAgeNonNumeric.headers, F.HasStrictTransportSecurity.MaxAgeNonNumeric.minMaxAgeSeconds, F.HasStrictTransportSecurity.MaxAgeNonNumeric.requireIncludeSubDomains, F.HasStrictTransportSecurity.MaxAgeNonNumeric.requirePreload), new FluentExpected(false, "Value must contain a Strict-Transport-Security header that meets requirements.")),
            new("NullHeaders", (null, 31536000, false, false), new FluentExpected(true))
        ];
    }

    public static class XContentTypeOptions
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasXContentTypeOptions.MatchingValue), (F.HasXContentTypeOptions.MatchingValue.headers, F.HasXContentTypeOptions.MatchingValue.expectedValue), new FluentExpected(true)),
            new(nameof(F.HasXContentTypeOptions.NonMatchingValue), (F.HasXContentTypeOptions.NonMatchingValue.headers, F.HasXContentTypeOptions.NonMatchingValue.expectedValue), new FluentExpected(false, "Value must contain an X-Content-Type-Options header with the expected value.")),
            new("NullHeaders", (null, "nosniff"), new FluentExpected(true))
        ];
    }

    public static class XFrameOptions
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasXFrameOptions.MatchingValue), (F.HasXFrameOptions.MatchingValue.headers, F.HasXFrameOptions.MatchingValue.expectedValue), new FluentExpected(true)),
            new(nameof(F.HasXFrameOptions.NonMatchingValue), (F.HasXFrameOptions.NonMatchingValue.headers, F.HasXFrameOptions.NonMatchingValue.expectedValue), new FluentExpected(false, "Value must contain an X-Frame-Options header with the expected value.")),
            new("NullHeaders", (null, "DENY"), new FluentExpected(true))
        ];
    }

    public static class ReferrerPolicy
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasReferrerPolicy.MatchingValue), (F.HasReferrerPolicy.MatchingValue.headers, F.HasReferrerPolicy.MatchingValue.expectedValue), new FluentExpected(true)),
            new(nameof(F.HasReferrerPolicy.NonMatchingValue), (F.HasReferrerPolicy.NonMatchingValue.headers, F.HasReferrerPolicy.NonMatchingValue.expectedValue), new FluentExpected(false, "Value must contain a Referrer-Policy header with the expected value.")),
            new("NullHeaders", (null, "strict-origin-when-cross-origin"), new FluentExpected(true))
        ];
    }

    public static class PermissionsPolicy
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasPermissionsPolicy.MatchingValue), (F.HasPermissionsPolicy.MatchingValue.headers, F.HasPermissionsPolicy.MatchingValue.expectedValue), new FluentExpected(true)),
            new(nameof(F.HasPermissionsPolicy.NonMatchingValue), (F.HasPermissionsPolicy.NonMatchingValue.headers, F.HasPermissionsPolicy.NonMatchingValue.expectedValue), new FluentExpected(false, "Value must contain a Permissions-Policy header with the expected value.")),
            new("NullHeaders", (null, "camera=()"), new FluentExpected(true))
        ];
    }

    public static class PermissionsPolicyContaining
    {
        private static string[]? ToArray(string? s) => s is null ? null : [s];

        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? requiredFragments)>> Cases =>
        [
            new(nameof(F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace), (F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace.headers, ToArray(F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace.requiredFragments)), new FluentExpected(true)),
            new(nameof(F.HasPermissionsPolicyContaining.NullHeaders), (F.HasPermissionsPolicyContaining.NullHeaders.headers, ToArray(F.HasPermissionsPolicyContaining.NullHeaders.requiredFragments)), new FluentExpected(true)),
            new(nameof(F.HasPermissionsPolicyContaining.HeaderMissing), (F.HasPermissionsPolicyContaining.HeaderMissing.headers, ToArray(F.HasPermissionsPolicyContaining.HeaderMissing.requiredFragments)), new FluentExpected(false, "Value must contain a Permissions-Policy header containing required fragments.")),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentMissing), (F.HasPermissionsPolicyContaining.FragmentMissing.headers, ToArray(F.HasPermissionsPolicyContaining.FragmentMissing.requiredFragments)), new FluentExpected(false, "Value must contain a Permissions-Policy header containing required fragments.")),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentIsWhitespace), (F.HasPermissionsPolicyContaining.FragmentIsWhitespace.headers, ToArray(F.HasPermissionsPolicyContaining.FragmentIsWhitespace.requiredFragments)), new FluentExpected(false, "Value must contain a Permissions-Policy header containing required fragments.")),
            new(nameof(F.HasPermissionsPolicyContaining.NullFragments), (F.HasPermissionsPolicyContaining.NullFragments.headers, ToArray(F.HasPermissionsPolicyContaining.NullFragments.requiredFragments)), new FluentExpected(false, "Value must contain a Permissions-Policy header containing required fragments."))
        ];
    }
}
