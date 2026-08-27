using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.HttpSecurityHeaderRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustHttpSecurityHeaderClausesTestData
{
    public static class ContentSecurityPolicyHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyHeader.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentSecurityPolicy.Missing));
    }

    public static class StrictTransportSecurityHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityHeader.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.StrictTransportSecurity.Missing));
    }

    public static class XContentTypeOptionsHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsHeader.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentTypeOptions.Missing));
    }

    public static class XFrameOptionsHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsHeader.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.FrameOptions.Missing));
    }

    public static class ReferrerPolicyHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyHeader.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ReferrerPolicy.Missing));
    }

    public static class PermissionsPolicyHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyHeader.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.PermissionsPolicy.Missing));
    }

    public static class ContentSecurityPolicyWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyWithDefaults.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentSecurityPolicy.Weak));
    }

    public static class StrictTransportSecurityWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityWithDefaults.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.StrictTransportSecurity.Weak));
    }

    public static class XContentTypeOptionsWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsWithDefaults.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentTypeOptions.Mismatch));
    }

    public static class XFrameOptionsWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsWithDefaults.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.FrameOptions.Mismatch));
    }

    public static class ReferrerPolicyWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyWithDefaults.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ReferrerPolicy.Mismatch));
    }

    public static class PermissionsPolicyWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyWithDefaults.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.PermissionsPolicy.NotContains));
    }

    public static class XContentTypeOptions
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasXContentTypeOptions.MatchingValue), F.HasXContentTypeOptions.MatchingValue, new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasXContentTypeOptions.NonMatchingValue), F.HasXContentTypeOptions.NonMatchingValue, new MustExpected(false, Code: MustCodes.Http.ContentTypeOptions.Mismatch)),
            new("NullHeaders", (null, "nosniff"), new MustExpected(false, "headers must not be null.", "headers"))
        ];
    }

    public static class XFrameOptions
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasXFrameOptions.MatchingValue), F.HasXFrameOptions.MatchingValue, new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasXFrameOptions.NonMatchingValue), F.HasXFrameOptions.NonMatchingValue, new MustExpected(false, Code: MustCodes.Http.FrameOptions.Mismatch)),
            new("NullHeaders", (null, "DENY"), new MustExpected(false, "headers must not be null.", "headers"))
        ];
    }

    public static class ReferrerPolicy
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasReferrerPolicy.MatchingValue), F.HasReferrerPolicy.MatchingValue, new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasReferrerPolicy.NonMatchingValue), F.HasReferrerPolicy.NonMatchingValue, new MustExpected(false, Code: MustCodes.Http.ReferrerPolicy.Mismatch)),
            new("NullHeaders", (null, "no-referrer"), new MustExpected(false, "headers must not be null.", "headers"))
        ];
    }

    public static class PermissionsPolicy
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasPermissionsPolicy.MatchingValue), F.HasPermissionsPolicy.MatchingValue, new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasPermissionsPolicy.NonMatchingValue), F.HasPermissionsPolicy.NonMatchingValue, new MustExpected(false, Code: MustCodes.Http.PermissionsPolicy.Mismatch)),
            new("NullHeaders", (null, "geolocation=()"), new MustExpected(false, "headers must not be null.", "headers"))
        ];
    }

    public static class StrictTransportSecurity
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)>> ValidCases =>
        [
            new(nameof(F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired), F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired, new MustExpected(true)),
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadTrue), F.HasStrictTransportSecurity.RequiresPreloadTrue, new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)>> InvalidCases =>
        [
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadFalse), F.HasStrictTransportSecurity.RequiresPreloadFalse, new MustExpected(false, Code: MustCodes.Http.StrictTransportSecurity.Weak)),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeLessThanMin), F.HasStrictTransportSecurity.MaxAgeLessThanMin, new MustExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeNonNumeric), F.HasStrictTransportSecurity.MaxAgeNonNumeric, new MustExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MinMaxAgeZero), F.HasStrictTransportSecurity.MinMaxAgeZero, new MustExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MinMaxAgeNegative), F.HasStrictTransportSecurity.MinMaxAgeNegative, new MustExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeNegativeValue), F.HasStrictTransportSecurity.MaxAgeNegativeValue, new MustExpected(false)),
            new("NullHeaders", (null, 31_536_000, true, false), new MustExpected(false, "headers must not be null.", "headers"))
        ];
    }

    public static class ContentSecurityPolicy
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)>> ValidCases =>
        [
            new(nameof(F.HasContentSecurityPolicy.AllRequiredWhitespace), F.HasContentSecurityPolicy.AllRequiredWhitespace, new MustExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.AllNullRequirements), F.HasContentSecurityPolicy.AllNullRequirements, new MustExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.OverrideDefaultSrc), F.HasContentSecurityPolicy.OverrideDefaultSrc, new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)>> InvalidCases =>
        [
            new("NullHeaders", (null, "'self'", "'none'", "'self'", "'none'"), new MustExpected(false, "headers must not be null.", "headers", Code: MustCodes.Http.ContentSecurityPolicy.Weak))
        ];
    }

    public static class PermissionsPolicyContaining
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)>> ValidCases =>
        [
            new(nameof(F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace), F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace, new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)>> InvalidCases =>
        [
            new(nameof(F.HasPermissionsPolicyContaining.HeaderMissing), F.HasPermissionsPolicyContaining.HeaderMissing, new MustExpected(false, Code: MustCodes.Http.PermissionsPolicy.NotContains)),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentIsWhitespace), F.HasPermissionsPolicyContaining.FragmentIsWhitespace, new MustExpected(false)),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentMissing), F.HasPermissionsPolicyContaining.FragmentMissing, new MustExpected(false)),
            new(nameof(F.HasPermissionsPolicyContaining.NullHeaders), F.HasPermissionsPolicyContaining.NullHeaders, new MustExpected(false)),
            new(nameof(F.HasPermissionsPolicyContaining.NullFragments), F.HasPermissionsPolicyContaining.NullFragments, new MustExpected(false))
        ];
    }

    public static class NotContentSecurityPolicyHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyHeader.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentSecurityPolicy.Present));
    }

    public static class NotContentSecurityPolicyWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyWithDefaults.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentSecurityPolicy.Strong));
    }

    public static class NotStrictTransportSecurityHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityHeader.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.StrictTransportSecurity.Present));
    }

    public static class NotStrictTransportSecurityWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityWithDefaults.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.StrictTransportSecurity.Strong));
    }

    public static class NotXContentTypeOptionsHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsHeader.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentTypeOptions.Present));
    }

    public static class NotXContentTypeOptionsWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsWithDefaults.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ContentTypeOptions.Match));
    }

    public static class NotXFrameOptionsHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsHeader.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.FrameOptions.Present));
    }

    public static class NotXFrameOptionsWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsWithDefaults.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.FrameOptions.Match));
    }

    public static class NotReferrerPolicyHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyHeader.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ReferrerPolicy.Present));
    }

    public static class NotReferrerPolicyWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyWithDefaults.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.ReferrerPolicy.Match));
    }

    public static class NotPermissionsPolicyHeader
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyHeader.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyHeader.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.PermissionsPolicy.Present));
    }

    public static class NotPermissionsPolicyWithDefaults
    {
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyWithDefaults.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyWithDefaults.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.PermissionsPolicy.Contains));
    }
}
