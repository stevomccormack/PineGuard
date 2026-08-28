using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.HttpSecurityHeaderRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardHttpSecurityHeaderClausesTestData
{
    public static class NotContentSecurityPolicyHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyHeader.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyHeader.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotContentSecurityPolicyWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyWithDefaults.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyWithDefaults.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotContentSecurityPolicy
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)>> ValidCases =>
        [
            new(nameof(F.HasContentSecurityPolicy.AllRequiredWhitespace), F.HasContentSecurityPolicy.AllRequiredWhitespace, new GuardExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.AllNullRequirements), F.HasContentSecurityPolicy.AllNullRequirements, new GuardExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.OverrideDefaultSrc), F.HasContentSecurityPolicy.OverrideDefaultSrc, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)>> InvalidCases =>
        [
            new(nameof(F.EmptyHeaders) + "_empty", (F.EmptyHeaders, null, null, null, null), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("Null_null", (null, null, null, null, null), new GuardExpected(false, typeof(ArgumentNullException), "headers"))
        ];
    }

    public static class NotStrictTransportSecurityHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityHeader.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityHeader.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotStrictTransportSecurityWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityWithDefaults.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityWithDefaults.InvalidScenarios.ToGuardCases("headers");
    }

    // Guard.Against.NotStrictTransportSecurity calls Must.Be.StrictTransportSecurity, which checks its own
    // "minMaxAgeSeconds must be positive" precondition before inspecting headers at all — a non-positive
    // minMaxAgeSeconds therefore attributes to "minMaxAgeSeconds", not "headers" (see MustHttpSecurityHeaderClauses.StrictTransportSecurity).
    public static class NotStrictTransportSecurity
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)>> ValidCases =>
        [
            new(nameof(F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired), F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired, new GuardExpected(true)),
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadTrue), F.HasStrictTransportSecurity.RequiresPreloadTrue, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)>> InvalidCases =>
        [
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadFalse), F.HasStrictTransportSecurity.RequiresPreloadFalse, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeLessThanMin), F.HasStrictTransportSecurity.MaxAgeLessThanMin, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeNonNumeric), F.HasStrictTransportSecurity.MaxAgeNonNumeric, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeNegativeValue), F.HasStrictTransportSecurity.MaxAgeNegativeValue, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasStrictTransportSecurity.MinMaxAgeZero), F.HasStrictTransportSecurity.MinMaxAgeZero, new GuardExpected(false, typeof(ArgumentException), "minMaxAgeSeconds")),
            new(nameof(F.HasStrictTransportSecurity.MinMaxAgeNegative), F.HasStrictTransportSecurity.MinMaxAgeNegative, new GuardExpected(false, typeof(ArgumentException), "minMaxAgeSeconds")),
            new("EmptyHeaders", (F.EmptyHeaders, 1, false, false), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("Null", (null, 1, false, false), new GuardExpected(false, typeof(ArgumentNullException), "headers"))
        ];
    }

    public static class NotXContentTypeOptionsHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsHeader.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsHeader.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotXContentTypeOptionsWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsWithDefaults.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsWithDefaults.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotXContentTypeOptions
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasXContentTypeOptions.MatchingValue), F.HasXContentTypeOptions.MatchingValue, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasXContentTypeOptions.NonMatchingValue), F.HasXContentTypeOptions.NonMatchingValue, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("EmptyHeaders", (F.EmptyHeaders, "nosniff"), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("Null", (null, "nosniff"), new GuardExpected(false, typeof(ArgumentNullException), "headers"))
        ];
    }

    public static class NotXFrameOptionsHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsHeader.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsHeader.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotXFrameOptionsWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsWithDefaults.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsWithDefaults.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotXFrameOptions
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasXFrameOptions.MatchingValue), F.HasXFrameOptions.MatchingValue, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasXFrameOptions.NonMatchingValue), F.HasXFrameOptions.NonMatchingValue, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("EmptyHeaders", (F.EmptyHeaders, "DENY"), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("Null", (null, "DENY"), new GuardExpected(false, typeof(ArgumentNullException), "headers"))
        ];
    }

    public static class NotReferrerPolicyHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyHeader.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyHeader.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotReferrerPolicyWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyWithDefaults.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyWithDefaults.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotReferrerPolicy
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasReferrerPolicy.MatchingValue), F.HasReferrerPolicy.MatchingValue, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasReferrerPolicy.NonMatchingValue), F.HasReferrerPolicy.NonMatchingValue, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("EmptyHeaders", (F.EmptyHeaders, "no-referrer"), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("Null", (null, "no-referrer"), new GuardExpected(false, typeof(ArgumentNullException), "headers"))
        ];
    }

    public static class NotPermissionsPolicyHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyHeader.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyHeader.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotPermissionsPolicyWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyWithDefaults.ValidScenarios.ToGuardCases("headers");
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyWithDefaults.InvalidScenarios.ToGuardCases("headers");
    }

    public static class NotPermissionsPolicy
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasPermissionsPolicy.MatchingValue), F.HasPermissionsPolicy.MatchingValue, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasPermissionsPolicy.NonMatchingValue), F.HasPermissionsPolicy.NonMatchingValue, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("EmptyHeaders", (F.EmptyHeaders, "geolocation=()"), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new("Null", (null, "geolocation=()"), new GuardExpected(false, typeof(ArgumentNullException), "headers"))
        ];
    }

    public static class NotPermissionsPolicyContaining
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)>> ValidCases =>
        [
            new(nameof(F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace), F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)>> InvalidCases =>
        [
            new(nameof(F.HasPermissionsPolicyContaining.HeaderMissing), F.HasPermissionsPolicyContaining.HeaderMissing, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentIsWhitespace), F.HasPermissionsPolicyContaining.FragmentIsWhitespace, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentMissing), F.HasPermissionsPolicyContaining.FragmentMissing, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasPermissionsPolicyContaining.NullFragments), F.HasPermissionsPolicyContaining.NullFragments, new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasPermissionsPolicyContaining.NullHeaders), F.HasPermissionsPolicyContaining.NullHeaders, new GuardExpected(false, typeof(ArgumentNullException), "headers"))
        ];
    }

    public static class ContentSecurityPolicyHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyHeader.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyHeader.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class ContentSecurityPolicyWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasContentSecurityPolicyWithDefaults.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasContentSecurityPolicyWithDefaults.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class StrictTransportSecurityHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityHeader.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityHeader.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class StrictTransportSecurityWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasStrictTransportSecurityWithDefaults.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasStrictTransportSecurityWithDefaults.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class XContentTypeOptionsHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsHeader.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsHeader.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class XContentTypeOptionsWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXContentTypeOptionsWithDefaults.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXContentTypeOptionsWithDefaults.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class XFrameOptionsHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsHeader.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsHeader.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class XFrameOptionsWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasXFrameOptionsWithDefaults.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasXFrameOptionsWithDefaults.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class ReferrerPolicyHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyHeader.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyHeader.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class ReferrerPolicyWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasReferrerPolicyWithDefaults.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasReferrerPolicyWithDefaults.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class PermissionsPolicyHeader
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyHeader.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyHeader.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }

    public static class PermissionsPolicyWithDefaults
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> ValidCases => F.HasPermissionsPolicyWithDefaults.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> InvalidCases => F.HasPermissionsPolicyWithDefaults.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "headers"));
    }
}
