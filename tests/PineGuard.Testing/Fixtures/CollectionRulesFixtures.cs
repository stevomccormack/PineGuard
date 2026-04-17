using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class CollectionRulesFixtures
{
    private static readonly IEnumerable<string> Null = null!;
    private static readonly IEnumerable<string> Empty = [];
    private static readonly IEnumerable<string> Single = ["a"];
    private static readonly IEnumerable<string> Multiple = ["a", "b", "c"];
    private static readonly IEnumerable<string> WithDuplicate = ["a", "b", "a"];

    public static class IsEmpty
    {
        public static RuleScenario<IEnumerable<string>>[] ValidScenarios =>
        [
            new(nameof(Empty), Empty, true)
        ];

        public static RuleScenario<IEnumerable<string>>[] InvalidScenarios =>
        [
            new(nameof(Null),     Null,     false),
            new(nameof(Single),   Single,   false),
            new(nameof(Multiple), Multiple, false)
        ];

        public static RuleScenario<IEnumerable<string>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNotEmpty
    {
        public static RuleScenario<IEnumerable<string>>[] ValidScenarios =>
        [
            new(nameof(Single),   Single,   true),
            new(nameof(Multiple), Multiple, true)
        ];

        public static RuleScenario<IEnumerable<string>>[] InvalidScenarios =>
        [
            new(nameof(Null),  Null,  false),
            new(nameof(Empty), Empty, false)
        ];

        public static RuleScenario<IEnumerable<string>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasExactCount
    {
        public static readonly (IEnumerable<string>? value, int count) MultipleThree = (Multiple, 3);
        public static readonly (IEnumerable<string>? value, int count) NullThree = (Null, 3);
        public static readonly (IEnumerable<string>? value, int count) EmptyThree = (Empty, 3);
        public static readonly (IEnumerable<string>? value, int count) MultipleTwo = (Multiple, 2);
        public static readonly (IEnumerable<string>? value, int count) SingleNeg = (Single, -1);

        public static RuleScenario<(IEnumerable<string>? value, int count)>[] ValidScenarios =>
        [
            new(nameof(MultipleThree), MultipleThree, true)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int count)>[] InvalidScenarios =>
        [
            new(nameof(NullThree),  NullThree,  false),
            new(nameof(EmptyThree), EmptyThree, false),
            new(nameof(MultipleTwo), MultipleTwo, false),
            new(nameof(SingleNeg),  SingleNeg,  false)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int count)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasMinCount
    {
        public static readonly (IEnumerable<string>? value, int min) MultipleTwo = (Multiple, 2);
        public static readonly (IEnumerable<string>? value, int min) MultipleThree = (Multiple, 3);
        public static readonly (IEnumerable<string>? value, int min) NullOne = (Null, 1);
        public static readonly (IEnumerable<string>? value, int min) EmptyOne = (Empty, 1);
        public static readonly (IEnumerable<string>? value, int min) MultipleFour = (Multiple, 4);

        public static RuleScenario<(IEnumerable<string>? value, int min)>[] ValidScenarios =>
        [
            new(nameof(MultipleTwo),   MultipleTwo,   true),
            new(nameof(MultipleThree), MultipleThree, true)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int min)>[] InvalidScenarios =>
        [
            new(nameof(NullOne),    NullOne,    false),
            new(nameof(EmptyOne),   EmptyOne,   false),
            new(nameof(MultipleFour), MultipleFour, false)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int min)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasMaxCount
    {
        public static readonly (IEnumerable<string>? value, int max) MultipleThree = (Multiple, 3);
        public static readonly (IEnumerable<string>? value, int max) MultipleFive = (Multiple, 5);
        public static readonly (IEnumerable<string>? value, int max) NullThree = (Null, 3);
        public static readonly (IEnumerable<string>? value, int max) MultipleTwo = (Multiple, 2);

        public static RuleScenario<(IEnumerable<string>? value, int max)>[] ValidScenarios =>
        [
            new(nameof(MultipleThree), MultipleThree, true),
            new(nameof(MultipleFive),  MultipleFive,  true)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int max)>[] InvalidScenarios =>
        [
            new(nameof(NullThree),  NullThree,  false),
            new(nameof(MultipleTwo), MultipleTwo, false)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int max)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasCountBetween
    {
        public static readonly (IEnumerable<string>? value, int min, int max, Inclusion inclusion) MultipleTwoFourInclusive = (Multiple, 2, 4, Inclusion.Inclusive);
        public static readonly (IEnumerable<string>? value, int min, int max, Inclusion inclusion) MultipleThreeThreeInclusive = (Multiple, 3, 3, Inclusion.Inclusive);
        public static readonly (IEnumerable<string>? value, int min, int max, Inclusion inclusion) NullTwoFourInclusive = (Null, 2, 4, Inclusion.Inclusive);
        public static readonly (IEnumerable<string>? value, int min, int max, Inclusion inclusion) MultipleFourSixInclusive = (Multiple, 4, 6, Inclusion.Inclusive);

        public static RuleScenario<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MultipleTwoFourInclusive),     MultipleTwoFourInclusive,     true),
            new(nameof(MultipleThreeThreeInclusive),  MultipleThreeThreeInclusive,  true)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(NullTwoFourInclusive),    NullTwoFourInclusive,    false),
            new(nameof(MultipleFourSixInclusive), MultipleFourSixInclusive, false)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasDistinctItems
    {
        public static RuleScenario<IEnumerable<string>>[] ValidScenarios =>
        [
            new(nameof(Multiple), Multiple, true)
        ];

        public static RuleScenario<IEnumerable<string>>[] InvalidScenarios =>
        [
            new(nameof(Null),          Null,          false),
            new(nameof(WithDuplicate), WithDuplicate, false)
        ];

        public static RuleScenario<IEnumerable<string>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasDuplicateItems
    {
        public static RuleScenario<IEnumerable<string>>[] ValidScenarios =>
        [
            new(nameof(WithDuplicate), WithDuplicate, true)
        ];

        public static RuleScenario<IEnumerable<string>>[] InvalidScenarios =>
        [
            new(nameof(Null),     Null,     false),
            new(nameof(Multiple), Multiple, false)
        ];

        public static RuleScenario<IEnumerable<string>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class ContainsNullItems
    {
        private static readonly IEnumerable<string?> NullCollection = null!;
        private static readonly IEnumerable<string?> WithNull = ["a", null];
        private static readonly IEnumerable<string?> NoNull = ["a", "b"];

        public static RuleScenario<IEnumerable<string?>>[] ValidScenarios =>
        [
            new(nameof(WithNull), WithNull, true)
        ];

        public static RuleScenario<IEnumerable<string?>>[] InvalidScenarios =>
        [
            new(nameof(NullCollection), NullCollection, false),
            new(nameof(NoNull),         NoNull,         false)
        ];

        public static RuleScenario<IEnumerable<string?>>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class Contains
    {
        public static readonly (IEnumerable<string>? value, string item) MultipleA = (Multiple, "a");
        public static readonly (IEnumerable<string>? value, string item) NullA = (Null, "a");
        public static readonly (IEnumerable<string>? value, string item) EmptyA = (Empty, "a");
        public static readonly (IEnumerable<string>? value, string item) MultipleZ = (Multiple, "z");

        public static RuleScenario<(IEnumerable<string>? value, string item)>[] ValidScenarios =>
        [
            new(nameof(MultipleA), MultipleA, true)
        ];

        public static RuleScenario<(IEnumerable<string>? value, string item)>[] InvalidScenarios =>
        [
            new(nameof(NullA),    NullA,    false),
            new(nameof(EmptyA),   EmptyA,   false),
            new(nameof(MultipleZ), MultipleZ, false)
        ];

        public static RuleScenario<(IEnumerable<string>? value, string item)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSubsetOf
    {
        public static readonly (IEnumerable<string>? value, IEnumerable<string>? other) SingleMultiple = (Single, Multiple);
        public static readonly (IEnumerable<string>? value, IEnumerable<string>? other) NullMultiple = (Null, Multiple);
        public static readonly (IEnumerable<string>? value, IEnumerable<string>? other) MultipleNull = (Multiple, null!);
        public static readonly (IEnumerable<string>? value, IEnumerable<string>? other) ZMultiple = (["z"], Multiple);

        public static RuleScenario<(IEnumerable<string>? value, IEnumerable<string>? other)>[] ValidScenarios =>
        [
            new(nameof(SingleMultiple), SingleMultiple, true)
        ];

        public static RuleScenario<(IEnumerable<string>? value, IEnumerable<string>? other)>[] InvalidScenarios =>
        [
            new(nameof(NullMultiple),  NullMultiple,  false),
            new(nameof(MultipleNull),  MultipleNull,  false),
            new(nameof(ZMultiple),     ZMultiple,     false)
        ];

        public static RuleScenario<(IEnumerable<string>? value, IEnumerable<string>? other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasIndex
    {
        public static readonly (IEnumerable<string>? value, int index) MultipleZero = (Multiple, 0);
        public static readonly (IEnumerable<string>? value, int index) MultipleTwo = (Multiple, 2);
        public static readonly (IEnumerable<string>? value, int index) NullZero = (Null, 0);
        public static readonly (IEnumerable<string>? value, int index) EmptyZero = (Empty, 0);
        public static readonly (IEnumerable<string>? value, int index) MultipleThree = (Multiple, 3);
        public static readonly (IEnumerable<string>? value, int index) MultipleNeg = (Multiple, -1);

        public static RuleScenario<(IEnumerable<string>? value, int index)>[] ValidScenarios =>
        [
            new(nameof(MultipleZero), MultipleZero, true),
            new(nameof(MultipleTwo),  MultipleTwo,  true)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int index)>[] InvalidScenarios =>
        [
            new(nameof(NullZero),     NullZero,     false),
            new(nameof(EmptyZero),    EmptyZero,    false),
            new(nameof(MultipleThree), MultipleThree, false),
            new(nameof(MultipleNeg),  MultipleNeg,  false)
        ];

        public static RuleScenario<(IEnumerable<string>? value, int index)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
