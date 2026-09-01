using PineGuard.Rules;
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

    public static class IsUlid
    {
        public static readonly string? Canonical = "01ARZ3NDEKTSV4RRFFQ69G5FAV";
        public static readonly string? Lowercase = "01arz3ndektsv4rrffq69g5fav";
        public static readonly string? MixedCase = "01ARz3ndEKTSV4RRFFQ69G5FAV";
        public static readonly string? Padded = $"  {Canonical}  ";
        public static readonly string? AllZeros = new('0', IdentifierRules.UlidLength);
        public static readonly string? AtMaxFirstChar = $"{IdentifierRules.MaxUlidFirstChar}{Canonical![1..]}";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? WhiteSpace = "   ";
        public static readonly string? ExcludedLetterI = $"{Canonical[..^1]}I";
        public static readonly string? ExcludedLetterL = $"{Canonical[..^1]}L";
        public static readonly string? ExcludedLetterO = $"{Canonical[..^1]}O";
        public static readonly string? ExcludedLetterU = $"{Canonical[..^1]}U";
        public static readonly string? LowercaseExcludedLetter = $"{Canonical[..^1]}i";
        public static readonly string? Hyphenated = $"{Canonical[..^1]}-";
        public static readonly string? TooShort = Canonical[..(IdentifierRules.UlidLength - 1)];
        public static readonly string? TooLong = $"{Canonical}Z";
        public static readonly string? AboveMaxFirstChar = $"{(char)(IdentifierRules.MaxUlidFirstChar + 1)}{Canonical[1..]}";
        public static readonly string? BelowMinFirstChar = $"-{Canonical[1..]}";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Canonical), Canonical, true), new(nameof(Lowercase), Lowercase, true), new(nameof(MixedCase), MixedCase, true), new(nameof(Padded), Padded, true)];
        public static RuleScenario<string?>[] ValidEdgeScenarios => [new(nameof(AllZeros), AllZeros, true), new(nameof(AtMaxFirstChar), AtMaxFirstChar, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(EmptyString), EmptyString, false), new(nameof(WhiteSpace), WhiteSpace, false), new(nameof(ExcludedLetterI), ExcludedLetterI, false), new(nameof(ExcludedLetterL), ExcludedLetterL, false), new(nameof(ExcludedLetterO), ExcludedLetterO, false), new(nameof(ExcludedLetterU), ExcludedLetterU, false), new(nameof(LowercaseExcludedLetter), LowercaseExcludedLetter, false), new(nameof(Hyphenated), Hyphenated, false)];
        public static RuleScenario<string?>[] InvalidEdgeScenarios => [new(nameof(TooShort), TooShort, false), new(nameof(TooLong), TooLong, false), new(nameof(AboveMaxFirstChar), AboveMaxFirstChar, false), new(nameof(BelowMinFirstChar), BelowMinFirstChar, false)];
        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
