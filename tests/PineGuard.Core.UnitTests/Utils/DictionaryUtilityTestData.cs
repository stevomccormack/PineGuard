using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class DictionaryUtilityTestData
{
    public static class TryGetCount
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("count 2", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, true, 2) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false, 0) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, IDictionary<string, int>? Dictionary, bool ExpectedFound, int ExpectedCount)
            : BaseCase(Name);

        #endregion Cases
    }

    public static class TryGetValue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("found", new Dictionary<string, int> { ["a"] = 1 }, "a", true, 1) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, "a", false, 0) },
            { new Case("missing", new Dictionary<string, int> { ["a"] = 1 }, "missing", false, 0) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, IDictionary<string, int>? Dictionary, string Key, bool ExpectedFound, int ExpectedValue)
            : BaseCase(Name);

        #endregion Cases
    }

    public static class TryGetKeyValue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("found", new Dictionary<string, int> { ["a"] = 1 }, "a", true, new KeyValuePair<string, int>("a", 1)) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, "a", false, default) },
            { new Case("missing", new Dictionary<string, int> { ["a"] = 1 }, "missing", false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, IDictionary<string, int>? Dictionary, string Key, bool ExpectedFound, KeyValuePair<string, int> ExpectedPair)
            : BaseCase(Name);

        #endregion Cases
    }

    public static class TryGetKey
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("found", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, 2, true, "b") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, 1, false, null) },
            { new Case("missing", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, 99, false, null) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, IDictionary<string, int>? Dictionary, int Value, bool ExpectedFound, string? ExpectedKey)
            : BaseCase(Name);

        #endregion Cases
    }
}
