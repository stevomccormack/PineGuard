using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.HttpSecurityHeaderRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class HttpSecurityHeaderRulesTestData
{
    public static class HasContentSecurityPolicyHeader
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasContentSecurityPolicyHeader.AllScenarios.ToRuleCases();
    }

    public static class HasStrictTransportSecurityHeader
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasStrictTransportSecurityHeader.AllScenarios.ToRuleCases();
    }

    public static class HasXFrameOptionsHeader
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasXFrameOptionsHeader.AllScenarios.ToRuleCases();
    }

    public static class HasReferrerPolicyHeader
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasReferrerPolicyHeader.AllScenarios.ToRuleCases();
    }

    public static class HasPermissionsPolicyHeader
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasPermissionsPolicyHeader.AllScenarios.ToRuleCases();
    }

    public static class HasXContentTypeOptionsHeader
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasXContentTypeOptionsHeader.AllScenarios.ToRuleCases();
    }

    public static class HasXContentTypeOptionsWithDefaults
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasXContentTypeOptionsWithDefaults.AllScenarios.ToRuleCases();
    }

    public static class HasXFrameOptionsWithDefaults
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasXFrameOptionsWithDefaults.AllScenarios.ToRuleCases();
    }

    public static class HasReferrerPolicyWithDefaults
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasReferrerPolicyWithDefaults.AllScenarios.ToRuleCases();
    }

    public static class HasPermissionsPolicyWithDefaults
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasPermissionsPolicyWithDefaults.AllScenarios.ToRuleCases();
    }

    public static class HasStrictTransportSecurityWithDefaults
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasStrictTransportSecurityWithDefaults.AllScenarios.ToRuleCases();
    }

    public static class HasContentSecurityPolicyWithDefaults
    {
        public static TheoryData<RuleCase<IReadOnlyDictionary<string, IEnumerable<string>>?>> Cases => F.HasContentSecurityPolicyWithDefaults.AllScenarios.ToRuleCases();
    }

    public static class HasXContentTypeOptions
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasXContentTypeOptions.MatchingValue), F.HasXContentTypeOptions.MatchingValue, new RuleExpected(true)),
            new(nameof(F.HasXContentTypeOptions.NonMatchingValue), F.HasXContentTypeOptions.NonMatchingValue, new RuleExpected(false))
        ];
    }

    public static class HasXFrameOptions
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasXFrameOptions.MatchingValue), F.HasXFrameOptions.MatchingValue, new RuleExpected(true)),
            new(nameof(F.HasXFrameOptions.NonMatchingValue), F.HasXFrameOptions.NonMatchingValue, new RuleExpected(false))
        ];
    }

    public static class HasReferrerPolicy
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasReferrerPolicy.MatchingValue), F.HasReferrerPolicy.MatchingValue, new RuleExpected(true)),
            new(nameof(F.HasReferrerPolicy.NonMatchingValue), F.HasReferrerPolicy.NonMatchingValue, new RuleExpected(false))
        ];
    }

    public static class HasPermissionsPolicy
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? expectedValue)>> Cases =>
        [
            new(nameof(F.HasPermissionsPolicy.MatchingValue), F.HasPermissionsPolicy.MatchingValue, new RuleExpected(true)),
            new(nameof(F.HasPermissionsPolicy.NonMatchingValue), F.HasPermissionsPolicy.NonMatchingValue, new RuleExpected(false))
        ];
    }

    public static class HasStrictTransportSecurity
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, int minMaxAgeSeconds, bool requireIncludeSubDomains, bool requirePreload)>> Cases =>
        [
            new(nameof(F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired), F.HasStrictTransportSecurity.IncludeSubDomainsNotRequired, new RuleExpected(true)),
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadFalse), F.HasStrictTransportSecurity.RequiresPreloadFalse, new RuleExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.RequiresPreloadTrue), F.HasStrictTransportSecurity.RequiresPreloadTrue, new RuleExpected(true)),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeLessThanMin), F.HasStrictTransportSecurity.MaxAgeLessThanMin, new RuleExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeNonNumeric), F.HasStrictTransportSecurity.MaxAgeNonNumeric, new RuleExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MinMaxAgeZero), F.HasStrictTransportSecurity.MinMaxAgeZero, new RuleExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MinMaxAgeNegative), F.HasStrictTransportSecurity.MinMaxAgeNegative, new RuleExpected(false)),
            new(nameof(F.HasStrictTransportSecurity.MaxAgeNegativeValue), F.HasStrictTransportSecurity.MaxAgeNegativeValue, new RuleExpected(false))
        ];
    }

    public static class HasContentSecurityPolicy
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredDefaultSrcValue, string? requiredObjectSrcValue, string? requiredBaseUriValue, string? requiredFrameAncestorsValue)>> Cases =>
        [
            new(nameof(F.HasContentSecurityPolicy.AllRequiredWhitespace), F.HasContentSecurityPolicy.AllRequiredWhitespace, new RuleExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.AllNullRequirements), F.HasContentSecurityPolicy.AllNullRequirements, new RuleExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.OverrideDefaultSrc), F.HasContentSecurityPolicy.OverrideDefaultSrc, new RuleExpected(true)),
            new(nameof(F.HasContentSecurityPolicy.SubstringValueDoesNotMatch), F.HasContentSecurityPolicy.SubstringValueDoesNotMatch, new RuleExpected(false))
        ];
    }

    public static class HasPermissionsPolicyContaining
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? requiredFragments)>> Cases =>
        [
            new(nameof(F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace), F.HasPermissionsPolicyContaining.FragmentFoundIgnoresWhitespace, new RuleExpected(true)),
            new(nameof(F.HasPermissionsPolicyContaining.HeaderMissing), F.HasPermissionsPolicyContaining.HeaderMissing, new RuleExpected(false)),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentIsWhitespace), F.HasPermissionsPolicyContaining.FragmentIsWhitespace, new RuleExpected(false)),
            new(nameof(F.HasPermissionsPolicyContaining.FragmentMissing), F.HasPermissionsPolicyContaining.FragmentMissing, new RuleExpected(false)),
            new(nameof(F.HasPermissionsPolicyContaining.NullHeaders), F.HasPermissionsPolicyContaining.NullHeaders, new RuleExpected(false)),
            new(nameof(F.HasPermissionsPolicyContaining.NullFragments), F.HasPermissionsPolicyContaining.NullFragments, new RuleExpected(false))
        ];
    }
}
