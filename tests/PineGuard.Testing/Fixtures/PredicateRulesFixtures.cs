using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class PredicateRulesFixtures
{
    public static class Satisfies
    {
        public static readonly (string? value, Func<string, bool> predicate) Matching = ("hello", x => x.Length > 3);
        public static readonly (string? value, Func<string, bool> predicate) NotMatching = ("hi", x => x.Length > 3);
        public static readonly (string? value, Func<string, bool> predicate) NullValue = (null, x => x.Length > 3);

        public static RuleScenario<(string? value, Func<string, bool> predicate)>[] ValidScenarios =>
        [
            new(nameof(Matching), Matching, true)
        ];

        public static RuleScenario<(string? value, Func<string, bool> predicate)>[] InvalidScenarios =>
        [
            new(nameof(NotMatching), NotMatching, false),
            new(nameof(NullValue),   NullValue,   false)
        ];

        public static RuleScenario<(string? value, Func<string, bool> predicate)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class NotSatisfies
    {
        public static readonly (string? value, Func<string, bool> predicate) NotMatching = ("hi", x => x.Length > 3);
        public static readonly (string? value, Func<string, bool> predicate) NullValue = (null, x => x.Length > 3);
        public static readonly (string? value, Func<string, bool> predicate) Matching = ("hello", x => x.Length > 3);

        public static RuleScenario<(string? value, Func<string, bool> predicate)>[] ValidScenarios =>
        [
            new(nameof(NotMatching), NotMatching, true),
            new(nameof(NullValue), NullValue, true)
        ];

        public static RuleScenario<(string? value, Func<string, bool> predicate)>[] InvalidScenarios =>
        [
            new(nameof(Matching), Matching, false)
        ];

        public static RuleScenario<(string? value, Func<string, bool> predicate)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
