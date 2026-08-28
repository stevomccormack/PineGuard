using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class CsvAttributesTestData
{
    // CsvLine — fixture-driven
    public static class CsvLine
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCsvLine.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid CSV line.", Code: MustCodes.Csv.Line.Invalid)
        });
    }

    // CsvHeaderLine — inline cases
    public static class CsvHeaderLine
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("exact", "Id,Name", new DataAnnotationExpected(true)),
            new("case insensitive", "id,name", new DataAnnotationExpected(true)),
            new("null", null, new DataAnnotationExpected(true)),
            new("missing header", "Id", new DataAnnotationExpected(false, Code: MustCodes.Csv.Header.Invalid)),
            new("wrong order", "Name,Id", new DataAnnotationExpected(false))
        ];
    }
}
