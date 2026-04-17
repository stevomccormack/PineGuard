using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── Bool ────────────────────────────────────────────────────────

    public static class BoolIsTrue
    {
        public static readonly string? TrueValue = "true";
        public static readonly string? Trimmed = " True ";
        public static readonly string? FalseValue = "false";
        public static readonly string? NonBool = "notabool";
        public static readonly string? NullValue = null;
        public static readonly string? Whitespace = " ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(TrueValue), TrueValue, true), new(nameof(Trimmed), Trimmed, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(FalseValue), FalseValue, false), new(nameof(NonBool), NonBool, false), new(nameof(NullValue), NullValue, false), new(nameof(Whitespace), Whitespace, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class BoolIsFalse
    {
        public static readonly string? FalseValue = "false";
        public static readonly string? Trimmed = " False ";
        public static readonly string? TrueValue = "true";
        public static readonly string? NonBool = "notabool";
        public static readonly string? NullValue = null;
        public static readonly string? Whitespace = " ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(FalseValue), FalseValue, true), new(nameof(Trimmed), Trimmed, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(TrueValue), TrueValue, false), new(nameof(NonBool), NonBool, false), new(nameof(NullValue), NullValue, false), new(nameof(Whitespace), Whitespace, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
