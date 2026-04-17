using System.Collections;
using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CollectionRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class CollectionRulesTestData
{
    private static IEnumerable<T> Enumerate<T>(params T[] items)
    {
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var t in items)
            yield return t;
    }

    private sealed class ReadOnlyCollectionOnly<T>(params T[] items) : IReadOnlyCollection<T>
    {
        public int Count => items.Length;
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }

    public static class IsEmpty
    {
        public static TheoryData<RuleCase<IEnumerable<string>>> Cases => F.IsEmpty.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<IEnumerable<string>?>> AdHocCases =>
        [
            new("Empty non-count", Enumerate<string>(), new RuleExpected(true)),
            new("Empty readonly", new ReadOnlyCollectionOnly<string>(), new RuleExpected(true)),
            new("Non-empty count", ["a", "b"], new RuleExpected(false)),
            new("Non-empty non-count", Enumerate("a", "b"), new RuleExpected(false)),
            new("Non-empty readonly", new ReadOnlyCollectionOnly<string>("a"), new RuleExpected(false))
        ];
    }

    public static class IsNotEmpty
    {
        public static TheoryData<RuleCase<IEnumerable<string>>> Cases => F.IsNotEmpty.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<IEnumerable<string>?>> AdHocCases =>
        [
            new("Non-empty non-count", Enumerate("a"), new RuleExpected(true)),
            new("Non-empty readonly", new ReadOnlyCollectionOnly<string>("a"), new RuleExpected(true)),
            new("Empty non-count", Enumerate<string>(), new RuleExpected(false)),
            new("Empty readonly", new ReadOnlyCollectionOnly<string>(), new RuleExpected(false))
        ];
    }

    public static class HasExactCount
    {
        public static TheoryData<RuleCase<(IEnumerable<string>? value, int count)>> Cases => F.HasExactCount.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, int Count)>> AdHocCases =>
        [
            new("Empty count zero", ([], 0), new RuleExpected(true)),
            new("Non-count three", (Enumerate("a", "b", "c"), 3), new RuleExpected(true)),
            new("Non-count wrong", (Enumerate("a", "b", "c"), 2), new RuleExpected(false)),
            new("ReadOnly three", (new ReadOnlyCollectionOnly<string>("a", "b", "c"), 3), new RuleExpected(true))
        ];
    }

    public static class HasMinCount
    {
        public static TheoryData<RuleCase<(IEnumerable<string>? value, int min)>> Cases => F.HasMinCount.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, int Min)>> AdHocCases =>
        [
            new("Empty zero", ([], 0), new RuleExpected(true)),
            new("Non-count two", (Enumerate("a", "b"), 2), new RuleExpected(true)),
            new("Non-count min zero", (Enumerate("a"), 0), new RuleExpected(true)),
            new("Non-count exceeds", (Enumerate("a"), 2), new RuleExpected(false)),
            new("ReadOnly two", (new ReadOnlyCollectionOnly<string>("a", "b"), 2), new RuleExpected(true)),
            new("NegativeMin", (["a"], -1), new RuleExpected(false))
        ];
    }

    public static class HasMaxCount
    {
        public static TheoryData<RuleCase<(IEnumerable<string>? value, int max)>> Cases => F.HasMaxCount.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, int Max)>> AdHocCases =>
        [
            new("Empty zero", ([], 0), new RuleExpected(true)),
            new("Non-count under", (Enumerate("a", "b"), 3), new RuleExpected(true)),
            new("Non-count over", (Enumerate("a", "b", "c", "d"), 3), new RuleExpected(false)),
            new("ReadOnly three", (new ReadOnlyCollectionOnly<string>("a", "b", "c"), 3), new RuleExpected(true)),
            new("NegativeMax", (["a"], -1), new RuleExpected(false))
        ];
    }

    public static class HasCountBetween
    {
        public static TheoryData<RuleCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)>> Cases => F.HasCountBetween.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, int Min, int Max, Inclusion Inclusion)>> AdHocCases =>
        [
            new("ReadOnly in range", (new ReadOnlyCollectionOnly<string>("a", "b", "c"), 2, 4, Inclusion.Inclusive), new RuleExpected(true)),
            new("Non-count in range", (Enumerate("a", "b", "c"), 2, 4, Inclusion.Inclusive), new RuleExpected(true)),
            new("Non-count exclusive ok", (Enumerate("a", "b", "c"), 2, 5, Inclusion.Exclusive), new RuleExpected(true)),
            new("Exclusive equal bounds false", (["a", "b", "c"], 3, 3, Inclusion.Exclusive), new RuleExpected(false)),
            new("NegativeMin", (["a"], -1, 3, Inclusion.Inclusive), new RuleExpected(false)),
            new("MinGtMax", (["a"], 4, 2, Inclusion.Inclusive), new RuleExpected(false)),
            new("ExclusiveUpperZero", (Enumerate("a"), 0, 0, Inclusion.Exclusive), new RuleExpected(false)),
            new("Non-count exceeds max", (Enumerate("a", "b", "c", "d", "e"), 1, 3, Inclusion.Inclusive), new RuleExpected(false))
        ];
    }

    public static class HasAny
    {
        public static readonly Func<string, bool> IsA = x => x == "a";

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, Func<string, bool> Predicate)>> Cases =>
        [
            new("Multiple has a", (["a", "b"], IsA), new RuleExpected(true)),
            new("Null returns false", (null, IsA), new RuleExpected(false)),
            new("Empty returns false", ([], IsA), new RuleExpected(false)),
            new("No match returns false", (["b", "c"], IsA), new RuleExpected(false))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null predicate", (["a"], null!), new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record InvalidCase(string Name, (IEnumerable<string>? Value, Func<string, bool>? Predicate) Value, ExpectedException ExpectedException)
            : ThrowsCase<(IEnumerable<string>? Value, Func<string, bool>? Predicate)>(Name, Value, ExpectedException);
    }

    public static class HasAll
    {
        public static readonly Func<string, bool> IsA = x => x == "a";

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, Func<string, bool> Predicate)>> Cases =>
        [
            new("All a returns true", (["a", "a"], IsA), new RuleExpected(true)),
            new("Empty returns true", ([], _ => false), new RuleExpected(true)),
            new("Null returns false", (null, IsA), new RuleExpected(false)),
            new("Partial match returns false", (["a", "b"], IsA), new RuleExpected(false))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null predicate", (["a"], null!), new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record InvalidCase(string Name, (IEnumerable<string>? Value, Func<string, bool>? Predicate) Value, ExpectedException ExpectedException)
            : ThrowsCase<(IEnumerable<string>? Value, Func<string, bool>? Predicate)>(Name, Value, ExpectedException);
    }

    public static class HasDistinctItems
    {
        public static TheoryData<RuleCase<IEnumerable<string>>> Cases => F.HasDistinctItems.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, IEqualityComparer<string>? Comparer)>> ComparerCases =>
        [
            new("Ordinal distinct", (["a", "A"], StringComparer.Ordinal), new RuleExpected(true)),
            new("OrdinalIgnoreCase duplicate", (["a", "A"], StringComparer.OrdinalIgnoreCase), new RuleExpected(false))
        ];
    }

    public static class HasDuplicateItems
    {
        public static TheoryData<RuleCase<IEnumerable<string>>> Cases => F.HasDuplicateItems.AllScenarios.ToRuleCases();
    }

    public static class ContainsNullItems
    {
        public static TheoryData<RuleCase<IEnumerable<string?>>> Cases => F.ContainsNullItems.AllScenarios.ToRuleCases();
    }

    public static class Contains
    {
        public static TheoryData<RuleCase<(IEnumerable<string>? value, string item)>> Cases => F.Contains.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, string Item)>> AdHocCases =>
        [
            new("Non-count contains", (Enumerate("a", "b", "c"), "b"), new RuleExpected(true)),
            new("Non-count missing", (Enumerate("a", "b", "c"), "z"), new RuleExpected(false))
        ];
    }

    public static class IsSubsetOf
    {
        public static TheoryData<RuleCase<(IEnumerable<string>? value, IEnumerable<string>? other)>> Cases => F.IsSubsetOf.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, IEnumerable<string>? Other)>> AdHocCases =>
        [
            new("Non-count subset", (Enumerate("a"), new HashSet<string> { "a", "b" }), new RuleExpected(true)),
            new("Empty subset", (Enumerate<string>(), ["a"]), new RuleExpected(true)),
            new("HashSet fast path", (["a"], new HashSet<string> { "a", "b" }), new RuleExpected(true))
        ];
    }

    public static class HasIndex
    {
        public static TheoryData<RuleCase<(IEnumerable<string>? value, int index)>> Cases => F.HasIndex.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<(IEnumerable<string>? Value, int Index)>> AdHocCases =>
        [
            new("Non-count index 1", (Enumerate("a", "b", "c"), 1), new RuleExpected(true)),
            new("ReadOnly index 0", (new ReadOnlyCollectionOnly<string>("a"), 0), new RuleExpected(true)),
            new("Non-count out of range", (Enumerate("a"), 1), new RuleExpected(false))
        ];
    }
}
