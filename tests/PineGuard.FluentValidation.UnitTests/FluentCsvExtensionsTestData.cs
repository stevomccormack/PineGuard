using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentCsvExtensionsTestData
{
    public static class CsvLine
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsCsvLine.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid CSV line.")
        });
    }

    public static class CsvHeaderLine
    {
        public static TheoryData<FluentCase<(string? line, IReadOnlyList<string>? expectedHeader)>> Cases =>
        [
            new(nameof(F.IsCsvHeaderLine.Matches), F.IsCsvHeaderLine.Matches, new FluentExpected(true)),
            new(nameof(F.IsCsvHeaderLine.Mismatch), F.IsCsvHeaderLine.Mismatch, new FluentExpected(false, "Value must be a valid CSV header line.")),
            new(nameof(F.IsCsvHeaderLine.NullExpected), F.IsCsvHeaderLine.NullExpected, new FluentExpected(false, "Value must be a valid CSV header line."))
        ];
    }

    public static class CsvRowLineWithSchema
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("ValidRow", "Alice,30", new FluentExpected(true)),
            new("InvalidType", "Alice,not-a-number", new FluentExpected(false, "Value must be a valid CSV row line."))
        ];

        public static IReadOnlyList<CsvColumnSchema> Schema { get; } =
        [
            new("name", CsvColumnType.String),
            new("age", CsvColumnType.Int32)
        ];
    }

    public static class CsvRowLineWithHeader
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("ValidRow", "Alice,30", new FluentExpected(true)),
            new("InvalidType", "Alice,not-a-number", new FluentExpected(false, "Value must be a valid CSV row line."))
        ];

        public static IReadOnlyList<string> Header { get; } = ["name", "age"];

        public static IReadOnlyDictionary<string, CsvColumnType> Types { get; } = new Dictionary<string, CsvColumnType>
        {
            ["name"] = CsvColumnType.String,
            ["age"] = CsvColumnType.Int32
        };
    }
}
