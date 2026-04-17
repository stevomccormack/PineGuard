using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class BoolRulesFixtures
{
    public static class IsTrue
    {
        public static readonly bool? True = true;
        public static readonly bool? Null = null;
        public static readonly bool? False = false;

        public static RuleScenario<bool?>[] ValidScenarios =>
        [
            new(nameof(True), True, true)
        ];

        public static RuleScenario<bool?>[] InvalidScenarios =>
        [
            new(nameof(Null),  Null,  false),
            new(nameof(False), False, false)
        ];

        public static RuleScenario<bool?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFalse
    {
        public static readonly bool? False = false;
        public static readonly bool? Null = null;
        public static readonly bool? True = true;

        public static RuleScenario<bool?>[] ValidScenarios =>
        [
            new(nameof(False), False, true)
        ];

        public static RuleScenario<bool?>[] InvalidScenarios =>
        [
            new(nameof(Null), Null,  false),
            new(nameof(True), True,  false)
        ];

        public static RuleScenario<bool?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class TrueRule
    {
        public static readonly bool TrueValue = true;
        public static readonly bool FalseValue = false;

        public static RuleScenario<bool>[] ValidScenarios =>
        [
            new(nameof(TrueValue), TrueValue, true)
        ];

        public static RuleScenario<bool>[] InvalidScenarios =>
        [
            new(nameof(FalseValue), FalseValue, false)
        ];

        public static RuleScenario<bool>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class FalseRule
    {
        public static readonly bool FalseValue = false;
        public static readonly bool TrueValue = true;

        public static RuleScenario<bool>[] ValidScenarios =>
        [
            new(nameof(FalseValue), FalseValue, true)
        ];

        public static RuleScenario<bool>[] InvalidScenarios =>
        [
            new(nameof(TrueValue), TrueValue, false)
        ];

        public static RuleScenario<bool>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
