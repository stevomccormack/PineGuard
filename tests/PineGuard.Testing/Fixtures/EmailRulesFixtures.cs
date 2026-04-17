using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class EmailRulesFixtures
{
    public static class IsEmail
    {
        public static readonly string? Standard = "user@example.com";
        public static readonly string? DisplayNameForm = "User <user@example.com>";
        public static readonly string? NotAnEmail = "not-an-email";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Standard),         Standard,         true),
            new(nameof(DisplayNameForm),  DisplayNameForm,  true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NotAnEmail), NotAnEmail, false),
            new(nameof(Null),       Null,       false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsStrictEmail
    {
        public static readonly string? Standard = "user@example.com";
        public static readonly string? DisplayNameForm = "User <user@example.com>";
        public static readonly string? SpaceInLocal = "user example@example.com";
        public static readonly string? Localhost = "user@localhost";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Standard), Standard, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(DisplayNameForm), DisplayNameForm, false),
            new(nameof(SpaceInLocal),    SpaceInLocal,    false),
            new(nameof(Localhost),       Localhost,       false),
            new(nameof(Null),            Null,            false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasAlias
    {
        public static readonly string? WithAlias = "user+alias@example.com";
        public static readonly string? WithoutAlias = "user@example.com";
        public static readonly string? DisplayNameForm = "User <user+alias@example.com>";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(WithAlias), WithAlias, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(WithoutAlias),   WithoutAlias,   false),
            new(nameof(DisplayNameForm), DisplayNameForm, false),
            new(nameof(Null),            Null,            false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
