namespace PineGuard.Core.UnitTests.Rules;

public static class DictionaryRulesTestData
{
    public static Func<IDictionary<string, int>?> NullDictionary => () => null;

    public static Func<IDictionary<string, int>?> EmptyDictionary => () => new Dictionary<string, int>();

    public static Func<IDictionary<string, int>?> NonEmptyDictionary => () => new Dictionary<string, int>
    {
        { "a", 1 },
        { "b", 2 },
    };

    public static class IsEmpty
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Null", NullDictionary, true),
            new("Empty", EmptyDictionary, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Non-empty", NonEmptyDictionary, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IDictionary<string, int>?> DictionaryFactory, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasItems
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Non-empty", NonEmptyDictionary, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", NullDictionary, false),
            new("Empty", EmptyDictionary, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IDictionary<string, int>?> DictionaryFactory, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasKey
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Existing key", NonEmptyDictionary, "a", true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null dictionary", NullDictionary, "a", false),
            new("Empty dictionary", EmptyDictionary, "a", false),
            new("Missing key", NonEmptyDictionary, "missing", false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IDictionary<string, int>?> DictionaryFactory, string Key, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasValue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Existing value", NonEmptyDictionary, 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null dictionary", NullDictionary, 2, false),
            new("Empty dictionary", EmptyDictionary, 2, false),
            new("Missing value", NonEmptyDictionary, 999, false),
        };

        #region Cases

        public sealed record Case(string Name, Func<IDictionary<string, int>?> DictionaryFactory, int Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasKeyValue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Existing pair", NonEmptyDictionary, "b", 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null dictionary", NullDictionary, "b", 2, false),
            new("Empty dictionary", EmptyDictionary, "b", 2, false),
            new("Missing key", NonEmptyDictionary, "missing", 2, false),
            new("Wrong value", NonEmptyDictionary, "b", 999, false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            Func<IDictionary<string, int>?> DictionaryFactory,
            string Key,
            int Value,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasAnyKey
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Key matches", NonEmptyDictionary, key => key == "a", true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null dictionary", NullDictionary, _ => true, false),
            new("Empty dictionary", EmptyDictionary, _ => true, false),
            new("No match", NonEmptyDictionary, _ => false, false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            Func<IDictionary<string, int>?> DictionaryFactory,
            Func<string, bool> Predicate,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasAnyValue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Value matches", NonEmptyDictionary, v => v == 1, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null dictionary", NullDictionary, _ => true, false),
            new("Empty dictionary", EmptyDictionary, _ => true, false),
            new("No match", NonEmptyDictionary, _ => false, false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            Func<IDictionary<string, int>?> DictionaryFactory,
            Func<int, bool> Predicate,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasAnyItem
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Item matches", NonEmptyDictionary, (k, v) => k == "b" && v == 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null dictionary", NullDictionary, (_, _) => true, false),
            new("Empty dictionary", EmptyDictionary, (_, _) => true, false),
            new("No match", NonEmptyDictionary, (_, _) => false, false),
        };

        #region Cases

        public sealed record Case(
            string Name,
            Func<IDictionary<string, int>?> DictionaryFactory,
            Func<string, int, bool> Predicate,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
