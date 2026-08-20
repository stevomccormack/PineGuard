using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.ReadOnlyDictionaryRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardReadOnlyDictionaryClausesTestData
{
    public static class NotEmpty
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, int>?>> ValidCases => F.IsEmpty.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, int>?>> NullCases => F.IsEmpty.InvalidScenarios.Only(nameof(F.IsEmpty.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, int>?>> InvalidCases => F.IsEmpty.InvalidScenarios.Except(nameof(F.IsEmpty.NullValue)).ToGuardCases("value");
    }

    public static class Empty
    {
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, int>?>> ValidCases => F.IsNotEmpty.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<IReadOnlyDictionary<string, int>?>> InvalidCases => F.IsNotEmpty.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotHasKey
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key)>> ValidCases => F.HasKey.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key)>> InvalidCases => F.HasKey.InvalidScenarios.ToGuardCases(s => s.Name == nameof(F.HasKey.NullDictionary) ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HasKey
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key)>> ValidCases => F.HasKey.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key)>> InvalidCases => F.HasKey.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHasValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, int value)>> ValidCases => F.HasValue.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, int value)>> InvalidCases => F.HasValue.InvalidScenarios.ToGuardCases(s => s.Name == nameof(F.HasValue.NullDictionary) ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HasValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, int value)>> ValidCases => F.HasValue.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, int value)>> InvalidCases => F.HasValue.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHasKeyValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>> ValidCases => F.HasKeyValue.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>> InvalidCases => F.HasKeyValue.InvalidScenarios.ToGuardCases(s => s.Name == nameof(F.HasKeyValue.NullDictionary) ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HasKeyValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>> ValidCases => F.HasKeyValue.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)>> InvalidCases => F.HasKeyValue.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHasAnyKey
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>> ValidCases => F.HasAnyKey.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>> InvalidCases => F.HasAnyKey.InvalidScenarios.ToGuardCases(s => s.Name == nameof(F.HasAnyKey.NullDictionary) ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HasAnyKey
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>> ValidCases => F.HasAnyKey.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)>> InvalidCases => F.HasAnyKey.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHasAnyValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>> ValidCases => F.HasAnyValue.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>> InvalidCases => F.HasAnyValue.InvalidScenarios.ToGuardCases(s => s.Name == nameof(F.HasAnyValue.NullDictionary) ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HasAnyValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>> ValidCases => F.HasAnyValue.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)>> InvalidCases => F.HasAnyValue.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHasAnyItem
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> ValidCases => F.HasAnyItem.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> InvalidCases => F.HasAnyItem.InvalidScenarios.ToGuardCases(s => s.Name == nameof(F.HasAnyItem.NullDictionary) ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class HasAnyItem
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> ValidCases => F.HasAnyItem.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> InvalidCases => F.HasAnyItem.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
