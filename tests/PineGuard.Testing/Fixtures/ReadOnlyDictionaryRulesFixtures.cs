using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class ReadOnlyDictionaryRulesFixtures
{
    private static readonly IReadOnlyDictionary<string, int> Populated = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
    private static readonly IReadOnlyDictionary<string, int> Empty = new Dictionary<string, int>();
    private static readonly IReadOnlyDictionary<string, int>? Null = null;

    public static class IsEmpty
    {
        public static readonly IReadOnlyDictionary<string, int>? NullValue = Null;
        public static readonly IReadOnlyDictionary<string, int> EmptyValue = Empty;
        public static readonly IReadOnlyDictionary<string, int> PopulatedValue = Populated;

        public static RuleScenario<IReadOnlyDictionary<string, int>?>[] ValidScenarios =>
        [
            new(nameof(EmptyValue), EmptyValue, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, int>?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(PopulatedValue), PopulatedValue, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, int>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsNotEmpty
    {
        public static readonly IReadOnlyDictionary<string, int> PopulatedValue = Populated;
        public static readonly IReadOnlyDictionary<string, int>? NullValue = Null;
        public static readonly IReadOnlyDictionary<string, int> EmptyValue = Empty;

        public static RuleScenario<IReadOnlyDictionary<string, int>?>[] ValidScenarios =>
        [
            new(nameof(PopulatedValue), PopulatedValue, true)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, int>?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyValue), EmptyValue, false)
        ];

        public static RuleScenario<IReadOnlyDictionary<string, int>?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasKey
    {
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key) ExistingKey = (Populated, "a");
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key) NullDictionary = (Null, "a");
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key) EmptyDictionary = (Empty, "a");
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key) MissingKey = (Populated, "missing");
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key) NullKey = (Populated, null!);

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, string key)>[] ValidScenarios =>
        [
            new(nameof(ExistingKey), ExistingKey, true)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, string key)>[] InvalidScenarios =>
        [
            new(nameof(NullDictionary), NullDictionary, false),
            new(nameof(EmptyDictionary), EmptyDictionary, false),
            new(nameof(MissingKey), MissingKey, false),
            new(nameof(NullKey), NullKey, false)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, string key)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasValue
    {
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, int value) ExistingValue = (Populated, 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, int value) NullDictionary = (Null, 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, int value) EmptyDictionary = (Empty, 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, int value) MissingValue = (Populated, 999);

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, int value)>[] ValidScenarios =>
        [
            new(nameof(ExistingValue), ExistingValue, true)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, int value)>[] InvalidScenarios =>
        [
            new(nameof(NullDictionary), NullDictionary, false),
            new(nameof(EmptyDictionary), EmptyDictionary, false),
            new(nameof(MissingValue), MissingValue, false)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, int value)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasKeyValue
    {
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key, int value) CorrectPair = (Populated, "a", 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key, int value) NullDictionary = (Null, "a", 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key, int value) EmptyDictionary = (Empty, "a", 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key, int value) WrongKey = (Populated, "missing", 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key, int value) WrongValue = (Populated, "a", 999);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, string key, int value) NullKey = (Populated, null!, 1);

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>[] ValidScenarios =>
        [
            new(nameof(CorrectPair), CorrectPair, true)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>[] InvalidScenarios =>
        [
            new(nameof(NullDictionary), NullDictionary, false),
            new(nameof(EmptyDictionary), EmptyDictionary, false),
            new(nameof(WrongKey), WrongKey, false),
            new(nameof(WrongValue), WrongValue, false),
            new(nameof(NullKey), NullKey, false)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasAnyKey
    {
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate) MatchingKey = (Populated, k => k == "a");
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate) NullDictionary = (Null, _ => true);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate) EmptyDictionary = (Empty, _ => true);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate) NoMatch = (Populated, _ => false);

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>[] ValidScenarios =>
        [
            new(nameof(MatchingKey), MatchingKey, true)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>[] InvalidScenarios =>
        [
            new(nameof(NullDictionary), NullDictionary, false),
            new(nameof(EmptyDictionary), EmptyDictionary, false),
            new(nameof(NoMatch), NoMatch, false)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasAnyValue
    {
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate) MatchingValue = (Populated, v => v == 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate) NullDictionary = (Null, _ => true);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate) EmptyDictionary = (Empty, _ => true);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate) NoMatch = (Populated, _ => false);

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>[] ValidScenarios =>
        [
            new(nameof(MatchingValue), MatchingValue, true)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>[] InvalidScenarios =>
        [
            new(nameof(NullDictionary), NullDictionary, false),
            new(nameof(EmptyDictionary), EmptyDictionary, false),
            new(nameof(NoMatch), NoMatch, false)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasAnyItem
    {
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate) MatchingItem = (Populated, (k, v) => k == "a" && v == 1);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate) NullDictionary = (Null, (_, _) => true);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate) EmptyDictionary = (Empty, (_, _) => true);
        public static readonly (IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate) NoMatch = (Populated, (_, _) => false);

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>[] ValidScenarios =>
        [
            new(nameof(MatchingItem), MatchingItem, true)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>[] InvalidScenarios =>
        [
            new(nameof(NullDictionary), NullDictionary, false),
            new(nameof(EmptyDictionary), EmptyDictionary, false),
            new(nameof(NoMatch), NoMatch, false)
        ];

        public static RuleScenario<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
