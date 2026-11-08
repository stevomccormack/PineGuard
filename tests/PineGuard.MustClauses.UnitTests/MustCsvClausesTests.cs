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

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.SchemaConfiguration.Cases), MemberType = typeof(MustCsvClausesTestData.SchemaConfiguration))]
    public void CsvRowLine_AttributesSchemaBeforeValue(MustCase<IReadOnlyList<CsvColumnSchema>?> tc)
    {
        // Act
        var result = Must.Be.CsvRowLine(value: null, schema: tc.Value);

        // Assert
        AssertResult(tc, result);
        Assert.Same(tc.Value, result.Value);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.Separator.InvalidCases), MemberType = typeof(MustCsvClausesTestData.Separator))]
    public void CsvFamily_AttributesInvalidSeparatorBeforeNullValue(MustCase<char> tc)
    {
        // Act
        var line = Must.Be.CsvLine(null, tc.Value);
        var header = Must.Be.CsvHeaderLine(null, ["a"], tc.Value);
        var row = Must.Be.CsvRowLine(null, [new CsvColumnSchema("a", CsvColumnType.String)], tc.Value);
        var mapped = Must.Be.CsvRowLine(null, ["a"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.String }, tc.Value);

        // Assert
        AssertResult(tc, line);
        AssertResult(tc, header);
        AssertResult(tc, row);
        AssertResult(tc, mapped);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Line.Invalid, line.Code);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Header.Invalid, header.Code);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Row.Invalid, row.Code);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Row.Invalid, mapped.Code);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.HeaderConfiguration.InvalidCases), MemberType = typeof(MustCsvClausesTestData.HeaderConfiguration))]
    public void CsvFamily_AttributesInvalidHeaderBeforeNullValue(MustCase<IReadOnlyList<string>?> tc)
    {
        // Act
        var result = Must.Be.CsvHeaderLine(null, tc.Value);
        var mapped = Must.Be.CsvRowLine(null, tc.Value, new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.String });

        // Assert
        AssertResult(tc, result);
        Assert.True(mapped.Failed);
        Assert.Equal("header", mapped.ParamName);
        Assert.Same(tc.Value, mapped.Value);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.TypeMapConfiguration.InvalidCases), MemberType = typeof(MustCsvClausesTestData.TypeMapConfiguration))]
    public void CsvRowLine_AttributesInvalidTypeMap(MustCase<IReadOnlyDictionary<string, CsvColumnType>?> tc)
    {
        // Act
        var result = Must.Be.CsvRowLine(null, ["a"], tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.ComparisonConfiguration.InvalidCases), MemberType = typeof(MustCsvClausesTestData.ComparisonConfiguration))]
    public void CsvFamily_RejectsUndefinedComparison(StringComparison comparison)
    {
        // Act
        var header = Must.Be.CsvHeaderLine(null, ["a"], comparison: comparison);
        var row = Must.Be.CsvRowLine(null, ["a"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.String }, headerNameComparison: comparison);

        // Assert
        Assert.True(header.Failed);
        Assert.Equal("comparison", header.ParamName);
        Assert.Equal(comparison, header.Value);
        Assert.True(row.Failed);
        Assert.Equal("headerNameComparison", row.ParamName);
        Assert.Equal(comparison, row.Value);
    }

    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.ConfiguredSeparator.Cases), MemberType = typeof(MustCsvClausesTestData.ConfiguredSeparator))]
    public void CsvFamily_UsesSeparatorAndPreservesOriginalValue(char separator)
    {
        // Arrange
        var value = $"a{separator}\"b{separator}c\"";

        // Act
        var result = Must.Be.CsvLine(value, separator);
        var header = Must.Be.CsvHeaderLine($"a{separator}b", ["a", "b"], separator);
        var row = Must.Be.CsvRowLine(value, [new CsvColumnSchema("a", CsvColumnType.String), new CsvColumnSchema("b", CsvColumnType.String)], separator);
        var mapped = Must.Be.CsvRowLine(value, ["a", "b"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.String }, separator);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(value, result.Value);
        Assert.Equal(value, result.Result);
        Assert.True(header.Success);
        Assert.True(row.Success);
        Assert.True(mapped.Success);
    }
}
