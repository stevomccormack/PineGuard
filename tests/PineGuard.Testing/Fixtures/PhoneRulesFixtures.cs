using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class PhoneRulesFixtures
{
    public static class IsPhoneNumber
    {
        public static readonly string? Formatted = "+1(425)555-0123";
        public static readonly string? DigitsOnly = "4255550123";
        public static readonly string? TooShort = "123";
        public static readonly string? ExtensionSuffix = "425-555-0123x";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Formatted),  Formatted,  true),
            new(nameof(DigitsOnly), DigitsOnly, true)
        ];

        public static RuleScenario<string?>[] ValidEdgeScenarios => [];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(TooShort),        TooShort,        false),
            new(nameof(ExtensionSuffix), ExtensionSuffix, false)
        ];

        public static RuleScenario<string?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
