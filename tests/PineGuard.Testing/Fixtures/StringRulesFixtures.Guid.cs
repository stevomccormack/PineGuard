using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── Guid ────────────────────────────────────────────────────────

    public static class GuidIsGuid
    {
        public static readonly string? ValidGuid = "11111111-1111-1111-1111-111111111111";
        public static readonly string? ValidGuidN = "11111111111111111111111111111111";
        public static readonly string? NotAGuid = "not-a-guid";
        public static readonly string? NullValue = null;
        public static readonly string? Space = " ";
        public static readonly string? Empty = string.Empty;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(ValidGuid), ValidGuid, true), new(nameof(ValidGuidN), ValidGuidN, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NotAGuid), NotAGuid, false), new(nameof(NullValue), NullValue, false), new(nameof(Space), Space, false), new(nameof(Empty), Empty, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class GuidIsNotEmpty
    {
        public static readonly string? ValidGuid = "11111111-1111-1111-1111-111111111111";
        public static readonly string? EmptyGuid = Guid.Empty.ToString("D");
        public static readonly string? NotAGuid = "not-a-guid";
        public static readonly string? NullValue = null;
        public static readonly string? Space = " ";
        public static readonly string? Empty = string.Empty;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(ValidGuid), ValidGuid, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(EmptyGuid), EmptyGuid, false), new(nameof(NotAGuid), NotAGuid, false), new(nameof(NullValue), NullValue, false), new(nameof(Space), Space, false), new(nameof(Empty), Empty, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class GuidHasVersion
    {
        public static readonly (string? value, int version) Hyphenated = ("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2", 4);
        public static readonly (string? value, int version) Braced = ("{d3b07384-d113-4ec7-8479-2eaf2dc7c5a2}", 4);
        public static readonly (string? value, int version) NoHyphens = ("d3b07384d1134ec784792eaf2dc7c5a2", 4);
        public static readonly (string? value, int version) UpperCase = ("D3B07384-D113-4EC7-8479-2EAF2DC7C5A2", 4);
        public static readonly (string? value, int version) AtMinVersion = ("d9428888-122b-11e1-b85c-61cd3cbb3210", GuidRules.MinVersion);
        public static readonly (string? value, int version) AtMaxVersion = ("320c3d4d-cc00-875b-8ec9-32d5f69181c0", GuidRules.MaxVersion);
        public static readonly (string? value, int version) Mismatch = ("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2", 7);
        public static readonly (string? value, int version) NotAGuid = ("not-a-guid", 4);
        public static readonly (string? value, int version) EmptyGuid = ("00000000-0000-0000-0000-000000000000", 4);
        public static readonly (string? value, int version) NullValue = (null, 4);
        public static readonly (string? value, int version) EmptyString = ("", 4);
        public static readonly (string? value, int version) Space = (" ", 4);
        public static readonly (string? value, int version) VersionBelowMin = ("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2", GuidRules.MinVersion - 1);
        public static readonly (string? value, int version) VersionAboveMax = ("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2", GuidRules.MaxVersion + 1);

        public static RuleScenario<(string? value, int version)>[] ValidScenarios => [new(nameof(Hyphenated), Hyphenated, true), new(nameof(Braced), Braced, true), new(nameof(NoHyphens), NoHyphens, true), new(nameof(UpperCase), UpperCase, true)];
        public static RuleScenario<(string? value, int version)>[] ValidEdgeScenarios => [new(nameof(AtMinVersion), AtMinVersion, true), new(nameof(AtMaxVersion), AtMaxVersion, true)];
        public static RuleScenario<(string? value, int version)>[] InvalidScenarios => [new(nameof(Mismatch), Mismatch, false), new(nameof(NotAGuid), NotAGuid, false), new(nameof(EmptyGuid), EmptyGuid, false)];
        public static RuleScenario<(string? value, int version)>[] InvalidEdgeScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(EmptyString), EmptyString, false), new(nameof(Space), Space, false), new(nameof(VersionBelowMin), VersionBelowMin, false), new(nameof(VersionAboveMax), VersionAboveMax, false)];
        public static RuleScenario<(string? value, int version)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int version)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int version)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
