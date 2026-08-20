using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class HttpSecurityHeaderRulesFixtures
{
    private static Dictionary<string, IEnumerable<string>> BuildHeaders(string name, string value) =>
        new(StringComparer.OrdinalIgnoreCase) { [name] = [value] };

    private static Dictionary<string, IEnumerable<string>> BuildHeaders(string name, string[] values) =>
        new(StringComparer.OrdinalIgnoreCase) { [name] = values };

    public static readonly IReadOnlyDictionary<string, IEnumerable<string>> EmptyHeaders =
        new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

    public static class HasContentSecurityPolicyHeader
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Present =
            BuildHeaders("Content-Security-Policy", "default-src 'self'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(Present), Present, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(EmptyHeaders), EmptyHeaders, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasStrictTransportSecurityHeader
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Present =
            BuildHeaders("Strict-Transport-Security", "max-age=31536000");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Missing =
            BuildHeaders("Content-Security-Policy", "default-src 'self'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(Present), Present, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(Missing), Missing, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasXFrameOptionsHeader
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Present =
            BuildHeaders("X-Frame-Options", "DENY");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Missing =
            BuildHeaders("Content-Security-Policy", "default-src 'self'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(Present), Present, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(Missing), Missing, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasReferrerPolicyHeader
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Present =
            BuildHeaders("Referrer-Policy", "strict-origin-when-cross-origin");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Missing =
            BuildHeaders("Content-Security-Policy", "default-src 'self'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(Present), Present, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(Missing), Missing, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasPermissionsPolicyHeader
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Present =
            BuildHeaders("Permissions-Policy", "geolocation=()");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Missing =
            BuildHeaders("Content-Security-Policy", "default-src 'self'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(Present), Present, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(Missing), Missing, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasXContentTypeOptionsHeader
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Present =
            BuildHeaders("X-Content-Type-Options", "nosniff");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Missing =
            BuildHeaders("Content-Security-Policy", "default-src 'self'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(Present), Present, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(Missing), Missing, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasXContentTypeOptions
    {
        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) MatchingValue =
            (BuildHeaders("X-Content-Type-Options", "nosniff"), "nosniff");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) NonMatchingValue =
            (BuildHeaders("X-Content-Type-Options", "nosniff"), "nope");
    }

    public static class HasXFrameOptions
    {
        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) MatchingValue =
            (BuildHeaders("X-Frame-Options", "DENY"), "DENY");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) NonMatchingValue =
            (BuildHeaders("X-Frame-Options", "DENY"), "SAMEORIGIN");
    }

    public static class HasReferrerPolicy
    {
        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) MatchingValue =
            (BuildHeaders("Referrer-Policy", "strict-origin-when-cross-origin"), "strict-origin-when-cross-origin");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) NonMatchingValue =
            (BuildHeaders("Referrer-Policy", "strict-origin-when-cross-origin"), "no-referrer");
    }

    public static class HasPermissionsPolicy
    {
        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) MatchingValue =
            (BuildHeaders("Permissions-Policy", "geolocation=(), microphone=(), camera=()"), "geolocation=(), microphone=(), camera=()");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue) NonMatchingValue =
            (BuildHeaders("Permissions-Policy", "geolocation=(), microphone=(), camera=()"), "geolocation=()");
    }

    public static class HasStrictTransportSecurity
    {
        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) IncludeSubDomainsNotRequired =
            (BuildHeaders("Strict-Transport-Security", "max-age=31536000"), 1, false, false);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) RequiresPreloadFalse =
            (BuildHeaders("Strict-Transport-Security", "max-age=31536000; includeSubDomains"), 31_536_000, true, true);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) RequiresPreloadTrue =
            (BuildHeaders("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload"), 31_536_000, true, true);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) MaxAgeLessThanMin =
            (BuildHeaders("Strict-Transport-Security", "max-age=100; includeSubDomains"), 200, true, false);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) MaxAgeNonNumeric =
            (BuildHeaders("Strict-Transport-Security", "max-age=abc; includeSubDomains"), 1, true, false);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) MinMaxAgeZero =
            (BuildHeaders("Strict-Transport-Security", "max-age=31536000; includeSubDomains"), 0, true, false);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) MinMaxAgeNegative =
            (BuildHeaders("Strict-Transport-Security", "max-age=31536000; includeSubDomains"), -1, true, false);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload) MaxAgeNegativeValue =
            (BuildHeaders("Strict-Transport-Security", "max-age=-1; includeSubDomains"), 1, true, false);
    }

    public static class HasStrictTransportSecurityWithDefaults
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MeetsDefaults =
            BuildHeaders("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> IgnoresNullAndWhitespaceCandidates =
            BuildHeaders("Strict-Transport-Security", ["", "   ", "max-age=31536000; includeSubDomains"]);

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> IgnoresEmptySegments =
            BuildHeaders("Strict-Transport-Security", "max-age=31536000;; includeSubDomains");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> SegmentsCannotBeParsed =
            BuildHeaders("Strict-Transport-Security", ";;");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MaxAgeMissingEquals =
            BuildHeaders("Strict-Transport-Security", "max-age; includeSubDomains");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MissingIncludeSubDomains =
            BuildHeaders("Strict-Transport-Security", "max-age=31536000");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> QuotedMaxAgeValue =
            BuildHeaders("Strict-Transport-Security", "max-age=\"31536000\"; includeSubDomains");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> UnrelatedDirectivePrefixedWithMaxAge =
            BuildHeaders("Strict-Transport-Security", "max-agex=31536000; includeSubDomains");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MaxAgeLeadingPlusSign =
            BuildHeaders("Strict-Transport-Security", "max-age=+31536000; includeSubDomains");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(MeetsDefaults), MeetsDefaults, true),
            new(nameof(IgnoresNullAndWhitespaceCandidates), IgnoresNullAndWhitespaceCandidates, true),
            new(nameof(IgnoresEmptySegments), IgnoresEmptySegments, true),
            new(nameof(QuotedMaxAgeValue), QuotedMaxAgeValue, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(SegmentsCannotBeParsed), SegmentsCannotBeParsed, false),
            new(nameof(MaxAgeMissingEquals), MaxAgeMissingEquals, false),
            new(nameof(EmptyHeaders), EmptyHeaders, false),
            new(nameof(MissingIncludeSubDomains), MissingIncludeSubDomains, false),
            new(nameof(UnrelatedDirectivePrefixedWithMaxAge), UnrelatedDirectivePrefixedWithMaxAge, false),
            new(nameof(MaxAgeLeadingPlusSign), MaxAgeLeadingPlusSign, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasContentSecurityPolicy
    {
        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue) AllRequiredWhitespace =
            (BuildHeaders("Content-Security-Policy", "garbage; still; parses"), " ", " ", "\t", "\n");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue) AllNullRequirements =
            (BuildHeaders("Content-Security-Policy", "default-src 'self'"), null, null, null, null);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue) OverrideDefaultSrc =
            (BuildHeaders("Content-Security-Policy", "default-src 'self'"), "'self'", null, null, null);

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue) SubstringValueDoesNotMatch =
            (BuildHeaders("Content-Security-Policy", "default-src evilexample.com"), "example.com", null, null, null);
    }

    public static class HasContentSecurityPolicyWithDefaults
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MeetsDefaults =
            BuildHeaders("Content-Security-Policy", "default-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MultipleSegmentsMatch =
            BuildHeaders("Content-Security-Policy", ["default-src; default-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none'"]);

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> IgnoresNullAndWhitespaceCandidates =
            BuildHeaders("Content-Security-Policy", ["", "   ", "default-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none'"]);

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> DirectiveNotDelimitedByWhitespace =
            BuildHeaders("Content-Security-Policy", "default-srcX 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> DirectiveTokenBoundary =
            BuildHeaders("Content-Security-Policy", "default-srcx 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MissingBaseUri =
            BuildHeaders("Content-Security-Policy", "default-src 'self'; object-src 'none'; frame-ancestors 'none'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MissingFrameAncestors =
            BuildHeaders("Content-Security-Policy", "default-src 'self'; object-src 'none'; base-uri 'self'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> BaseUriValueMismatch =
            BuildHeaders("Content-Security-Policy", "default-src 'self'; object-src 'none'; base-uri 'none'; frame-ancestors 'none'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> SegmentsCannotBeParsed =
            BuildHeaders("Content-Security-Policy", ";;");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MissingObjectSrc =
            BuildHeaders("Content-Security-Policy", "default-src 'self'; base-uri 'self'; frame-ancestors 'none'");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(MeetsDefaults), MeetsDefaults, true),
            new(nameof(MultipleSegmentsMatch), MultipleSegmentsMatch, true),
            new(nameof(IgnoresNullAndWhitespaceCandidates), IgnoresNullAndWhitespaceCandidates, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(DirectiveNotDelimitedByWhitespace), DirectiveNotDelimitedByWhitespace, false),
            new(nameof(DirectiveTokenBoundary), DirectiveTokenBoundary, false),
            new(nameof(MissingBaseUri), MissingBaseUri, false),
            new(nameof(MissingFrameAncestors), MissingFrameAncestors, false),
            new(nameof(BaseUriValueMismatch), BaseUriValueMismatch, false),
            new(nameof(EmptyHeaders), EmptyHeaders, false),
            new(nameof(SegmentsCannotBeParsed), SegmentsCannotBeParsed, false),
            new(nameof(MissingObjectSrc), MissingObjectSrc, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasPermissionsPolicyContaining
    {
        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments) FragmentFoundIgnoresWhitespace =
            (BuildHeaders("Permissions-Policy", ["   ", "geolocation=(), microphone=(), camera=()"]), "geolocation=()");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments) HeaderMissing =
            (EmptyHeaders, "geolocation=()");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments) FragmentIsWhitespace =
            (BuildHeaders("Permissions-Policy", "geolocation=(), microphone=(), camera=()"), "   ");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments) FragmentMissing =
            (BuildHeaders("Permissions-Policy", "geolocation=(), microphone=()"), "camera=()");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments) NullHeaders =
            (null, "geolocation=()");

        public static readonly (IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments) NullFragments =
            (BuildHeaders("Permissions-Policy", "geolocation=()"), null);
    }

    public static class HasPermissionsPolicyWithDefaults
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> DisablesGeoMicCamera =
            BuildHeaders("Permissions-Policy", "geolocation=(), microphone=(), camera=()");

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(DisablesGeoMicCamera), DisablesGeoMicCamera, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(EmptyHeaders), EmptyHeaders, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasXContentTypeOptionsWithDefaults
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> NoSniff =
            BuildHeaders("X-Content-Type-Options", HttpSecurityHeaderRules.DefaultXContentTypeOptionsValue);

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(NoSniff), NoSniff, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(EmptyHeaders), EmptyHeaders, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasXFrameOptionsWithDefaults
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> Deny =
            BuildHeaders("X-Frame-Options", HttpSecurityHeaderRules.DefaultXFrameOptionsValue);

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(Deny), Deny, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(EmptyHeaders), EmptyHeaders, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasReferrerPolicyWithDefaults
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> StrictOriginWhenCrossOrigin =
            BuildHeaders("Referrer-Policy", HttpSecurityHeaderRules.DefaultReferrerPolicyValue);

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>>? Null = null;

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] ValidScenarios =>
        [
            new(nameof(StrictOriginWhenCrossOrigin), StrictOriginWhenCrossOrigin, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] InvalidScenarios =>
        [
            new(nameof(EmptyHeaders), EmptyHeaders, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, IEnumerable<string>>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
