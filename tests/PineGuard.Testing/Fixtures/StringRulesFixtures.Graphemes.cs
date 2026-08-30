using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── Graphemes ───────────────────────────────────────────────────

    public static class GraphemesHasExactCount
    {
        public const string CombiningMark = "cafe\u0301";
        public const string SurrogatePair = "\uD83D\uDE00";
        public const string ZwjFamily = "\uD83D\uDC68\u200D\uD83D\uDC69\u200D\uD83D\uDC67\u200D\uD83D\uDC66";

        public static readonly (string? value, int count) AsciiThree = ("abc", 3);
        public static readonly (string? value, int count) CarriageReturnLineFeedThree = ("a\r\nb", 3);
        public static readonly (string? value, int count) CombiningMarkFour = (CombiningMark, 4);
        public static readonly (string? value, int count) SurrogatePairOne = (SurrogatePair, 1);
        public static readonly (string? value, int count) ZwjFamilyOne = (ZwjFamily, 1);
        public static readonly (string? value, int count) EmptyZero = ("", 0);
        public static readonly (string? value, int count) AsciiTwo = ("abc", 2);
        public static readonly (string? value, int count) CombiningMarkCodeUnitCount = (CombiningMark, 5);
        public static readonly (string? value, int count) NullValue = (null, 3);
        public static readonly (string? value, int count) NegativeCount = ("abc", -1);

        public static RuleScenario<(string? value, int count)>[] ValidScenarios => [new(nameof(AsciiThree), AsciiThree, true), new(nameof(CarriageReturnLineFeedThree), CarriageReturnLineFeedThree, true), new(nameof(CombiningMarkFour), CombiningMarkFour, true)];
        public static RuleScenario<(string? value, int count)>[] ValidEdgeScenarios => [new(nameof(SurrogatePairOne), SurrogatePairOne, true), new(nameof(ZwjFamilyOne), ZwjFamilyOne, true), new(nameof(EmptyZero), EmptyZero, true)];
        public static RuleScenario<(string? value, int count)>[] InvalidScenarios => [new(nameof(AsciiTwo), AsciiTwo, false), new(nameof(CombiningMarkCodeUnitCount), CombiningMarkCodeUnitCount, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int count)>[] InvalidEdgeScenarios => [new(nameof(NegativeCount), NegativeCount, false)];
        public static RuleScenario<(string? value, int count)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int count)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int count)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class GraphemesHasMinCount
    {
        public static readonly (string? value, int min) AboveMin = ("abcd", 3);
        public static readonly (string? value, int min) ZwjFamilyOne = (GraphemesHasExactCount.ZwjFamily, 1);
        public static readonly (string? value, int min) AtMin = ("abc", 3);
        public static readonly (string? value, int min) EmptyZero = ("", 0);
        public static readonly (string? value, int min) BelowMin = ("ab", 3);
        public static readonly (string? value, int min) CombiningMarkCodeUnitCount = (GraphemesHasExactCount.CombiningMark, 5);
        public static readonly (string? value, int min) NullValue = (null, 1);
        public static readonly (string? value, int min) NegativeMin = ("abc", -1);

        public static RuleScenario<(string? value, int min)>[] ValidScenarios => [new(nameof(AboveMin), AboveMin, true), new(nameof(ZwjFamilyOne), ZwjFamilyOne, true)];
        public static RuleScenario<(string? value, int min)>[] ValidEdgeScenarios => [new(nameof(AtMin), AtMin, true), new(nameof(EmptyZero), EmptyZero, true)];
        public static RuleScenario<(string? value, int min)>[] InvalidScenarios => [new(nameof(BelowMin), BelowMin, false), new(nameof(CombiningMarkCodeUnitCount), CombiningMarkCodeUnitCount, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int min)>[] InvalidEdgeScenarios => [new(nameof(NegativeMin), NegativeMin, false)];
        public static RuleScenario<(string? value, int min)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int min)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int min)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class GraphemesHasMaxCount
    {
        public static readonly (string? value, int max) BelowMax = ("ab", 3);
        public static readonly (string? value, int max) CombiningMarkFour = (GraphemesHasExactCount.CombiningMark, 4);
        public static readonly (string? value, int max) AtMax = ("abc", 3);
        public static readonly (string? value, int max) EmptyZero = ("", 0);
        public static readonly (string? value, int max) ZwjFamilyOne = (GraphemesHasExactCount.ZwjFamily, 1);
        public static readonly (string? value, int max) AboveMax = ("abcd", 3);
        public static readonly (string? value, int max) NullValue = (null, 3);
        public static readonly (string? value, int max) NegativeMax = ("abc", -1);

        public static RuleScenario<(string? value, int max)>[] ValidScenarios => [new(nameof(BelowMax), BelowMax, true), new(nameof(CombiningMarkFour), CombiningMarkFour, true)];
        public static RuleScenario<(string? value, int max)>[] ValidEdgeScenarios => [new(nameof(AtMax), AtMax, true), new(nameof(EmptyZero), EmptyZero, true), new(nameof(ZwjFamilyOne), ZwjFamilyOne, true)];
        public static RuleScenario<(string? value, int max)>[] InvalidScenarios => [new(nameof(AboveMax), AboveMax, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int max)>[] InvalidEdgeScenarios => [new(nameof(NegativeMax), NegativeMax, false)];
        public static RuleScenario<(string? value, int max)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int max)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int max)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class GraphemesHasCountBetween
    {
        public static readonly (string? value, int min, int max, Inclusion inclusion) WithinRange = ("abc", 2, 5, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) CombiningMarkWithinRange = (GraphemesHasExactCount.CombiningMark, 3, 4, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) AtMinInclusive = ("abc", 3, 5, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) AtMaxInclusive = ("abc", 1, 3, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) WithinExclusive = ("abc", 2, 4, Inclusion.Exclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) OutsideRange = ("abcdef", 1, 3, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) NullValue = (null, 1, 3, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) AtMinExclusive = ("abc", 3, 5, Inclusion.Exclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) AtMaxExclusive = ("abc", 1, 3, Inclusion.Exclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) NegativeMin = ("abc", -1, 3, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) NegativeMax = ("abc", 0, -1, Inclusion.Inclusive);
        public static readonly (string? value, int min, int max, Inclusion inclusion) MinAboveMax = ("abc", 4, 2, Inclusion.Inclusive);

        public static RuleScenario<(string? value, int min, int max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(WithinRange), WithinRange, true), new(nameof(CombiningMarkWithinRange), CombiningMarkWithinRange, true)];
        public static RuleScenario<(string? value, int min, int max, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(AtMinInclusive), AtMinInclusive, true), new(nameof(AtMaxInclusive), AtMaxInclusive, true), new(nameof(WithinExclusive), WithinExclusive, true)];
        public static RuleScenario<(string? value, int min, int max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(OutsideRange), OutsideRange, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, int min, int max, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(AtMinExclusive), AtMinExclusive, false), new(nameof(AtMaxExclusive), AtMaxExclusive, false), new(nameof(NegativeMin), NegativeMin, false), new(nameof(NegativeMax), NegativeMax, false), new(nameof(MinAboveMax), MinAboveMax, false)];
        public static RuleScenario<(string? value, int min, int max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int min, int max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int min, int max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
