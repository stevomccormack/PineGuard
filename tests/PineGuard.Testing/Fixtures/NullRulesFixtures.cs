using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class NullRulesFixtures
{
    public static class IsNull
    {
        public static readonly object? Null = null;
        public static readonly object? NonNull = new();

        public static RuleScenario<object?>[] ValidScenarios => [new(nameof(Null), Null, true)];
        public static RuleScenario<object?>[] InvalidScenarios => [new(nameof(NonNull), NonNull, false)];
        public static RuleScenario<object?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNotNull
    {
        public static readonly object? NonNull = new();
        public static readonly object? Null = null;

        public static RuleScenario<object?>[] ValidScenarios => [new(nameof(NonNull), NonNull, true)];
        public static RuleScenario<object?>[] InvalidScenarios => [new(nameof(Null), Null, false)];
        public static RuleScenario<object?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
