using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class ChecksumRulesFixtures
{
    public static class IsLuhn
    {
        public static readonly string? Digits = "79927398713";
        public static readonly string? CardNumber = "4539148803436467";
        public static readonly string? Hyphenated = "4539-1488-0343-6467";
        public static readonly string? Spaced = "4539 1488 0343 6467";
        public static readonly string? Padded = "  79927398713  ";
        public static readonly string? AtMinLength = "18";
        public static readonly string? WrongCheckDigit = "79927398711";
        public static readonly string? CardWrongCheckDigit = "4539148803436468";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";
        public static readonly string? WhiteSpace = " ";
        public static readonly string? Letters = "abc";
        public static readonly string? MixedAlphanumeric = "4539-1488-0343-646a";
        public static readonly string? BelowMinLength = "0";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Digits), Digits, true), new(nameof(CardNumber), CardNumber, true), new(nameof(Hyphenated), Hyphenated, true), new(nameof(Spaced), Spaced, true), new(nameof(Padded), Padded, true)];
        public static RuleScenario<string?>[] ValidEdgeScenarios => [new(nameof(AtMinLength), AtMinLength, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(WrongCheckDigit), WrongCheckDigit, false), new(nameof(CardWrongCheckDigit), CardWrongCheckDigit, false), new(nameof(NullValue), NullValue, false), new(nameof(EmptyString), EmptyString, false), new(nameof(WhiteSpace), WhiteSpace, false), new(nameof(Letters), Letters, false), new(nameof(MixedAlphanumeric), MixedAlphanumeric, false)];
        public static RuleScenario<string?>[] InvalidEdgeScenarios => [new(nameof(BelowMinLength), BelowMinLength, false)];
        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
