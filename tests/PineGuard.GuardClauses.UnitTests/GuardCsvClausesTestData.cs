using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardCsvClausesTestData
{
    public static class NotCsvLine
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsCsvLine.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsCsvLine.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    public static class NotCsvHeaderLine
    {
        public static TheoryData<GuardCase<(string? line, IReadOnlyList<string>? expectedHeader)>> ValidCases =>
        [
            new(nameof(F.IsCsvHeaderLine.Matches), F.IsCsvHeaderLine.Matches, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? line, IReadOnlyList<string>? expectedHeader)>> InvalidCases =>
        [
            new(nameof(F.IsCsvHeaderLine.Mismatch), F.IsCsvHeaderLine.Mismatch, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsCsvHeaderLine.NullExpected), F.IsCsvHeaderLine.NullExpected, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotCsvRowLineSchema
    {
        public static TheoryData<GuardCase<(string? line, IReadOnlyList<CsvColumnSchema>? schema)>> ValidCases =>
        [
            new("ValidRow", ("foo,123", [new CsvColumnSchema("Col1", CsvColumnType.String), new CsvColumnSchema("Col2", CsvColumnType.Int32)]), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? line, IReadOnlyList<CsvColumnSchema>? schema)>> InvalidCases =>
        [
            new("NullLine", (null, [new CsvColumnSchema("Col1", CsvColumnType.String)]), new GuardExpected(false, typeof(ArgumentNullException), "value")),
            new("InvalidRow", ("foo,abc", [new CsvColumnSchema("Col1", CsvColumnType.String), new CsvColumnSchema("Col2", CsvColumnType.Int32)]), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    public static class NotCsvRowLineHeaderTypes
    {
        public static TheoryData<GuardCase<(string? line, IReadOnlyList<string>? header, IReadOnlyDictionary<string, CsvColumnType>? types)>> ValidCases =>
        [
            new("ValidRow", ("foo,123", ["Col1", "Col2"], new Dictionary<string, CsvColumnType> { { "Col2", CsvColumnType.Int32 } }), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(string? line, IReadOnlyList<string>? header, IReadOnlyDictionary<string, CsvColumnType>? types)>> InvalidCases =>
        [
            new("NullLine", (null, ["Col1", "Col2"], null), new GuardExpected(false, typeof(ArgumentNullException), "value")),
            new("InvalidRow", ("foo,abc", ["Col1", "Col2"], new Dictionary<string, CsvColumnType> { { "Col2", CsvColumnType.Int32 } }), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }
}
