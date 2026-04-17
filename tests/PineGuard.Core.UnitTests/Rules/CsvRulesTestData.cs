using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class CsvRulesTestData
{
    public static class IsCsvLine
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsCsvLine.AllScenarios.ToRuleCases();
    }

    public static class IsCsvHeaderLine
    {
        public static TheoryData<RuleCase<(string? line, IReadOnlyList<string>? expectedHeader)>> Cases =>
        [
            new("matches", ("a,b", (IReadOnlyList<string>)["a", "b"]), new RuleExpected(true)),
            new("mismatch", ("a,b", (IReadOnlyList<string>)["a", "c"]), new RuleExpected(false)),
            new("null expected", ("a,b", null), new RuleExpected(false))
        ];
    }

    public static class IsCsvRowLineSchema
    {
        public static TheoryData<RuleCase<(string? line, IReadOnlyList<CsvColumnSchema>? schema)>> Cases =>
        [
            new("valid", ("1", (IReadOnlyList<CsvColumnSchema>)[new CsvColumnSchema("a", CsvColumnType.Int32)]), new RuleExpected(true)),
            new("invalid", ("x", (IReadOnlyList<CsvColumnSchema>)[new CsvColumnSchema("a", CsvColumnType.Int32)]), new RuleExpected(false)),
            new("null schema", ("1", null), new RuleExpected(false))
        ];
    }

    public static class IsCsvRowLineHeaderTypes
    {
        public static TheoryData<RuleCase<(string? line, IReadOnlyList<string>? header, IReadOnlyDictionary<string, CsvColumnType> types)>> Cases =>
        [
            new("valid", ("1", (IReadOnlyList<string>)["a"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }), new RuleExpected(true)),
            new("invalid", ("x", (IReadOnlyList<string>)["a"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }), new RuleExpected(false)),
            new("null header", ("1", null, new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.Int32 }), new RuleExpected(false))
        ];
    }
}
