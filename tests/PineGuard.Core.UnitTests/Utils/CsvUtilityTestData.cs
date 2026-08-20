using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CsvUtilityTestData
{
    public static class TryParseCsvLine
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "a,b,c", true, ["a", "b", "c"]),
            new("empty field", "a,,c", true, ["a", "", "c"]),
            new("quoted field", "\"a,b\",c", true, ["a,b", "c"]),
            new("escaped quote", "\"a\"\"b\",c", true, ["a\"b", "c"]),
            new("after quote whitespace", "\"a\" ,b", true, ["a", "b"]),
            new("before quote whitespace", "a, \"b,c\"", true, ["a", "b,c"]),
            new("quoted field at end", "a,\"b,c\"", true, ["a", "b,c"])
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("whitespace", "  ", false, null),
            new("newline", "a\n", false, null),
            new("unclosed quote", "\"a", false, null),
            new("quote mid field", "a\"b", false, null),
            new("after closing quote invalid", "\"a\"x", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, IReadOnlyList<string>? ExpectedOutValue)
            : TryCase<string?, IReadOnlyList<string>?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseCsvLineInvalidSeparator
    {
        public static TheoryData<char> Separators =>
        [
            '"',
            '\r',
            '\n'
        ];
    }

    public static class TryParseCsvHeaderLine
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("trims", " a , b ", true, ["a", "b"])
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("empty field", "a, ", false, null),
            new("invalid csv line", "\"a", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, IReadOnlyList<string>? ExpectedOutValue)
            : TryCase<string?, IReadOnlyList<string>?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseCsvHeaderLineExpected
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("matches", ("a,b", ["a", "b"], StringComparison.OrdinalIgnoreCase), true),
            new("matches ignore case", ("A,B", ["a", "b"], StringComparison.OrdinalIgnoreCase), true),
            new("mismatch", ("a,b", ["a", "c"], StringComparison.OrdinalIgnoreCase), false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null expected", ("a,b", null, StringComparison.OrdinalIgnoreCase), false),
            new("empty expected", ("a,b", [], StringComparison.OrdinalIgnoreCase), false),
            new("invalid header line", ("\"a", ["a"], StringComparison.OrdinalIgnoreCase), false),
            new("count mismatch", ("a", ["a", "b"], StringComparison.OrdinalIgnoreCase), false),
            new("expected entry whitespace", ("a,b", ["a", "  "], StringComparison.OrdinalIgnoreCase), false)
        ];

        public sealed record ValidCase(string Name, (string? Line, IReadOnlyList<string>? ExpectedHeader, StringComparison Comparison) Value, bool Expected)
            : IsCase<(string? Line, IReadOnlyList<string>? ExpectedHeader, StringComparison Comparison)>(Name, Value, Expected);
    }

    public static class TryParseCsvRowLineExpectedCount
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("matches", ("a,b", 2), true),
            new("count mismatch", ("a,b", 3), false)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("expected <=0", ("a", 0), false),
            new("null line", (null, 1), false)
        ];

        public sealed record ValidCase(string Name, (string? Line, int ExpectedCount) Value, bool Expected)
            : IsCase<(string? Line, int ExpectedCount)>(Name, Value, Expected);
    }

    public static class TryParseCsvRowLineSchema
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("required ok", ("1,2", [new CsvColumnSchema("a", CsvColumnType.Int32), new CsvColumnSchema("b", CsvColumnType.Int32)]), true),
            new("optional empty ok", ("1,", [new CsvColumnSchema("a", CsvColumnType.Int32), new CsvColumnSchema("b", CsvColumnType.Int32, IsRequired: false)]), true),
            new("string always ok", ("hello", [new CsvColumnSchema("a", CsvColumnType.String)]), true), new("optional string missing", ("A,", [new CsvColumnSchema("R1", CsvColumnType.String, IsRequired: true), new CsvColumnSchema("O1", CsvColumnType.String, IsRequired: false)]), true),
            new("all supported types", ("9223372036854775807, 123.45, 1.5, 2.5, 3d6f0a19-9a4e-4fd0-9f61-83c9d7f0f1f8, true, 2024-01-02, 03:04:05, 2024-01-02T03:04:05+00:00",
            [
                new CsvColumnSchema("a", CsvColumnType.Int64),
                new CsvColumnSchema("b", CsvColumnType.Decimal),
                new CsvColumnSchema("c", CsvColumnType.Single),
                new CsvColumnSchema("d", CsvColumnType.Double),
                new CsvColumnSchema("e", CsvColumnType.Guid),
                new CsvColumnSchema("f", CsvColumnType.Bool),
                new CsvColumnSchema("g", CsvColumnType.DateOnly),
                new CsvColumnSchema("h", CsvColumnType.TimeOnly),
                new CsvColumnSchema("i", CsvColumnType.DateTimeOffset)
            ]), true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("schema null", ("a", null), false),
            new("schema empty", ("a", []), false),
            new("column name whitespace", ("a", [new CsvColumnSchema("  ", CsvColumnType.String)]), false),
            new("max length <=0", ("a", [new CsvColumnSchema("a", CsvColumnType.String, MaxLength: 0)]), false),
            new("required empty", ("", [new CsvColumnSchema("a", CsvColumnType.String)]), false),
            new("required field empty segment", ("a,", [new CsvColumnSchema("a", CsvColumnType.String), new CsvColumnSchema("b", CsvColumnType.String)]), false),
            new("required field whitespace", ("   ", [new CsvColumnSchema("Col1", CsvColumnType.String, IsRequired: true)]), false),
            new("too long", ("abcdef", [new CsvColumnSchema("a", CsvColumnType.String, MaxLength: 3)]), false),
            new("type mismatch", ("nope", [new CsvColumnSchema("a", CsvColumnType.Int32)]), false),
            new("unknown type", ("a", [new CsvColumnSchema("a", (CsvColumnType)123)]), false),
            new("maxlength enforced on raw padded field", ("\"     ab\"", [new CsvColumnSchema("a", CsvColumnType.String, MaxLength: 3)]), false),
            new("maxlength enforced on raw whitespace-only optional field", ("\"      \"", [new CsvColumnSchema("a", CsvColumnType.String, MaxLength: 3, IsRequired: false)]), false)
        ];

        public sealed record ValidCase(string Name, (string? Line, IReadOnlyList<CsvColumnSchema>? Schema) Value, bool Expected)
            : IsCase<(string? Line, IReadOnlyList<CsvColumnSchema>? Schema)>(Name, Value, Expected);
    }

    public static class TryParseCsvRowLineHeaderTypes
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("exact key lookup", ("1", ["a"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }, StringComparison.Ordinal), true),
            new("non-exact match", ("1", ["A"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }, StringComparison.OrdinalIgnoreCase), true),
            new("fallback to string on missing key", ("1", ["a"], new Dictionary<string, CsvColumnType> { ["b"] = CsvColumnType.Int32 }, StringComparison.Ordinal), true),
            new("ordinal ignores dictionary's own comparer", ("abc", ["ID"], new Dictionary<string, CsvColumnType>(StringComparer.OrdinalIgnoreCase) { ["id"] = CsvColumnType.Int32 }, StringComparison.Ordinal), true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null header", ("1", null, new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }, StringComparison.OrdinalIgnoreCase), false),
            new("empty header", ("1", [], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }, StringComparison.OrdinalIgnoreCase), false),
            new("header name whitespace", ("1", ["  "], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }, StringComparison.OrdinalIgnoreCase), false),
            new("null types", ("1", ["a"], null, StringComparison.OrdinalIgnoreCase), false),
            new("empty types", ("1", ["a"], new Dictionary<string, CsvColumnType>(), StringComparison.OrdinalIgnoreCase), false),
            new("missing type exact", ("1", ["a"], new Dictionary<string, CsvColumnType> { ["b"] = CsvColumnType.Int32 }, StringComparison.Ordinal), true),
            new("missing type non-exact", ("1", ["a"], new Dictionary<string, CsvColumnType> { ["b"] = CsvColumnType.Int32 }, StringComparison.OrdinalIgnoreCase), true),
            new("types key whitespace", ("1", ["a"], new Dictionary<string, CsvColumnType> { ["  "] = CsvColumnType.Int32 }, StringComparison.OrdinalIgnoreCase), true)
        ];

        public sealed record ValidCase(string Name, (string? Line, IReadOnlyList<string>? Header, IReadOnlyDictionary<string, CsvColumnType>? Types, StringComparison Comparison) Value, bool Expected)
            : IsCase<(string? Line, IReadOnlyList<string>? Header, IReadOnlyDictionary<string, CsvColumnType>? Types, StringComparison Comparison)>(Name, Value, Expected);
    }
}
