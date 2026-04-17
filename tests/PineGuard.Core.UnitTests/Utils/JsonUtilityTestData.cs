using System.Text.Json;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class JsonUtilityTestData
{
    public static class TryGetRootKind
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("object", "{\"a\":1}", true, JsonValueKind.Object),
            new("array", "[1,2]", true, JsonValueKind.Array),
            new("string", "\"x\"", true, JsonValueKind.String)];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, default),
            new("whitespace", "   ", false, default),
            new("invalid json", "{", false, default)];

        public sealed record ValidCase(string Name, string? Value, bool Expected, JsonValueKind ExpectedOutValue)
            : TryCase<string?, JsonValueKind>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryGetRootKindSpan
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("object span", "{\"a\":1}", true, JsonValueKind.Object),
            new("array span", "[1,2]", true, JsonValueKind.Array)];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("empty span", "", false, default),
            new("whitespace span", "   ", false, default),
            new("invalid span", "{", false, default)];

        public sealed record ValidCase(
            string Name, string RawValue, bool Expected, JsonValueKind ExpectedOutValue)
            : TryCase<string, JsonValueKind>(Name, RawValue, Expected, ExpectedOutValue);
    }
}
