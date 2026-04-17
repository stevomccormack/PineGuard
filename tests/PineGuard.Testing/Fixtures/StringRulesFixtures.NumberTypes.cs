using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── NumberTypes ──────────────────────────────────────────────────

    public static class IsDecimal
    {
        public static readonly string? Valid = "1.23";
        public static readonly string? Negative = "-1.2";
        public static readonly string? NullValue = null;
        public static readonly string? Space = " ";
        public static readonly string? NotNumeric = "not";
        public static readonly string? TooManyDecimals = "1.234";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Valid), Valid, true), new(nameof(Negative), Negative, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Space), Space, false), new(nameof(NotNumeric), NotNumeric, false), new(nameof(TooManyDecimals), TooManyDecimals, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsExactDecimal
    {
        public static readonly string? Valid = "1.20";
        public static readonly string? NullValue = null;
        public static readonly string? Space = " ";
        public static readonly string? NotNumeric = "not";
        public static readonly string? NotEnoughDecimals = "1.2";
        public static readonly string? TooManyDecimals = "1.230";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Valid), Valid, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Space), Space, false), new(nameof(NotNumeric), NotNumeric, false), new(nameof(NotEnoughDecimals), NotEnoughDecimals, false), new(nameof(TooManyDecimals), TooManyDecimals, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsInt32
    {
        public static readonly string? MaxValue = "2147483647";
        public static readonly string? MaxValuePlus1 = "2147483648";
        public static readonly string? NullValue = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(MaxValue), MaxValue, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(MaxValuePlus1), MaxValuePlus1, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsInt64
    {
        public static readonly string? MaxValue = "9223372036854775807";
        public static readonly string? MaxValuePlus1 = "9223372036854775808";
        public static readonly string? NullValue = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(MaxValue), MaxValue, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(MaxValuePlus1), MaxValuePlus1, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsInt32InRange
    {
        public static readonly (string text, int min, int max, Inclusion inclusion) BetweenInclusive = ("5", 1, 10, Inclusion.Inclusive);
        public static readonly (string text, int min, int max, Inclusion inclusion) AtMinInclusive = ("1", 1, 10, Inclusion.Inclusive);
        public static readonly (string text, int min, int max, Inclusion inclusion) AtMinExclusive = ("1", 1, 10, Inclusion.Exclusive);
        public static readonly (string text, int min, int max, Inclusion inclusion) NotNumeric = ("not", 1, 10, Inclusion.Inclusive);

        public static RuleScenario<(string text, int min, int max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(BetweenInclusive), BetweenInclusive, true)];
        public static RuleScenario<(string text, int min, int max, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(AtMinInclusive), AtMinInclusive, true)];
        public static RuleScenario<(string text, int min, int max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NotNumeric), NotNumeric, false)];
        public static RuleScenario<(string text, int min, int max, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(AtMinExclusive), AtMinExclusive, false)];
        public static RuleScenario<(string text, int min, int max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string text, int min, int max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string text, int min, int max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsInt64InRange
    {
        public static readonly (string text, long min, long max, Inclusion inclusion) BetweenInclusive = ("5", 1L, 10L, Inclusion.Inclusive);
        public static readonly (string text, long min, long max, Inclusion inclusion) AtMinInclusive = ("1", 1L, 10L, Inclusion.Inclusive);
        public static readonly (string text, long min, long max, Inclusion inclusion) AtMinExclusive = ("1", 1L, 10L, Inclusion.Exclusive);
        public static readonly (string text, long min, long max, Inclusion inclusion) NotNumeric = ("not", 1L, 10L, Inclusion.Inclusive);

        public static RuleScenario<(string text, long min, long max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(BetweenInclusive), BetweenInclusive, true)];
        public static RuleScenario<(string text, long min, long max, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(AtMinInclusive), AtMinInclusive, true)];
        public static RuleScenario<(string text, long min, long max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NotNumeric), NotNumeric, false)];
        public static RuleScenario<(string text, long min, long max, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(AtMinExclusive), AtMinExclusive, false)];
        public static RuleScenario<(string text, long min, long max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string text, long min, long max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string text, long min, long max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
