using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustCsvClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.CsvLine.ValidCases), MemberType = typeof(MustCsvClausesTestData.CsvLine))]
    [MemberData(nameof(MustCsvClausesTestData.CsvLine.InvalidCases), MemberType = typeof(MustCsvClausesTestData.CsvLine))]
    public void CsvLine_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.CsvLine(tc.Value, paramName: "line");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.CsvHeaderLine.ValidCases), MemberType = typeof(MustCsvClausesTestData.CsvHeaderLine))]
    [MemberData(nameof(MustCsvClausesTestData.CsvHeaderLine.InvalidCases), MemberType = typeof(MustCsvClausesTestData.CsvHeaderLine))]
    public void CsvHeaderLine_BehavesAsExpected(MustCase<(string? Line, string[]? Header)> tc)
    {
        // Act
        var result = Must.Be.CsvHeaderLine(tc.Value.Line, tc.Value.Header, paramName: "line");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.CsvRowLineSchema.ValidCases), MemberType = typeof(MustCsvClausesTestData.CsvRowLineSchema))]
    [MemberData(nameof(MustCsvClausesTestData.CsvRowLineSchema.InvalidCases), MemberType = typeof(MustCsvClausesTestData.CsvRowLineSchema))]
    public void CsvRowLine_Schema_BehavesAsExpected(MustCase<(string? Line, CsvColumnSchema[] Schema)> tc)
    {
        // Act
        var result = Must.Be.CsvRowLine(tc.Value.Line, tc.Value.Schema, paramName: "line");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.CsvRowLineTypes.ValidCases), MemberType = typeof(MustCsvClausesTestData.CsvRowLineTypes))]
    [MemberData(nameof(MustCsvClausesTestData.CsvRowLineTypes.InvalidCases), MemberType = typeof(MustCsvClausesTestData.CsvRowLineTypes))]
    public void CsvRowLine_Types_BehavesAsExpected(MustCase<(string? Line, string[] Header, Dictionary<string, CsvColumnType> Types)> tc)
    {
        // Act
        var result = Must.Be.CsvRowLine(tc.Value.Line, tc.Value.Header, tc.Value.Types, paramName: "line");

        // Assert
        AssertResult(tc, result);
    }
}
