using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class DictionaryUtilityTestData
{
    public static class TryGetCount
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("count 2", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, true, 2)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, 0)
        ];

        public sealed record ValidCase(string Name, IDictionary<string, int>? Value, bool Expected, int ExpectedOutValue)
            : TryCase<IDictionary<string, int>?, int>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryGetValue
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1 }, "a", true, 1)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, "a", false, 0),
            new("missing", new Dictionary<string, int> { ["a"] = 1 }, "missing", false, 0)
        ];

        public sealed record ValidCase : TryCase<(IDictionary<string, int>? dictionary, string key), int>
        {
            public ValidCase(string name, IDictionary<string, int>? dictionary, string key, bool expected, int expectedOutValue)
                : base(name, (dictionary, key), expected, expectedOutValue) { }
        }
    }

    public static class TryGetKeyValue
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1 }, "a", true, new KeyValuePair<string, int>("a", 1))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, "a", false, default),
            new("missing", new Dictionary<string, int> { ["a"] = 1 }, "missing", false, default)
        ];

        public sealed record ValidCase : TryCase<(IDictionary<string, int>? dictionary, string key), KeyValuePair<string, int>>
        {
            public ValidCase(string name, IDictionary<string, int>? dictionary, string key, bool expected, KeyValuePair<string, int> expectedOutValue)
                : base(name, (dictionary, key), expected, expectedOutValue) { }
        }
    }

    public static class TryGetKey
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, 2, true, "b")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, 1, false, null),
            new("missing", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, 99, false, null)
        ];

        public sealed record ValidCase : TryCase<(IDictionary<string, int>? dictionary, int searchValue), string?>
        {
            public ValidCase(string name, IDictionary<string, int>? dictionary, int searchValue, bool expected, string? expectedOutValue)
                : base(name, (dictionary, searchValue), expected, expectedOutValue) { }
        }
    }

    public static class TryGetAnyKey
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, k => k == "b", true, "b")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null dict", null, _ => true, false, null),
            new("not found", new Dictionary<string, int> { ["a"] = 1 }, k => k == "missing", false, null)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null predicate", new Dictionary<string, int>(), null!, new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record ValidCase : TryCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate), string?>
        {
            public ValidCase(string name, IDictionary<string, int>? dictionary, Func<string, bool> predicate, bool expected, string? expectedOutValue)
                : base(name, (dictionary, predicate), expected, expectedOutValue) { }
        }

        public sealed record InvalidCase : ThrowsCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)>
        {
            public InvalidCase(string name, IDictionary<string, int>? dictionary, Func<string, bool> predicate, ExpectedException expectedException)
                : base(name, (dictionary, predicate), expectedException) { }
        }
    }

    public static class TryGetAnyValue
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, v => v == 2, true, 2)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null dict", null, _ => true, false, 0),
            new("not found", new Dictionary<string, int> { ["a"] = 1 }, v => v == 99, false, 0)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null predicate", new Dictionary<string, int>(), null!, new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record ValidCase : TryCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate), int>
        {
            public ValidCase(string name, IDictionary<string, int>? dictionary, Func<int, bool> predicate, bool expected, int expectedOutValue)
                : base(name, (dictionary, predicate), expected, expectedOutValue) { }
        }

        public sealed record InvalidCase : ThrowsCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)>
        {
            public InvalidCase(string name, IDictionary<string, int>? dictionary, Func<int, bool> predicate, ExpectedException expectedException)
                : base(name, (dictionary, predicate), expectedException) { }
        }
    }

    public static class TryGetAnyItem
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("found", new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 }, (k, v) => k == "b" && v == 2, true, new KeyValuePair<string, int>("b", 2))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null dict", null, (_, _) => true, false, default),
            new("not found", new Dictionary<string, int> { ["a"] = 1 }, (_, v) => v == 99, false, default)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null predicate", new Dictionary<string, int>(), null!, new ExpectedException(typeof(ArgumentNullException)))
        ];

        public sealed record ValidCase : TryCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate), KeyValuePair<string, int>>
        {
            public ValidCase(string name, IDictionary<string, int>? dictionary, Func<string, int, bool> predicate, bool expected, KeyValuePair<string, int> expectedOutValue)
                : base(name, (dictionary, predicate), expected, expectedOutValue) { }
        }

        public sealed record InvalidCase : ThrowsCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)>
        {
            public InvalidCase(string name, IDictionary<string, int>? dictionary, Func<string, int, bool> predicate, ExpectedException expectedException)
                : base(name, (dictionary, predicate), expectedException) { }
        }
    }
}
