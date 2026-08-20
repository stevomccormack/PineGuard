using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class IdentifierRulesFixtures
{
    public static class IsSlug
    {
        public static readonly string? KebabCase = "hello-world";
        public static readonly string? SingleWord = "hello";
        public static readonly string? NotKebab = "HelloWorld";
        public static readonly string? SpacesNotAllowed = "hello world";
        public static readonly string? Null = null;
        public static readonly string? Empty = "";
        public static readonly string? Whitespace = "   ";
        public static readonly string? LeadingDash = "-hello";
        public static readonly string? TrailingDash = "hello-";
        public static readonly string? DoubleDash = "hello--world";
        public static readonly string? UnicodeLetters = "crème-brûlée";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(KebabCase), KebabCase, true),
            new(nameof(SingleWord), SingleWord, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Null), Null, false),
            new(nameof(Empty), Empty, false),
            new(nameof(Whitespace), Whitespace, false),
            new(nameof(NotKebab), NotKebab, false),
            new(nameof(SpacesNotAllowed), SpacesNotAllowed, false),
            new(nameof(LeadingDash), LeadingDash, false),
            new(nameof(TrailingDash), TrailingDash, false),
            new(nameof(DoubleDash), DoubleDash, false),
            new(nameof(UnicodeLetters), UnicodeLetters, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
