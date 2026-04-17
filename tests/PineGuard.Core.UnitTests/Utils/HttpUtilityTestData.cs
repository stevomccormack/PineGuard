using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class HttpUtilityTestData
{
    public static class TryGetHeaderValues
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("exact", (new Dictionary<string, IEnumerable<string>> { ["X-Test"] = ["a"] }, "X-Test"), true),
            new("case-insensitive", (new Dictionary<string, IEnumerable<string>> { ["X-Test"] = ["a"] }, "x-test"), true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null headers", (null, "X"), false),
            new("null name", (new Dictionary<string, IEnumerable<string>> { ["X"] = ["a"] }, null), false),
            new("missing", (new Dictionary<string, IEnumerable<string>> { ["X"] = ["a"] }, "Y"), false)
        ];

        public sealed record ValidCase(string Name, (IReadOnlyDictionary<string, IEnumerable<string>>? Headers, string? Name) Value, bool Expected)
            : IsCase<(IReadOnlyDictionary<string, IEnumerable<string>>? Headers, string? Name)>(Name, Value, Expected);
    }

    public static class TryGetSingleHeaderValue
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("single", (new Dictionary<string, IEnumerable<string>> { ["X"] = [" a "] }, "X"), true, "a"),
            new("ignores blanks", (new Dictionary<string, IEnumerable<string>> { ["X"] = [" ", "a"] }, "X"), true, "a")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("zero", (new Dictionary<string, IEnumerable<string>> { ["X"] = [" "] }, "X"), false, null),
            new("more than one", (new Dictionary<string, IEnumerable<string>> { ["X"] = ["a", "b"] }, "X"), false, null),
            new("null headers", (null, "X"), false, null)
        ];

        public sealed record ValidCase(string Name, (IReadOnlyDictionary<string, IEnumerable<string>>? Headers, string? Name) Value, bool Expected, string? ExpectedOutValue)
            : TryCase<(IReadOnlyDictionary<string, IEnumerable<string>>? Headers, string? Name), string?>(Name, Value, Expected, ExpectedOutValue);
    }
}
