using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class CollectionRulesTestData
{
    public static IEnumerable<int> Enumerate(params int[] items)
    {
        for (var i = 0; i < items.Length; i++)
            yield return items[i];
    }

    private static readonly Func<IEnumerable<int>?> NullEnumerable = () => null;

    private static readonly Func<IEnumerable<int>?> EmptyCountEnumerable = () => new List<int>();

    private static readonly Func<IEnumerable<int>?> EmptyNonCountEnumerable = () => Enumerate();

    private static readonly Func<IEnumerable<int>?> NonEmptyCountEnumerable = () => new List<int> { 1, 2, 3 };

    private static readonly Func<IEnumerable<int>?> NonEmptyNonCountEnumerable = () => Enumerate(1, 2, 3);

    public static class IsEmpty
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Empty count", EmptyCountEnumerable, true),
            new("Empty non-count", EmptyNonCountEnumerable, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, false),
            new("Non-empty count", NonEmptyCountEnumerable, false),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasItems
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Non-empty count", NonEmptyCountEnumerable, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, false),
            new("Empty count", EmptyCountEnumerable, false),
            new("Empty non-count", EmptyNonCountEnumerable, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasExactCount
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Empty count", EmptyCountEnumerable, 0, true),
            new("Empty non-count", EmptyNonCountEnumerable, 0, true),
            new("Non-empty count", NonEmptyCountEnumerable, 3, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, 3, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, 0, false),
            new("Non-empty count wrong", NonEmptyCountEnumerable, 2, false),
            new("Non-empty non-count wrong", NonEmptyNonCountEnumerable, 2, false),
            new("Negative count", NonEmptyCountEnumerable, -1, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, int Count, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasMinCount
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Empty count", EmptyCountEnumerable, 0, true),
            new("Empty non-count", EmptyNonCountEnumerable, 0, true),
            new("Non-empty count", NonEmptyCountEnumerable, 0, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, 0, false),
            new("Empty count too few", EmptyCountEnumerable, 1, false),
            new("Empty non-count too few", EmptyNonCountEnumerable, 1, false),
            new("Non-empty count too few", NonEmptyCountEnumerable, 4, false),
            new("Negative min", NonEmptyNonCountEnumerable, -1, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, int Min, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasMaxCount
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Empty count", EmptyCountEnumerable, 0, true),
            new("Empty non-count", EmptyNonCountEnumerable, 0, true),
            new("Non-empty count", NonEmptyCountEnumerable, 3, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, 3, true),
            new("Non-empty non-count under", NonEmptyNonCountEnumerable, 4, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, 0, false),
            new("Non-empty count too many", NonEmptyCountEnumerable, 2, false),
            new("Non-empty non-count too many", NonEmptyNonCountEnumerable, 2, false),
            new("Negative max", NonEmptyCountEnumerable, -1, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, int Max, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasCountBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Non-empty count inclusive", NonEmptyCountEnumerable, 0, 3, Inclusion.Inclusive, true),
            new("Non-empty non-count inclusive", NonEmptyNonCountEnumerable, 3, 3, Inclusion.Inclusive, true),
            new("Non-empty non-count exclusive", NonEmptyNonCountEnumerable, 0, 4, Inclusion.Exclusive, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, 0, 3, Inclusion.Inclusive, false),
            new("Negative min", NonEmptyCountEnumerable, -1, 3, Inclusion.Inclusive, false),
            new("Min greater than max", NonEmptyCountEnumerable, 3, 2, Inclusion.Inclusive, false),
            new("Exclusive equal bounds", NonEmptyCountEnumerable, 3, 3, Inclusion.Exclusive, false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            Func<IEnumerable<int>?> ValueFactory,
            int Min,
            int Max,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasIndex
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Non-empty count", NonEmptyCountEnumerable, 0, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, 0, false),
            new("Empty count", EmptyCountEnumerable, 0, false),
            new("Empty non-count", EmptyNonCountEnumerable, 0, false),
            new("Non-empty count out of range", NonEmptyCountEnumerable, 3, false),
            new("Negative index", NonEmptyNonCountEnumerable, -1, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, int Index, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class Contains
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Non-empty count", NonEmptyCountEnumerable, 2, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, 2, false),
            new("Empty count", EmptyCountEnumerable, 2, false),
            new("Non-empty count missing", NonEmptyCountEnumerable, 9, false),
            new("Non-empty non-count missing", NonEmptyNonCountEnumerable, 9, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, int Item, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasAny
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Non-empty count", NonEmptyCountEnumerable, x => x == 2, true),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, x => x > 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, _ => true, false),
            new("Empty count", EmptyCountEnumerable, _ => true, false),
            new("Non-empty count no match", NonEmptyCountEnumerable, x => x == 9, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, Func<int, bool> Predicate, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasAll
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Empty count", EmptyCountEnumerable, _ => false, true),
            new("Non-empty count", NonEmptyCountEnumerable, x => x > 0, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullEnumerable, _ => true, false),
            new("Non-empty non-count", NonEmptyNonCountEnumerable, x => x < 3, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IEnumerable<int>?> ValueFactory, Func<int, bool> Predicate, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsSubsetOf
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Array subset", () => new[] { 1, 2 }, () => new[] { 1, 2, 3 }, true),
            new("Non-count subset", () => Enumerate(1, 2), () => new HashSet<int> { 1, 2, 3 }, true),
            new("Empty subset", EmptyNonCountEnumerable, NonEmptyCountEnumerable, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null value", NullEnumerable, NonEmptyCountEnumerable, false),
            new("Null other", NonEmptyCountEnumerable, NullEnumerable, false),
            new("Non-subset", NonEmptyNonCountEnumerable, () => new[] { 1, 3 }, false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            Func<IEnumerable<int>?> ValueFactory,
            Func<IEnumerable<int>?> OtherFactory,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
