using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class DictionaryUtilityTestData
{
    public static class TryGetCount
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("count 2", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, true, 2),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, 0),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, IDictionary<string, int>? Dictionary, bool ExpectedReturn, int ExpectedOutValue)
            : TryCase<IDictionary<string, int>?, int>(Name, Dictionary, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryGetValue
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1 }, "a", true, 1),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, "a", false, 0),
            new("missing", new Dictionary<string, int> { ["a"] = 1 }, "missing", false, 0),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(
            string Name,
            IDictionary<string, int>? Dictionary,
            string Key,
            bool ExpectedReturn,
            int ExpectedOutValue)
            : TryCase<(IDictionary<string, int>? Dictionary, string Key), int>(Name, (Dictionary, Key), ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryGetKeyValue
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1 }, "a", true, new KeyValuePair<string, int>("a", 1)),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, "a", false, default),
            new("missing", new Dictionary<string, int> { ["a"] = 1 }, "missing", false, default),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(
            string Name,
            IDictionary<string, int>? Dictionary,
            string Key,
            bool ExpectedReturn,
            KeyValuePair<string, int> ExpectedOutValue)
            : TryCase<(IDictionary<string, int>? Dictionary, string Key), KeyValuePair<string, int>>(Name, (Dictionary, Key), ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryGetKey
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, 2, true, "b"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, 1, false, null),
            new("missing", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, 99, false, null),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(
            string Name,
            IDictionary<string, int>? Dictionary,
            int SearchValue,
            bool ExpectedReturn,
            string? ExpectedOutValue)
            : TryCase<(IDictionary<string, int>? Dictionary, int Value), string?>(Name, (Dictionary, SearchValue), ExpectedReturn, ExpectedOutValue);

        #endregion
    }
}
