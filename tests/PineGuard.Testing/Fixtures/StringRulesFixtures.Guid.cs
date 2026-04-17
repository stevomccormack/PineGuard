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
}
