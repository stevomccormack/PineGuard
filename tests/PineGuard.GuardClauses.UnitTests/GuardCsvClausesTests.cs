using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardCsvClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvLine.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvLine))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvLine.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvLine))]
    public void NotCsvLine_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotCsvLine(value));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvLine(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvHeaderLine.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvHeaderLine))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvHeaderLine.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvHeaderLine))]
    public void NotCsvHeaderLine_BehavesAsExpected(GuardCase<(string? line, IReadOnlyList<string>? expectedHeader)> tc)
    {
        var value = tc.Value.line;
        var result = AssertResult(tc, () => Guard.Against.NotCsvHeaderLine(value, tc.Value.expectedHeader));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvHeaderLine(value, tc.Value.expectedHeader, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineSchema.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineSchema))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineSchema.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineSchema))]
    public void NotCsvRowLineSchema_BehavesAsExpected(GuardCase<(string? line, IReadOnlyList<CsvColumnSchema>? schema)> tc)
    {
        var value = tc.Value.line;
        var result = AssertResult(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.schema));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.schema, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes.ValidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes))]
    [MemberData(nameof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes.InvalidCases), MemberType = typeof(GuardCsvClausesTestData.NotCsvRowLineHeaderTypes))]
    public void NotCsvRowLineHeaderTypes_BehavesAsExpected(GuardCase<(string? line, IReadOnlyList<string>? header, IReadOnlyDictionary<string, CsvColumnType>? types)> tc)
    {
        var value = tc.Value.line;
        var result = AssertResult(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.header, tc.Value.types));
        AssertCustomMessage(tc, () => Guard.Against.NotCsvRowLine(value, tc.Value.header, tc.Value.types, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.Separator.Cases), MemberType = typeof(GuardCsvClausesTestData.Separator))]
    public void CsvFamily_InvalidSeparatorHasConfigurationCode(char separator)
    {
        // Act
        var line = Assert.Throws<ArgumentException>(() => Guard.Against.NotCsvLine(null, separator));
        var header = Assert.Throws<ArgumentException>(() => Guard.Against.NotCsvHeaderLine(null, ["a"], separator));
        var row = Assert.Throws<ArgumentException>(() => Guard.Against.NotCsvRowLine(null, [new CsvColumnSchema("a", CsvColumnType.String)], separator));
        var mapped = Assert.Throws<ArgumentException>(() => Guard.Against.NotCsvRowLine(null, ["a"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.String }, separator));

        // Assert
        Assert.Equal("separator", line.ParamName);
        Assert.Equal("separator", header.ParamName);
        Assert.Equal("separator", row.ParamName);
        Assert.Equal("separator", mapped.ParamName);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Line.Invalid, line.Data[GuardFailure.CodeDataKey]);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Header.Invalid, header.Data[GuardFailure.CodeDataKey]);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Row.Invalid, row.Data[GuardFailure.CodeDataKey]);
        Assert.Equal(PineGuard.Codes.MustCodes.Csv.Row.Invalid, mapped.Data[GuardFailure.CodeDataKey]);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.SchemaConfiguration.Cases), MemberType = typeof(GuardCsvClausesTestData.SchemaConfiguration))]
    public void CsvRowLine_AttributesInvalidSchema(GuardCase<IReadOnlyList<CsvColumnSchema>?> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.NotCsvRowLine(null, tc.Value));

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(GuardCsvClausesTestData.ConfiguredSeparator.Cases), MemberType = typeof(GuardCsvClausesTestData.ConfiguredSeparator))]
    public void CsvFamily_UsesConfiguredSeparator(char separator)
    {
        // Arrange
        var value = $"a{separator}\"b{separator}c\"";

        // Act
        var line = Guard.Against.NotCsvLine(value, separator);
        var header = Guard.Against.NotCsvHeaderLine($"a{separator}b", ["a", "b"], separator);
        var row = Guard.Against.NotCsvRowLine(value, [new CsvColumnSchema("a", CsvColumnType.String), new CsvColumnSchema("b", CsvColumnType.String)], separator);
        var mapped = Guard.Against.NotCsvRowLine(value, ["a", "b"], new Dictionary<string, CsvColumnType> { ["a"] = CsvColumnType.String }, separator);

        // Assert
        Assert.Equal(value, line);
        Assert.Equal($"a{separator}b", header);
        Assert.Equal(value, row);
        Assert.Equal(value, mapped);
    }
}
