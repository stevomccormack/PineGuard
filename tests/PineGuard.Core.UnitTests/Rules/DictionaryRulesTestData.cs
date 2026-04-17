using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DictionaryRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DictionaryRulesTestData
{
    public static class IsEmpty
    {
        public static TheoryData<RuleCase<IDictionary<string, int>?>> Cases => F.IsEmpty.AllScenarios.ToRuleCases();
    }

    public static class IsNotEmpty
    {
        public static TheoryData<RuleCase<IDictionary<string, int>?>> Cases => F.IsNotEmpty.AllScenarios.ToRuleCases();
    }

    public static class HasKey
    {
        public static TheoryData<RuleCase<(IDictionary<string, int>? dictionary, string key)>> Cases => F.HasKey.AllScenarios.ToRuleCases();
    }

    public static class HasValue
    {
        public static TheoryData<RuleCase<(IDictionary<string, int>? dictionary, int value)>> Cases => F.HasValue.AllScenarios.ToRuleCases();
    }

    public static class HasKeyValue
    {
        public static TheoryData<RuleCase<(IDictionary<string, int>? dictionary, string key, int value)>> Cases => F.HasKeyValue.AllScenarios.ToRuleCases();
    }

    public static class HasAnyKey
    {
        public static TheoryData<RuleCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>> Cases => F.HasAnyKey.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null predicate", (new Dictionary<string, int>(), null!), new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record InvalidCase(string Name, (IDictionary<string, int> Dictionary, Func<string, bool>? Predicate) Value, ExpectedException ExpectedException)
            : ThrowsCase<(IDictionary<string, int> Dictionary, Func<string, bool>? Predicate)>(Name, Value, ExpectedException);
    }

    public static class HasAnyValue
    {
        public static TheoryData<RuleCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>> Cases => F.HasAnyValue.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null predicate", (new Dictionary<string, int>(), null!), new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record InvalidCase(string Name, (IDictionary<string, int> Dictionary, Func<int, bool>? Predicate) Value, ExpectedException ExpectedException)
            : ThrowsCase<(IDictionary<string, int> Dictionary, Func<int, bool>? Predicate)>(Name, Value, ExpectedException);
    }

    public static class HasAnyItem
    {
        public static TheoryData<RuleCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>> Cases => F.HasAnyItem.AllScenarios.ToRuleCases();

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null predicate", (new Dictionary<string, int>(), null!), new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record InvalidCase(string Name, (IDictionary<string, int> Dictionary, Func<string, int, bool>? Predicate) Value, ExpectedException ExpectedException)
            : ThrowsCase<(IDictionary<string, int> Dictionary, Func<string, int, bool>? Predicate)>(Name, Value, ExpectedException);
    }
}
