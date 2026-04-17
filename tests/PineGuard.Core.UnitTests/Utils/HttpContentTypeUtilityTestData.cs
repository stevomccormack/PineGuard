using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class HttpContentTypeUtilityTestData
{
    public static class TryGetMediaType
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("no parameters", "application/json", true, "application/json"),
            new("with charset", "application/json; charset=utf-8", true, "application/json"),
            new("trim", "  text/xml  ", true, "text/xml"),
            new("semi only", "application/json;", true, "application/json")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("whitespace", "   ", false, null),
            new("semicolon whitespace", ";", false, null),
            new("blank before semi", " ; charset=utf-8", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryGetContentTypeMediaTypes
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("single header", new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/json"] }, true, ["application/json"]),
            new("multiple values", new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/json; charset=utf-8", "text/plain"] }, true, ["application/json", "text/plain"])
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null headers", null, false, null),
            new("no content-type", new Dictionary<string, IEnumerable<string>> { ["X"] = ["y"] }, false, null),
            new("only invalid media types", new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = [" ", ";"] }, false, null)
        ];

        public sealed record ValidCase(string Name, IReadOnlyDictionary<string, IEnumerable<string>>? Value, bool Expected, IReadOnlyList<string>? ExpectedOutValue)
            : TryCase<IReadOnlyDictionary<string, IEnumerable<string>>?, IReadOnlyList<string>?>(Name, Value, Expected, ExpectedOutValue);
    }
}
