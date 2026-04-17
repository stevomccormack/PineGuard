using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.DictionaryRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDictionaryClausesTestData
{
    private static readonly IDictionary<string, int> Populated = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

    public static class Empty
    {
        public static TheoryData<MustCase<IDictionary<string, int>?>> ValidCases => F.IsEmpty.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IDictionary<string, int>?>> InvalidCases => F.IsEmpty.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must be empty."));
    }

    public static class NotEmpty
    {
        public static TheoryData<MustCase<IDictionary<string, int>?>> ValidCases => F.IsNotEmpty.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<IDictionary<string, int>?>> InvalidCases => F.IsNotEmpty.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must not be empty and have items."));
    }

    public static class HasKey
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key)>> ValidCases => F.HasKey.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key)>> InvalidCases => F.HasKey.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must contain the specified key."));
    }

    public static class NotHasKey
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key)>> ValidCases => F.HasKey.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key)>> InvalidCases => F.HasKey.ValidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must not contain the specified key."));
    }

    public static class HasValue
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, int value)>> ValidCases => F.HasValue.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, int value)>> InvalidCases => F.HasValue.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must contain the specified value."));
    }

    public static class NotHasValue
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, int value)>> ValidCases => F.HasValue.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, int value)>> InvalidCases => F.HasValue.ValidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must not contain the specified value."));
    }

    public static class HasKeyValue
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key, int value)>> ValidCases => F.HasKeyValue.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key, int value)>> InvalidCases => F.HasKeyValue.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must contain the specified key/value pair."));
    }

    public static class NotHasKeyValue
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key, int value)>> ValidCases => F.HasKeyValue.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, string key, int value)>> InvalidCases => F.HasKeyValue.ValidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must not contain the specified key/value pair."));
    }

    public static class HasAnyKey
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>> ValidCases => F.HasAnyKey.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>> InvalidCases => F.HasAnyKey.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must contain a key that matches the predicate."));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>> NullPredicateCases => [new("Null predicate", (Populated, null!), new MustExpected(false, "predicate must not be null.", "predicate"))];
    }

    public static class NotHasAnyKey
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>> ValidCases => F.HasAnyKey.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>> InvalidCases => F.HasAnyKey.ValidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must not contain a key that matches the predicate."));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>> NullPredicateCases => [new("Null predicate", (Populated, null!), new MustExpected(false, "predicate must not be null.", "predicate"))];
    }

    public static class HasAnyValue
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>> ValidCases => F.HasAnyValue.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>> InvalidCases => F.HasAnyValue.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must contain a value that matches the predicate."));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>> NullPredicateCases => [new("Null predicate", (Populated, null!), new MustExpected(false, "predicate must not be null.", "predicate"))];
    }

    public static class NotHasAnyValue
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>> ValidCases => F.HasAnyValue.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>> InvalidCases => F.HasAnyValue.ValidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must not contain a value that matches the predicate."));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>> NullPredicateCases => [new("Null predicate", (Populated, null!), new MustExpected(false, "predicate must not be null.", "predicate"))];
    }

    public static class HasAnyItem
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> ValidCases => F.HasAnyItem.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> InvalidCases => F.HasAnyItem.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must contain an item that matches the predicate."));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> NullPredicateCases => [new("Null predicate", (Populated, null!), new MustExpected(false, "predicate must not be null.", "predicate"))];
    }

    public static class NotHasAnyItem
    {
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> ValidCases => F.HasAnyItem.InvalidScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> InvalidCases => F.HasAnyItem.ValidScenarios.ToMustCases(_ => new MustExpected(false, "dictionary must not contain an item that matches the predicate."));
        public static TheoryData<MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> NullPredicateCases => [new("Null predicate", (Populated, null!), new MustExpected(false, "predicate must not be null.", "predicate"))];
    }
}
