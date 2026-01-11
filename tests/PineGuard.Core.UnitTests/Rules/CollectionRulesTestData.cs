using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class CollectionRulesTestData
{
    public static IEnumerable<int> Enumerate(params int[] items)
    {
        for (var i = 0; i < items.Length; i++)
            yield return items[i];
    }

    private static readonly IEnumerable<int>? NullEnumerable = null;

    private static readonly IEnumerable<int> EmptyCountEnumerable = new List<int>();

    private static readonly IEnumerable<int> EmptyNonCountEnumerable = Enumerate();

    private static readonly IEnumerable<int> NonEmptyCountEnumerable = new List<int> { 1, 2, 3 };

    private static readonly IEnumerable<int> NonEmptyNonCountEnumerable = Enumerate(1, 2, 3);

    public static class IsEmpty
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Empty count", EmptyCountEnumerable, true),
            new("Empty non-count", EmptyNonCountEnumerable, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", NullEnumerable, false),
            new("Non-empty count", NonEmptyCountEnumerable, false),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, false),
        ];

        #region Case Records

        public sealed record Case(string Name, IEnumerable<int>? Value, bool ExpectedReturn)
            : IsCase<IEnumerable<int>?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasItems
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Non-empty count", NonEmptyCountEnumerable, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", NullEnumerable, false),
            new("Empty count", EmptyCountEnumerable, false),
            new("Empty non-count", EmptyNonCountEnumerable, false),
        ];

        #region Case Records

        public sealed record Case(string Name, IEnumerable<int>? Value, bool ExpectedReturn)
            : HasCase<IEnumerable<int>?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasExactCount
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Empty count", (Value: EmptyCountEnumerable, Count: 0), true),
            new("Empty non-count", (Value: EmptyNonCountEnumerable, Count: 0), true),
            new("Non-empty count", (Value: NonEmptyCountEnumerable, Count: 3), true),
            new("Non-empty non-count", (Value: NonEmptyNonCountEnumerable, Count: 3), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Count: 0), false),
            new("Non-empty count wrong", (Value: NonEmptyCountEnumerable, Count: 2), false),
            new("Non-empty non-count wrong", (Value: NonEmptyNonCountEnumerable, Count: 2), false),
            new("Negative count", (Value: NonEmptyCountEnumerable, Count: -1), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, int Count) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, int Count)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasMinCount
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Empty count", (Value: EmptyCountEnumerable, Min: 0), true),
            new("Empty non-count", (Value: EmptyNonCountEnumerable, Min: 0), true),
            new("Non-empty count", (Value: NonEmptyCountEnumerable, Min: 0), true),
            new("Non-empty non-count", (Value: NonEmptyNonCountEnumerable, Min: 2), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Min: 0), false),
            new("Empty count too few", (Value: EmptyCountEnumerable, Min: 1), false),
            new("Empty non-count too few", (Value: EmptyNonCountEnumerable, Min: 1), false),
            new("Non-empty count too few", (Value: NonEmptyCountEnumerable, Min: 4), false),
            new("Negative min", (Value: NonEmptyNonCountEnumerable, Min: -1), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, int Min) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, int Min)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasMaxCount
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Empty count", (Value: EmptyCountEnumerable, Max: 0), true),
            new("Empty non-count", (Value: EmptyNonCountEnumerable, Max: 0), true),
            new("Non-empty count", (Value: NonEmptyCountEnumerable, Max: 3), true),
            new("Non-empty non-count", (Value: NonEmptyNonCountEnumerable, Max: 3), true),
            new("Non-empty non-count under", (Value: NonEmptyNonCountEnumerable, Max: 4), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Max: 0), false),
            new("Non-empty count too many", (Value: NonEmptyCountEnumerable, Max: 2), false),
            new("Non-empty non-count too many", (Value: NonEmptyNonCountEnumerable, Max: 2), false),
            new("Negative max", (Value: NonEmptyCountEnumerable, Max: -1), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, int Max) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, int Max)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasCountBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(
                "Non-empty count inclusive",
                (Value: NonEmptyCountEnumerable, Min: 0, Max: 3, Inclusion: Inclusion.Inclusive),
                true),
            new(
                "Non-empty non-count inclusive",
                (Value: NonEmptyNonCountEnumerable, Min: 3, Max: 3, Inclusion: Inclusion.Inclusive),
                true),
            new(
                "Non-empty non-count exclusive",
                (Value: NonEmptyNonCountEnumerable, Min: 0, Max: 4, Inclusion: Inclusion.Exclusive),
                true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Min: 0, Max: 3, Inclusion: Inclusion.Inclusive), false),
            new(
                "Negative min",
                (Value: NonEmptyCountEnumerable, Min: -1, Max: 3, Inclusion: Inclusion.Inclusive),
                false),
            new(
                "Min greater than max",
                (Value: NonEmptyCountEnumerable, Min: 3, Max: 2, Inclusion: Inclusion.Inclusive),
                false),
            new(
                "Exclusive equal bounds",
                (Value: NonEmptyCountEnumerable, Min: 3, Max: 3, Inclusion: Inclusion.Exclusive),
                false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, int Min, int Max, Inclusion Inclusion) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, int Min, int Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasIndex
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Non-empty count", (Value: NonEmptyCountEnumerable, Index: 0), true),
            new("Non-empty non-count", (Value: NonEmptyNonCountEnumerable, Index: 2), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Index: 0), false),
            new("Empty count", (Value: EmptyCountEnumerable, Index: 0), false),
            new("Empty non-count", (Value: EmptyNonCountEnumerable, Index: 0), false),
            new("Non-empty count out of range", (Value: NonEmptyCountEnumerable, Index: 3), false),
            new("Negative index", (Value: NonEmptyNonCountEnumerable, Index: -1), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, int Index) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, int Index)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class Contains
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Non-empty count", (Value: NonEmptyCountEnumerable, Item: 2), true),
            new("Non-empty non-count", (Value: NonEmptyNonCountEnumerable, Item: 2), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Item: 2), false),
            new("Empty count", (Value: EmptyCountEnumerable, Item: 2), false),
            new("Non-empty count missing", (Value: NonEmptyCountEnumerable, Item: 9), false),
            new("Non-empty non-count missing", (Value: NonEmptyNonCountEnumerable, Item: 9), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, int Item) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, int Item)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasAny
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Non-empty count", (Value: NonEmptyCountEnumerable, Predicate: x => x == 2), true),
            new("Non-empty non-count", (Value: NonEmptyNonCountEnumerable, Predicate: x => x > 2), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Predicate: _ => true), false),
            new("Empty count", (Value: EmptyCountEnumerable, Predicate: _ => true), false),
            new("Non-empty count no match", (Value: NonEmptyCountEnumerable, Predicate: x => x == 9), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, Func<int, bool> Predicate) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, Func<int, bool> Predicate)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasAll
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Empty count", (Value: EmptyCountEnumerable, Predicate: _ => false), true),
            new("Non-empty count", (Value: NonEmptyCountEnumerable, Predicate: x => x > 0), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", (Value: NullEnumerable, Predicate: _ => true), false),
            new(
                "Non-empty non-count",
                (Value: NonEmptyNonCountEnumerable, Predicate: x => x < 3),
                false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, Func<int, bool> Predicate) Value,
            bool ExpectedReturn)
            : HasCase<(IEnumerable<int>? Value, Func<int, bool> Predicate)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsSubsetOf
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Array subset", (Value: [1, 2], Other: [1, 2, 3]), true),
            new("Non-count subset", (Value: Enumerate(1, 2), Other: new HashSet<int> { 1, 2, 3 }), true),
            new("Empty subset", (Value: EmptyNonCountEnumerable, Other: NonEmptyCountEnumerable), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null value", (Value: NullEnumerable, Other: NonEmptyCountEnumerable), false),
            new("Null other", (Value: NonEmptyCountEnumerable, Other: NullEnumerable), false),
            new("Non-subset", (Value: NonEmptyNonCountEnumerable, Other: [1, 3]), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IEnumerable<int>? Value, IEnumerable<int>? Other) Value,
            bool ExpectedReturn)
            : IsCase<(IEnumerable<int>? Value, IEnumerable<int>? Other)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
