using PineGuard.Testing.UnitTests;

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
        public static TheoryData<Case> ValidCases =>
        [
            new("Null", NullDictionary(), true),
            new("Empty", EmptyDictionary(), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Non-empty", NonEmptyDictionary(), false),
        ];

        #region Case Records

        public sealed record Case(string Name, IDictionary<string, int>? Value, bool ExpectedReturn)
            : IsCase<IDictionary<string, int>?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasItems
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Non-empty", NonEmptyDictionary(), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", NullDictionary(), false),
            new("Empty", EmptyDictionary(), false),
        ];

        #region Case Records

        public sealed record Case(string Name, IDictionary<string, int>? Value, bool ExpectedReturn)
            : HasCase<IDictionary<string, int>?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasKey
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Existing key", (NonEmptyDictionary(), Key: "a"), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null dictionary", (NullDictionary(), Key: "a"), false),
            new("Empty dictionary", (EmptyDictionary(), Key: "a"), false),
            new("Missing key", (NonEmptyDictionary(), Key: "missing"), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IDictionary<string, int>? Dictionary, string Key) Value,
            bool ExpectedReturn)
            : HasCase<(IDictionary<string, int>? Dictionary, string Key)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasValue
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Existing value", (NonEmptyDictionary(), SearchValue: 2), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null dictionary", (NullDictionary(), SearchValue: 2), false),
            new("Empty dictionary", (EmptyDictionary(), SearchValue: 2), false),
            new("Missing value", (NonEmptyDictionary(), SearchValue: 999), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IDictionary<string, int>? Dictionary, int SearchValue) Value,
            bool ExpectedReturn)
            : HasCase<(IDictionary<string, int>? Dictionary, int SearchValue)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasKeyValue
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Existing pair", (NonEmptyDictionary(), Key: "b", SearchValue: 2), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null dictionary", (NullDictionary(), Key: "b", SearchValue: 2), false),
            new("Empty dictionary", (EmptyDictionary(), Key: "b", SearchValue: 2), false),
            new("Missing key", (NonEmptyDictionary(), Key: "missing", SearchValue: 2), false),
            new("Wrong value", (NonEmptyDictionary(), Key: "b", SearchValue: 999), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IDictionary<string, int>? Dictionary, string Key, int SearchValue) Value,
            bool ExpectedReturn)
            : HasCase<(IDictionary<string, int>? Dictionary, string Key, int SearchValue)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasAnyKey
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Key matches", (NonEmptyDictionary(), Predicate: key => key == "a"), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null dictionary", (NullDictionary(), Predicate: _ => true), false),
            new("Empty dictionary", (EmptyDictionary(), Predicate: _ => true), false),
            new("No match", (NonEmptyDictionary(), Predicate: _ => false), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IDictionary<string, int>? Dictionary, Func<string, bool> Predicate) Value,
            bool ExpectedReturn)
            : HasCase<(IDictionary<string, int>? Dictionary, Func<string, bool> Predicate)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasAnyValue
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Value matches", (NonEmptyDictionary(), Predicate: v => v == 1), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null dictionary", (NullDictionary(), Predicate: _ => true), false),
            new("Empty dictionary", (EmptyDictionary(), Predicate: _ => true), false),
            new("No match", (NonEmptyDictionary(), Predicate: _ => false), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IDictionary<string, int>? Dictionary, Func<int, bool> Predicate) Value,
            bool ExpectedReturn)
            : HasCase<(IDictionary<string, int>? Dictionary, Func<int, bool> Predicate)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasAnyItem
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Item matches", (NonEmptyDictionary(), Predicate: (k, v) => k == "b" && v == 2), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null dictionary", (NullDictionary(), Predicate: (_, _) => true), false),
            new("Empty dictionary", (EmptyDictionary(), Predicate: (_, _) => true), false),
            new("No match", (NonEmptyDictionary(), Predicate: (_, _) => false), false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (IDictionary<string, int>? Dictionary, Func<string, int, bool> Predicate) Value,
            bool ExpectedReturn)
            : HasCase<(IDictionary<string, int>? Dictionary, Func<string, int, bool> Predicate)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
