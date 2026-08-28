using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

#pragma warning disable CS0618
namespace PineGuard.MustClauses.UnitTests;

public static class MustCsvClausesTestData
{
    public static class CsvLine
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsCsvLine.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsCsvLine.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new MustExpected(false, "line must not be null.", "line"),
            _ => new MustExpected(false, "line must be a valid CSV line.", Code: MustCodes.Csv.Line.Invalid)
        });
    }

    public static class CsvHeaderLine
    {
        public static TheoryData<MustCase<(string? Line, string[]? Header)>> ValidCases =>
        [
            new("match", ("Col1,Col2", ["Col1", "Col2"]), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(string? Line, string[]? Header)>> InvalidCases =>
        [
            new("mismatch",    ("Col1,Col2", ["Col1"]),  new MustExpected(false, "line must be a valid CSV header line.", Code: MustCodes.Csv.Header.Invalid)),
            new("null input",  (null, ["Col1"]),  new MustExpected(false, "line must not be null.", "line")),
            new("null header", ("Col1",      null),   new MustExpected(false, "line must be a valid CSV header line."))
        ];
    }

    public static class CsvRowLineSchema
    {
        private static readonly CsvColumnSchema[] Schema =
        [
            new("Id", CsvColumnType.Int32),
            new("Name", CsvColumnType.String)
        ];

        public static TheoryData<MustCase<(string? Line, CsvColumnSchema[] Schema)>> ValidCases =>
        [
            new("match", ("1,Steve", Schema), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(string? Line, CsvColumnSchema[] Schema)>> InvalidCases =>
        [
            new("type mismatch",  ("Nan,Steve", Schema), new MustExpected(false, "line must be a valid CSV row line.", Code: MustCodes.Csv.Row.Invalid)),
            new("count mismatch", ("1",         Schema), new MustExpected(false, "line must be a valid CSV row line.")),
            new("null input",     (null,        Schema), new MustExpected(false, "line must not be null.", "line"))
        ];
    }

    public static class CsvRowLineTypes
    {
        public static readonly string[] Header = ["Id", "Name"];
        public static readonly Dictionary<string, CsvColumnType> Types = new()
        {
            { "Id", CsvColumnType.Int32 },
            { "Name", CsvColumnType.String }
        };

        public static TheoryData<MustCase<(string? Line, string[] Header, Dictionary<string, CsvColumnType> Types)>> ValidCases =>
        [
            new("match", ("1,Steve", Header, Types), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(string? Line, string[] Header, Dictionary<string, CsvColumnType> Types)>> InvalidCases =>
        [
            new("type mismatch",  ("Nan,Steve", Header, Types), new MustExpected(false, "line must be a valid CSV row line.", Code: MustCodes.Csv.Row.Invalid)),
            new("count mismatch", ("1",         Header, Types), new MustExpected(false, "line must be a valid CSV row line.")),
            new("null input",     (null,        Header, Types), new MustExpected(false, "line must not be null.", "line"))
        ];
    }
}
